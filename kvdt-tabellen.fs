\ ===================================================================
\  kvdt-tabellen.fs  --  GENERIERT von werkzeug/tabellen_erzeugen.py
\  Quelle: KBV Datensatzbeschreibung KVDT (PDF), Version 6.08 vom 24. August 2026
\  Nicht von Hand bearbeiten - Generator anpassen und neu erzeugen.
\ ===================================================================

\ Feldtabelle:  FK  Typ  Laengenart  a  b  c  feld" Bezeichnung"
\   Typ: 110 = n(umerisch)  97 = a(lphanumerisch)  100 = d(atum)
\   Laengenart: 0 = fest (a, b oder c)  1 = von a bis b  2 = hoechstens a
create feldtabelle
   102  97 2  60  0 0 feld" Softwareverantwortlicher (SV)"
   103  97 2  60  0 0 feld" Software"
   105  97 1  15 17 0 feld" KBV-Prüfnummer"
   111  97 2  60  0 0 feld" E-Mail-Adresse des SV"
   121  97 2  60  0 0 feld" Straße des SV"
   122  97 2   7  0 0 feld" PLZ des SV"
   123  97 2  60  0 0 feld" Ort des SV"
   124  97 2  60  0 0 feld" Telefonnummer des SV"
   125  97 2  60  0 0 feld" Telefaxnummer des SV"
   126  97 2  60  0 0 feld" Regionaler Systembetreuer (SB)"
   127  97 2  60  0 0 feld" Straße des SB"
   128  97 2   7  0 0 feld" PLZ des SB"
   129  97 2  60  0 0 feld" Ort des SB"
   130  97 2  60  0 0 feld" Telefonnummer des SB"
   131  97 2  60  0 0 feld" Telefaxnummer des SB"
   132  97 2  60  0 0 feld" Release-Stand der Software"
   201 110 0   9  0 0 feld" Betriebs- (BSNR) oder Nebenbetriebsstättennummer (NBSNR)"
   203  97 2  60  0 0 feld" (N)BSNR-/Krankenhaus-Bezeichnung"
   205  97 2  60  0 0 feld" Straße der (N)BSNR-/ Krankenhaus- Adresse"
   208  97 2  60  0 0 feld" Telefonnummer"
   209  97 2  60  0 0 feld" Telefaxnummer"
   211  97 2  60  0 0 feld" Arztname oder Erläuterung"
   212 110 0   9  0 0 feld" Lebenslange Arztnummer (LANR)"
   213 110 0   9  0 0 feld" Krankenhaus-IK (im Rahmen der ASV-Abrechnung)"
   214 110 0   2  0 0 feld" KV-Bereich"
   215  97 2   7  0 0 feld" PLZ der (N)BSNR-/ Krankenhaus-Adresse"
   216  97 2  60  0 0 feld" Ort der (N)BSNR-/ Krankenhaus-Adresse"
   218  97 2  60  0 0 feld" E-Mail der Betriebsstätte/Praxis/Krankenhaus"
   219  97 2 100  0 0 feld" Titel des Arztes"
   220  97 2  45  0 0 feld" Arztvorname"
   221  97 2  20  0 0 feld" Namenszusatz des Arztes"
   222 110 0   9  0 0 feld" ASV-Teamnummer"
   223 110 0   9  0 0 feld" Pseudo-LANR für Krankenhausärzte im"
   224  97 2  20  0 0 feld" Produkttypversion des Konnektors"
   225 110 1   1  2 0 feld" TI-Fachanwendung / TI-Komponente"
   226 110 0   1  0 0 feld" Systemunterstützung / Ausstattung der Praxis"
   227 100 0   8  0 0 feld" Ablaufdatum des Konnektorzertifikats"
   228  97 2  60  0 0 feld" Produktname des Konnektors"
   300 110 0   1  0 0 feld" Abrechnung von (zertifikatspflichtigen) Laborleistungen"
   301 110 0   1  0 0 feld" pnSD/uu-Analysen"
   302  97 2  60  0 0 feld" Gerätetyp"
   303  97 2  60  0 0 feld" Hersteller"
   304 110 0   3  0 0 feld" Analyt-ID"
   305 110 0   1  0 0 feld" RV-Zertifikat"
  3000  97 2  20  0 0 feld" Patientennummer"
  3003  97 2  60  0 0 feld" Schein-ID"
  3006  97 1   5 11 0 feld" CDM Version"
  3010 110 0  14  0 0 feld" Datum und Uhrzeit der Onlineprüfung und -aktualisierung (Tim"
  3011 110 0   1  0 0 feld" Ergebnis der Onlineprüfung und -aktualisierung"
  3012 110 2   5  0 0 feld" Error-Code"
  3013  97 2 128  0 0 feld" Prüfziffer des Fachdienstes"
  3100  97 2  20  0 0 feld" Namenszusatz"
  3101  97 2  45  0 0 feld" Name"
  3102  97 2  45  0 0 feld" Vorname"
  3103 110 0   8  0 0 feld" Geburtsdatum"
  3104  97 2  20  0 0 feld" Titel"
  3105 110 1   6 12 0 feld" Versichertennummer"
  3107  97 2  46  0 0 feld" Straße"
  3108 110 0   1  0 0 feld" Versichertenart"
  3109  97 2   9  0 0 feld" Hausnummer"
  3110  97 0   1  0 0 feld" Geschlecht"
  3112  97 2  10  0 0 feld" PLZ"
  3113  97 2  40  0 0 feld" Ort"
  3114  97 2   3  0 0 feld" Wohnsitzlaendercode"
  3115  97 2  40  0 0 feld" Anschriftenzusatz"
  3116 110 0   2  0 0 feld" WOP"
  3119  97 0  10  0 0 feld" Versicherten_ID"
  3120  97 2  20  0 0 feld" Vorsatzwort"
  3121  97 2  10  0 0 feld" PostfachPLZ"
  3122  97 2  40  0 0 feld" PostfachOrt"
  3123  97 2   8  0 0 feld" Postfach"
  3124  97 2   3  0 0 feld" PostfachWohnsitzlaendercode"
  3673  97 0   3  5 6 feld" Dauerdiagnose (ICD-Code)"
  3674  97 0   1  0 0 feld" Diagnosensicherheit Dauerdiagnose"
  3675  97 0   1  0 0 feld" Seitenlokalisation Dauerdiagnose"
  3676  97 2  60  0 0 feld" Diagnosenerläuterung Dauerdiagnose"
  3677  97 2  60  0 0 feld" Diagnosenausnahmetatbestand Dauerdiagnosen"
  4101 110 0   5  0 0 feld" Quartal"
  4102 100 0   8  0 0 feld" Ausstellungsdatum"
  4103 110 0   1  0 0 feld" Vermittlungs-/Kontaktart"
  4104 110 0   5  0 0 feld" Abrechnungs-VKNR"
  4105  97 2  60  0 0 feld" Ergänzende Informationen zur Vermittlungs-/Kontaktart"
  4106 110 0   2  0 0 feld" Kostenträger-Abrechnungsbereich (KTAB)"
  4108  97 2  40  0 0 feld" Zulassungsnummer (mobiles Lesegerät)"
  4109 100 0   8  0 0 feld" Letzter Einlesetag der Versichertenkarte im Quartal"
  4110 100 0   8  0 0 feld" Versicherungsschutz Ende"
  4111 110 0   9  0 0 feld" Kostentraegerkennung"
  4112 110 0   1  0 0 feld" eEB vorhanden"
  4114  97 0  14  0 0 feld" Vermittlungscode"
  4115 100 0   8  0 0 feld" Tag der Terminvermittlung"
  4121 110 0   1  0 0 feld" Gebührenordnung"
  4122 110 0   2  0 0 feld" Abrechnungsgebiet"
  4123 110 0   2  0 0 feld" Personenkreis / Untersuchungskategorie"
  4124  97 2  60  0 0 feld" SKT-Zusatzangaben"
  4125 110 0  16  0 0 feld" Gültigkeitszeitraum von ... bis ..."
  4126  97 2  60  0 0 feld" SKT-Bemerkungen"
  4131  97 0   2  0 0 feld" BesonderePersonengruppe"
  4132  97 0   2  0 0 feld" DMP_Kennzeichnung"
  4133 100 0   8  0 0 feld" VersicherungsschutzBeginn"
  4134  97 2  45  0 0 feld" Kostentraegername"
  4202 110 0   1  0 0 feld" Unfall, Unfallfolgen"
  4204 110 0   1  0 0 feld" eingeschränkter Leistungsanspruch gemäß §16 Abs. 3a SGB V"
  4205  97 2  60  0 0 feld" Auftrag"
  4206 100 0   8  0 0 feld" Mutm. Tag der Entbindung"
  4207  97 2  60  0 0 feld" Diagnose/ Verdachtsdiagnose"
  4208  97 2  60  0 0 feld" Befund/Medikation"
  4209  97 2  60  0 0 feld" Zusätzliche Angaben zu Untersuchungen"
  4214 100 0   8  0 0 feld" Behandlungstag bei IVD-Leistungen"
  4217 110 0   9  0 0 feld" (N)BSNR des Erstveranlassers"
  4218 110 0   9  0 0 feld" (N)BSNR des Überweisers"
  4219  97 2  60  0 0 feld" Überweisung von anderen Ärzten"
  4220  97 2  60  0 0 feld" Überweisung an"
  4221 110 0   1  0 0 feld" Kurativ / Präventiv / ESS / bei belegärztlicher Behandlung"
  4225 110 0   9  0 0 feld" ASV-Teamnummer des Erstveranlassers"
  4226 110 0   9  0 0 feld" ASV-Teamnummer des Überweisers"
  4229 110 0   5  0 0 feld" Ausnahmeindikation"
  4233 110 0  16  0 0 feld" Stationäre Behandlung von... bis..."
  4234 110 0   1  0 0 feld" anerkannte Psychotherapie"
  4235 100 0   8  0 0 feld" Datum des Anerkennungsbescheides"
  4236 110 0   1  0 0 feld" Abklärung somatischer Ursachen vor Aufnahme einer Psychother"
  4239 110 0   2  0 0 feld" Scheinuntergruppe"
  4241 110 0   9  0 0 feld" Lebenslange Arztnummer (LANR) des Erstveranlassers"
  4242 110 0   9  0 0 feld" Lebenslange Arztnummer des Überweisers"
  4243  97 2  60  0 0 feld" Weiterbehandelnder Arzt"
  4247 100 0   8  0 0 feld" Antragsdatum (des Anerkennungsescheides)"
  4248 110 0   9  0 0 feld" Pseudo-LANR (für Krankenhausärzte im Rahmen der ASV-Abrechnu"
  4249 110 0   9  0 0 feld" Pseudo-LANR (für Krankenhausärzte im Rahmen der ASV-Abrechnu"
  4250 110 0   1  0 0 feld" Kombinationsbehandlung aus Einzelund Gruppentherapie"
  4251 110 0   1  0 0 feld" Durchführungsart der Kombinationsbehandlung"
  4252 110 2   3  0 0 feld" Gesamtanzahl bewilligter Therapieeinheiten für den Versicher"
  4253  97 0   5  6 0 feld" Bewilligte GOP für den Versicherten"
  4254 110 2   3  0 0 feld" Anzahl der abgerechneten GOPen für den Versicherten"
  4255 110 2   3  0 0 feld" Gesamtanzahl bewilligter Therapieeinheiten für die Bezugsper"
  4256  97 0   5  6 0 feld" Bewilligte GOP für die Bezugsperson"
  4257 110 2   3  0 0 feld" Anzahl der abgerechneten GOPen für die Bezugsperson"
  4299 110 0   9  0 0 feld" Lebenslange Arztnummer (LANR) des Vertragspsychotherapeuten"
  5000 100 0   8  0 0 feld" Leistungstag"
  5001  97 2   9  0 0 feld" Gebührennummer (GNR)"
  5002  97 2  60  0 0 feld" Art der Untersuchung"
  5003 110 0   9  0 0 feld" (N)BSNR des vermittelten Facharztes"
  5005 110 0   3  0 0 feld" Multiplikator"
  5006 110 0   4  0 0 feld" Um-Uhrzeit"
  5008 110 2   3  0 0 feld" DKM"
  5009  97 2  60  0 0 feld" freier Begründungstext"
  5010  97 2  60  0 0 feld" Chargennummer"
  5011  97 2  60  0 0 feld" Sachkosten-Bezeichnung"
  5012 110 2  10  0 0 feld" Sachkosten / Materialkosten in Cent"
  5013 110 0   3  0 0 feld" Prozent der Leistung"
  5015  97 2  60  0 0 feld" Organ"
  5016  97 2  60  0 0 feld" Name des Arztes"
  5017  97 2  60  0 0 feld" Besuchsort bei Hausbesuchen"
  5018  97 0   2  0 0 feld" Zone bei Besuchen"
  5019  97 2  60  0 0 feld" Erbringungsort / Standort des Gerätes"
  5020 110 0   1  0 0 feld" Wiederholungsuntersuchung"
  5021 110 0   4  0 0 feld" Jahr der letzten Krebsfrüherkennungs-untersuchung"
  5023  97 0   1  0 0 feld" GO-Nummern-Zusatz"
  5024  97 0   1  0 0 feld" GNR-Zusatzkennzeichen für poststationär erbrachte Leistungen"
  5025 100 0   8  0 0 feld" Aufnahmedatum"
  5026 100 0   8  0 0 feld" Entlassungsdatum"
  5034 100 0   8  0 0 feld" OP-Datum"
  5035  97 2   8  0 0 feld" OP-Schlüssel"
  5036  97 0   5  6 0 feld" GNR als Begründung"
  5037 110 2   3  0 0 feld" Gesamt-Schnitt-Naht-Zeit (Minuten)"
  5038  97 2  60  0 0 feld" Komplikation"
  5040  97 2   8  0 0 feld" Patientennummer der eDokumentation Hautkrebs-Screening"
  5041  97 0   1  0 0 feld" Seitenlokalisation OPS"
  5042 110 2   5  0 0 feld" Mengenangabe Kontrast-/Arzneimittel"
  5043 110 0   1  0 0 feld" Maßeinheit Kontrast-/Arzneimittel"
  5050  97 0  10  0 0 feld" Melde-ID Implantateregister"
  5051  97 2 512  0 0 feld" Hash-String Implantate-register"
  5052  97 0  64  0 0 feld" Hash-Wert Implantateregister"
  5074  97 2  60  0 0 feld" Name Hersteller/ Lieferant"
  5075  97 2  60  0 0 feld" Artikel-/ Modellnummer"
  5076  97 2  20  0 0 feld" Rechnungsnummer"
  5077  97 2  20  0 0 feld" HGNC-Gensymbol"
  5078  97 2  60  0 0 feld" Gen-Name"
  5079  97 2  60  0 0 feld" Art der Erkrankung"
  5098 110 0   9  0 0 feld" (N)BSNR des Ortes der Leistungserbringung"
  5099 110 0   9  0 0 feld" Lebenslange Arztnummer (LANR) des Vertragsarztes/Vertragspsy"
  5100 110 0   9  0 0 feld" ASV-Teamnummer des Vertragsarztes"
  5101 110 0   9  0 0 feld" Pseudo-LANR (für Krankenhausärzte im Rahmen der ASV-Abrechnu"
  5102 110 0   9  0 0 feld" Krankenhaus-IK (im Rahmen der ASV-Abrechnung)"
  6001  97 0   3  5 6 feld" ICD-Code"
  6003  97 0   1  0 0 feld" Diagnosensicherheit"
  6004  97 0   1  0 0 feld" Seitenlokalisation"
  6006  97 2  60  0 0 feld" Diagnosenerläuterung"
  6008  97 2  60  0 0 feld" Diagnosenausnahmetatbestand"
  8000  97 0   4  0 0 feld" Satzart"
  9102 110 0   2  0 0 feld" Empfänger"
  9103 100 0   8  0 0 feld" Erstellungsdatum"
  9106 110 0   1  0 0 feld" verwendeter Zeichensatz"
  9115 100 0   8  0 0 feld" Erstellungsdatum ADT-Datenpaket"
  9132 110 0   1  0 0 feld" enthaltene Datenpakete dieser Datei"
  9204 110 0   5  0 0 feld" Abrechnungsquartal"
  9212  97 2  11  0 0 feld" Version der Satzbeschreibung"
  9250  97 1  15 17 0 feld" AVWG-Prüfnummer der AVS"
  9251  97 1  15 17 0 feld" HMV-Prüfnummer"
  9260 110 0   2  0 0 feld" Anzahl Teilabrechnungen"
  9261 110 0   2  0 0 feld" Abrechnungsteil x von y"
  9901  97 2  60  0 0 feld" Systeminterner Parameter"
  0 0 0 0 0 0 feld" "

\ Satztabellen:  FK  Vorkommen(1|0=n)  Feldart  Ebene  st,
\   Feldart als Zeichen: M = unbedingtes Mussfeld, m = bedingtes Mussfeld,
\   P = bedingtes Mussfeld ohne Bedingung (gebunden an das uebergeordnete Feld,
\   Kap. 1.5.3), K = Kannfeld, k = bedingtes Kannfeld
\   Ebene: Hierarchiestufe aus der Spalte Vorkommen (1 = Satzebene)
create st-con0
  8000 1  77 1 st,   \ 8000 1 M  Satzart
  9103 1  77 1 st,   \ 9103 1 M  Erstellungsdatum
  9106 1  77 1 st,   \ 9106 1 M  verwendeter Zeichensatz
  9132 0  77 1 st,   \ 9132 n M  enthaltene Datenpakete dieser Date  [ist, unter der]
  0 0 0 0 st,
create st-con9
  8000 1  77 1 st,   \ 8000 1 M  Satzart
  0 0 0 0 st,
create st-besa
  8000 1  77 1 st,   \ 8000 1 M  Satzart
   201 0 109 1 st,   \ 0201 n m  Betriebs- (BSNR) oder Nebenbetrieb  [Regel 857 Regel 875 Regel 878]
   203 1  80 2 st,   \   0203 1 P  (N)BSNR-/Krankenhaus-Bezeichnung
   212 0 109 2 st,   \   0212 n m  Lebenslange Arztnummer (LANR)  [Regel 836 Regel 875]
   219 1 107 3 st,   \     0219 1 k  Titel des Arztes
   220 1 107 3 st,   \     0220 1 k  Arztvorname
   221 1 107 3 st,   \     0221 1 k  Namenszusatz des Arztes
   211 1  80 3 st,   \     0211 1 P  Arztname oder Erläuterung
   222 0  75 3 st,   \     0222 n K  ASV-Teamnummer
   223 0 109 2 st,   \   0223 n m  Pseudo-LANR für Krankenhausärzte i  [Regel 836]
   219 1 107 3 st,   \     0219 1 k  Titel des Arztes
   220 1 107 3 st,   \     0220 1 k  Arztvorname
   221 1 107 3 st,   \     0221 1 k  Namenszusatz des Arztes
   211 1  80 3 st,   \     0211 1 P  Arztname oder Erläuterung
   222 0  80 3 st,   \     0222 n P  ASV-Teamnummer
   205 1  80 2 st,   \   0205 1 P  Straße der (N)BSNR-/ Krankenhaus- 
   215 1  80 2 st,   \   0215 1 P  PLZ der (N)BSNR-/ Krankenhaus-Adre
   216 1  80 2 st,   \   0216 1 P  Ort der (N)BSNR-/ Krankenhaus-Adre
   208 1  80 2 st,   \   0208 1 P  Telefonnummer
   209 1 107 2 st,   \   0209 1 k  Telefaxnummer
   218 1 107 2 st,   \   0218 1 k  E-Mail der Betriebsstätte/Praxis/K
   224 1 109 2 st,   \   0224 1 m  Produkttypversion des Konnektors  [Falls die Information]
   227 1 109 3 st,   \     0227 1 m  Ablaufdatum des Konnektorzertifika  [Regel 872]
   228 0 109 3 st,   \     0228 n m  Produktname des Konnektors  [Regel 874]
   225 0 109 2 st,   \   0225 n m  TI-Fachanwendung / TI-Komponente  [Regel 858 Regel 862]
   226 1  80 3 st,   \     0226 1 P  Systemunterstützung / Ausstattung 
   213 0 109 1 st,   \ 0213 n m  Krankenhaus-IK (im Rahmen der ASV-  [Regel 857]
   214 1  80 2 st,   \   0214 1 P  KV-Bereich
   203 1  80 2 st,   \   0203 1 P  (N)BSNR-/Krankenhaus-Bezeichnung
   212 0 109 2 st,   \   0212 n m  Lebenslange Arztnummer (LANR)  [Regel 836]
   219 1 107 3 st,   \     0219 1 k  Titel des Arztes
   220 1 107 3 st,   \     0220 1 k  Arztvorname
   221 1 107 3 st,   \     0221 1 k  Namenszusatz des Arztes
   211 1  80 3 st,   \     0211 1 P  Arztname oder Erläuterung
   222 0  75 3 st,   \     0222 n K  ASV-Teamnummer
   223 0 109 2 st,   \   0223 n m  Pseudo-LANR für Krankenhausärzte i  [Regel 836]
   219 1 107 3 st,   \     0219 1 k  Titel des Arztes
   220 1 107 3 st,   \     0220 1 k  Arztvorname
   221 1 107 3 st,   \     0221 1 k  Namenszusatz des Arztes
   211 1  80 3 st,   \     0211 1 P  Arztname oder Erläuterung
   222 0  80 3 st,   \     0222 n P  ASV-Teamnummer
   205 1  80 2 st,   \   0205 1 P  Straße der (N)BSNR-/ Krankenhaus- 
   215 1  80 2 st,   \   0215 1 P  PLZ der (N)BSNR-/ Krankenhaus-Adre
   216 1  80 2 st,   \   0216 1 P  Ort der (N)BSNR-/ Krankenhaus-Adre
   208 1  80 2 st,   \   0208 1 P  Telefonnummer
   209 1 107 2 st,   \   0209 1 k  Telefaxnummer
   218 1 107 2 st,   \   0218 1 k  E-Mail der Betriebsstätte/Praxis/K
   224 1 109 2 st,   \   0224 1 m  Produkttypversion des Konnektors  [Falls die Information über die]
   227 1 109 3 st,   \     0227 1 m  Ablaufdatum des Konnektorzertifika  [Regel 872]
   228 0 109 3 st,   \     0228 n m  Produktname des Konnektors  [Regel 874]
   225 0 109 2 st,   \   0225 n m  TI-Fachanwendung / TI-Komponente  [Regel 858 Regel 862]
   226 1  80 3 st,   \     0226 1 P  Systemunterstützung / Ausstattung 
  0 0 0 0 st,
create st-rvsa
  8000 1 109 1 st,   \ 8000 1 m  Satzart  [Regel 743]
   201 0  77 1 st,   \ 0201 n M  Betriebs- (BSNR) oder Nebenbetrieb
   300 1  80 2 st,   \   0300 1 P  Abrechnung von (zertifikatspflicht
   301 1 109 3 st,   \     0301 1 m  pnSD/uu-Analysen  [Regel 740]
   302 0 109 4 st,   \       0302 n m  Gerätetyp  [Regel 741 Regel 748]
   303 1  80 5 st,   \         0303 1 P  Hersteller
   304 0 109 3 st,   \     0304 n m  Analyt-ID  [Regel 740]
   305 1  80 4 st,   \       0305 1 P  RV-Zertifikat
  0 0 0 0 st,
create st-adt0
  8000 1  77 1 st,   \ 8000 1 M  Satzart
   105 1  77 1 st,   \ 0105 1 M  KBV-Prüfnummer
  9102 1  77 1 st,   \ 9102 1 M  Empfänger
  9212 1  77 1 st,   \ 9212 1 M  Version der Satzbeschreibung
   102 1  77 1 st,   \ 0102 1 M  Softwareverantwortlicher (SV)
   121 1  77 1 st,   \ 0121 1 M  Straße des SV
   122 1  77 1 st,   \ 0122 1 M  PLZ des SV
   123 1  77 1 st,   \ 0123 1 M  Ort des SV
   124 1  77 1 st,   \ 0124 1 M  Telefonnummer des SV
   125 1  75 1 st,   \ 0125 1 K  Telefaxnummer des SV
   111 1  75 1 st,   \ 0111 1 K  E-Mail-Adresse des SV
   126 1  77 1 st,   \ 0126 1 M  Regionaler Systembetreuer (SB)
   127 1  77 1 st,   \ 0127 1 M  Straße des SB
   128 1  77 1 st,   \ 0128 1 M  PLZ des SB
   129 1  77 1 st,   \ 0129 1 M  Ort des SB
   130 1  77 1 st,   \ 0130 1 M  Telefonnummer des SB
   131 1  75 1 st,   \ 0131 1 K  Telefaxnummer des SB
   103 1  77 1 st,   \ 0103 1 M  Software
   132 1  75 1 st,   \ 0132 1 K  Release-Stand der Software
  9115 1  75 1 st,   \ 9115 1 K  Erstellungsdatum ADT-Datenpaket
  9260 1  75 1 st,   \ 9260 1 K  Anzahl Teilabrechnungen
  9261 1  80 2 st,   \   9261 1 P  Abrechnungsteil x von y
  9204 1  77 1 st,   \ 9204 1 M  Abrechnungsquartal
  9250 0  75 1 st,   \ 9250 n K  AVWG-Prüfnummer der AVS
  9251 0  75 1 st,   \ 9251 n K  HMV-Prüfnummer
  0 0 0 0 st,
create st-adt9
  8000 1  77 1 st,   \ 8000 1 M  Satzart
  0 0 0 0 st,
create st-0101
  8000 1  77 1 st,   \ 8000 1 M  Satzart
  3000 1  75 1 st,   \ 3000 1 K  Patientennummer
  3003 1  75 1 st,   \ 3003 1 K  Schein-ID
  3006 1 109 1 st,   \ 3006 1 m  CDM Version  [Regel 307]
  3010 1 109 1 st,   \ 3010 1 m  Datum und Uhrzeit der Onlineprüfun  [falls eGK eingelesen wurde und]
  3011 1  80 2 st,   \   3011 1 P  Ergebnis der Onlineprüfung und -ak
  3012 1 107 2 st,   \   3012 1 k  Error-Code
  3013 1 107 2 st,   \   3013 1 k  Prüfziffer des Fachdienstes
  3100 1  75 1 st,   \ 3100 1 K  Namenszusatz
  3120 1  75 1 st,   \ 3120 1 K  Vorsatzwort
  3101 1  77 1 st,   \ 3101 1 M  Name
  3102 1  77 1 st,   \ 3102 1 M  Vorname
  3103 1  77 1 st,   \ 3103 1 M  Geburtsdatum
  3104 1  75 1 st,   \ 3104 1 K  Titel
  3105 1 109 1 st,   \ 3105 1 m  Versichertennummer  [Regel 776]
  3119 1 109 1 st,   \ 3119 1 m  Versicherten_ID  [Regel 776]
  3107 1  75 1 st,   \ 3107 1 K  Straße
  3109 1  75 1 st,   \ 3109 1 K  Hausnummer
  3115 1  75 1 st,   \ 3115 1 K  Anschriftenzusatz
  3112 1 109 1 st,   \ 3112 1 m  PLZ  [Regel 479]
  3114 1 109 1 st,   \ 3114 1 m  Wohnsitzlaendercode  [Regel 784]
  3113 1  75 1 st,   \ 3113 1 K  Ort
  3121 1 109 1 st,   \ 3121 1 m  PostfachPLZ  [Regel 479]
  3122 1  75 1 st,   \ 3122 1 K  PostfachOrt
  3123 1  75 1 st,   \ 3123 1 K  Postfach
  3124 1 109 1 st,   \ 3124 1 m  PostfachWohnsitzlaendercode  [Regel 784]
  3116 1 109 1 st,   \ 3116 1 m  WOP  [Regel 774]
  3108 1  77 1 st,   \ 3108 1 M  Versichertenart
  3110 1  77 1 st,   \ 3110 1 M  Geschlecht
  4101 1  77 1 st,   \ 4101 1 M  Quartal
  4102 1  75 1 st,   \ 4102 1 K  Ausstellungsdatum
  4104 1  77 1 st,   \ 4104 1 M  Abrechnungs-VKNR
  4106 1  77 1 st,   \ 4106 1 M  Kostenträger-Abrechnungsbereich (K
  4108 1  75 1 st,   \ 4108 1 K  Zulassungsnummer (mobiles Lesegerä
  4109 1 109 1 st,   \ 4109 1 m  Letzter Einlesetag der Versicherte  [falls Versichertenkarte eingel]
  4112 1  75 1 st,   \ 4112 1 K  eEB vorhanden  [Regel 895]
  4133 1 109 1 st,   \ 4133 1 m  VersicherungsschutzBeginn  [Regel 775]
  4110 1  75 1 st,   \ 4110 1 K  Versicherungsschutz Ende
  4111 1  77 1 st,   \ 4111 1 M  Kostentraegerkennung
  4134 1 109 1 st,   \ 4134 1 m  Kostentraegername  [Regel 777]
  4131 1  77 1 st,   \ 4131 1 M  BesonderePersonengruppe
  4132 1  77 1 st,   \ 4132 1 M  DMP_Kennzeichnung
  4121 1  77 1 st,   \ 4121 1 M  Gebührenordnung
  4122 1  77 1 st,   \ 4122 1 M  Abrechnungsgebiet
  4123 1  75 1 st,   \ 4123 1 K  Personenkreis / Untersuchungskateg
  4124 1  75 1 st,   \ 4124 1 K  SKT-Zusatzangaben
  4125 1  75 1 st,   \ 4125 1 K  Gültigkeitszeitraum von ... bis ..
  4126 0  75 1 st,   \ 4126 n K  SKT-Bemerkungen
  4202 1  75 1 st,   \ 4202 1 K  Unfall, Unfallfolgen
  4204 1  75 1 st,   \ 4204 1 K  eingeschränkter Leistungsanspruch 
  4206 1  75 1 st,   \ 4206 1 K  Mutm. Tag der Entbindung
  4234 1  75 1 st,   \ 4234 1 K  anerkannte Psychotherapie
  4235 0  80 2 st,   \   4235 n P  Datum des Anerkennungsbescheides
  4299 1 107 3 st,   \     4299 1 k  Lebenslange Arztnummer (LANR) des 
  4247 1  75 3 st,   \     4247 1 K  Antragsdatum (des Anerkennungsesch
  4250 1 107 3 st,   \     4250 1 k  Kombinationsbehandlung aus Einzelu
  4251 1  80 4 st,   \       4251 1 P  Durchführungsart der Kombinationsb
  4252 1 107 3 st,   \     4252 1 k  Gesamtanzahl bewilligter Therapiee  [Regel 850 Regel 852 Regel 897]
  4253 0 107 3 st,   \     4253 n k  Bewilligte GOP für den Versicherte  [Regel 850 Regel 853 Regel 897]
  4254 1  80 4 st,   \       4254 1 P  Anzahl der abgerechneten GOPen für
  4255 1 107 3 st,   \     4255 1 k  Gesamtanzahl bewilligter Therapiee  [Regel 851 Regel 852]
  4256 0 107 3 st,   \     4256 n k  Bewilligte GOP für die Bezugsperso  [Regel 851 Regel 853]
  4257 1  80 4 st,   \       4257 1 P  Anzahl der abgerechneten GOPen für
  4236 1  75 1 st,   \ 4236 1 K  Abklärung somatischer Ursachen vor
  4239 1  77 1 st,   \ 4239 1 M  Scheinuntergruppe
  4103 1  75 1 st,   \ 4103 1 K  Vermittlungs-/Kontaktart  [Regel 870 Regel 877]
  4114 1 107 2 st,   \   4114 1 k  Vermittlungscode
  4115 1 107 2 st,   \   4115 1 k  Tag der Terminvermittlung  [Regel 877 Regel 886 Regel 887]
  4105 0 107 2 st,   \   4105 n k  Ergänzende Informationen zur Vermi
  5000 0  77 1 st,   \ 5000 n M  Leistungstag
  5001 0  80 2 st,   \   5001 n P  Gebührennummer (GNR)
  5002 1 107 3 st,   \     5002 1 k  Art der Untersuchung
  5003 1 107 3 st,   \     5003 1 k  (N)BSNR des vermittelten Facharzte
  5005 1 107 3 st,   \     5005 1 k  Multiplikator  [Regel 894]
  5006 1 107 3 st,   \     5006 1 k  Um-Uhrzeit
  5008 1 107 3 st,   \     5008 1 k  DKM
  5009 0 107 3 st,   \     5009 n k  freier Begründungstext
  5010 1 109 3 st,   \     5010 1 m  Chargennummer  [Regel 868]
  5012 0 107 3 st,   \     5012 n k  Sachkosten / Materialkosten in Cen
  5011 0  80 4 st,   \       5011 n P  Sachkosten-Bezeichnung
  5074 1 107 4 st,   \       5074 1 k  Name Hersteller/ Lieferant
  5075 1 107 4 st,   \       5075 1 k  Artikel-/ Modellnummer
  5076 1 107 4 st,   \       5076 1 k  Rechnungsnummer
  5013 1 107 3 st,   \     5013 1 k  Prozent der Leistung
  5015 0 107 3 st,   \     5015 n k  Organ
  5016 0 107 3 st,   \     5016 n k  Name des Arztes
  5017 1 107 3 st,   \     5017 1 k  Besuchsort bei Hausbesuchen
  5018 1 107 3 st,   \     5018 1 k  Zone bei Besuchen
  5019 1 107 3 st,   \     5019 1 k  Erbringungsort / Standort des Gerä
  5020 1 107 3 st,   \     5020 1 k  Wiederholungsuntersuchung
  5021 1 107 4 st,   \       5021 1 k  Jahr der letzten Krebsfrüherkennun
  5023 1 107 3 st,   \     5023 1 k  GO-Nummern-Zusatz
  5024 1 107 3 st,   \     5024 1 k  GNR-Zusatzkennzeichen für poststat
  5025 1 107 3 st,   \     5025 1 k  Aufnahmedatum
  5026 1 107 3 st,   \     5026 1 k  Entlassungsdatum
  5034 1 107 3 st,   \     5034 1 k  OP-Datum
  5035 0 107 3 st,   \     5035 n k  OP-Schlüssel
  5041 1 107 4 st,   \       5041 1 k  Seitenlokalisation OPS  [Regel 706]
  5036 0 107 3 st,   \     5036 n k  GNR als Begründung
  5037 1 109 3 st,   \     5037 1 m  Gesamt-Schnitt-Naht-Zeit (Minuten)  [Simultaneingriff]
  5038 0 107 3 st,   \     5038 n k  Komplikation
  5040 1 107 3 st,   \     5040 1 k  Patientennummer der eDokumentation
  5050 0 109 3 st,   \     5050 n m  Melde-ID Implantateregister  [Regel 888 Regel 894]
  5051 1 109 4 st,   \       5051 1 m  Hash-String Implantate-register  [Regel 889]
  5052 1 109 4 st,   \       5052 1 m  Hash-Wert Implantateregister  [Regel 890]
  5042 1 107 3 st,   \     5042 1 k  Mengenangabe Kontrast-/Arzneimitte
  5043 1  80 4 st,   \       5043 1 P  Maßeinheit Kontrast-/Arzneimittel
  5077 0 109 3 st,   \     5077 n m  HGNC-Gensymbol  [Regel 770 Regel 816 Regel 828 ]
  5078 0 109 4 st,   \       5078 n m  Gen-Name  [Regel 816]
  5079 0 109 3 st,   \     5079 n m  Art der Erkrankung  [Regel 770 Regel 816 Regel 828 ]
  5098 1 109 3 st,   \     5098 1 m  (N)BSNR des Ortes der Leistungserb  [Regel 859]
  5102 1 109 3 st,   \     5102 1 m  Krankenhaus-IK (im Rahmen der ASV-  [Regel 859]
  5099 1 109 3 st,   \     5099 1 m  Lebenslange Arztnummer (LANR) des   [Regel 837]
  5101 1 109 3 st,   \     5101 1 m  Pseudo-LANR (für Krankenhausärzte   [Regel 837]
  5100 1  75 3 st,   \     5100 1 K  ASV-Teamnummer des Vertragsarztes
  6001 0 109 1 st,   \ 6001 n m  ICD-Code  [Regel 486 Regel 828 Regel 829 ]
  6003 1  80 2 st,   \   6003 1 P  Diagnosensicherheit
  6004 1 107 2 st,   \   6004 1 k  Seitenlokalisation
  6006 0 107 2 st,   \   6006 n k  Diagnosenerläuterung
  6008 0 109 2 st,   \   6008 n m  Diagnosenausnahmetatbestand  [Regel 491]
  3673 0 109 1 st,   \ 3673 n m  Dauerdiagnose (ICD-Code)  [Regel 486]
  3674 1  80 2 st,   \   3674 1 P  Diagnosensicherheit Dauerdiagnose
  3675 1 107 2 st,   \   3675 1 k  Seitenlokalisation Dauerdiagnose
  3676 0 107 2 st,   \   3676 n k  Diagnosenerläuterung Dauerdiagnose
  3677 0 109 2 st,   \   3677 n m  Diagnosenausnahmetatbestand Dauerd  [Regel 491]
  0 0 0 0 st,
create st-0102
  8000 1  77 1 st,   \ 8000 1 M  Satzart
  3000 1  75 1 st,   \ 3000 1 K  Patientennummer
  3003 1  75 1 st,   \ 3003 1 K  Schein-ID
  3006 1 109 1 st,   \ 3006 1 m  CDM Version  [Regel 307]
  3010 1 109 1 st,   \ 3010 1 m  Datum und Uhrzeit der Onlineprüfun  [falls eGK eingelesen wurde und]
  3011 1  80 2 st,   \   3011 1 P  Ergebnis der Onlineprüfung und -ak
  3012 1 107 2 st,   \   3012 1 k  Error-Code
  3013 1 107 2 st,   \   3013 1 k  Prüfziffer des Fachdienstes
  3100 1  75 1 st,   \ 3100 1 K  Namenszusatz
  3120 1  75 1 st,   \ 3120 1 K  Vorsatzwort
  3101 1  77 1 st,   \ 3101 1 M  Name
  3102 1  77 1 st,   \ 3102 1 M  Vorname
  3103 1  77 1 st,   \ 3103 1 M  Geburtsdatum
  3104 1  75 1 st,   \ 3104 1 K  Titel
  3105 1 109 1 st,   \ 3105 1 m  Versichertennummer  [Regel 776]
  3119 1 109 1 st,   \ 3119 1 m  Versicherten_ID  [Regel 776]
  3107 1  75 1 st,   \ 3107 1 K  Straße
  3109 1  75 1 st,   \ 3109 1 K  Hausnummer
  3115 1  75 1 st,   \ 3115 1 K  Anschriftenzusatz
  3112 1 109 1 st,   \ 3112 1 m  PLZ  [Regel 479]
  3114 1 109 1 st,   \ 3114 1 m  Wohnsitzlaendercode  [Regel 784]
  3113 1  75 1 st,   \ 3113 1 K  Ort
  3121 1 109 1 st,   \ 3121 1 m  PostfachPLZ  [Regel 479]
  3122 1  75 1 st,   \ 3122 1 K  PostfachOrt
  3123 1  75 1 st,   \ 3123 1 K  Postfach
  3124 1 109 1 st,   \ 3124 1 m  PostfachWohnsitzlaendercode  [Regel 784]
  3116 1 109 1 st,   \ 3116 1 m  WOP  [Regel 774]
  3108 1  77 1 st,   \ 3108 1 M  Versichertenart
  3110 1  77 1 st,   \ 3110 1 M  Geschlecht
  4101 1  77 1 st,   \ 4101 1 M  Quartal
  4102 1 109 1 st,   \ 4102 1 m  Ausstellungsdatum  [Regel 405 Regel 406]
  4104 1  77 1 st,   \ 4104 1 M  Abrechnungs-VKNR
  4106 1  77 1 st,   \ 4106 1 M  Kostenträger-Abrechnungsbereich (K
  4108 1  75 1 st,   \ 4108 1 K  Zulassungsnummer (mobiles Lesegerä
  4109 1 109 1 st,   \ 4109 1 m  Letzter Einlesetag der Versicherte  [falls Versichertenkarte eingel]
  4112 1  75 1 st,   \ 4112 1 K  eEB vorhanden  [Regel 895]
  4133 1 109 1 st,   \ 4133 1 m  VersicherungsschutzBeginn  [Regel 775]
  4110 1  75 1 st,   \ 4110 1 K  Versicherungsschutz Ende
  4111 1  77 1 st,   \ 4111 1 M  Kostentraegerkennung
  4134 1 109 1 st,   \ 4134 1 m  Kostentraegername  [Regel 777]
  4131 1  77 1 st,   \ 4131 1 M  BesonderePersonengruppe
  4132 1  77 1 st,   \ 4132 1 M  DMP_Kennzeichnung
  4121 1  77 1 st,   \ 4121 1 M  Gebührenordnung
  4122 1  77 1 st,   \ 4122 1 M  Abrechnungsgebiet
  4123 1  75 1 st,   \ 4123 1 K  Personenkreis / Untersuchungskateg
  4124 1  75 1 st,   \ 4124 1 K  SKT-Zusatzangaben
  4125 1  75 1 st,   \ 4125 1 K  Gültigkeitszeitraum von ... bis ..
  4126 0  75 1 st,   \ 4126 n K  SKT-Bemerkungen
  4202 1  75 1 st,   \ 4202 1 K  Unfall, Unfallfolgen
  4204 1  75 1 st,   \ 4204 1 K  eingeschränkter Leistungsanspruch 
  4205 0 109 1 st,   \ 4205 n m  Auftrag  [Regel 744 Regel 755]
  4206 1  75 1 st,   \ 4206 1 K  Mutm. Tag der Entbindung
  4207 0  75 1 st,   \ 4207 n K  Diagnose/ Verdachtsdiagnose
  4208 0  75 1 st,   \ 4208 n K  Befund/Medikation
  4209 0 107 1 st,   \ 4209 n k  Zusätzliche Angaben zu Untersuchun  [Regel 756]
  4214 1 109 1 st,   \ 4214 1 m  Behandlungstag bei IVD-Leistungen  [Regel 901]
  4217 1 107 1 st,   \ 4217 1 k  (N)BSNR des Erstveranlassers  [Regel 431]
  4241 1  80 2 st,   \   4241 1 P  Lebenslange Arztnummer (LANR) des 
  4225 1 107 1 st,   \ 4225 1 k  ASV-Teamnummer des Erstveranlasser  [Regel 431]
  4241 1 109 2 st,   \   4241 1 m  Lebenslange Arztnummer (LANR) des   [Regel 844]
  4248 1 109 2 st,   \   4248 1 m  Pseudo-LANR (für Krankenhausärzte   [Regel 844]
  4218 1 109 1 st,   \ 4218 1 m  (N)BSNR des Überweisers  [Regel 328]
  4242 1  80 2 st,   \   4242 1 P  Lebenslange Arztnummer des Überwei
  4226 1 109 1 st,   \ 4226 1 m  ASV-Teamnummer des Überweisers  [Regel 328]
  4242 1 109 2 st,   \   4242 1 m  Lebenslange Arztnummer des Überwei  [Regel 845]
  4249 1 109 2 st,   \   4249 1 m  Pseudo-LANR (für Krankenhausärzte   [Regel 845]
  4219 1 109 1 st,   \ 4219 1 m  Überweisung von anderen Ärzten  [Regel 328]
  4220 1 109 1 st,   \ 4220 1 m  Überweisung an  [Regel 320]
  4221 1 109 1 st,   \ 4221 1 m  Kurativ / Präventiv / ESS / bei be  [Regel 404]
  4229 1 107 1 st,   \ 4229 1 k  Ausnahmeindikation  [Regel 432]
  4234 1  75 1 st,   \ 4234 1 K  anerkannte Psychotherapie
  4235 0  80 2 st,   \   4235 n P  Datum des Anerkennungsbescheides
  4299 1 107 3 st,   \     4299 1 k  Lebenslange Arztnummer (LANR) des 
  4247 1  75 3 st,   \     4247 1 K  Antragsdatum (des Anerkennungsesch
  4250 1 107 3 st,   \     4250 1 k  Kombinationsbehandlung aus Einzelu
  4251 1  80 4 st,   \       4251 1 P  Durchführungsart der Kombinationsb
  4252 1 107 3 st,   \     4252 1 k  Gesamtanzahl bewilligter Therapiee  [Regel 850 Regel 852 Regel 897]
  4253 0 107 3 st,   \     4253 n k  Bewilligte GOP für den Versicherte  [Regel 850 Regel 853 Regel 897]
  4254 1  80 4 st,   \       4254 1 P  Anzahl der abgerechneten GOPen für
  4255 1 107 3 st,   \     4255 1 k  Gesamtanzahl bewilligter Therapiee  [Regel 851 Regel 852]
  4256 0 107 3 st,   \     4256 n k  Bewilligte GOP für die Bezugsperso  [Regel 851 Regel 853]
  4257 1  80 4 st,   \       4257 1 P  Anzahl der abgerechneten GOPen für
  4239 1  77 1 st,   \ 4239 1 M  Scheinuntergruppe
  4103 1  75 1 st,   \ 4103 1 K  Vermittlungs-/Kontaktart  [Regel 870 Regel 877]
  4114 1 107 2 st,   \   4114 1 k  Vermittlungscode
  4115 1 107 2 st,   \   4115 1 k  Tag der Terminvermittlung  [Regel 877 Regel 886 Regel 887]
  4105 0 107 2 st,   \   4105 n k  Ergänzende Informationen zur Vermi
  5000 0  77 1 st,   \ 5000 n M  Leistungstag
  5001 0  80 2 st,   \   5001 n P  Gebührennummer (GNR)
  5002 1 107 3 st,   \     5002 1 k  Art der Untersuchung
  5003 1 107 3 st,   \     5003 1 k  (N)BSNR des vermittelten Facharzte
  5005 1 107 3 st,   \     5005 1 k  Multiplikator  [Regel 894]
  5006 1 107 3 st,   \     5006 1 k  Um-Uhrzeit
  5008 1 107 3 st,   \     5008 1 k  DKM
  5009 0 107 3 st,   \     5009 n k  freier Begründungstext
  5010 1 109 3 st,   \     5010 1 m  Chargennummer  [Regel 868]
  5012 0 107 3 st,   \     5012 n k  Sachkosten / Materialkosten in Cen
  5011 0  80 4 st,   \       5011 n P  Sachkosten-Bezeichnung
  5074 1 107 4 st,   \       5074 1 k  Name Hersteller/ Lieferant
  5075 1 107 4 st,   \       5075 1 k  Artikel-/ Modellnummer
  5076 1 107 4 st,   \       5076 1 k  Rechnungsnummer
  5013 1 107 3 st,   \     5013 1 k  Prozent der Leistung
  5015 0 107 3 st,   \     5015 n k  Organ
  5016 0 107 3 st,   \     5016 n k  Name des Arztes
  5017 1 107 3 st,   \     5017 1 k  Besuchsort bei Hausbesuchen
  5018 1 107 3 st,   \     5018 1 k  Zone bei Besuchen
  5019 1 107 3 st,   \     5019 1 k  Erbringungsort / Standort des Gerä
  5020 1 107 3 st,   \     5020 1 k  Wiederholungsuntersuchung
  5021 1 107 4 st,   \       5021 1 k  Jahr der letzten Krebsfrüherkennun
  5023 1 107 3 st,   \     5023 1 k  GO-Nummern-Zusatz
  5024 1 107 3 st,   \     5024 1 k  GNR-Zusatzkennzeichen für poststat
  5025 1 107 3 st,   \     5025 1 k  Aufnahmedatum
  5026 1 107 3 st,   \     5026 1 k  Entlassungsdatum
  5034 1 107 3 st,   \     5034 1 k  OP-Datum
  5035 0 107 3 st,   \     5035 n k  OP-Schlüssel
  5041 1 107 4 st,   \       5041 1 k  Seitenlokalisation OPS  [Regel 706]
  5036 0 107 3 st,   \     5036 n k  GNR als Begründung
  5037 1 109 3 st,   \     5037 1 m  Gesamt-Schnitt-Naht-Zeit (Minuten)  [Simultaneingriff]
  5038 0 107 3 st,   \     5038 n k  Komplikation
  5040 1 107 3 st,   \     5040 1 k  Patientennummer der eDokumentation
  5050 0 109 3 st,   \     5050 n m  Melde-ID Implantateregister  [Regel 888 Regel 894]
  5051 1 109 4 st,   \       5051 1 m  Hash-String Implantate-register  [Regel 889]
  5052 1 109 4 st,   \       5052 1 m  Hash-Wert Implantateregister  [Regel 890]
  5042 1 107 3 st,   \     5042 1 k  Mengenangabe Kontrast-/Arzneimitte
  5043 1  80 4 st,   \       5043 1 P  Maßeinheit Kontrast-/Arzneimittel
  5077 0 109 3 st,   \     5077 n m  HGNC-Gensymbol  [Regel 770 Regel 816 Regel 828 ]
  5078 0 109 4 st,   \       5078 n m  Gen-Name  [Regel 816 Regel 891 Regel 893]
  5079 0 109 3 st,   \     5079 n m  Art der Erkrankung  [Regel 770 Regel 816 Regel 828 ]
  5098 1 109 3 st,   \     5098 1 m  (N)BSNR des Ortes der Leistungserb  [Regel 859]
  5102 1 109 3 st,   \     5102 1 m  Krankenhaus-IK (im Rahmen der ASV-  [Regel 859]
  5099 1 109 3 st,   \     5099 1 m  Lebenslange Arztnummer (LANR) des   [Regel 837]
  5101 1 109 3 st,   \     5101 1 m  Pseudo-LANR (für Krankenhausärzte   [Regel 837]
  5100 1  75 3 st,   \     5100 1 K  ASV-Teamnummer des Vertragsarztes
  6001 0 109 1 st,   \ 6001 n m  ICD-Code  [Regel 486 Regel 828 Regel 829 ]
  6003 1  80 2 st,   \   6003 1 P  Diagnosensicherheit
  6004 1 107 2 st,   \   6004 1 k  Seitenlokalisation
  6006 0 107 2 st,   \   6006 n k  Diagnosenerläuterung
  6008 0 109 2 st,   \   6008 n m  Diagnosenausnahmetatbestand  [Regel 491]
  3673 0 109 1 st,   \ 3673 n m  Dauerdiagnose (ICD-Code)  [Regel 486]
  3674 1  80 2 st,   \   3674 1 P  Diagnosensicherheit Dauerdiagnose
  3675 1 107 2 st,   \   3675 1 k  Seitenlokalisation Dauerdiagnose
  3676 0 107 2 st,   \   3676 n k  Diagnosenerläuterung Dauerdiagnose
  3677 0 109 2 st,   \   3677 n m  Diagnosenausnahmetatbestand Dauerd  [Regel 491]
  0 0 0 0 st,
create st-0103
  8000 1  77 1 st,   \ 8000 1 M  Satzart
  3000 1  75 1 st,   \ 3000 1 K  Patientennummer
  3003 1  75 1 st,   \ 3003 1 K  Schein-ID
  3006 1 109 1 st,   \ 3006 1 m  CDM Version  [Regel 307]
  3010 1 109 1 st,   \ 3010 1 m  Datum und Uhrzeit der Onlineprüfun  [falls eGK eingelesen wurde und]
  3011 1  80 2 st,   \   3011 1 P  Ergebnis der Onlineprüfung und -ak
  3012 1 107 2 st,   \   3012 1 k  Error-Code
  3013 1 107 2 st,   \   3013 1 k  Prüfziffer des Fachdienstes
  3100 1  75 1 st,   \ 3100 1 K  Namenszusatz
  3120 1  75 1 st,   \ 3120 1 K  Vorsatzwort
  3101 1  77 1 st,   \ 3101 1 M  Name
  3102 1  77 1 st,   \ 3102 1 M  Vorname
  3103 1  77 1 st,   \ 3103 1 M  Geburtsdatum
  3104 1  75 1 st,   \ 3104 1 K  Titel
  3105 1 109 1 st,   \ 3105 1 m  Versichertennummer  [Regel 776]
  3119 1 109 1 st,   \ 3119 1 m  Versicherten_ID  [Regel 776]
  3107 1  75 1 st,   \ 3107 1 K  Straße
  3109 1  75 1 st,   \ 3109 1 K  Hausnummer
  3115 1  75 1 st,   \ 3115 1 K  Anschriftenzusatz
  3112 1 109 1 st,   \ 3112 1 m  PLZ  [Regel 479]
  3114 1 109 1 st,   \ 3114 1 m  Wohnsitzlaendercode  [Regel 784]
  3113 1  75 1 st,   \ 3113 1 K  Ort
  3121 1 109 1 st,   \ 3121 1 m  PostfachPLZ  [Regel 479]
  3122 1  75 1 st,   \ 3122 1 K  PostfachOrt
  3123 1  75 1 st,   \ 3123 1 K  Postfach
  3124 1 109 1 st,   \ 3124 1 m  PostfachWohnsitzlaendercode  [Regel 784]
  3116 1 109 1 st,   \ 3116 1 m  WOP  [Regel 774]
  3108 1  77 1 st,   \ 3108 1 M  Versichertenart
  3110 1  77 1 st,   \ 3110 1 M  Geschlecht
  4101 1  77 1 st,   \ 4101 1 M  Quartal
  4102 1  75 1 st,   \ 4102 1 K  Ausstellungsdatum
  4104 1  77 1 st,   \ 4104 1 M  Abrechnungs-VKNR
  4106 1  77 1 st,   \ 4106 1 M  Kostenträger-Abrechnungsbereich (K
  4108 1  75 1 st,   \ 4108 1 K  Zulassungsnummer (mobiles Lesegerä
  4109 1 109 1 st,   \ 4109 1 m  Letzter Einlesetag der Versicherte  [falls Versichertenkarte eingel]
  4112 1  75 1 st,   \ 4112 1 K  eEB vorhanden  [Regel 895]
  4133 1 109 1 st,   \ 4133 1 m  VersicherungsschutzBeginn  [Regel 775]
  4110 1  75 1 st,   \ 4110 1 K  Versicherungsschutz Ende
  4111 1  77 1 st,   \ 4111 1 M  Kostentraegerkennung
  4134 1 109 1 st,   \ 4134 1 m  Kostentraegername  [Regel 777]
  4131 1  77 1 st,   \ 4131 1 M  BesonderePersonengruppe
  4132 1  77 1 st,   \ 4132 1 M  DMP_Kennzeichnung
  4121 1  77 1 st,   \ 4121 1 M  Gebührenordnung
  4122 1  77 1 st,   \ 4122 1 M  Abrechnungsgebiet
  4123 1  75 1 st,   \ 4123 1 K  Personenkreis / Untersuchungskateg
  4124 1  75 1 st,   \ 4124 1 K  SKT-Zusatzangaben
  4126 0  75 1 st,   \ 4126 n K  SKT-Bemerkungen
  4202 1  75 1 st,   \ 4202 1 K  Unfall, Unfallfolgen
  4204 1  75 1 st,   \ 4204 1 K  eingeschränkter Leistungsanspruch 
  4205 0 109 1 st,   \ 4205 n m  Auftrag  [Regel 746]
  4206 1  75 1 st,   \ 4206 1 K  Mutm. Tag der Entbindung
  4207 0 109 1 st,   \ 4207 n m  Diagnose/ Verdachtsdiagnose  [Regel 746]
  4208 0 109 1 st,   \ 4208 n m  Befund/Medikation  [Regel 746]
  4218 1 109 1 st,   \ 4218 1 m  (N)BSNR des Überweisers  [Regel 746]
  4242 1  80 2 st,   \   4242 1 P  Lebenslange Arztnummer des Überwei
  4233 0 109 1 st,   \ 4233 n m  Stationäre Behandlung von... bis..  [Regel 354]
  4239 1  77 1 st,   \ 4239 1 M  Scheinuntergruppe
  4103 1  75 1 st,   \ 4103 1 K  Vermittlungs-/Kontaktart  [Regel 870 Regel 877]
  4114 1 107 2 st,   \   4114 1 k  Vermittlungscode
  4115 1 107 2 st,   \   4115 1 k  Tag der Terminvermittlung  [Regel 877 Regel 886 Regel 887]
  4105 0 107 2 st,   \   4105 n k  Ergänzende Informationen zur Vermi
  5000 0  77 1 st,   \ 5000 n M  Leistungstag
  5001 0  80 2 st,   \   5001 n P  Gebührennummer (GNR)
  5002 1 107 3 st,   \     5002 1 k  Art der Untersuchung
  5003 1 107 3 st,   \     5003 1 k  (N)BSNR des vermittelten Facharzte
  5005 1 107 3 st,   \     5005 1 k  Multiplikator  [Regel 894]
  5006 1 107 3 st,   \     5006 1 k  Um-Uhrzeit
  5008 1 107 3 st,   \     5008 1 k  DKM
  5009 0 107 3 st,   \     5009 n k  freier Begründungstext
  5010 1 109 3 st,   \     5010 1 m  Chargennummer  [Regel 868]
  5012 0 107 3 st,   \     5012 n k  Sachkosten / Materialkosten in Cen
  5011 0  80 4 st,   \       5011 n P  Sachkosten-Bezeichnung
  5074 1 107 4 st,   \       5074 1 k  Name Hersteller/ Lieferant
  5075 1 107 4 st,   \       5075 1 k  Artikel-/ Modellnummer
  5076 1 107 4 st,   \       5076 1 k  Rechnungsnummer
  5013 1 107 3 st,   \     5013 1 k  Prozent der Leistung
  5015 0 107 3 st,   \     5015 n k  Organ
  5016 0 107 3 st,   \     5016 n k  Name des Arztes
  5018 1 107 3 st,   \     5018 1 k  Zone bei Besuchen
  5019 1 107 3 st,   \     5019 1 k  Erbringungsort / Standort des Gerä
  5020 1 107 3 st,   \     5020 1 k  Wiederholungsuntersuchung
  5021 1 107 4 st,   \       5021 1 k  Jahr der letzten Krebsfrüherkennun
  5023 1 107 3 st,   \     5023 1 k  GO-Nummern-Zusatz
  5024 1 107 3 st,   \     5024 1 k  GNR-Zusatzkennzeichen für poststat
  5025 1 107 3 st,   \     5025 1 k  Aufnahmedatum
  5026 1 107 3 st,   \     5026 1 k  Entlassungsdatum
  5034 1 107 3 st,   \     5034 1 k  OP-Datum
  5035 0 107 3 st,   \     5035 n k  OP-Schlüssel
  5041 1 107 4 st,   \       5041 1 k  Seitenlokalisation OPS  [Regel 706]
  5036 0 107 3 st,   \     5036 n k  GNR als Begründung
  5037 1 109 3 st,   \     5037 1 m  Gesamt-Schnitt-Naht-Zeit (Minuten)  [Simultaneingriff]
  5038 0 107 3 st,   \     5038 n k  Komplikation
  5040 1 107 3 st,   \     5040 1 k  Patientennummer der eDokumentation
  5050 0 109 3 st,   \     5050 n m  Melde-ID Implantateregister  [Regel 888 Regel 894]
  5051 1 109 4 st,   \       5051 1 m  Hash-String Implantate-register  [Regel 889]
  5052 1 109 4 st,   \       5052 1 m  Hash-Wert Implantateregister  [Regel 890]
  5042 1 107 3 st,   \     5042 1 k  Mengenangabe Kontrast-/Arzneimitte
  5043 1  80 4 st,   \       5043 1 P  Maßeinheit Kontrast-/Arzneimittel
  5077 0 109 3 st,   \     5077 n m  HGNC-Gensymbol  [Regel 770 Regel 816 Regel 828 ]
  5078 0 109 4 st,   \       5078 n m  Gen-Name  [Regel 816 Regel 891 Regel 893]
  5079 0 109 3 st,   \     5079 n m  Art der Erkrankung  [Regel 770 Regel 816 Regel 828 ]
  5098 1  77 3 st,   \     5098 1 M  (N)BSNR des Ortes der Leistungserb
  5099 1  77 3 st,   \     5099 1 M  Lebenslange Arztnummer (LANR) des 
  6001 0 109 1 st,   \ 6001 n m  ICD-Code  [Regel 486 Regel 828 Regel 829 ]
  6003 1  77 2 st,   \   6003 1 M  Diagnosensicherheit
  6004 1 107 2 st,   \   6004 1 k  Seitenlokalisation
  6006 0 107 2 st,   \   6006 n k  Diagnosenerläuterung
  6008 0 109 2 st,   \   6008 n m  Diagnosenausnahmetatbestand  [Regel 491]
  3673 0 109 1 st,   \ 3673 n m  Dauerdiagnose (ICD-Code)  [Regel 486]
  3674 1  80 2 st,   \   3674 1 P  Diagnosensicherheit Dauerdiagnose
  3675 1 107 2 st,   \   3675 1 k  Seitenlokalisation Dauerdiagnose
  3676 0 107 2 st,   \   3676 n k  Diagnosenerläuterung Dauerdiagnose
  3677 0 109 2 st,   \   3677 n m  Diagnosenausnahmetatbestand Dauerd  [Regel 491]
  0 0 0 0 st,
create st-0104
  8000 1  77 1 st,   \ 8000 1 M  Satzart
  3000 1  75 1 st,   \ 3000 1 K  Patientennummer
  3003 1  75 1 st,   \ 3003 1 K  Schein-ID
  3006 1 109 1 st,   \ 3006 1 m  CDM Version  [Regel 307]
  3010 1 109 1 st,   \ 3010 1 m  Datum und Uhrzeit der Onlineprüfun  [falls eGK eingelesen wurde und]
  3011 1  80 2 st,   \   3011 1 P  Ergebnis der Onlineprüfung und -ak
  3012 1 107 2 st,   \   3012 1 k  Error-Code
  3013 1 107 2 st,   \   3013 1 k  Prüfziffer des Fachdienstes
  3100 1  75 1 st,   \ 3100 1 K  Namenszusatz
  3120 1  75 1 st,   \ 3120 1 K  Vorsatzwort
  3101 1  77 1 st,   \ 3101 1 M  Name
  3102 1  77 1 st,   \ 3102 1 M  Vorname
  3103 1  77 1 st,   \ 3103 1 M  Geburtsdatum
  3104 1  75 1 st,   \ 3104 1 K  Titel
  3105 1 109 1 st,   \ 3105 1 m  Versichertennummer  [Regel 776]
  3119 1 109 1 st,   \ 3119 1 m  Versicherten_ID  [Regel 776]
  3107 1  75 1 st,   \ 3107 1 K  Straße
  3109 1  75 1 st,   \ 3109 1 K  Hausnummer
  3115 1  75 1 st,   \ 3115 1 K  Anschriftenzusatz
  3112 1 109 1 st,   \ 3112 1 m  PLZ  [Regel 479]
  3114 1 109 1 st,   \ 3114 1 m  Wohnsitzlaendercode  [Regel 784]
  3113 1  75 1 st,   \ 3113 1 K  Ort
  3121 1 109 1 st,   \ 3121 1 m  PostfachPLZ  [Regel 479]
  3122 1  75 1 st,   \ 3122 1 K  PostfachOrt
  3123 1  75 1 st,   \ 3123 1 K  Postfach
  3124 1 109 1 st,   \ 3124 1 m  PostfachWohnsitzlaendercode  [Regel 784]
  3116 1 109 1 st,   \ 3116 1 m  WOP  [Regel 774]
  3108 1  77 1 st,   \ 3108 1 M  Versichertenart
  3110 1  77 1 st,   \ 3110 1 M  Geschlecht
  4101 1  77 1 st,   \ 4101 1 M  Quartal
  4104 1  77 1 st,   \ 4104 1 M  Abrechnungs-VKNR
  4106 1  77 1 st,   \ 4106 1 M  Kostenträger-Abrechnungsbereich (K
  4108 1  75 1 st,   \ 4108 1 K  Zulassungsnummer (mobiles Lesegerä
  4109 1 109 1 st,   \ 4109 1 m  Letzter Einlesetag der Versicherte  [falls Versichertenkarte eingel]
  4112 1  75 1 st,   \ 4112 1 K  eEB vorhanden  [Regel 895]
  4133 1 109 1 st,   \ 4133 1 m  VersicherungsschutzBeginn  [Regel 775]
  4110 1  75 1 st,   \ 4110 1 K  Versicherungsschutz Ende
  4111 1  77 1 st,   \ 4111 1 M  Kostentraegerkennung
  4134 1 109 1 st,   \ 4134 1 m  Kostentraegername  [Regel 777]
  4131 1  77 1 st,   \ 4131 1 M  BesonderePersonengruppe
  4132 1  77 1 st,   \ 4132 1 M  DMP_Kennzeichnung
  4121 1  77 1 st,   \ 4121 1 M  Gebührenordnung
  4122 1  77 1 st,   \ 4122 1 M  Abrechnungsgebiet
  4123 1  75 1 st,   \ 4123 1 K  Personenkreis / Untersuchungskateg
  4124 1  75 1 st,   \ 4124 1 K  SKT-Zusatzangaben
  4125 1  75 1 st,   \ 4125 1 K  Gültigkeitszeitraum von ... bis ..
  4126 0  75 1 st,   \ 4126 n K  SKT-Bemerkungen
  4202 1  75 1 st,   \ 4202 1 K  Unfall, Unfallfolgen
  4239 1  77 1 st,   \ 4239 1 M  Scheinuntergruppe
  4103 1  75 1 st,   \ 4103 1 K  Vermittlungs-/Kontaktart  [Regel 870 Regel 877]
  4114 1 107 2 st,   \   4114 1 k  Vermittlungscode
  4115 1 107 2 st,   \   4115 1 k  Tag der Terminvermittlung  [Regel 877 Regel 886 Regel 887]
  4105 0 107 2 st,   \   4105 n k  Ergänzende Informationen zur Vermi
  4243 1  77 1 st,   \ 4243 1 M  Weiterbehandelnder Arzt
  5000 0  77 1 st,   \ 5000 n M  Leistungstag
  5001 0  80 2 st,   \   5001 n P  Gebührennummer (GNR)
  5002 1 107 3 st,   \     5002 1 k  Art der Untersuchung
  5003 1 107 3 st,   \     5003 1 k  (N)BSNR des vermittelten Facharzte
  5005 1 107 3 st,   \     5005 1 k  Multiplikator  [Regel 894]
  5006 1 107 3 st,   \     5006 1 k  Um-Uhrzeit
  5008 1 107 3 st,   \     5008 1 k  DKM
  5009 0 107 3 st,   \     5009 n k  freier Begründungstext
  5010 1 109 3 st,   \     5010 1 m  Chargennummer  [Regel 868]
  5012 0 107 3 st,   \     5012 n k  Sachkosten / Materialkosten in Cen
  5011 0  80 4 st,   \       5011 n P  Sachkosten-Bezeichnung
  5074 1 107 4 st,   \       5074 1 k  Name Hersteller/ Lieferant
  5075 1 107 4 st,   \       5075 1 k  Artikel-/ Modellnummer
  5076 1 107 4 st,   \       5076 1 k  Rechnungsnummer
  5013 1 107 3 st,   \     5013 1 k  Prozent der Leistung
  5015 0 107 3 st,   \     5015 n k  Organ
  5016 0 107 3 st,   \     5016 n k  Name des Arztes
  5017 1 107 3 st,   \     5017 1 k  Besuchsort bei Hausbesuchen
  5018 1 107 3 st,   \     5018 1 k  Zone bei Besuchen
  5019 1 107 3 st,   \     5019 1 k  Erbringungsort / Standort des Gerä
  5020 1 107 3 st,   \     5020 1 k  Wiederholungsuntersuchung
  5021 1 107 4 st,   \       5021 1 k  Jahr der letzten Krebsfrüherkennun
  5023 1 107 3 st,   \     5023 1 k  GO-Nummern-Zusatz
  5024 1 107 3 st,   \     5024 1 k  GNR-Zusatzkennzeichen für poststat
  5025 1 107 3 st,   \     5025 1 k  Aufnahmedatum
  5026 1 107 3 st,   \     5026 1 k  Entlassungsdatum
  5034 1 107 3 st,   \     5034 1 k  OP-Datum
  5035 0 107 3 st,   \     5035 n k  OP-Schlüssel
  5041 1 107 4 st,   \       5041 1 k  Seitenlokalisation OPS  [Regel 706]
  5036 0 107 3 st,   \     5036 n k  GNR als Begründung
  5037 1 109 3 st,   \     5037 1 m  Gesamt-Schnitt-Naht-Zeit (Minuten)  [Simultaneingriff]
  5038 0 107 3 st,   \     5038 n k  Komplikation
  5040 1 107 3 st,   \     5040 1 k  Patientennummer der eDokumentation
  5050 0 109 3 st,   \     5050 n m  Melde-ID Implantateregister  [Regel 888 Regel 894]
  5051 1 109 4 st,   \       5051 1 m  Hash-String Implantate-register  [Regel 889]
  5052 1 109 4 st,   \       5052 1 m  Hash-Wert Implantateregister  [Regel 890]
  5042 1 107 3 st,   \     5042 1 k  Mengenangabe Kontrast-/Arzneimitte
  5043 1  80 4 st,   \       5043 1 P  Maßeinheit Kontrast-/Arzneimittel
  5077 0 109 3 st,   \     5077 n m  HGNC-Gensymbol  [Regel 770 Regel 816 Regel 828 ]
  5078 0 109 4 st,   \       5078 n m  Gen-Name  [Regel 816 Regel 891 Regel 893]
  5079 0 109 3 st,   \     5079 n m  Art der Erkrankung  [Regel 770 Regel 816 Regel 828 ]
  5098 1 109 3 st,   \     5098 1 m  (N)BSNR des Ortes der Leistungserb  [Regel 859]
  5102 1 109 3 st,   \     5102 1 m  Krankenhaus-IK (im Rahmen der ASV-  [Regel 859]
  5099 1 109 3 st,   \     5099 1 m  Lebenslange Arztnummer (LANR) des   [Regel 837]
  5101 1 109 3 st,   \     5101 1 m  Pseudo-LANR (für Krankenhausärzte   [Regel 837]
  5100 1  75 3 st,   \     5100 1 K  ASV-Teamnummer des Vertragsarztes
  6001 0 109 1 st,   \ 6001 n m  ICD-Code  [Regel 486 Regel 828 Regel 829 ]
  6003 1  80 2 st,   \   6003 1 P  Diagnosensicherheit
  6004 1 107 2 st,   \   6004 1 k  Seitenlokalisation
  6006 0 107 2 st,   \   6006 n k  Diagnosenerläuterung
  6008 0 109 2 st,   \   6008 n m  Diagnosenausnahmetatbestand  [Regel 491]
  3673 0 109 1 st,   \ 3673 n m  Dauerdiagnose (ICD-Code)  [Regel 486]
  3674 1  80 2 st,   \   3674 1 P  Diagnosensicherheit Dauerdiagnose
  3675 1 107 2 st,   \   3675 1 k  Seitenlokalisation Dauerdiagnose
  3676 0 107 2 st,   \   3676 n k  Diagnosenerläuterung Dauerdiagnose
  3677 0 109 2 st,   \   3677 n m  Diagnosenausnahmetatbestand Dauerd  [Regel 491]
  0 0 0 0 st,
