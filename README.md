# May the Forth be with you – KVDT-Prüfer in Forth

Ein Prüfer für KVDT-Abrechnungsdateien, geschrieben in ANS-Forth, auf Basis der
**KBV-Datensatzbeschreibung KVDT, Version 6.08 vom 24. August 2026**.

Umfang: **Container-Sätze** (`con0`, `besa`, `rvsa`, `con9`) und das
**ADT-Datenpaket** (`adt0`, `0101`–`0104`, `adt9`). KADT-, SADT- und
Hybrid-DRG-Pakete werden erkannt und übersprungen (Hinweis), aber nicht geprüft.

> **Kein Ersatz für das KBV-Prüfmodul.** Viele Regeln brauchen externe Stammdateien
> (EBM, SDICD, Kostenträger, PLZ, …), die hier nicht vorliegen – siehe unten.
> Alle Testdaten sind frei erfunden.

## Starten

1. gforth installieren (Windows-Installer unter <https://gforth.org>), sodass
   `gforth` im `PATH` liegt.
2. Testsuite: `tests.bat` (oder `gforth tests.fs`)
3. Datei prüfen: `pruefe.bat testdaten\00_gueltig.kvdt`

Beispiel – eine gültige Datei, die jemand im Editor als UTF-8 gespeichert hat:

```text
Zeile    13  FK 0216  F K1.6.1  Laengenangabe <> tatsaechliche Laenge (Inhalt + 9)  <KÃ¶ln>
Zeile    13  FK 0216  W K1.8    Zeichenfolge sieht nach UTF-8 aus - Datei muss ISO 8859-15 sein  <KÃ¶ln>
...
Meldungen: 6 Fehler, 6 Warnungen, 0 Hinweise
Ergebnis:  FEHLERHAFT (mit Warnungen)
```

Jede Meldung: Zeile, Feldkennung, Prüfstatus (**F**ehler / **W**arnung / **I**nfo
wie in der Regeltabelle), Regelnummer der KBV (`R324`) bzw. Kapitel der
Grundlagen (`K1.6.1`), Text und Feldinhalt.

## Was geprüft wird

| Bereich | Quelle in der Doku |
| --- | --- |
| Feldaufbau: Längenangabe = Inhalt + 9, CR LF, keine leeren Felder | Kap. 1.6.1 |
| Zeichensatz ISO 8859-15 (Steuerzeichen, Verdacht auf UTF-8) | Kap. 1.8 |
| Länge und Typ (`n`, `a`, `d`) **jedes** Feldes, keine führenden Nullen bei variabler Länge, keine Rand-Leerzeichen | Feldtabellen 2.3.1 / 3.5.1 |
| zulässige Felder je Satzart, Reihenfolge und **Hierarchie** (welches Feld zu welchem übergeordneten Feld gehört), Vorkommen 1/n je übergeordnetem Feld, Mussfelder (M) und an das übergeordnete Feld gebundene Mussfelder (m ohne Bedingung), Leistungstage aufsteigend | Satztabellen 2.2 / 3.4, Kap. 1.5 |
| Anordnung der Sätze in der Datei | Kap. 2.1 / 3.3 |
| **168 von 204 Regeln** der Container- und ADT-Regeltabelle | Kap. 2.3.2 / 3.5.2 |

### Nicht geprüfte Regeln (36) und warum

- **Stammdateien nötig:** 201, 202, 203\*, 204, 210, 213 (Kassen-/Anbieterdatei),
  222, 223 (HGNC, OPS), 478, 783 (SDPLZ), 489–492, 728, 729, 761, 817 (SDICD),
  701–705, 868, 888–890, 892, 901 (EBM-Stammdatei), 734, 763 (Kostenträger),
  211, 749 (Schlüsseltabelle S_NVV_RV_Zertifikat)
- **Nur für die KV-Software:** 999\*
- **Noch offen:** 840 (regulärer Ausdruck für 0132 bei KV 94–96)
- **Von keinem Feld referenziert:** 100

## Aufbau

| Datei | Inhalt |
| --- | --- |
| `kvdt.fs` | Prüf-Engine und alle Regeln (ANS-Forth, ca. 1200 Zeilen) |
| `kvdt-tabellen.fs` | **Generiert** aus der Original-PDF: Feldtabelle (200 Felder) und 10 Satztabellen mit Hierarchie-Ebenen |
| `werkzeug/tabellen_erzeugen.py` | Liest `spec/KBV_ITA_VGEX_Datensatzbeschreibung_KVDT.pdf` (Spaltenpositionen) und erzeugt `kvdt-tabellen.fs` |
| `tests.fs` | 55 Unit-Tests der Formatregeln + 85 Dateitests |
| `testdaten/` | Eine gültige Datei und 85 gezielt fehlerhafte Varianten (Endung `.kvdt`, weil Windows `CON` als Gerätenamen reserviert – echte Abrechnungsdateien heißen `*.CON`, die prüft der Prüfer genauso) |
| `werkzeug/testdaten_erzeugen.py` | Erzeugt die Testdaten neu (ISO 8859-15, CR LF) |
| `pruefe.fs`, `pruefe.bat`, `tests.bat` | Aufruf über die Kommandozeile (gforth) |

Neue Version der Datensatzbeschreibung? PDF nach `spec/` legen und

```text
pip install pdfplumber
python werkzeug/tabellen_erzeugen.py spec/KBV_ITA_VGEX_Datensatzbeschreibung_KVDT.pdf
```

Längen, Typen, Mussfelder, Reihenfolge und Hierarchie kommen dann direkt aus
der neuen Doku. Die Hierarchie-Ebene liest der Generator aus der x-Position des
Eintrags in der Spalte „Vorkommen 1 2 3 4 5“ – deshalb die PDF und nicht eine
Textfassung. Zur Kontrolle wurden die Tabellen einmal unabhängig aus der
Textfassung und aus der PDF extrahiert: Feldkennungen, Längen, Typen, Vorkommen
und Feldarten stimmen in beiden Fällen überein.

## Auslegungen – hier lohnt sich der Blick eines Experten

Die Textfassung des PDFs und manche Formulierungen lassen Spielraum. Diese
Entscheidungen wurden getroffen und sollten von jemandem geprüft werden, der die
Zertifizierung kennt:

1. **„entweder … oder“** (Regeln 837, 859, 844, 845): als *mindestens eines*
   umgesetzt. Bei 836/857 steht ausdrücklich „es können auch beide vorkommen“,
   bei diesen vier nicht – ist dort *genau eines* gemeint?
2. **Regeln 899/900** (4214 gegen 5000): bei mehreren Leistungstagen wird gegen
   den *frühesten* geprüft (wie es Regel 886 ausdrücklich sagt).
3. **Regel 776:** In der Spalte Prüfstatus steht in der Doku „776“ statt F/W/I –
   hier als **F** angenommen.
4. **Bedingte Mussfelder (m):** Steht in der Spalte „Bedingung“ nichts, wird das
   Feld als an das übergeordnete Feld gebunden gewertet (Kap. 1.5.3) und je
   Instanz verlangt – z. B. 6003 zu jedem 6001, 5001 zu jedem 5000, 4242 zu 4218.
   Bedingungen in Textform („falls eGK eingelesen …“) werden nicht geprüft.
5. **Regeln 837/859** gelten je GNR, weil 5098/5099/5101/5102 laut Satztabelle
   unter 5001 hängen. Der Anforderungskatalog (K2: „die LANR in FK 5099 oder die
   Pseudo-LANR in Feld 5101“) spricht eher für *genau eines* – siehe Punkt 1.

## Was beim Lesen der Doku aufgefallen ist

- Das **Beispiel für FK 0105** in der Feldtabelle („X/1401/36/id9“) verletzt die
  eigene Formatregel 052 und die Feldlänge 15–17.
- **FK 5076** (Rechnungsnummer): Feldtabelle sagt ≤ 20 Zeichen, das
  Feldverzeichnis (Kap. 7) ≤ 60. Geprüft wird nach der Feldtabelle.
- Regel 776: Prüfstatus fehlt (siehe oben).
- Und in eigener Sache: Die erste Version der gültigen Testdatei hatte in der
  Überweisung 4218 ohne die LANR des Überweisers (4242). Erst die Hierarchie aus
  der PDF hat gezeigt, dass 4242 unter 4218 ein Mussfeld ist.

## Vorschlag für die Vorführung

1. `tests.bat` – 140 Tests, alle grün.
2. `pruefe.bat testdaten\k1_utf8_gespeichert.kvdt` – das Umlaut-Problem kennt jeder.
3. `kvdt-tabellen.fs` zeigen: 200 Felder und 10 Satztabellen mit eingerückter
   Hierarchie, direkt aus der KBV-PDF generiert.
4. **Der eigentliche Moment:** Er sucht sich eine Regel aus der Liste der
   Auslegungen (oder eine der nicht umgesetzten) und schaut zu, wie sie live
   geklärt, umgesetzt und mit einer neuen Testdatei abgedeckt wird.
