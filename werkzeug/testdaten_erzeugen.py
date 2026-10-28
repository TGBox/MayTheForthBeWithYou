"""Erzeugt synthetische KVDT-Testdateien fuer den Forth-Pruefer.

Alle Patienten, Aerzte und Nummern sind frei erfunden.
Zeichensatz ISO-8859-15, Zeilenende CRLF, Laengenangabe inkl. CRLF.

Aufruf (aus dem Projektordner):  python werkzeug/testdaten_erzeugen.py
"""
from pathlib import Path

ZIEL = Path(__file__).resolve().parent.parent / "testdaten"


def lanr(arztnr6: str, fachgruppe: str = "01") -> str:
    """LANR: 6 Ziffern Arztnummer + Pruefziffer + 2 Ziffern Fachgruppe."""
    s = sum(int(d) * (4 if i % 2 == 0 else 9) for i, d in enumerate(arztnr6))
    return arztnr6 + str((10 - s % 10) % 10) + fachgruppe


def gueltige_saetze():
    """Liefert die Saetze einer gueltigen Datei als Liste von (FK, Inhalt)."""
    return [
        ("8000", "con0"), ("9103", "28102026"),
        ("8000", "besa"), ("0201", "721234500"),
        ("8000", "rvsa"), ("0212", lanr("123456")),
        # Fall 1
        ("8000", "0101"), ("3000", "1001"),
        ("3101", "Müller-Lüdenscheidt"), ("3102", "Werner"),
        ("3103", "12031958"), ("3110", "M"), ("4101", "32026"),
        ("5000", "14072026"), ("5001", "03000"), ("5001", "03230"),
        ("6001", "J06.9"), ("6003", "G"),
        # Fall 2
        ("8000", "0101"), ("3000", "1002"),
        ("3101", "Klöbner"), ("3102", "Erwin"),
        ("3103", "29021960"), ("3110", "M"), ("4101", "32026"),
        ("5000", "02092026"), ("5001", "03000"),
        ("6001", "I10.90"), ("6003", "G"),
        ("6001", "U07.1!"), ("6003", "V"),
        # Fall 3 (Ueberweisung)
        ("8000", "0102"), ("3000", "1003"),
        ("3101", "Mustermann"), ("3102", "Erika"),
        ("3103", "01011970"), ("3110", "W"), ("4101", "32026"),
        ("5000", "30092026"), ("5001", "01436"),
        ("6001", "M54.5"), ("6003", "A"),
        ("8000", "con9"),
    ]


def zeile(fk: str, inhalt: str, enc="iso-8859-15") -> bytes:
    rumpf = (fk + inhalt).encode(enc)
    return f"{len(rumpf) + 5:03d}".encode() + rumpf + b"\r\n"


def datei(saetze, enc="iso-8859-15") -> bytes:
    return b"".join(zeile(fk, inh, enc) for fk, inh in saetze)


def ersetze(fk, alt, neu):
    """Ersetzt das erste Feld (fk, alt) durch (fk, neu)."""
    s = gueltige_saetze()
    s[s.index((fk, alt))] = (fk, neu)
    return datei(s)


def ohne(fk, inhalt):
    s = gueltige_saetze()
    s.remove((fk, inhalt))
    return datei(s)


def zeile_manipulieren(nr, fn):
    """Wendet fn auf die Rohbytes der Zeile nr (1-basiert) an."""
    zeilen = datei(gueltige_saetze()).split(b"\r\n")[:-1]
    zeilen = [z + b"\r\n" for z in zeilen]
    zeilen[nr - 1] = fn(zeilen[nr - 1])
    return b"".join(zeilen)


def main():
    ZIEL.mkdir(exist_ok=True)
    s = gueltige_saetze()
    dateien = {
        "00_gueltig.kvdt": datei(s),
        "01_zeile_zu_kurz.kvdt": zeile_manipulieren(8, lambda z: b"0073\r\n"),
        "02_laengenangabe_falsch.kvdt": zeile_manipulieren(9, lambda z: b"099" + z[3:]),
        "03_feldkennung_nicht_numerisch.kvdt": zeile_manipulieren(8, lambda z: z.replace(b"3000", b"3O00", 1)),
        "04_zeilenende_nur_lf.kvdt": zeile_manipulieren(10, lambda z: z[:-2] + b"\n"),
        "05_satzartfolge_falsch.kvdt": datei([s[2], s[3], s[0], s[1]] + s[4:]),
        "05b_con9_fehlt.kvdt": datei(s[:-1]),
        "06_feld_vor_erstem_satz.kvdt": datei([("9103", "28102026")] + s),
        "07_pflichtfeld_fehlt.kvdt": ohne("3103", "12031958"),
        "08_datum_ungueltig.kvdt": ersetze("3103", "29021960", "29021961"),
        "09_geburt_nach_leistung.kvdt": ersetze("3103", "01011970", "01102026"),
        "10_quartal_ungueltig.kvdt": ersetze("4101", "32026", "52026"),
        "11_leistung_ausserhalb_quartal.kvdt": ersetze("5000", "30092026", "01102026"),
        "12_icd_format.kvdt": ersetze("6001", "M54.5", "M5.45"),
        "13_diagnosensicherheit.kvdt": ersetze("6003", "A", "X"),
        "14_lanr_pruefziffer.kvdt": ersetze("0212", lanr("123456"), "123456001"),
        "15_bsnr_format.kvdt": ersetze("0201", "721234500", "72123450"),
        "16_gop_format.kvdt": ersetze("5001", "01436", "1436"),
        "17_geschlecht.kvdt": ersetze("3110", "W", "F"),
        "18_leistung_ohne_tag.kvdt": ohne("5000", "02092026"),
        # Die gueltige Datei, aber "versehentlich" als UTF-8 gespeichert:
        "19_als_utf8_gespeichert.kvdt": datei(s).decode("iso-8859-15").encode("utf-8"),
    }
    for name, inhalt in dateien.items():
        (ZIEL / name).write_bytes(inhalt)
    print(f"{len(dateien)} Dateien in {ZIEL} geschrieben.")


if __name__ == "__main__":
    main()
