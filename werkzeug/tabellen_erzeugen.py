"""Erzeugt kvdt-tabellen.fs aus der Textfassung der KBV-Datensatzbeschreibung KVDT.

Aufruf:
    python werkzeug/tabellen_erzeugen.py <KBV_ITA_VGEX_Datensatzbeschreibung_KVDT.pdf.txt>

Extrahiert fuer Container-Saetze und ADT-Datenpaket:
  * Feldtabellen (Kap. 2.3.1 und 3.5.1): Feldkennung, Laenge, Typ, Bezeichnung
  * Satztabellen (Kap. 2.2 und 3.4): Feldkennung, Vorkommen (1/n), Feldart (M/m/K/k)

Die Textfassung des PDFs hat Eigenheiten (Symbolzeichen aus dem PDF-Font,
umbrochene Zellen, verlorene Spalteneinrueckung). Wo die automatische
Erkennung nicht eindeutig ist, greifen die KORREKTUREN unten - jede mit
Begruendung.
"""
import re
import sys
from pathlib import Path

ZIEL = Path(__file__).resolve().parent.parent / "kvdt-tabellen.fs"

# Abschnitte (Zeilennummern in der Textfassung von Version 6.08)
FELDTABELLEN = [("Container", "CONTAINER-FELDTABELLE", "2.3.2 Container-Regeltabelle"),
                ("ADT", "ADT-FELDTABELLE", "3.5.2 ADT-Regeltabelle")]
SATZTABELLEN = [  # Satzart, Startmarke, Endmarke
    ("con0", "2.2.1 Satzart: Container-Header", "2.2.2 Satzart"),
    ("con9", "2.2.2 Satzart: Container-Abschluss", "2.2.3 Satzart"),
    ("besa", "2.2.3 Satzart: Betriebsstättendaten", "2.2.4 Satzart"),
    ("rvsa", "2.2.4 Satzart: Ringversuchszertifikate", "2.3 ANFORDERUNGEN"),
    ("adt0", "3.4.1 Satzart: ADT-Datenpaket-Header", "3.4.2 Satzart"),
    ("adt9", "3.4.2 Satzart: ADT-Datenpaket-Abschluss", "3.4.3 Satzart"),
    ("0101", "3.4.3 Satzart: Ambulante Behandlung", "3.4.4 Satzart"),
    ("0102", "3.4.4 Satzart: Überweisung", "3.4.5 Satzart"),
    ("0103", "3.4.5 Satzart: Belegärztliche Behandlung", "3.4.6 Satzart"),
    ("0104", "3.4.6 Satzart: Notfalldienst", "3.5 ANFORDERUNGEN"),
]

# Manuelle Korrekturen der Feldtabelle: FK -> (Laengenangabe, Typ, Bezeichnung, Begruendung)
KORREKTUREN = {
    "4204": ("1", "n", "eingeschränkter Leistungsanspruch gemäß §16 Abs. 3a SGB V",
             "Text 'Abs. 3a' wurde als Laenge 3, Typ a erkannt; Tabelle: 1 n"),
    "4233": ("16", "n", "Stationäre Behandlung von... bis...",
             "Bezeichnung und Laenge stehen ohne Leerzeichen hintereinander"),
    "5001": ("<=9", "a", "Gebührennummer (GNR)",
             "Tabelle: '<= 9 bzw. 5, 6'; 5/6 Stellen ergeben sich aus Regel 042"),
}


def normalisieren(text: str) -> str:
    """Symbolzeichen der PDF-Schrift durch echte Zeichen ersetzen."""
    return (text.replace("", "≤").replace("", "≠").replace("", "≥")
                .replace("\x02", "").replace("", "").replace("", "")
                .replace("", ""))


def abschnitt(zeilen, start, ende):
    nach_inhalt = zeilen.index("1 GRUNDLAGEN")  # Inhaltsverzeichnis ueberspringen
    i = next(n for n in range(nach_inhalt, len(zeilen)) if zeilen[n].startswith(start))
    j = next(n for n in range(i + 1, len(zeilen)) if zeilen[n].startswith(ende))
    raus = []
    for z in zeilen[i + 1:j]:
        if z.startswith("Seite ") and "Datensatzbeschreibung KVDT" in z:
            continue
        if z.startswith("FK ") or z.endswith("FELDTABELLE") or re.match(r"^1 2 3", z):
            continue
        if z.startswith("________________") or re.match(r"^\d+ Der Satz", z):
            continue
        raus.append(z)
    return raus


def feldtabelle(zeilen, start, ende):
    eintraege, akt = [], None
    for z in abschnitt(zeilen, start, ende):
        m = re.match(r"^(\d{4}) (\S.*)$", z)
        if m and not re.match(r"^\d{4} *=", z):
            akt = [m.group(1), m.group(2)]
            eintraege.append(akt)
        elif akt:
            akt[1] += "\n" + z
    felder = {}
    for fk, t in eintraege:
        m = re.search(r"(?:(?<=\n)|(?<= )|(?<=[a-zäöüß)\-]))(≤ ?)?"
                      r"(\d+(?: ?[-–] ?\d+|(?:, ?\d+)+)?) ?([nad])(?=\s|$)", t)
        if fk in KORREKTUREN:
            laenge, typ, name, _ = KORREKTUREN[fk]
        elif not m:
            raise SystemExit(f"Feld {fk}: Laenge/Typ nicht erkannt")
        else:
            zahl = m.group(2).replace(" ", "").replace("–", "-")
            laenge = ("<=" if m.group(1) else "") + zahl
            typ = m.group(3)
            name = t[:m.start()]
        name = " ".join(name.replace("≤", "").split())
        name = re.sub(r"\s*\d+$", "", name)  # Fussnotenziffer am Ende
        felder[fk] = (laenge, typ, name)
    return felder


def satztabelle(zeilen, start, ende):
    eintraege, akt = [], None
    for z in abschnitt(zeilen, start, ende):
        m = re.match(r"^(\d{4}) (1|n) (.*)$", z)
        if m:
            akt = [m.group(1), m.group(2), m.group(3)]
            eintraege.append(akt)
        elif akt:
            akt[2] += "\n" + z
    zeilen_raus = []
    for fk, vork, t in eintraege:
        flach = " ".join(t.split("\n"))
        frei = re.search(r"(?:^|\s)([MmKk])(?=\s|$)", flach)
        klebt = None
        for g in re.finditer(r"([a-zäöüß)\-/])([MmKk])(?=\s|$)", flach):
            if re.search(r"(datu|Datu|zeitrau|\bi|\(i)$", flach[:g.start() + 1]):
                continue  # ...datum, ...zeitraum, im - kein Feldart-Kuerzel
            klebt = g
            break
        if frei and (not klebt or frei.start() < klebt.start()):
            art = frei.group(1)
        elif klebt:
            art = klebt.group(2)
        else:
            raise SystemExit(f"Satztabelle {start}: Feldart fuer {fk} nicht erkannt")
        zeilen_raus.append((fk, vork, art))
    return zeilen_raus


def laenge_codieren(laenge):
    """-> (art, a, b, c):  0 = feste Laenge(n) a|b|c,  1 = Bereich a..b,  2 = max. a"""
    if laenge.startswith("<="):
        return 2, int(laenge[2:]), 0, 0
    if "-" in laenge:
        a, b = laenge.split("-")
        return 1, int(a), int(b), 0
    werte = [int(x) for x in laenge.split(",")] + [0, 0]
    return 0, werte[0], werte[1], werte[2]


def main():
    if len(sys.argv) != 2:
        raise SystemExit(__doc__)
    text = normalisieren(Path(sys.argv[1]).read_text(encoding="utf-8"))
    zeilen = text.split("\n")
    version = re.search(r"Version (\d+\.\d+) / (\d+\. \w+ \d{4})", text)

    felder = {}
    for _, start, ende in FELDTABELLEN:
        felder.update(feldtabelle(zeilen, start, ende))

    raus = [
        "\\ ===================================================================",
        "\\  kvdt-tabellen.fs  --  GENERIERT von werkzeug/tabellen_erzeugen.py",
        f"\\  Quelle: KBV Datensatzbeschreibung KVDT, Version {version.group(1)} "
        f"vom {version.group(2)}",
        "\\  Nicht von Hand bearbeiten - Generator anpassen und neu erzeugen.",
        "\\ ===================================================================",
        "",
        "\\ Feldtabelle:  FK  Typ  Laengenart  a  b  c  feld\" Bezeichnung\"",
        "\\   Typ: 110 = n(umerisch)  97 = a(lphanumerisch)  100 = d(atum)",
        "\\   Laengenart: 0 = fest (a, b oder c)  1 = von a bis b  2 = hoechstens a",
        "create feldtabelle",
    ]
    for fk in sorted(felder):
        laenge, typ, name = felder[fk]
        art, a, b, c = laenge_codieren(laenge)
        kommentar = f"  \\ Korrektur: {KORREKTUREN[fk][3]}" if fk in KORREKTUREN else ""
        name = name.replace('"', "'")[:60]
        raus.append(f"  {int(fk):4d} {ord(typ):3d} {art} {a:3d} {b:2d} {c} "
                    f"feld\" {name}\"{kommentar}")
    raus += ["  0 0 0 0 0 0 feld\" \"", ""]

    raus += [
        "\\ Satztabellen:  FK  Vorkommen(1|0=n)  Feldart(Zeichen)  Zone  st,",
        "\\   Zone (nur Behandlungsfaelle): 0 = Kopf, 1 = Leistungen (ab 5000),",
        "\\   2 = Diagnosen (ab 6001), 3 = Dauerdiagnosen (ab 3673)",
    ]
    for sa, start, ende in SATZTABELLEN:
        zeilen_sa = satztabelle(zeilen, start, ende)
        raus.append(f"create st-{sa}")
        zone = 0
        for fk, vork, art in zeilen_sa:
            if sa.startswith("010"):
                zone = {"5000": max(zone, 1), "6001": max(zone, 2),
                        "3673": max(zone, 3)}.get(fk, zone)
            if fk not in felder:
                raise SystemExit(f"Satztabelle {sa}: FK {fk} fehlt in Feldtabelle")
            raus.append(f"  {int(fk):4d} {1 if vork == '1' else 0} {ord(art):3d} {zone} st,"
                        f"   \\ {fk} {vork} {art}  {felder[fk][2][:40]}")
        raus.append("  0 0 0 0 st,")
    ZIEL.write_text("\n".join(raus) + "\n", encoding="utf-8")
    print(f"{len(felder)} Felder, {len(SATZTABELLEN)} Satztabellen -> {ZIEL}")


if __name__ == "__main__":
    main()
