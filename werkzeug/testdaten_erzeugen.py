"""Erzeugt synthetische KVDT-Testdateien (Datensatzbeschreibung KVDT 6.08).

Alle Patienten, Aerzte, Praxen und Nummern sind frei erfunden.
Zeichensatz ISO-8859-15, Feldende CR LF, Laengenangabe = Inhalt + 9.

Aufruf (aus dem Projektordner):  python werkzeug/testdaten_erzeugen.py
"""
from pathlib import Path

ZIEL = Path(__file__).resolve().parent.parent / "testdaten"


def pz49(ziffern6: str) -> str:
    """Pruefziffer Modulo 10, Gewichte 4/9 (LANR Regel 050, ASV-Teamnummer Regel 059)."""
    s = sum(int(d) * (4 if i % 2 == 0 else 9) for i, d in enumerate(ziffern6))
    return str((10 - s % 10) % 10)


def versicherten_id(praefix9: str) -> str:
    """Versicherten-ID (Regel 054): Buchstabe + 8 Ziffern + Pruefziffer."""
    ziffern = f"{ord(praefix9[0]) - 64:02d}" + praefix9[1:]
    summe = 0
    for i, z in enumerate(ziffern):
        p = int(z) * (1 if i % 2 == 0 else 2)
        summe += p // 10 + p % 10
    return praefix9 + str(summe % 10)


BSNR = "271234500"                       # KV-Kennung 27 (Koeln)
LANR = "123456" + pz49("123456") + "01"  # 123456601
VID = versicherten_id("M58031240")


def satz(art, *felder):
    return [("8000", art)] + list(felder)


def gueltige_datei():
    """Liste von Saetzen; jeder Satz ist eine Liste von (FK, Inhalt)."""
    ti = []
    for wert in ["1", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13"]:
        ti += [("0225", wert), ("0226", "1")]
    leistung = [("5098", BSNR), ("5099", LANR)]
    return [
        satz("con0", ("9103", "20261015"), ("9106", "4"), ("9132", "1")),
        satz("besa", ("0201", BSNR), ("0203", "Praxis Dr. Muster"),
             ("0212", LANR), ("0220", "Hans"), ("0211", "Muster"),
             ("0205", "Domstr. 1"), ("0215", "50667"), ("0216", "Köln"),
             ("0208", "0221/123456"), ("0224", "PTV5"), ("0227", "20281231"),
             ("0228", "Beispiel-Konnektor"), *ti),
        satz("rvsa", ("0201", BSNR), ("0300", "0")),
        satz("adt0", ("0105", "Y/9/2607/36/123"), ("9102", "27"), ("9212", "ADT0726.01"),
             ("0102", "Muster Software GmbH"), ("0121", "Hauptstr. 7"), ("0122", "56068"),
             ("0123", "Koblenz"), ("0124", "0261/000000"), ("0126", "Systemhaus Beispiel"),
             ("0127", "Ringstr. 3"), ("0128", "50859"), ("0129", "Köln"),
             ("0130", "0221/000000"), ("0103", "MayTheForth PVS"), ("9115", "20261015"),
             ("9204", "32026")),
        # Fall 1: eGK eingelesen (Online-Pruefung, CDM 5.2.0)
        satz("0101", ("3000", "1001"), ("3006", "5.2.0"), ("3010", "20260714093012"),
             ("3011", "2"), ("3101", "Müller-Lüdenscheidt"), ("3102", "Werner"),
             ("3103", "19580312"), ("3119", VID), ("3107", "Badstr."), ("3109", "1"),
             ("3112", "50859"), ("3114", "D"), ("3113", "Köln"), ("3116", "38"),
             ("3108", "1"), ("3110", "M"), ("4101", "32026"), ("4104", "27123"),
             ("4106", "00"), ("4109", "20260714"), ("4133", "20250101"),
             ("4111", "104212505"), ("4134", "Beispiel BKK"), ("4131", "00"),
             ("4132", "00"), ("4121", "0"), ("4122", "00"), ("4239", "00"),
             ("5000", "20260714"), ("5001", "03000"), *leistung,
             ("5001", "03230"), *leistung,
             ("5000", "20260902"), ("5001", "03230"), *leistung,
             ("6001", "J06.9"), ("6003", "G")),
        # Fall 2: Ueberweisung (Mit-/Weiterbehandlung)
        satz("0102", ("3000", "1002"), ("3101", "Klöbner"), ("3102", "Erwin"),
             ("3103", "19600229"), ("3107", "Seestr."), ("3109", "7"), ("3112", "10115"),
             ("3113", "Berlin"), ("3108", "3"), ("3110", "M"), ("4101", "32026"),
             ("4104", "27123"), ("4106", "00"), ("4111", "104212505"), ("4131", "00"),
             ("4132", "00"), ("4121", "0"), ("4122", "00"), ("4218", "271111100"),
             ("4242", "111111" + pz49("111111") + "01"), ("4220", "Radiologie"), ("4239", "24"),
             ("5000", "20260805"), ("5001", "34210"), *leistung,
             ("6001", "I10.90"), ("6003", "G"), ("3673", "E11.90"), ("3674", "G")),
        # Fall 3: Belegaerztliche Behandlung
        satz("0103", ("3000", "1003"), ("3101", "Mustermann"), ("3102", "Erika"),
             ("3103", "19700101"), ("3112", "50667"), ("3113", "Köln"), ("3108", "1"),
             ("3110", "W"), ("4101", "32026"), ("4104", "27123"), ("4106", "00"),
             ("4111", "104212505"), ("4131", "00"), ("4132", "00"), ("4121", "0"),
             ("4122", "00"), ("4233", "2026091420260918"), ("4239", "30"),
             ("5000", "20260914"), ("5001", "01436"), *leistung,
             ("6001", "M54.5"), ("6003", "A")),
        # Fall 4: Notfall
        satz("0104", ("3000", "1004"), ("3101", "Hoppenstedt"), ("3102", "Dicki"),
             ("3103", "20190601"), ("3112", "53111"), ("3113", "Bonn"), ("3108", "3"),
             ("3110", "M"), ("4101", "32026"), ("4104", "27123"), ("4106", "00"),
             ("4111", "104212505"), ("4131", "00"), ("4132", "00"), ("4121", "0"),
             ("4122", "00"), ("4239", "41"), ("4243", "Hausarztpraxis Beispiel"),
             ("5000", "20260924"), ("5001", "01210"), *leistung,
             ("6001", "R50.9"), ("6003", "G")),
        satz("adt9"),
        satz("con9"),
    ]


def zeile(fk, inhalt, enc="iso-8859-15"):
    rumpf = (fk + inhalt).encode(enc)
    return f"{len(rumpf) + 5:03d}".encode() + rumpf + b"\r\n"


def datei(saetze):
    return b"".join(zeile(fk, inh) for s in saetze for fk, inh in s)


# --- Varianten -------------------------------------------------------

def satz_von(saetze, art):
    return next(s for s in saetze if s[0][1] == art)


def setze(art, fk, neu, alt=None):
    """Ersetzt im ersten Satz der Satzart den Inhalt des (ersten passenden) Feldes."""
    s = gueltige_datei()
    st = satz_von(s, art)
    i = next(k for k, (f, v) in enumerate(st) if f == fk and (alt is None or v == alt))
    st[i] = (fk, neu)
    return datei(s)


def entferne(art, fk):
    s = gueltige_datei()
    st = satz_von(s, art)
    st.remove(next(x for x in st if x[0] == fk))
    return datei(s)


def fuege_ein(art, nach_fk, fk, wert):
    s = gueltige_datei()
    st = satz_von(s, art)
    i = next(k for k, (f, _) in enumerate(st) if f == nach_fk)
    st.insert(i + 1, (fk, wert))
    return datei(s)


def aendere(art, fn, nr=0):
    """Wendet fn(satz) auf den nr-ten Satz der Satzart an (Satz = Liste von (FK, Inhalt))."""
    s = gueltige_datei()
    st = [x for x in s if x[0][1] == art][nr]
    fn(st)
    return datei(s)


def ersetze_feld(fk, neu, alt=None):
    def fn(st):
        i = next(k for k, (f, v) in enumerate(st) if f == fk and (alt is None or v == alt))
        st[i] = (fk, neu)
    return fn


def entferne_feld(*fks):
    def fn(st):
        st[:] = [x for x in st if x[0] not in fks]
    return fn


def einfuegen(nach_fk, *felder, vor=False):
    def fn(st):
        i = next(k for k, (f, _) in enumerate(st) if f == nach_fk)
        st[i + (0 if vor else 1):i + (0 if vor else 1)] = list(felder)
    return fn


def alles(*fns):
    def fn(st):
        for f in fns:
            f(st)
    return fn


def roh(nr, fn):
    """Wendet fn auf die Rohbytes der Zeile nr (1-basiert) an."""
    zeilen = [z + b"\r\n" for z in datei(gueltige_datei()).split(b"\r\n")[:-1]]
    zeilen[nr - 1] = fn(zeilen[nr - 1])
    return b"".join(zeilen)


def zeilennummer(art, fk):
    n = 0
    for st in gueltige_datei():
        for f, _ in st:
            n += 1
            if st[0][1] == art and f == fk:
                return n
    raise KeyError((art, fk))


def ohne_satz(art):
    return datei([s for s in gueltige_datei() if s[0][1] != art])


def vertausche_saetze(a, b):
    s = gueltige_datei()
    i, j = [k for k, st in enumerate(s) if st[0][1] in (a, b)][:2]
    s[i], s[j] = s[j], s[i]
    return datei(s)


def main():
    ZIEL.mkdir(exist_ok=True)
    for alt in ZIEL.glob("*.kvdt"):
        alt.unlink()
    z_name = zeilennummer("0101", "3101")
    dateien = {
        "00_gueltig.kvdt": datei(gueltige_datei()),
        # Grundlagen (Kap. 1)
        "k1_laengenangabe_falsch.kvdt": roh(z_name, lambda z: b"099" + z[3:]),
        "k1_zeilenende_nur_lf.kvdt": roh(z_name, lambda z: z[:-2] + b"\n"),
        "k1_leeres_feld.kvdt": roh(z_name, lambda z: b"0093101\r\n"),
        "k1_utf8_gespeichert.kvdt": datei(gueltige_datei()).decode("iso-8859-15").encode("utf-8"),
        "k1_feldlaenge.kvdt": setze("0101", "3101", "X" * 46),
        "k1_feldtyp_n.kvdt": setze("0101", "4104", "27A23"),
        "k1_datum_kalender.kvdt": setze("0101", "5000", "20260931", "20260902"),
        "k1_mussfeld_fehlt.kvdt": entferne("0101", "3110"),
        "k1_feld_nicht_zulaessig.kvdt": fuege_ein("0101", "4239", "4243", "Dr. X"),
        "k1_reihenfolge.kvdt": fuege_ein("0101", "6003", "3108", "1"),
        "k1_leistungstage_absteigend.kvdt": setze("0101", "5000", "20260701", "20260902"),
        # Anordnung der Saetze (Kap. 2.1, 3.3)
        "k1_hierarchie_mussfeld.kvdt": aendere("0104", entferne_feld("6003")),
        "k1_leistungstag_ohne_gnr.kvdt": aendere("0104", einfuegen("5099", ("5000", "20260925"))),
        "k1_vorkommen_doppelt.kvdt": aendere("0104", einfuegen("5001", ("5002", "Abstrich"), ("5002", "Abstrich"))),
        # letzte GNR in Fall 1 ohne 5098/5099 - Regeln 837/859 gelten je GNR
        "r837_je_gnr.kvdt": aendere("0101", lambda st: st.__delitem__(slice(len(st) - 4, len(st) - 2))),
        "k2_besa_vor_con0.kvdt": vertausche_saetze("con0", "besa"),
        "k2_con9_fehlt.kvdt": ohne_satz("con9"),
        "k2_adt9_fehlt.kvdt": ohne_satz("adt9"),
        # Container-Regeln
        "r743_rvsa_fehlt.kvdt": ohne_satz("rvsa"),
        "r050_lanr_pruefziffer.kvdt": setze("besa", "0212", "123456001"),
        "r819_bsnr_kv_kennung.kvdt": setze("besa", "0201", "041234500"),
        "r858_ti_wert_fehlt.kvdt": aendere("besa", lambda st: st.__delitem__(slice(
            st.index(("0225", "1")), st.index(("0225", "1")) + 2))),
        "r182_zeichensatz.kvdt": setze("con0", "9106", "2"),
        # ADT-Regeln
        "r016_quartal_format.kvdt": setze("0101", "4101", "52026"),
        "r021_geburtsdatum.kvdt": setze("0101", "3103", "19581312"),
        "r022_icd_format.kvdt": setze("0101", "6001", "J06.9!"),
        "r054_versicherten_id.kvdt": setze("0101", "3119", VID[:-1] + str((int(VID[-1]) + 1) % 10)),
        "r324_leistung_ausserhalb_quartal.kvdt": setze("0101", "5000", "20261001", "20260902"),
        "r331_scheinuntergruppe.kvdt": setze("0101", "4239", "24"),
        "r328_ueberweiser_fehlt.kvdt": aendere("0102", entferne_feld("4218", "4242")),
        "r354_stationaer_fehlt.kvdt": entferne("0103", "4233"),
        "r486_keine_diagnose.kvdt": datei([[x for x in st if not (st[0][1] == "0104" and x[0] in ("6001", "6003"))]
                                         for st in gueltige_datei()]),
        "r497_gnr_format.kvdt": setze("0101", "5001", "3000", "03000"),
        "r533_geschlecht.kvdt": setze("0101", "3110", "F"),
        "r715_lanr_nicht_in_besa.kvdt": setze("0104", "5099", "654321" + pz49("654321") + "01"),
        "r774_wop_fehlt.kvdt": entferne("0101", "3116"),
        "r776_versicherten_id_fehlt.kvdt": entferne("0101", "3119"),
        "r813_alte_version.kvdt": setze("adt0", "9212", "ADT0426.01"),
        "r856_z017_nicht_gesichert.kvdt": datei([[("6001", "Z01.7") if (st[0][1] == "0104" and x[0] == "6001")
                                                else ("6003", "V") if (st[0][1] == "0104" and x[0] == "6003") else x
                                                for x in st] for st in gueltige_datei()]),
        # --- weitere Regeln (Kontext)
        "r307_cdm_fehlt.kvdt": aendere("0101", entferne_feld("3006")),
        "r308_cdm_zu_neu.kvdt": aendere("0101", ersetze_feld("3006", "5.3.0")),
        "r309_errorcode.kvdt": aendere("0101", einfuegen("3011", ("3012", "99999"))),
        "r313_geburt_nach_leistung.kvdt": aendere("0103", ersetze_feld("3103", "20260915")),
        "r315_versicherungsschutz_ende.kvdt": aendere("0101", einfuegen("4133", ("4110", "20260801"))),
        "r319_erstveranlasser_gleich.kvdt": aendere("0102", alles(
            ersetze_feld("4239", "27"), entferne_feld("4220"),
            einfuegen("4101", ("4102", "20260801")), einfuegen("4122", ("4205", "Labor")),
            einfuegen("4218", ("4217", "271111100"), ("4241", LANR), vor=True), einfuegen("4242", ("4221", "1")))),
        "r320_ueberweisung_an_fehlt.kvdt": aendere("0102", entferne_feld("4220")),
        "r363_gueltigkeit.kvdt": aendere("0104", einfuegen("4122", ("4125", "2026090120260920"))),
        "r404_kurativ_ohne_sug.kvdt": aendere("0102", einfuegen("4220", ("4221", "1"))),
        "r406_ausstellung_nach_erstellung.kvdt": aendere("0101", einfuegen("4101", ("4102", "20261020"))),
        "r432_ausnahmeindikation.kvdt": aendere("0102", einfuegen("4220", ("4229", "32001"))),
        "r479_keine_plz.kvdt": aendere("0104", entferne_feld("3112")),
        "r706_seitenlokalisation.kvdt": aendere("0101", einfuegen("5098", ("5035", "5-301.1"), ("5041", "B"), vor=True)),
        "r725_lanr_format.kvdt": aendere("0103", ersetze_feld("5099", "123456001")),
        "r738_teilabrechnung.kvdt": aendere("adt0", einfuegen("9115", ("9260", "02"), ("9261", "03"))),
        "r746_beleg_mitbehandlung.kvdt": aendere("0103", ersetze_feld("4239", "31")),
        "r762_lanr_obsolet.kvdt": aendere("0104", ersetze_feld("5099", "888888800")),
        "r770_gen_doku.kvdt": aendere("0101", einfuegen("5099", ("5001", "11511"), ("5098", BSNR), ("5099", LANR))),
        "r775_versicherungsbeginn.kvdt": aendere("0101", entferne_feld("4133")),
        "r778_ktab.kvdt": aendere("0101", ersetze_feld("4131", "07")),
        "r784_laendercode.kvdt": aendere("0101", entferne_feld("3114")),
        "r837_859_arzt_ort.kvdt": aendere("0104", entferne_feld("5098", "5099")),
        "r853_gop_doppelt.kvdt": aendere("0101", einfuegen("4239", ("4234", "1"), ("4235", "20260101"), ("4252", "25"),
                                                     ("4253", "35401"), ("4254", "3"), ("4253", "35401"), ("4254", "2"), vor=True)),
        "r870_vermittlung.kvdt": aendere("0101", einfuegen("4239", ("4103", "3"), ("4115", "20260710"))),
        "r875_lanr_doppelt.kvdt": aendere("besa", einfuegen("0211", ("0212", LANR), ("0211", "Muster"))),
        "r886_terminvermittlung_alt.kvdt": aendere("0102", einfuegen("4239", ("4103", "3"), ("4115", "20260501"))),
        "r820_erstveranlasser_format.kvdt": aendere("0102", alles(ersetze_feld("4239", "27"), entferne_feld("4220"),
            einfuegen("4101", ("4102", "20260801")), einfuegen("4122", ("4205", "Labor")),
            einfuegen("4218", ("4217", "041111100"), ("4241", LANR), vor=True), einfuegen("4242", ("4221", "1")))),
        "r821_ueberweiser_format.kvdt": aendere("0102", ersetze_feld("4218", "041111100")),
        "r850_psychotherapie.kvdt": aendere("0101", einfuegen("4239", ("4234", "1"), ("4235", "20260101"), ("4252", "25"), vor=True)),
        "r895_eeb.kvdt": aendere("0101", einfuegen("4109", ("4112", "1"))),
        "r9132_paket_fehlt.kvdt": aendere("con0", ersetze_feld("9132", "3")),
        "r740_ringversuch.kvdt": aendere("rvsa", ersetze_feld("0300", "1")),
        "r872_konnektor.kvdt": aendere("besa", entferne_feld("0227")),
        "r059_asv_team.kvdt": aendere("0101", einfuegen("5099", ("5100", "001234560"))),
        "r496_goae_format.kvdt": aendere("0101", ersetze_feld("4121", "3")),
        "r052_pruefnummer.kvdt": aendere("adt0", ersetze_feld("0105", "X/1401/36/id9")),
        "r860_dauerdiagnose.kvdt": aendere("0102", ersetze_feld("3673", "Z01.7")),
        "r876_onlinepruefung.kvdt": aendere("0101", entferne_feld("4109")),
        "r005_uhrzeit.kvdt": aendere("0101", einfuegen("5001", ("5006", "2460"))),
        "r710_sachkosten.kvdt": aendere("0101", einfuegen("5001", ("5012", "1000000"), ("5011", "Material"))),
        "r894_implantat.kvdt": aendere("0101", einfuegen("5001", ("5005", "002"), ("5050", "ABCDEFGHIJ"))),
        "r707_multiplikator.kvdt": aendere("0101", einfuegen("5001", ("5005", "002"), ("5042", "80"), ("5043", "1"))),
        "r891_gensymbol.kvdt": aendere("0101", einfuegen("5001", ("5077", "999999"))),
        "r744_auftrag.kvdt": aendere("0102", ersetze_feld("4239", "21")),
        "r175_satzart_unbekannt.kvdt": aendere("0104", ersetze_feld("8000", "0105")),
        "k1_feld_vor_satz.kvdt": b"0179103" + b"20261015\r\n" + datei(gueltige_datei()),
    }
    for name, inhalt in dateien.items():
        (ZIEL / name).write_bytes(inhalt)
    print(f"{len(dateien)} Dateien in {ZIEL} geschrieben.")


if __name__ == "__main__":
    main()
