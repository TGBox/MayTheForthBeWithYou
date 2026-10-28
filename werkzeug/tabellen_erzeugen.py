"""Erzeugt kvdt-tabellen.fs direkt aus der PDF der KBV-Datensatzbeschreibung KVDT.

Aufruf (aus dem Projektordner):
    pip install pdfplumber
    python werkzeug/tabellen_erzeugen.py spec/KBV_ITA_VGEX_Datensatzbeschreibung_KVDT.pdf

Gelesen werden fuer Container-Saetze und ADT-Datenpaket:
  * Feldtabellen (Kap. 2.3.1, 3.5.1): Feldkennung, Bezeichnung, Laenge, Typ
  * Satztabellen (Kap. 2.2, 3.4):     Feldkennung, Vorkommen (1/n), Ebene der
    Hierarchie (Spalte "Vorkommen 1 2 3 4 5"), Feldart (M/m/K/k), Bedingung

Die Ebene ergibt sich aus der x-Position des Vorkommen-Eintrags auf der Seite.
Diese Information fehlt in einer reinen Textfassung - deshalb die PDF.
"""
import re
import sys
from pathlib import Path

import pdfplumber

ZIEL = Path(__file__).resolve().parent.parent / "kvdt-tabellen.fs"
SATZARTEN = ["con0", "con9", "besa", "rvsa", "adt0", "adt9", "0101", "0102", "0103", "0104"]
FELDTABELLEN = ("CONTAINER-FELDTABELLE", "ADT-FELDTABELLE")
KLEINER_GLEICH = ("", "≤")          # Symbolschrift bzw. Unicode


def woerter(seite):
    return sorted(seite.extract_words(), key=lambda w: (round(w["top"]), w["x0"]))


def zeile(ws, top, x_von, x_bis, toleranz=3.5):
    return [w for w in ws if abs(w["top"] - top) < toleranz and x_von <= w["x0"] < x_bis]


def bereich(ws, top_von, top_bis, x_von, x_bis):
    return [w for w in ws if top_von <= w["top"] < top_bis and x_von <= w["x0"] < x_bis]


def text_zusammen(ws):
    """Woerter zeilenweise zusammensetzen, Silbentrennung am Zeilenende aufloesen."""
    raus = ""
    letzte_top = None
    for w in sorted(ws, key=lambda w: (round(w["top"]), w["x0"])):
        t = w["text"]
        if not raus:
            raus = t
        elif letzte_top is not None and abs(w["top"] - letzte_top) > 3 and raus.endswith("-"):
            # Trennstrich am Zeilenende: "Unter-|suchung" -> "Untersuchung", "ASV-|Abrechnung" bleibt
            raus = (raus[:-1] if t[:1].islower() else raus) + t
        else:
            raus += " " + t
        letzte_top = w["top"]
    return raus


def feldtabellen(pdf):
    felder = {}
    for seite in pdf.pages:
        ws = woerter(seite)
        titel = [w for w in ws if w["text"] in FELDTABELLEN]
        laenge_kopf = [w for w in ws if w["text"] == "Länge" and titel and w["top"] > titel[0]["top"]]
        if not laenge_kopf:
            continue
        kopf = {w["text"]: w for w in ws if w["text"] in ("FK", "Feldbezeichnung", "Länge", "Typ", "Regel")
                and abs(w["top"] - laenge_kopf[0]["top"]) < 3}
        if len(kopf) < 5:
            continue
        x_name, x_laenge, x_typ, x_regel = (kopf[k]["x0"] for k in ("Feldbezeichnung", "Länge", "Typ", "Regel"))
        fk_zeilen = [w for w in ws if re.fullmatch(r"\d{4}", w["text"]) and w["x0"] < x_name - 5
                     and w["top"] > kopf["FK"]["top"] + 10]
        for k, w in enumerate(fk_zeilen):
            top = w["top"]
            fuss = min([x["top"] for x in ws if x["text"].startswith("____")], default=seite.height - 40)
            naechste = fk_zeilen[k + 1]["top"] - 2 if k + 1 < len(fk_zeilen) else fuss
            links = [x["top"] for x in ws if x["top"] > top + 3 and x["x0"] < x_name - 5]
            naechste = min([naechste] + links)        # z. B. Ueberschrift des naechsten Kapitels
            laenge = zeile(ws, top, x_laenge - 5, x_typ - 3)
            typ = zeile(ws, top, x_typ - 3, x_regel - 3)
            name = bereich(ws, top - 3, naechste, x_name - 2, x_laenge - 5)
            if not laenge or not typ:
                raise SystemExit(f"Feldtabelle S. {seite.page_number}: Laenge/Typ fuer {w['text']} fehlt")
            l_text = "".join(x["text"] for x in sorted(laenge, key=lambda x: x["x0"]))
            hoechstens = any(z in l_text for z in KLEINER_GLEICH)
            if hoechstens:   # evtl. Fussnotenziffer davor (z. B. 4124 "5 <= 60")
                l_text = re.split("|".join(KLEINER_GLEICH), l_text)[-1]
            l_text = l_text.replace("–", "-")
            felder[w["text"]] = (("<=" if hoechstens else "") + l_text, typ[0]["text"], text_zusammen(name))
    return felder


def satztabellen(pdf):
    saetze, aktuell = {}, None
    for seite in pdf.pages:
        ws = woerter(seite)
        koepfe = []
        for vk in [w for w in ws if w["text"] == "Vorkommen"]:
            gleich = {n: [x for x in ws if x["text"] == n and abs(x["top"] - vk["top"]) < 3]
                      for n in ("Feldbezeichnung", "Feldart", "Bedingung", "Erläuterung")}
            if not all(gleich.values()):
                continue
            spalten = [x for x in ws if 5 < x["top"] - vk["top"] < 40
                       and vk["x0"] - 5 < x["x0"] < gleich["Feldbezeichnung"][0]["x0"] and x["text"] in "12345"]
            if len(spalten) >= 3:
                koepfe.append(dict(top=vk["top"], spalten=sorted(x["x0"] for x in spalten),
                                   art=gleich["Feldart"][0]["x0"], bed=gleich["Bedingung"][0]["x0"],
                                   erl=gleich["Erläuterung"][0]["x0"]))
        ereignisse = []
        for w in ws:
            if w["text"] == "Satzart:" and w["x0"] < 120:
                for t in [x["text"].strip("”“„\"") for x in ws if abs(x["top"] - w["top"]) < 2]:
                    if t in SATZARTEN:
                        ereignisse.append((w["top"], "kopf", t))
            if w["text"].startswith("ANFORDERUNGEN"):
                ereignisse.append((w["top"], "ende", None))
        fk_zeilen = [w for w in ws if re.fullmatch(r"\d{4}", w["text"]) and 55 < w["x0"] < 75] if koepfe else []
        for k, w in enumerate(fk_zeilen):
            ereignisse.append((w["top"], "feld", k))
        for top, art, wert in sorted(ereignisse, key=lambda e: e[0]):
            if art == "kopf":
                aktuell = wert
                saetze.setdefault(aktuell, [])
                continue
            if art == "ende":
                aktuell = None
                continue
            if aktuell is None:
                continue
            w = fk_zeilen[wert]
            kopf = max([h for h in koepfe if h["top"] < top], key=lambda h: h["top"])
            naechste = fk_zeilen[wert + 1]["top"] if wert + 1 < len(fk_zeilen) else 1e9
            naechste = min(naechste, min([h["top"] for h in koepfe if h["top"] > top], default=1e9))
            sp = kopf["spalten"]
            vork = zeile(ws, top, sp[0] - 5, sp[-1] + 8)
            vork = [x for x in vork if x["text"] in ("1", "n")]
            if not vork:
                raise SystemExit(f"Satztabelle {aktuell} S. {seite.page_number}: Vorkommen fuer {w['text']} fehlt")
            ebene = min(range(len(sp)), key=lambda i: abs(sp[i] - vork[0]["x0"])) + 1
            feldart = [x["text"] for x in bereich(ws, top - 3.5, naechste - 3.5, kopf["art"] - 12, kopf["bed"] - 3)
                       if x["text"] in ("M", "m", "K", "k")]
            bedingung = text_zusammen(bereich(ws, top - 3.5, naechste - 3.5, kopf["bed"] - 3, kopf["erl"] - 3))
            if not feldart:
                raise SystemExit(f"Satztabelle {aktuell} S. {seite.page_number}: Feldart fuer {w['text']} fehlt")
            saetze[aktuell].append((w["text"], vork[0]["text"], ebene, feldart[0], bedingung))
    return saetze


def laenge_codieren(laenge):
    """-> (art, a, b, c):  0 = feste Laenge(n) a|b|c,  1 = Bereich a..b,  2 = hoechstens a"""
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
    pdf = pdfplumber.open(sys.argv[1])
    erste_seite = pdf.pages[0].extract_text() or ""
    version = re.search(r"Version (\d+\.\d+) / (\d+\. \w+ \d{4})", erste_seite)
    felder = feldtabellen(pdf)
    saetze = satztabellen(pdf)
    fehlend = [s for s in SATZARTEN if not saetze.get(s)]
    if fehlend:
        raise SystemExit(f"Satztabellen nicht gefunden: {fehlend}")

    raus = [
        "\\ ===================================================================",
        "\\  kvdt-tabellen.fs  --  GENERIERT von werkzeug/tabellen_erzeugen.py",
        f"\\  Quelle: KBV Datensatzbeschreibung KVDT (PDF), Version {version.group(1)} "
        f"vom {version.group(2)}" if version else "\\  Quelle: KBV Datensatzbeschreibung KVDT (PDF)",
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
        name = name.replace('"', "'")[:60]
        raus.append(f"  {int(fk):4d} {ord(typ):3d} {art} {a:3d} {b:2d} {c} feld\" {name}\"")
    raus += ["  0 0 0 0 0 0 feld\" \"", ""]

    raus += [
        "\\ Satztabellen:  FK  Vorkommen(1|0=n)  Feldart  Ebene  st,",
        "\\   Feldart als Zeichen: M = unbedingtes Mussfeld, m = bedingtes Mussfeld,",
        "\\   P = bedingtes Mussfeld ohne Bedingung (gebunden an das uebergeordnete Feld,",
        "\\   Kap. 1.5.3), K = Kannfeld, k = bedingtes Kannfeld",
        "\\   Ebene: Hierarchiestufe aus der Spalte Vorkommen (1 = Satzebene)",
    ]
    for sa in SATZARTEN:
        raus.append(f"create st-{sa}")
        for fk, vork, ebene, art, bedingung in saetze[sa]:
            if fk not in felder:
                raise SystemExit(f"Satztabelle {sa}: FK {fk} fehlt in der Feldtabelle")
            code = "P" if art == "m" and ebene > 1 and not bedingung else art
            einr = "  " * (ebene - 1)
            hinweis = f"  [{bedingung[:30]}]" if bedingung else ""
            raus.append(f"  {int(fk):4d} {1 if vork == '1' else 0} {ord(code):3d} {ebene} st,"
                        f"   \\ {einr}{fk} {vork} {code}  {felder[fk][2][:34]}{hinweis}")
        raus.append("  0 0 0 0 st,")
    ZIEL.write_text("\n".join(raus) + "\n", encoding="utf-8")
    print(f"{len(felder)} Felder, {len(SATZARTEN)} Satztabellen -> {ZIEL}")


if __name__ == "__main__":
    main()
