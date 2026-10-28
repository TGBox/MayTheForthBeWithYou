\ ===================================================================
\  tests.fs  --  Testsuite fuer kvdt.fs
\  Aufruf:  gforth tests.fs        (aus dem Projektordner)
\ ===================================================================

include kvdt.fs

variable #ok  variable #nok

: bestanden ( a u -- )  1 #ok +!   ." [ OK ]   " type cr ;
: durchgefallen ( a u -- ) 1 #nok +! ." [FAIL]   " type cr ;

\ --- Unit-Tests einzelner Worte ------------------------------------

: pruefe-wahr ( f a u -- )  rot if bestanden else durchgefallen then ;

: unit-tests ( -- )
  cr ." Einzelne Regeln" cr
  s" 29022024" datum dup if nip then      s" 29.02.2024 ist gueltig (Schaltjahr)"  pruefe-wahr
  s" 29022100" datum dup if nip then 0=   s" 29.02.2100 ist ungueltig (kein Schaltjahr)" pruefe-wahr
  s" 29022000" datum dup if nip then      s" 29.02.2000 ist gueltig (400er-Regel)"  pruefe-wahr
  s" 31042026" datum dup if nip then 0=   s" 31.04.2026 ist ungueltig"             pruefe-wahr
  s" 14072026" datum drop quartal 3 =     s" Juli liegt in Quartal 3"              pruefe-wahr
  s" 123456601" drop lanr-pruefziffer 6 = s" LANR-Pruefziffer 123456 -> 6"         pruefe-wahr
  s" J06.9"  icd?                         s" ICD J06.9 gueltig"                    pruefe-wahr
  s" I10.90" icd?                         s" ICD I10.90 gueltig"                   pruefe-wahr
  s" U07.1!" icd?                         s" ICD U07.1! gueltig"                   pruefe-wahr
  s" J06"    icd?                         s" ICD J06 gueltig"                      pruefe-wahr
  s" J06."   icd? 0=                      s" ICD J06. ungueltig"                   pruefe-wahr
  s" j06.9"  icd? 0=                      s" ICD j06.9 ungueltig (Kleinbuchstabe)" pruefe-wahr
  s" 03000"  gop?                         s" GOP 03000 gueltig"                    pruefe-wahr
  s" 03000H" gop?                         s" GOP 03000H gueltig"                   pruefe-wahr
  s" 0300"   gop? 0=                      s" GOP 0300 ungueltig"                   pruefe-wahr ;

\ --- Dateitests: erwartete Fehlerzahl und ausgeloeste Regel --------

: erwarte ( datei-a datei-u #fehler regel -- )
  2swap 2dup 2>r                           ( n regel a u )  ( R: a u )
  true still !  pruefe  false still !
  1 swap lshift                            ( n maske )
  over 0= if drop 0 then                   \ bei 0 Fehlern keine Regel
  regeln @ =  swap #fehler @ = and
  2r> rot if bestanden
  else durchgefallen ."          -> erhalten: " #fehler @ . ." Fehler, Regelmaske " regeln @ . cr
  then ;

: datei-tests ( -- )
  cr ." Testdateien" cr
  s" testdaten/00_gueltig.kvdt"                      0  0 erwarte
  s" testdaten/01_zeile_zu_kurz.kvdt"                1  1 erwarte
  s" testdaten/02_laengenangabe_falsch.kvdt"         1  2 erwarte
  s" testdaten/03_feldkennung_nicht_numerisch.kvdt"  1  3 erwarte
  s" testdaten/04_zeilenende_nur_lf.kvdt"            1  4 erwarte
  s" testdaten/05_satzartfolge_falsch.kvdt"          2  5 erwarte
  s" testdaten/05b_con9_fehlt.kvdt"                  1  5 erwarte
  s" testdaten/06_feld_vor_erstem_satz.kvdt"         1  6 erwarte
  s" testdaten/07_pflichtfeld_fehlt.kvdt"            1  7 erwarte
  s" testdaten/08_datum_ungueltig.kvdt"              1  8 erwarte
  s" testdaten/09_geburt_nach_leistung.kvdt"         1  9 erwarte
  s" testdaten/10_quartal_ungueltig.kvdt"            1 10 erwarte
  s" testdaten/11_leistung_ausserhalb_quartal.kvdt"  1 11 erwarte
  s" testdaten/12_icd_format.kvdt"                   1 12 erwarte
  s" testdaten/13_diagnosensicherheit.kvdt"          1 13 erwarte
  s" testdaten/14_lanr_pruefziffer.kvdt"             1 14 erwarte
  s" testdaten/15_bsnr_format.kvdt"                  1 15 erwarte
  s" testdaten/16_gop_format.kvdt"                   1 16 erwarte
  s" testdaten/17_geschlecht.kvdt"                   1 17 erwarte
  s" testdaten/18_leistung_ohne_tag.kvdt"            1 18 erwarte
  s" testdaten/19_als_utf8_gespeichert.kvdt"         2  2 erwarte ;

: alle-tests ( -- )
  0 #ok !  0 #nok !
  unit-tests  datei-tests
  cr #ok @ . ." bestanden, " #nok @ . ." fehlgeschlagen" cr ;

alle-tests
bye
