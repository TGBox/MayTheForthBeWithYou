\ ===================================================================
\  tests.fs  --  Testsuite fuer kvdt.fs
\  Aufruf:  gforth tests.fs        (aus dem Projektordner)
\ ===================================================================

include kvdt.fs

variable #ok  variable #nok

: bestanden     ( a u -- )  1 #ok +!   ." [ OK ]   " type cr ;
: durchgefallen ( a u -- )  1 #nok +!  ." [FAIL]   " type cr ;
: pruefe-wahr   ( f a u -- )  rot if bestanden else durchgefallen then ;
: pruefe-falsch ( f a u -- )  rot 0= if bestanden else durchgefallen then ;

\ --- Unit-Tests einzelner Formatregeln -------------------------------

: unit-tests ( -- )
  cr ." Formatregeln" cr
  s" 20240229" datum? s" d: 20240229 ist ein gueltiges Datum"              pruefe-wahr
  s" 20261301" datum? s" d: Monat 13 ungueltig"                            pruefe-falsch
  20240229 kalender?  s" Kalender: 29.02.2024 (Schaltjahr)"                pruefe-wahr
  21000229 kalender?  s" Kalender: 29.02.2100 (kein Schaltjahr)"           pruefe-falsch
  20000229 kalender?  s" Kalender: 29.02.2000 (400er-Regel)"               pruefe-wahr
  s" 19660000" datum021? s" R021: JJJJ0000 erlaubt"                        pruefe-wahr
  s" 19661000" datum021? s" R021: JJJJMM00 erlaubt"                        pruefe-wahr
  s" 00000000" datum021? s" R021: 00000000 erlaubt"                        pruefe-wahr
  s" 19661300" datum021? s" R021: Monat 13 unzulaessig"                    pruefe-falsch
  s" 32026" qjjjj?     s" R016: 32026"                                     pruefe-wahr
  s" 52026" qjjjj?     s" R016: Quartal 5 unzulaessig"                     pruefe-falsch
  s" 2359" hhmm?       s" R005: 2359"                                      pruefe-wahr
  s" 2400" hhmm?       s" R005: 2400 unzulaessig"                          pruefe-falsch
  s" 20260714093012" zeitstempel? s" R060: Zeitstempel"                    pruefe-wahr
  s" 20260714096012" zeitstempel? s" R060: Minute 60 unzulaessig"          pruefe-falsch
  s" J06.9"  icd?      s" R022: J06.9"                                     pruefe-wahr
  s" I10.90" icd?      s" R022: I10.90"                                    pruefe-wahr
  s" M54.9-" icd?      s" R022: M54.9- (ann.n-)"                            pruefe-wahr
  s" J06"    icd?      s" R022: J06"                                       pruefe-wahr
  s" U07.1!" icd?      s" R022: Ausrufezeichen gehoert nicht in 6001"      pruefe-falsch
  s" j06.9"  icd?      s" R022: Kleinbuchstabe unzulaessig"                pruefe-falsch
  s" 03000"  gnr042?   s" R042: 03000"                                     pruefe-wahr
  s" 35401B" gnr042?   s" R042: 35401B"                                    pruefe-wahr
  s" 0300"   gnr042?   s" R042: 4 Stellen unzulaessig"                     pruefe-falsch
  s" 1"      gnr035?   s" R035: GOAe 1"                                    pruefe-wahr
  s" A"      gnr035?   s" R035: GOAe A (nur G-alpha)"                      pruefe-wahr
  s" 5855a1" gnr035?   s" R035: GOAe 5855a1"                               pruefe-wahr
  s" 250/A"  gnr035?   s" R035: GOAe 250/A (Lkz)"                          pruefe-wahr
  s" 12345"  gnr035?   s" R035: 5 Ziffern unzulaessig"                     pruefe-falsch
  s" 123456601" lanr?  s" R050: LANR 123456601"                            pruefe-wahr
  s" 123456001" lanr?  s" R050: falsche Pruefziffer"                       pruefe-falsch
  s" 555555601" lanr?  s" R050: 555555 unzulaessig"                        pruefe-falsch
  s" 001234566" asv?   s" R059: Beispiel aus der Doku 001234566"           pruefe-wahr
  s" 555555101" pseudo-lanr? s" R063: 555555101"                           pruefe-wahr
  s" 555555001" pseudo-lanr? s" R063: Ordnungsnummer 0 unzulaessig"        pruefe-falsch
  s" A123456780" versid? s" R054: A123456780"                              pruefe-wahr
  s" T555558879" versid? s" R054: T555558879 (Pruefziffer ok)"             pruefe-wahr
  s" A123456781" versid? s" R054: falsche Pruefziffer"                     pruefe-falsch
  s" 5.2.0"  cdm055?   s" R055: 5.2.0"                                     pruefe-wahr
  s" 5.2"    cdm055?   s" R055: 5.2 unvollstaendig"                        pruefe-falsch
  s" 5.10.0" cdm-vergleich 0>  s" CDM 5.10.0 > 5.2.0"                      pruefe-wahr
  s" 5.1.9"  cdm-vergleich 0<  s" CDM 5.1.9 < 5.2.0"                       pruefe-wahr
  s" ADT1026.01" version031? s" R031: ADT1026.01"                          pruefe-wahr
  s" ADT1326.01" version031? s" R031: Monat 13 unzulaessig"                pruefe-falsch
  s" Y/9/2607/36/123" pruefnummer052? s" R052: Y/9/2607/36/123"            pruefe-wahr
  s" X/1401/36/id9" pruefnummer052? s" R052: Beispiel aus der Doku verletzt Regel 052" pruefe-falsch
  s" 27123"  vknr017?  s" R017: 27123"                                     pruefe-wahr
  s" 04123"  vknr017?  s" R017: KV-Kennung 04 unzulaessig"                 pruefe-falsch
  s" 271234500" bsnr-819? s" R049: BSNR 271234500"                         pruefe-wahr
  s" 352712345" bsnr-819? s" R061: Krankenhaus-BSNR 352712345"             pruefe-wahr
  s" 742712363" bsnr-819? s" R062: SAPV-BSNR 742712363"                    pruefe-wahr
  s" 742712300" bsnr-819? s" R062: SAPV ohne 63 am Ende"                   pruefe-falsch
  s" 05" s" 01-03,06-21" in-liste? s" Liste: 05 nicht in 01-03,06-21"      pruefe-falsch
  s" 17" s" 01-03,06-21" in-liste? s" Liste: 17 in 06-21"                  pruefe-wahr ;

\ --- Dateitests: Anzahl Fehler/Warnungen/Hinweise und ausgeloeste Regel

variable e-a  variable e-u  variable e-f  variable e-w  variable e-i  variable e-r
: erwarte ( datei-a datei-u #F #W #I regel -- )
  e-r ! e-i ! e-w ! e-f ! e-u ! e-a !
  true still !  e-a @ e-u @ pruefe  false still !
  #F @ e-f @ =  #W @ e-w @ = and  #I @ e-i @ = and
  e-r @ 0= if true else e-r @ getroffen? then and
  e-a @ e-u @ rot if bestanden
  else durchgefallen
    ."          -> erhalten: " #F @ . ." F, " #W @ . ." W, " #I @ . ." I; Regeln: "
    #treffer @ 0 ?do treffer i cells + @ . loop cr
  then ;

: datei-tests ( -- )
  cr ." Testdateien (F W I Regel)" cr
  s" testdaten/00_gueltig.kvdt"                           0  0  0     0 erwarte
  s" testdaten/k1_datum_kalender.kvdt"                    0  1  0  9003 erwarte
  s" testdaten/k1_feld_nicht_zulaessig.kvdt"              1  0  0  9004 erwarte
  s" testdaten/k1_feld_vor_satz.kvdt"                     1  0  0  9004 erwarte
  s" testdaten/k1_feldlaenge.kvdt"                        1  0  0  9003 erwarte
  s" testdaten/k1_feldtyp_n.kvdt"                         2  0  0  9003 erwarte
  s" testdaten/k1_hierarchie_mussfeld.kvdt"                1  0  0  9006 erwarte
  s" testdaten/k1_leistungstag_ohne_gnr.kvdt"              1  0  0  9006 erwarte
  s" testdaten/k1_vorkommen_doppelt.kvdt"                  1  0  0  9004 erwarte
  s" testdaten/r837_je_gnr.kvdt"                           2  0  0   837 erwarte
  s" testdaten/k1_laengenangabe_falsch.kvdt"              1  0  0  9001 erwarte
  s" testdaten/k1_leeres_feld.kvdt"                       2  0  0  9001 erwarte
  s" testdaten/k1_leistungstage_absteigend.kvdt"          1  0  0  9004 erwarte
  s" testdaten/k1_mussfeld_fehlt.kvdt"                    1  0  0  9006 erwarte
  s" testdaten/k1_reihenfolge.kvdt"                       1  0  0  9004 erwarte
  s" testdaten/k1_utf8_gespeichert.kvdt"                  6  6  0  9002 erwarte
  s" testdaten/k1_zeilenende_nur_lf.kvdt"                 1  0  0  9001 erwarte
  s" testdaten/k2_adt9_fehlt.kvdt"                        1  0  0  9005 erwarte
  s" testdaten/k2_besa_vor_con0.kvdt"                     3  0  0  9005 erwarte
  s" testdaten/k2_con9_fehlt.kvdt"                        1  0  0  9005 erwarte
  s" testdaten/r005_uhrzeit.kvdt"                         1  0  0     5 erwarte
  s" testdaten/r016_quartal_format.kvdt"                  1  0  0    16 erwarte
  s" testdaten/r021_geburtsdatum.kvdt"                    1  0  0    21 erwarte
  s" testdaten/r022_icd_format.kvdt"                      1  0  0    22 erwarte
  s" testdaten/r050_lanr_pruefziffer.kvdt"                7  0  0    50 erwarte
  s" testdaten/r052_pruefnummer.kvdt"                     2  0  0    52 erwarte
  s" testdaten/r054_versicherten_id.kvdt"                 1  0  0    54 erwarte
  s" testdaten/r059_asv_team.kvdt"                        0  1  1    59 erwarte
  s" testdaten/r175_satzart_unbekannt.kvdt"               1  0  0   175 erwarte
  s" testdaten/r182_zeichensatz.kvdt"                     1  0  0   182 erwarte
  s" testdaten/r307_cdm_fehlt.kvdt"                       1  1  0   307 erwarte
  s" testdaten/r308_cdm_zu_neu.kvdt"                      0  1  0   308 erwarte
  s" testdaten/r309_errorcode.kvdt"                       0  1  0   309 erwarte
  s" testdaten/r313_geburt_nach_leistung.kvdt"            1  0  0   313 erwarte
  s" testdaten/r315_versicherungsschutz_ende.kvdt"        0  1  0   315 erwarte
  s" testdaten/r319_erstveranlasser_gleich.kvdt"          1  0  0   319 erwarte
  s" testdaten/r320_ueberweisung_an_fehlt.kvdt"           1  0  0   320 erwarte
  s" testdaten/r324_leistung_ausserhalb_quartal.kvdt"     1  0  0   324 erwarte
  s" testdaten/r328_ueberweiser_fehlt.kvdt"               1  0  0   328 erwarte
  s" testdaten/r331_scheinuntergruppe.kvdt"               1  0  0   331 erwarte
  s" testdaten/r354_stationaer_fehlt.kvdt"                1  0  0   354 erwarte
  s" testdaten/r363_gueltigkeit.kvdt"                     1  0  0   363 erwarte
  s" testdaten/r404_kurativ_ohne_sug.kvdt"                1  0  0   404 erwarte
  s" testdaten/r406_ausstellung_nach_erstellung.kvdt"     1  0  0   406 erwarte
  s" testdaten/r432_ausnahmeindikation.kvdt"              1  0  0   432 erwarte
  s" testdaten/r479_keine_plz.kvdt"                       1  0  0   479 erwarte
  s" testdaten/r486_keine_diagnose.kvdt"                  1  0  0   486 erwarte
  s" testdaten/r496_goae_format.kvdt"                     3  0  0   496 erwarte
  s" testdaten/r497_gnr_format.kvdt"                      1  0  0   497 erwarte
  s" testdaten/r533_geschlecht.kvdt"                      1  0  0   533 erwarte
  s" testdaten/r706_seitenlokalisation.kvdt"              1  0  0   706 erwarte
  s" testdaten/r707_multiplikator.kvdt"                   0  1  0   707 erwarte
  s" testdaten/r710_sachkosten.kvdt"                      0  1  0   710 erwarte
  s" testdaten/r715_lanr_nicht_in_besa.kvdt"              1  0  0   715 erwarte
  s" testdaten/r725_lanr_format.kvdt"                     2  0  0   725 erwarte
  s" testdaten/r738_teilabrechnung.kvdt"                  1  0  0   738 erwarte
  s" testdaten/r740_ringversuch.kvdt"                     0  1  0   740 erwarte
  s" testdaten/r743_rvsa_fehlt.kvdt"                      0  1  0   743 erwarte
  s" testdaten/r744_auftrag.kvdt"                         1  0  0   744 erwarte
  s" testdaten/r746_beleg_mitbehandlung.kvdt"             1  0  0   746 erwarte
  s" testdaten/r762_lanr_obsolet.kvdt"                    2  0  0   762 erwarte
  s" testdaten/r770_gen_doku.kvdt"                        0  1  0   770 erwarte
  s" testdaten/r774_wop_fehlt.kvdt"                       1  0  0   774 erwarte
  s" testdaten/r775_versicherungsbeginn.kvdt"             1  0  0   775 erwarte
  s" testdaten/r776_versicherten_id_fehlt.kvdt"           1  0  0   776 erwarte
  s" testdaten/r778_ktab.kvdt"                            1  0  0   778 erwarte
  s" testdaten/r784_laendercode.kvdt"                     1  0  0   784 erwarte
  s" testdaten/r813_alte_version.kvdt"                    0  1  0   813 erwarte
  s" testdaten/r819_bsnr_kv_kennung.kvdt"                 7  2  0   819 erwarte
  s" testdaten/r820_erstveranlasser_format.kvdt"          1  0  0   820 erwarte
  s" testdaten/r821_ueberweiser_format.kvdt"              1  0  0   821 erwarte
  s" testdaten/r837_859_arzt_ort.kvdt"                    2  0  0   837 erwarte
  s" testdaten/r850_psychotherapie.kvdt"                  1  1  0   850 erwarte
  s" testdaten/r853_gop_doppelt.kvdt"                     1  0  0   853 erwarte
  s" testdaten/r856_z017_nicht_gesichert.kvdt"            0  1  0   856 erwarte
  s" testdaten/r858_ti_wert_fehlt.kvdt"                   1  0  0   858 erwarte
  s" testdaten/r860_dauerdiagnose.kvdt"                   0  1  0   860 erwarte
  s" testdaten/r870_vermittlung.kvdt"                     1  0  0   870 erwarte
  s" testdaten/r872_konnektor.kvdt"                       1  0  0   872 erwarte
  s" testdaten/r875_lanr_doppelt.kvdt"                    1  0  0   875 erwarte
  s" testdaten/r876_onlinepruefung.kvdt"                  0  1  0   876 erwarte
  s" testdaten/r886_terminvermittlung_alt.kvdt"           0  1  0   886 erwarte
  s" testdaten/r891_gensymbol.kvdt"                       0  1  0   891 erwarte
  s" testdaten/r894_implantat.kvdt"                       0  1  0   894 erwarte
  s" testdaten/r895_eeb.kvdt"                             0  1  0   895 erwarte
  s" testdaten/r9132_paket_fehlt.kvdt"                    0  1  0  9007 erwarte ;

: alle-tests ( -- )
  0 #ok !  0 #nok !
  unit-tests  datei-tests
  cr #ok @ . ." bestanden, " #nok @ . ." fehlgeschlagen" cr ;

alle-tests
bye
