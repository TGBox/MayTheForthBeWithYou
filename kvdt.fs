\ ===================================================================
\  kvdt.fs  --  Vereinfachter KVDT-Pruefer in ANS-Forth
\
\  Prueft Dateien im xDT-Zeilenformat  LLL FFFF Inhalt CR LF
\  (LLL = Zeilenlaenge inkl. CRLF, FFFF = Feldkennung).
\
\  WICHTIG: Das ist ein Demo-Regelwerk, angelehnt an KVDT. Es ersetzt
\  nicht das KBV-Pruefmodul und ist nicht gegen die aktuelle
\  KVDT-Datensatzbeschreibung abgeglichen.
\
\  Benutzung:   s" datei.kvdt" pruefe
\  Laeuft mit gforth und anderen ANS-Forth-Systemen (getestet: pForth).
\ ===================================================================

decimal

\ -------------------------------------------------------------------
\  Hilfsworte
\ -------------------------------------------------------------------

: ziffer?        ( c -- f )  [char] 0 [char] 9 1+ within ;
: grossbuchstabe? ( c -- f ) [char] A [char] Z 1+ within ;

: digits? ( a u -- f )            \ nur Ziffern und nicht leer?
  dup 0= if 2drop false exit then
  true -rot over + swap ?do i c@ ziffer? and loop ;

: zahl ( a u -- n )  0 0 2swap >number 2drop drop ;

: .0n ( n stellen -- )           \ Zahl mit fuehrenden Nullen
  >r s>d <# r> 0 ?do # loop #> type ;

\ ISO-8859-15 -> UTF-8 fuer die Ausgabe (vereinfacht wie Latin-1)
: emit-latin1 ( c -- )
  dup 128 < if emit exit then
  dup 6 rshift 192 or emit  63 and 128 or emit ;
: .latin1 ( a u -- )  over + swap ?do i c@ emit-latin1 loop ;

\ -------------------------------------------------------------------
\  Fehlerbuchhaltung
\ -------------------------------------------------------------------

variable #fehler
variable regeln          \ Bitmaske: Bit n gesetzt = Regel Rn ausgeloest
variable still           \ true = keine Ausgabe (fuer die Testsuite)
variable zeile#
variable fk              \ aktuelle Feldkennung als Zahl, -1 = unbekannt
variable inh-a  variable inh-u   \ aktueller Feldinhalt

: .fk ( -- )  fk @ 0< if ." ????" else fk @ 4 .0n then ;
: .inhalt ( -- )
  inh-u @ 0= if exit then
  ."   <" inh-a @ inh-u @ 40 min .latin1 ." >" ;

\ Verbucht einen Fehler; liefert true, wenn der Aufrufer die Meldung
\ ausgeben soll.  Zeilenanfang, Feldkennung und Regel sind dann schon
\ geschrieben.
: fehler ( regel -- ausgeben? )
  1 over lshift regeln @ or regeln !
  1 #fehler +!
  still @ if drop false exit then
  ." Zeile " zeile# @ 5 .r ."   FK " .fk ."   R" 2 .0n ."   " true ;

: melde ( regel a u -- )   rot fehler if type .inhalt cr else 2drop then ;

\ -------------------------------------------------------------------
\  Datum TTMMJJJJ
\ -------------------------------------------------------------------

: schaltjahr? ( j -- f )
  dup 4 mod 0=  over 100 mod 0<> and  swap 400 mod 0= or ;

create monatstage 31 c, 28 c, 31 c, 30 c, 31 c, 30 c,
                  31 c, 31 c, 30 c, 31 c, 30 c, 31 c,

: tage-im-monat ( m j -- n )
  over 2 = if nip schaltjahr? if 29 else 28 then
           else drop 1- monatstage + c@ then ;

variable dt-t  variable dt-m  variable dt-j

: datum ( a u -- jjjjmmtt true | false )
  dup 8 <> if 2drop false exit then
  2dup digits? 0= if 2drop false exit then
  drop  dup 2 zahl dt-t !  dup 2 + 2 zahl dt-m !  4 + 4 zahl dt-j !
  dt-m @ 1 13 within 0= if false exit then
  dt-j @ 1900 < if false exit then
  dt-t @ 1  dt-m @ dt-j @ tage-im-monat 1+  within 0= if false exit then
  dt-j @ 10000 *  dt-m @ 100 * +  dt-t @ +  true ;

: jahr    ( jjjjmmtt -- j )  10000 / ;
: quartal ( jjjjmmtt -- q )  100 / 100 mod 1- 3 / 1+ ;

\ -------------------------------------------------------------------
\  Formatpruefungen einzelner Felder
\ -------------------------------------------------------------------

\ ICD-10-GM:  A99  |  A99.9  |  A99.99   optional mit ! * + am Ende
: icd? ( a u -- f )
  dup 0= if 2drop false exit then
  2dup + 1- c@  dup [char] ! =  over [char] * = or  swap [char] + = or
  if 1- then
  dup 3 < if 2drop false exit then
  over c@ grossbuchstabe?  2 pick 1+ c@ ziffer? and  2 pick 2 + c@ ziffer? and
  0= if 2drop false exit then
  dup 3 = if 2drop true exit then
  dup 5 < over 6 > or if 2drop false exit then
  over 3 + c@ [char] . <> if 2drop false exit then
  swap 4 + swap 4 - digits? ;

\ LANR: 9 Ziffern, Stelle 7 = Pruefziffer ueber Stellen 1-6,
\ Gewichte abwechselnd 4 und 9.
: lanr-pruefziffer ( a -- n )
  0  6 0 do  over i + c@ [char] 0 -  i 1 and if 9 else 4 then  * +  loop
  nip  10 mod 10 swap - 10 mod ;

\ EBM-GOP: 5 Ziffern, optional ein Grossbuchstabe als Suffix
: gop? ( a u -- f )
  dup 6 = if 2dup + 1- c@ grossbuchstabe? 0= if 2drop false exit then 1- then
  dup 5 = if digits? else 2drop false then ;

variable gesucht
: einer-von? ( a u zeichen-a zeichen-u -- f )  \ Inhalt = genau 1 Zeichen aus Liste?
  2swap 1 <> if drop 2drop false exit then
  c@ gesucht !  over + swap  false -rot
  ?do i c@ gesucht @ = or loop ;

\ -------------------------------------------------------------------
\  Satzarten und Satzartfolge
\ -------------------------------------------------------------------
\  Codes: 0 = noch kein Satz, 1 = con0, 2 = besa, 3 = rvsa,
\         4 = Behandlungsfall 0101..0104, 5 = con9

variable satzart         \ Satzart des aktuellen Satzes (Code)
variable folge           \ zuletzt korrekt erkannte Satzart (Code)
variable satz-zeile      \ Zeile, in der der aktuelle Satz beginnt
variable satz-a  variable satz-u   \ Satzart als Text
variable gefunden        \ Bitmaske gefundener Pflichtfelder
variable geburt          \ JJJJMMTT oder 0
variable abr-q  variable abr-j     \ Abrechnungsquartal aus 4101, 0 = keins
variable leistungstag    \ JJJJMMTT oder 0
variable #saetze  variable #faelle

: satzart-code ( a u -- code | -1 )
  2dup s" con0" compare 0= if 2drop 1 exit then
  2dup s" besa" compare 0= if 2drop 2 exit then
  2dup s" rvsa" compare 0= if 2drop 3 exit then
  2dup s" con9" compare 0= if 2drop 5 exit then
  2dup s" 0101" compare 0= >r  2dup s" 0102" compare 0= r> or >r
  2dup s" 0103" compare 0= r> or >r  s" 0104" compare 0= r> or
  if 4 else -1 then ;

: erlaubt? ( alt neu -- f )
  swap case
    0 of 1 = endof
    1 of 2 = endof
    2 of 3 = endof
    3 of dup 3 = swap 4 = or endof
    4 of dup 4 = swap 5 = or endof
    >r drop false r>
  endcase ;

\ Pflichtfelder: Feldkennung, Bit
create pflichtfelder
   201 ,   1 ,   212 ,   2 ,  3101 ,   4 ,  3102 ,   8 ,
  3103 ,  16 ,  3110 ,  32 ,  4101 ,  64 ,  9103 , 128 ,
8 constant #pflichtfelder

: pf-eintrag ( i -- fk bit )  2 cells * pflichtfelder + 2@ swap ;

: pflicht-bit ( fk -- bit | 0 )
  #pflichtfelder 0 do
    dup i pf-eintrag >r = if drop r> unloop exit then r> drop
  loop drop 0 ;

: benoetigt ( code -- maske )
  case
    1 of 128 endof               \ con0: 9103 Erstellungsdatum
    2 of 1 endof                 \ besa: 0201 BSNR
    3 of 2 endof                 \ rvsa: 0212 LANR
    4 of 4 8 or 16 or 32 or 64 or endof   \ Fall: 3101 3102 3103 3110 4101
    0 swap
  endcase ;

: satz-abschliessen ( -- )
  satzart @ 1 < if exit then
  fk @ >r  inh-u @ >r  zeile# @ >r
  satz-zeile @ zeile# !  0 inh-u !
  satzart @ benoetigt  gefunden @ invert and   ( fehlende )
  #pflichtfelder 0 do
    i pf-eintrag  2 pick and if
      fk !  7 fehler if ." Pflichtfeld fehlt in Satzart "
                       satz-a @ satz-u @ type cr then
    else drop then
  loop drop
  r> zeile# !  r> inh-u !  r> fk ! ;

: satz-beginnen ( -- )
  satz-abschliessen
  1 #saetze +!
  inh-a @ inh-u @ satz-u ! satz-a !
  zeile# @ satz-zeile !
  0 gefunden !  0 geburt !  0 abr-q !  0 abr-j !  0 leistungstag !
  inh-a @ inh-u @ satzart-code  dup satzart !
  dup 4 = if 1 #faelle +! then
  dup 0< if drop 5 s" Unbekannte Satzart" melde exit then
  folge @ over erlaubt? if folge ! exit then
  \ Resynchronisieren, damit ein Fehler keine Lawine an Folgefehlern ausloest
  folge @ max folge !
  5 s" Satzart an dieser Stelle nicht erlaubt" melde ;

\ -------------------------------------------------------------------
\  Feldregeln
\ -------------------------------------------------------------------

: inhalt ( -- a u )  inh-a @ inh-u @ ;

: pruefe-leistungstag ( jjjjmmtt -- )
  dup leistungstag !
  geburt @ if dup geburt @ < if
    9 s" Leistungstag liegt vor dem Geburtsdatum" melde then then
  abr-q @ if
    dup quartal abr-q @ <>  over jahr abr-j @ <> or if
      11 s" Leistungstag liegt nicht im Abrechnungsquartal" melde then then
  drop ;

: feld-regeln ( -- )
  fk @ case
    201 of inhalt dup 9 = >r digits? r> and 0= if
             15 s" BSNR muss 9-stellig numerisch sein" melde then endof
    212 of inhalt dup 9 <> >r digits? 0= r> or if
             14 s" LANR muss 9-stellig numerisch sein" melde
           else inh-a @ dup lanr-pruefziffer swap 6 + c@ [char] 0 - <> if
             14 s" LANR-Pruefziffer (Stelle 7) falsch" melde then then endof
    3103 of inhalt datum if geburt !
           else 8 s" Ungueltiges Datum (TTMMJJJJ)" melde then endof
    3110 of inhalt s" MWXD" einer-von? 0= if
             17 s" Geschlecht muss M, W, X oder D sein" melde then endof
    4101 of inhalt dup 5 = >r 2dup digits? r> and if
             drop dup c@ [char] 0 -  dup 1 5 within if
               abr-q !  1+ 4 zahl abr-j !
             else drop drop 10 s" Quartal muss 1-4 sein (QJJJJ)" melde then
           else 2drop 10 s" Quartal muss Format QJJJJ haben" melde then endof
    5000 of inhalt datum if pruefe-leistungstag
           else 8 s" Ungueltiges Datum (TTMMJJJJ)" melde then endof
    5001 of leistungstag @ 0= if
             18 s" Leistung (5001) ohne vorherigen Leistungstag (5000)" melde then
           inhalt gop? 0= if
             16 s" GOP muss 5 Ziffern (+ opt. Buchstabe) haben" melde then endof
    6001 of inhalt icd? 0= if
             12 s" ICD-10-Code hat ungueltiges Format" melde then endof
    6003 of inhalt s" AGVZ" einer-von? 0= if
             13 s" Diagnosensicherheit muss A, G, V oder Z sein" melde then endof
    9103 of inhalt datum if drop
           else 8 s" Ungueltiges Datum (TTMMJJJJ)" melde then endof
  endcase ;

: feld ( -- )
  fk @ 8000 = if satz-beginnen exit then
  satzart @ 0= if 6 s" Feld ausserhalb eines Satzes (8000 fehlt)" melde exit then
  fk @ pflicht-bit gefunden @ or gefunden !
  feld-regeln ;

\ -------------------------------------------------------------------
\  Zeilenebene
\ -------------------------------------------------------------------

variable crlf

: zeile ( a u crlf? -- )
  crlf !
  -1 fk !  0 inh-u !
  dup 8 < if 2dup inh-u ! inh-a !
             1 s" Zeile zu kurz (min. 3+4+1 Zeichen)" melde 2drop exit then
  over 3 + 4 2dup digits? if zahl fk ! else 2drop then
  over 7 + inh-a !  dup 7 - inh-u !
  crlf @ 0= if 4 s" Zeilenende ist nicht CR LF" melde then
  over 3 digits? 0= if 2 s" Laengenangabe nicht numerisch" melde 2drop exit then
  over 3 zahl  over 2 +  2dup <> if
    2 fehler if ." Laengenangabe " swap 3 .0n ." , tatsaechlich " 3 .0n
                .inhalt cr else 2drop then
  else 2drop then
  2drop
  fk @ 0< if 3 s" Feldkennung nicht numerisch" melde exit then
  feld ;

\ -------------------------------------------------------------------
\  Datei laden und zeilenweise abarbeiten
\ -------------------------------------------------------------------

variable puffer  variable puffer#  variable #zeilen
variable pos  variable ende

: datei-laden ( a u -- )
  r/o bin open-file throw >r
  r@ file-size throw drop  dup puffer# !
  dup 0= if 1+ then allocate throw puffer !
  puffer @ puffer# @ r@ read-file throw drop
  r> close-file throw ;

: lf-suchen ( a -- a' )
  begin dup ende @ < if dup c@ 10 <> else false then while 1+ repeat ;

: zeilen-abarbeiten ( -- )
  puffer @ pos !  puffer @ puffer# @ + ende !
  begin pos @ ende @ < while
    1 zeile# +!
    pos @  dup lf-suchen          ( start lf )
    dup 1+ pos !
    over -                        ( start len )
    dup if 2dup + 1- c@ 13 = else false then
    if 1- true else false then    ( start len crlf? )
    zeile
  repeat ;

: zuruecksetzen ( -- )
  0 #fehler !  0 regeln !  0 zeile# !  -1 fk !  0 inh-u !
  0 satzart !  0 folge !  0 #saetze !  0 #faelle ! ;

: .zusammenfassung ( a u -- )
  cr ." Datei:   " type cr
  ." Zeilen:  " #zeilen @ . ."   Saetze: " #saetze @ . ."   Faelle: " #faelle @ . cr
  #fehler @ if ." Ergebnis: FEHLERHAFT, " #fehler @ . ." Fehler" cr
           else ." Ergebnis: OK" cr then ;

: pruefe ( a u -- )
  zuruecksetzen
  2dup datei-laden
  zeilen-abarbeiten
  satz-abschliessen
  zeile# @ #zeilen !
  folge @ 5 <> if
    zeile# @ 1+ zeile# !  -1 fk !  0 inh-u !
    5 s" Datei endet ohne Abschlusssatz con9" melde then
  puffer @ free throw
  still @ if 2drop else .zusammenfassung then ;
