# May the Forth be with you – ein KVDT-Prüfer in Forth

Ein kleiner Prüfer für Dateien im KVDT-Zeilenformat, geschrieben in ANS-Forth.
Er liest eine Abrechnungsdatei Zeile für Zeile, prüft Aufbau, Satzartfolge,
Pflichtfelder und eine Reihe von Plausibilitätsregeln und meldet Fehler mit
Zeile, Feldkennung und Regel – ähnlich wie man es vom KBV-Prüfmodul kennt.

> **Ehrlicher Hinweis:** Das ist ein **Demo-Regelwerk**, angelehnt an KVDT.
> Es ersetzt nicht das KBV-Prüfmodul und ist **nicht** gegen die aktuelle
> KVDT-Datensatzbeschreibung abgeglichen. Satzartfolge, Pflichtfelder und
> Regeln sind bewusst vereinfacht. Wer die Datensatzbeschreibung kennt, wird
> Abweichungen finden – und genau das ist die Einladung (siehe unten).
> Alle Testdaten sind frei erfunden.

## Starten

1. gforth installieren (Windows-Installer unter <https://gforth.org>), sodass
   `gforth` im `PATH` liegt.
2. Testsuite ausführen: `tests.bat` (oder `gforth tests.fs`)
3. Eine Datei prüfen: `pruefe.bat testdaten\00_gueltig.kvdt`

Interaktiv geht es auch:

```bash
gforth kvdt.fs
s" testdaten/08_datum_ungueltig.kvdt" pruefe
```

Beispielausgabe:

```text
Zeile     9  FK 3101  R02  Laengenangabe 028, tatsaechlich 030  <MÃ¼ller-LÃ¼denscheidt>
Zeile    21  FK 3101  R02  Laengenangabe 016, tatsaechlich 017  <KlÃ¶bner>

Datei:   testdaten/19_als_utf8_gespeichert.kvdt
Zeilen:  43   Saetze: 7   Faelle: 3
Ergebnis: FEHLERHAFT, 2 Fehler
```

(Hier wurde eine gültige ISO-8859-15-Datei versehentlich als UTF-8 gespeichert –
jeder Umlaut kostet ein Byte mehr, und die Längenangaben stimmen nicht mehr.)

## Regeln

| Regel | Prüfung |
| --- | --- |
| R01 | Zeile mindestens 3 (Länge) + 4 (Feldkennung) + 1 (Inhalt) Zeichen |
| R02 | Längenangabe numerisch und gleich der tatsächlichen Zeilenlänge inkl. CRLF |
| R03 | Feldkennung 4-stellig numerisch |
| R04 | Zeilenende CR LF |
| R05 | Satzartfolge `con0` → `besa` → `rvsa`+ → `0101`–`0104`* → `con9`, bekannte Satzart, Datei endet mit `con9` |
| R06 | Kein Feld vor dem ersten Satz (8000) |
| R07 | Pflichtfelder: `con0` 9103 · `besa` 0201 · `rvsa` 0212 · Fall 3101, 3102, 3103, 3110, 4101 |
| R08 | Datumsfelder (3103, 5000, 9103) gültig im Format TTMMJJJJ inkl. Schaltjahre |
| R09 | Leistungstag (5000) nicht vor Geburtsdatum (3103) |
| R10 | Abrechnungsquartal (4101) im Format QJJJJ, Q = 1–4 |
| R11 | Leistungstag liegt im Abrechnungsquartal |
| R12 | ICD-10-Code (6001): `A99`, `A99.9` oder `A99.99`, optional `!`, `*`, `+` |
| R13 | Diagnosensicherheit (6003): A, G, V oder Z |
| R14 | LANR (0212): 9 Ziffern, Prüfziffer an Stelle 7 (Gewichte 4/9 über Stellen 1–6) |
| R15 | BSNR (0201): 9 Ziffern |
| R16 | GOP (5001): 5 Ziffern, optional ein Großbuchstabe |
| R17 | Geschlecht (3110): M, W, X oder D |
| R18 | Leistung (5001) nur nach einem Leistungstag (5000) im selben Fall |

## Dateien

| Datei | Inhalt |
| --- | --- |
| `kvdt.fs` | Der Prüfer (ANS-Forth, ca. 330 Zeilen) |
| `tests.fs` | 15 Unit-Tests einzelner Regeln + 21 Dateitests |
| `pruefe.fs` | Kommandozeilen-Einstieg (gforth-spezifisch wegen `next-arg`) |
| `pruefe.bat`, `tests.bat` | Startskripte für Windows |
| `testdaten/` | Eine gültige Datei und je eine gezielt kaputte Datei pro Regel |
| `werkzeug/testdaten_erzeugen.py` | Erzeugt die Testdaten neu (ISO-8859-15, CRLF) |

## Vorschlag für die Vorführung

1. **Zeigen, dass es läuft:** `tests.bat` – 36 Tests, alle grün.
2. **Eine Fehlerdatei prüfen:** z. B. `19_als_utf8_gespeichert.kvdt` – das
   Umlaut-Problem kennt jeder, der schon einmal xDT-Dateien angefasst hat.
3. **Den Code zeigen:** `kvdt.fs` ist kommentiert und in kleine Worte zerlegt,
   klassischer Forth-Stil. Er soll ruhig nach Stilbrüchen suchen.
4. **Der eigentliche Moment:** Er gibt eine Regel vor, die er aus der
   KVDT-Zertifizierung kennt, und schaut zu, wie sie live umgesetzt, mit einer
   neuen Testdatei abgedeckt und die Suite wieder grün wird.

Ideen, falls ihm spontan nichts einfällt:

- Leistungstag darf nicht nach dem Erstellungsdatum (9103) liegen.
- Eine Patientennummer (3000) darf in einer Datei nur einmal vorkommen.
- Seitenlokalisation (6004) nur mit erlaubten Werten.
- Statistik am Ende: Anzahl Fälle pro Satzart und häufigste GOPs.
- Eine Regel, die in der echten Datensatzbeschreibung anders steht als hier –
  und die er selbst korrigieren lässt.
