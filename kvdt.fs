\ ===================================================================
\  kvdt.fs  --  KVDT-Pruefer in ANS-Forth
\
\  Grundlage: KBV Datensatzbeschreibung KVDT, Version 6.08 (24.08.2026)
\  Umfang:    Container-Saetze (con0, besa, rvsa, con9) und ADT-Datenpaket
\             (adt0, 0101, 0102, 0103, 0104, adt9)
\
\  Geprueft werden
\   * Feldaufbau (Kap. 1.6.1), Zeichensatz (Kap. 1.8)
\   * Feldtabellen: Laenge und Typ jedes Feldes (Kap. 2.3.1, 3.5.1)
\   * Satztabellen: zulaessige Felder, Reihenfolge, Mussfelder (Kap. 1.5, 2.2, 3.4)
\   * Anordnung der Saetze in der Datei (Kap. 2.1, 3.3)
\   * Alle Regeln der Regeltabellen, die ohne externe Stammdateien
\     (EBM, SDICD, SDKT, SDPLZ, Anbieter, OPS, Schluesseltabellen) pruefbar
\     sind (Kap. 2.3.2, 3.5.2)
\
\  Feld- und Satztabellen stehen in kvdt-tabellen.fs und werden aus der
\  Datensatzbeschreibung generiert (werkzeug/tabellen_erzeugen.py).
\
\  Benutzung:   s" datei.kvdt" pruefe   (echte Abrechnungsdateien heissen *.CON)
\ ===================================================================

decimal

\ -------------------------------------------------------------------
\  Hilfsworte
\ -------------------------------------------------------------------

: ziffer?         ( c -- f )  [char] 0 [char] 9 1+ within ;
: grossbuchstabe? ( c -- f )  [char] A [char] Z 1+ within ;
: alnum?          ( c -- f )
  dup ziffer? over grossbuchstabe? or swap [char] a [char] z 1+ within or ;

: digits? ( a u -- f )            \ nur Ziffern und nicht leer?
  dup 0= if 2drop false exit then
  true -rot over + swap ?do i c@ ziffer? and loop ;

: zahl ( a u -- n )  0 0 2swap >number 2drop drop ;

: .0n ( n stellen -- )            \ Zahl mit fuehrenden Nullen
  >r s>d <# r> 0 ?do # loop #> type ;

: str, ( a u -- )  dup c,  here swap dup allot move ;

\ ISO-8859-15 -> UTF-8 fuer die Bildschirmausgabe (vereinfacht wie Latin-1)
: emit-latin1 ( c -- )
  dup 128 < if emit exit then
  dup 6 rshift 192 or emit  63 and 128 or emit ;
: .latin1 ( a u -- )  over + swap ?do i c@ emit-latin1 loop ;

variable sz
: zeichen-suchen ( a u c -- a' u' )   \ Rest ab dem ersten Zeichen c
  sz !  begin dup if over c@ sz @ <> else false then while 1 /string repeat ;

\ Listen wie "01-03,06,17" -- Elemente durch Komma, Bereiche mit Bindestrich
variable v-a  variable v-u
: wert ( -- a u )  v-a @ v-u @ ;

: element? ( ia iu -- f )
  2dup [char] - zeichen-suchen  dup 0= if 2drop wert compare 0= exit then
  1 /string 2>r  drop  2r@ drop 1- over -          ( ia lo-u )
  wert nip over <> if 2drop 2r> 2drop false exit then
  wert 2swap compare 0< if 2r> 2drop false exit then
  wert 2r> compare 0> 0= ;

: in-liste? ( a u liste-a liste-u -- f )
  2swap v-u ! v-a !
  begin dup while
    2dup [char] , zeichen-suchen
    2swap 2over nip -  element? if 2drop true exit then
    dup if 1 /string then
  repeat 2drop false ;

\ -------------------------------------------------------------------
\  Datum und Zeit
\ -------------------------------------------------------------------

: schaltjahr? ( j -- f )
  dup 4 mod 0=  over 100 mod 0<> and  swap 400 mod 0= or ;

create monatstage 31 c, 28 c, 31 c, 30 c, 31 c, 30 c,
                  31 c, 31 c, 30 c, 31 c, 30 c, 31 c,
: tage-im-monat ( m j -- n )
  over 2 = if nip schaltjahr? if 29 else 28 then
           else drop 1- monatstage + c@ then ;

: tt ( jjjjmmtt -- t ) 100 mod ;
: mm ( jjjjmmtt -- m ) 100 / 100 mod ;
: jj ( jjjjmmtt -- j ) 10000 / ;

\ Feldtyp d: JJJJMMTT mit TT 01-31, MM 01-12, JJJJ 0001-9999 (Kap. 1.6.2)
: datum? ( a u -- f )
  2dup digits? 0= if 2drop false exit then
  dup 8 <> if 2drop false exit then
  zahl  dup tt 1 32 within  over mm 1 13 within and  swap jj 0> and ;

\ Zusatzpruefung: existiert das Datum im Kalender?
: kalender? ( jjjjmmtt -- f )
  dup tt  over mm  rot jj  tage-im-monat 1+ swap 1 rot within ;

: tage ( jjjjmmtt -- n )            \ fortlaufende Tagesnummer
  dup jj  over mm  rot tt >r
  dup 3 < if 12 + swap 1- swap then  >r
  dup 365 *  over 4 / +  over 100 / -  swap 400 / +
  r> 3 - 153 * 2 + 5 / +  r> + ;

: jjjjq ( jjjjmmtt -- n )  dup jj 10 *  swap mm 1- 3 / 1+ + ;   \ z.B. 20263
: qjjjj>n ( a u -- n )  over c@ [char] 0 - >r  1 /string zahl 10 * r> + ;

[defined] time&date [if]
  : heute ( -- jjjjmmtt )
    time&date  10000 *  swap 100 * +  +  >r 2drop drop r> ;
[else]
  : heute ( -- 0 ) 0 ;    \ System ohne Uhr: Regeln 304/887 entfallen
[then]

\ -------------------------------------------------------------------
\  Feld- und Satztabellen (generiert)
\ -------------------------------------------------------------------

: feld" ( fk typ laengenart a b c "text<">" -- )
  >r >r >r >r >r  ,  r> , r> , r> , r> , r> ,
  [char] " parse str, align ;

: st, ( fk vorkommen feldart zone -- )  >r >r >r , r> , r> , r> , ;

include ./kvdt-tabellen.fs

: ft-naechster ( e -- e' )  6 cells +  dup c@ 1+ +  aligned ;
: ft-suchen ( fk -- e | 0 )
  feldtabelle begin dup @ while
    2dup @ = if nip exit then  ft-naechster
  repeat 2drop 0 ;
: ft-typ  ( e -- c )   cell+ @ ;
: ft-art  ( e -- n )   2 cells + @ ;
: ft-a    ( e -- n )   3 cells + @ ;
: ft-b    ( e -- n )   4 cells + @ ;
: ft-c    ( e -- n )   5 cells + @ ;
: ft-name ( e -- a u ) 6 cells + count ;

\ -------------------------------------------------------------------
\  Speicher fuer Felder und Saetze
\ -------------------------------------------------------------------

variable felder   variable #felder
variable saetze   variable #saetze

: feld   ( i -- adr )  4 cells *  felder @ + ;
: f-zeile ( i -- n )   feld @ ;
: f-fk    ( i -- fk )  feld cell+ @ ;
: f$      ( i -- a u ) feld 2 cells + 2@ ;
: feld-merken ( zeile fk a u -- )
  #felder @ feld >r  r@ 2 cells + 2!  r@ cell+ !  r> !  1 #felder +! ;

: satz    ( s -- adr )  3 cells *  saetze @ + ;
: s-anfang ( s -- i )   satz @ ;
: s-ende   ( s -- i )   satz cell+ @ ;
: s-art    ( s -- n )   satz 2 cells + @ ;

\ -------------------------------------------------------------------
\  Meldungen
\ -------------------------------------------------------------------
\  Regelnummern < 1000 stammen aus den Regeltabellen der KBV.
\  9xxx sind Pruefungen aus den Grundlagenkapiteln (ohne Regelnummer).

variable #F  variable #W  variable #I
variable still                       \ true = keine Ausgabe (Tests)
variable ort-zeile  variable ort-fk  variable ort-a  variable ort-u
variable extra-xt                    \ optionale Zusatzausgabe zur Meldung
variable liste-a  variable liste-u
: .liste ( -- )  ."  " liste-a @ liste-u @ type ;

create treffer 512 cells allot  variable #treffer
: treffer+ ( r -- )
  #treffer @ 512 < if treffer #treffer @ cells + !  1 #treffer +! else drop then ;
: getroffen? ( r -- f )
  false #treffer @ 0 ?do over treffer i cells + @ = or loop nip ;

: ort ( i -- )
  dup f-zeile ort-zeile !  dup f-fk ort-fk !  f$ ort-u ! ort-a ! ;

: .regel ( r -- )
  dup 1000 < if ." R" 3 .0n exit then
  case
    9001 of ." K1.6.1" endof   9002 of ." K1.8  " endof
    9003 of ." K1.6.2" endof   9004 of ." K1.5.2" endof
    9005 of ." K2.1  " endof   9006 of ." K1.5.3" endof
    9007 of ." K2.3.1" endof   ." ------"
  endcase ;

: .inhalt ( -- )
  ort-u @ 0= if exit then
  ."   <" ort-a @ ort-u @ 50 min .latin1 ." >" ;

: melde ( regel status a u -- )
  2>r
  dup [char] F = if 1 #F +! then
  dup [char] W = if 1 #W +! then
  dup [char] I = if 1 #I +! then
  over treffer+
  still @ if 2drop 2r> 2drop 0 extra-xt ! exit then
  ." Zeile " ort-zeile @ 5 .r
  ."   FK " ort-fk @ dup 0< if drop ." ----" else 4 .0n then
  ."   " emit ."  " .regel ."   " 2r> type
  extra-xt @ ?dup if execute then  0 extra-xt !
  .inhalt cr ;

: fehler! ( regel a u -- )  >r >r [char] F r> r> melde ;
: warnung! ( regel a u -- )  >r >r [char] W r> r> melde ;
: hinweis! ( regel a u -- )  >r >r [char] I r> r> melde ;

\ -------------------------------------------------------------------
\  Arbeiten innerhalb eines Satzes (Bereich rs..re der Felder)
\ -------------------------------------------------------------------

variable rs  variable re
: im-satz ( s -- )  dup s-anfang rs !  s-ende re ! ;

: finde ( fk -- i true | false )
  re @ rs @ ?do  i f-fk over = if drop i true unloop exit then  loop
  drop false ;
: hat?   ( fk -- f )     finde dup if nip then ;
: anzahl ( fk -- n )     0  re @ rs @ ?do over i f-fk = if 1+ then loop nip ;
: wert@  ( fk -- a u )   finde if f$ else 0 0 then ;
: wert=  ( fk a u -- f ) rot wert@ compare 0= ;
: wert-in? ( fk liste-a liste-u -- f )
  rot finde if f$ 2swap in-liste? else 2drop false then ;
: satz-ort ( -- )  rs @ ort ;
: fk-ort ( fk -- )  finde if ort else satz-ort then ;

\ Wichtige Saetze der Datei (Satznummern, -1 = nicht vorhanden)
variable con0-satz  variable besa-satz  variable rvsa-satz  variable adt0-satz

: wert-im-satz? ( fk a u s -- f )   \ hat Satz s ein Feld fk mit Inhalt a u?
  dup 0< if 2drop 2drop false exit then
  dup s-ende swap s-anfang ?do
    2 pick i f-fk = if 2dup i f$ compare 0= if 2drop drop true unloop exit then then
  loop 2drop drop false ;

: wert-aus-satz ( fk s -- a u )     \ erstes Feld fk in Satz s
  dup 0< if 2drop 0 0 exit then
  dup s-ende swap s-anfang ?do
    dup i f-fk = if drop i f$ unloop exit then
  loop drop 0 0 ;

: vorquartal? ( -- f )   \ Behandlungsquartal (4101) <> Abrechnungsquartal (9204)
  4101 wert@  9204 adt0-satz @ wert-aus-satz  compare 0<> ;

\ -------------------------------------------------------------------
\  Formatregeln
\ -------------------------------------------------------------------

: qjjjj? ( a u -- f )                       \ Regel 016
  2dup digits? 0= if 2drop false exit then
  5 <> if drop false exit then  c@ [char] 1 [char] 5 within ;

: hhmm? ( a u -- f )                        \ Regel 005
  2dup digits? 0= if 2drop false exit then
  4 <> if drop false exit then
  dup 2 zahl 24 <  swap 2 + 2 zahl 60 < and ;

: zeitstempel? ( a u -- f )                 \ Regel 060 JJJJMMTThhmmss
  2dup digits? 0= if 2drop false exit then
  14 <> if drop false exit then
  dup 8 datum? 0= if drop false exit then
  8 + dup 2 zahl 24 <  over 2 + 2 zahl 60 < and  swap 4 + 2 zahl 60 < and ;

: datum021? ( a u -- f )                    \ Regel 021 inkl. Teildaten
  2dup datum? if 2drop true exit then
  2dup digits? 0= if 2drop false exit then
  dup 8 <> if 2drop false exit then
  zahl dup 0= if drop true exit then           \ 00000000
  dup 10000 mod 0= if drop true exit then      \ JJJJ0000
  dup tt 0= swap mm 1 13 within and ;          \ JJJJMM00

: zwei-daten? ( a u -- f )                  \ Regel 058 JJJJMMTTJJJJMMTT
  16 <> if drop false exit then
  dup 8 datum?  swap 8 + 8 datum? and ;

: icd? ( a u -- f )                         \ Regel 022 ann, ann.n, ann.nn, ann.n-
  dup 3 < if 2drop false exit then
  over c@ grossbuchstabe?  2 pick 1+ 2 digits? and
  0= if 2drop false exit then
  dup 3 = if 2drop true exit then
  over 3 + c@ [char] . <> if 2drop false exit then
  dup 5 = if drop 4 + c@ ziffer? exit then
  dup 6 <> if 2drop false exit then
  drop 4 +  dup c@ ziffer?  swap 1+ c@  dup ziffer?  swap [char] - = or and ;

: gnr042? ( a u -- f )                      \ Regel 042 nnnnn, nnnnn[G-alpha]
  dup 6 = if 2dup + 1- c@ grossbuchstabe? 0= if 2drop false exit then 1- then
  dup 5 = if digits? else 2drop false then ;

: gop-basis= ( a u basis-a basis-u -- f )   \ 5001 = basis[G-alpha]?
  2 pick 5 < if 2drop 2drop false exit then
  2over drop 5 2swap compare if 2drop false exit then
  dup 5 = if 2drop true exit then
  dup 6 <> if 2drop false exit then
  drop 5 + c@ grossbuchstabe? ;
: gop-in? ( a u liste-a liste-u -- f )      \ Liste von 5-stelligen GOPen
  begin dup 4 > while
    2over 2over drop 5 gop-basis= if 2drop 2drop true exit then
    5 /string  dup if 1 /string then
  repeat 2drop 2drop false ;

\ Regel 035 (GOAe):  [G-alpha]n[n[n[n]]][K-alpha[aerw]][/Lkz]  oder  G-alpha[/Lkz]
variable g-a  variable g-u
: g-zeichen ( -- c ) g-a @ c@ ;
: g-weiter ( -- ) g-a @ 1+ g-a !  -1 g-u +! ;
: g-ende? ( -- f ) g-u @ 0= ;
variable lz
: lkz? ( c -- f )   \ Lkz ::= A-Z 0-9 # $ * < >
  dup grossbuchstabe? over ziffer? or if drop true exit then
  lz !  false s" #$*<>" over + swap ?do i c@ lz @ = or loop ;
: gnr035? ( a u -- f )
  g-u ! g-a !  g-ende? if false exit then
  0  g-zeichen grossbuchstabe? if g-weiter 1+ then        ( g-buchstaben )
  0  begin g-ende? 0= if g-zeichen ziffer? else false then over 4 < and
     while 1+ g-weiter repeat                               ( g n )
  dup 0= if over 0= if 2drop false exit then then
  dup if g-ende? 0= if g-zeichen dup grossbuchstabe? swap [char] a [char] d 1+ within or if
         g-weiter
         g-ende? 0= if g-zeichen dup grossbuchstabe? swap [char] 1 [char] 5 within or
                       if g-weiter then then then then then
  2drop
  g-ende? if true exit then
  g-zeichen [char] / <> if false exit then  g-weiter
  g-u @ 1 <> if false exit then  g-zeichen lkz? ;

: pruefziffer-49 ( a -- n )      \ Stellen 1-6, Gewichte 4/9, Modulo 10
  0  6 0 do  over i + c@ [char] 0 -  i 1 and if 9 else 4 then  * +  loop
  nip 10 mod 10 swap - 10 mod ;

: lanr? ( a u -- f )                        \ Regel 050/056 nnnnnnmff
  2dup digits? 0= if 2drop false exit then
  9 <> if drop false exit then
  dup 6 s" 555555" compare 0= if drop false exit then
  dup pruefziffer-49  swap 6 + c@ [char] 0 -  = ;

: asv? ( a u -- f )                         \ Regel 059 00nnnnnnP
  2dup digits? 0= if 2drop false exit then
  9 <> if drop false exit then
  dup 2 s" 00" compare if drop false exit then
  dup 2 + pruefziffer-49  swap 8 + c@ [char] 0 -  = ;

: pseudo-lanr? ( a u -- f )                 \ Regel 063/064 555555nff
  2dup digits? 0= if 2drop false exit then
  9 <> if drop false exit then
  dup 6 s" 555555" compare if drop false exit then
  6 + c@ [char] 1 [char] 9 1+ within ;

: kv-kennung? ( a -- f )   2 s" 01-03,06-21,24,25,27,28,31,37-73,78-81,83,85-88,93-96,98,99" in-liste? ;
: bsnr049? ( a u -- f )  9 = if kv-kennung? else drop false then ;
: bsnr061? ( a u -- f )
  9 <> if drop false exit then  dup 2 s" 35" compare if drop false exit then  2 + kv-kennung? ;
: bsnr062? ( a u -- f )
  9 <> if drop false exit then  dup 2 s" 74" compare if drop false exit then
  dup 7 + 2 s" 63" compare if drop false exit then  2 + kv-kennung? ;
: bsnr-819? ( a u -- f )          \ Regel 819/823: 35 -> 061, 74 -> 062, sonst 049
  over 2 s" 35" compare 0= if bsnr061? exit then
  over 2 s" 74" compare 0= if bsnr062? exit then  bsnr049? ;

: versid? ( a u -- f )            \ Regel 054 annnnnnnnP mit Pruefziffer
  10 <> if drop false exit then
  dup c@ grossbuchstabe? 0= if drop false exit then
  dup 1+ 9 digits? 0= if drop false exit then
  dup c@ [char] A - 1+  dup 10 / swap 10 mod        ( a z1 z2 )
  2 *  dup 9 > if 10 - 1+ then  +                    ( a summe )
  8 0 do  over i 1+ + c@ [char] 0 -  i 1 and if 2 else 1 then *
          dup 9 > if 10 - 1+ then  +  loop
  10 mod  swap 9 + c@ [char] 0 - = ;

: zahlen-punkt? ( a u -- a' u' f )  \ 1-3 Ziffern; Hilfswort fuer 055
  0 >r  begin dup if over c@ ziffer? else false then r@ 3 < and
        while 1 /string r> 1+ >r repeat  r> 0> ;
: cdm055? ( a u -- f )            \ Regel 055 n[n][n].n[n][n].n[n][n]
  zahlen-punkt? 0= if 2drop false exit then
  dup 0= if 2drop false exit then  over c@ [char] . <> if 2drop false exit then 1 /string
  zahlen-punkt? 0= if 2drop false exit then
  dup 0= if 2drop false exit then  over c@ [char] . <> if 2drop false exit then 1 /string
  zahlen-punkt? 0= if 2drop false exit then  nip 0= ;

: teilzahl ( a u -- a' u' n )  0 0 2swap >number 2swap drop ;
: cdm-vergleich ( a u -- n )      \ <0 / 0 / >0 im Vergleich zu 5.2.0
  2dup cdm055? 0= if 2drop 0 exit then
  teilzahl >r 1 /string  teilzahl >r 1 /string  teilzahl nip nip
  r> r>                                             ( c b a )
  5 - ?dup if nip nip exit then
  2 - ?dup if nip exit then ;

: version031? ( a u -- f )        \ Regel 031 [a]aaaMMJJ.nn
  dup 10 11 1+ within 0= if 2drop false exit then
  dup 11 = if 1 /string then
  over 3 0 do dup i + c@ alnum? 0= if drop unloop 2drop false exit then loop drop
  3 /string  over 4 digits? 0= if 2drop false exit then
  over 2 zahl 1 13 within 0= if 2drop false exit then
  4 /string  over c@ [char] . <> if 2drop false exit then
  1 /string digits? ;

: pruefnummer052? ( a u -- f )    \ Regel 052 a/n[n][n]/JJMM/nn/aaa
  dup 15 18 within 0= if 2drop false exit then
  over c@ dup [char] V = over [char] X = or over [char] Y = or swap [char] Z = or
  0= if 2drop false exit then
  1 /string  over c@ [char] / <> if 2drop false exit then  1 /string
  zahlen-punkt? 0= if 2drop false exit then
  over c@ [char] / <> if 2drop false exit then  1 /string
  dup 11 <> if 2drop false exit then
  drop  dup 4 digits?  over 2 + 2 zahl 1 13 within and
  over 4 + c@ [char] / = and  over 5 + 2 digits? and
  over 7 + c@ [char] / = and  0= if drop false exit then
  8 +  dup c@ alnum?  over 1+ c@ alnum? and  swap 2 + c@ alnum? and ;

: vknr017? ( a u -- f )           \ Regel 017 nnmmm
  2dup digits? 0= if 2drop false exit then
  5 <> if drop false exit then
  2 s" 01-03,06-21,24,25,27,28,31,37-74,78-81,83,85-91,93-96,98,99" in-liste? ;

\ -------------------------------------------------------------------
\  Wertelisten ("erlaubter Inhalt")
\ -------------------------------------------------------------------
\  FK  Regel  Status  Liste

: wl ( fk regel status "liste" -- )  >r >r , r> , r> ,  bl word count str, align ;
create wertelisten
  0214 532 char F wl 01-03,17,18,19,20,21,24,25,27,28,31,37,39-45,47-51,55,60-70,72,73,78-81,83,85-87,93-96,99
  0225 177 char F wl 1,3,4,5,6,7,8,9,10,11,12,13
  0226 147 char F wl 0,1
  0300 147 char F wl 0,1
  0301 107 char F wl 0,1,2
  0305 107 char F wl 0,1,2
  9106 182 char F wl 4
  9132 124 char F wl 1,3,6,7
  3011 528 char F wl 1,2,3,4,5,6
  3108 116 char F wl 1,3,5
  3110 533 char F wl M,W,U,X,D
  3116 531 char F wl 00,01,02,03,17,20,38,46,47,48,49,50,51,52,55,60,61,62,71,72,73,78,83,88,93,98
  3674 109 char F wl V,Z,A,G
  3675 110 char F wl R,L,B
  4103 108 char F wl 1,2,3,4,6
  4106 174 char F wl 00,01,02,03,04,05,06,07,08,09
  4112 142 char F wl 1
  4121 113 char F wl 0,1,2,3
  4122 131 char F wl 00,01,02,03,04,05,06,07,08,09,10,14,15
  4123 149 char F wl 01,02,03,04,05,06,07,08,09,10,11,12
  4131 534 char F wl 00,04,06,07,08,09
  4132 536 char F wl 00-13,30-58
  4202 142 char F wl 1
  4204 142 char F wl 1
  4221 205 char F wl 1,2,3,4
  4234 142 char F wl 1
  4236 142 char F wl 1
  4250 142 char F wl 1
  4251 176 char F wl 1,2,3,4
  5005 535 char F wl 002-999
  5018 111 char F wl Z1,Z2,Z3,Z4
  5020 147 char F wl 0,1
  5024 521 char F wl N
  5043 106 char F wl 1,2,3
  6003 109 char F wl V,Z,A,G
  6004 110 char F wl R,L,B
  9102 532 char F wl 01-03,17,18,19,20,21,24,25,27,28,31,37,39-45,47-51,55,60-70,72,73,78-81,83,85-87,93-96,99
  9260 129 char F wl 02-99
  9261 132 char F wl 01-99
  0 0 0 wl -

: wl-naechster ( e -- e' )  3 cells +  dup c@ 1+ +  aligned ;
: wertelisten-pruefen ( i -- )
  wertelisten begin dup @ while
    over f-fk over @ = if
      over f$  2 pick 3 cells + count  in-liste? 0= if
        over ort
        dup 3 cells + count liste-u ! liste-a !  ['] .liste extra-xt !
        dup cell+ @  over 2 cells + @  s" Wert nicht erlaubt; erlaubt:" melde
      then
    then wl-naechster
  repeat 2drop ;

\ -------------------------------------------------------------------
\  Pruefung einzelner Felder (Feldtabelle + Formatregeln)
\ -------------------------------------------------------------------

: laenge-ok? ( u e -- f )
  dup ft-art case
    0 of  >r dup r@ ft-a =  over r@ ft-b = r@ ft-b 0<> and or
             swap r> ft-c tuck = swap 0<> and or  endof
    1 of  dup ft-a swap ft-b 1+ within  endof
    2 of  ft-a 1+ 1 swap within  endof
    nip nip true swap
  endcase ;

: .laenge ( e -- )
  dup ft-art case
    0 of dup ft-a . dup ft-b ?dup if ." oder " . then
         ft-c ?dup if ." oder " . then endof
    1 of dup ft-a 0 .r ." -" ft-b . endof
    2 of ." hoechstens " ft-a . endof
  endcase ;

variable akt-e
: .akt-laenge ( -- )  ."  " akt-e @ .laenge ;
: feldtabelle-pruefen ( i -- )
  dup f-fk ft-suchen dup akt-e ! 0= if
    drop 9007 s" Feldkennung im Container/ADT nicht definiert" fehler! exit then
  dup f$ nip akt-e @ laenge-ok? 0= if
    ['] .akt-laenge extra-xt !  9003 s" Feldlaenge falsch, erlaubt:" fehler!
  then
  akt-e @ ft-typ case
    [char] n of dup f$ digits? 0= if 9003 s" Feldtyp n: nur Ziffern erlaubt" fehler!
                else akt-e @ ft-art 0<> if dup f$ drop c@ [char] 0 =
                     over f$ nip 1 > and if
                     9003 s" Feldtyp n mit variabler Laenge: keine fuehrenden Nullen" fehler! then then
                then endof
    [char] d of dup f$ datum? 0= if 9003 s" Feldtyp d: Datum JJJJMMTT erwartet" fehler!
                else dup f$ zahl kalender? 0= if
                     9003 s" Datum existiert nicht im Kalender (Zusatzpruefung)" warnung! then
                then endof
    [char] a of dup f$ drop c@ bl =  over f$ + 1- c@ bl = or if
                9003 s" Feldtyp a: fuehrende/nachfolgende Leerzeichen unzulaessig" fehler! then endof
  endcase
  drop ;

\ -------------------------------------------------------------------
\  Zeilenebene (Kap. 1.6.1 Feldaufbau, Kap. 1.8 Zeichensatz)
\ -------------------------------------------------------------------

: zeichensatz-pruefen ( a u -- )   \ Steuerzeichen und 0x80-0x9F unzulaessig
  0 -rot over + swap ?do
    i c@ dup bl < over 127 160 within or if drop 1+ leave then
    dup 194 196 within  i 1+ c@ 128 192 within and  if drop 2 + leave then drop
  loop
  case
    1 of 9002 s" unzulaessiges Zeichen (nicht in ISO 8859-15-Tabelle)" fehler! endof
    2 of 9002 s" Zeichenfolge sieht nach UTF-8 aus - Datei muss ISO 8859-15 sein" warnung! endof
  endcase ;

variable crlf
: zeile ( zeile# a u crlf? -- )
  crlf !
  -1 ort-fk !  2dup ort-u ! ort-a !  rot ort-zeile !
  dup 7 < if 9001 s" Zeile zu kurz fuer Laenge + Feldkennung" fehler! 2drop exit then
  over 3 + 4 digits? 0= if 9001 s" Feldkennung nicht numerisch" fehler! 2drop exit then
  over 3 + 4 zahl ort-fk !
  over 7 + ort-a !  dup 7 - ort-u !
  crlf @ 0= if 9001 s" Feldende ist nicht CR LF" fehler! then
  over 3 digits? 0= if 9001 s" Laengenangabe nicht numerisch" fehler!
  else over 3 zahl over 2 + <> if
    9001 [char] F s" Laengenangabe <> tatsaechliche Laenge (Inhalt + 9)" melde then
  then
  dup 7 = if 9001 s" leeres Feld (ohne Inhalt) unzulaessig" fehler! 2drop exit then
  ort-a @ ort-u @  -trailing nip 0= if
    9001 s" Feld enthaelt nur Leerzeichen" fehler! 2drop exit then
  ort-a @ ort-u @ zeichensatz-pruefen
  drop ort-zeile @ ort-fk @ ort-a @ ort-u @ feld-merken  drop ;

\ -------------------------------------------------------------------
\  Datei laden, Zeilen und Saetze bilden
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

: zeilen-zaehlen ( -- n )
  0 puffer @ puffer# @ + puffer @ ?do i c@ 10 = if 1+ then loop 1+ ;

: zeilen-lesen ( -- )
  zeilen-zaehlen dup 4 cells * allocate throw felder !
                     3 cells * allocate throw saetze !
  0 #felder !  0 #zeilen !
  puffer @ pos !  puffer @ puffer# @ + ende !
  begin pos @ ende @ < while
    1 #zeilen +!
    #zeilen @  pos @ dup lf-suchen  dup 1+ pos !  over -
    dup if 2dup + 1- c@ 13 = else false then
    if 1- true else false then
    zeile
  repeat ;

\ Satzarten
 1 constant sa-con0   2 constant sa-besa   3 constant sa-rvsa   4 constant sa-adt0
 5 constant sa-0101   6 constant sa-0102   7 constant sa-0103   8 constant sa-0104
 9 constant sa-adt9  10 constant sa-con9  11 constant sa-fremd-kopf
12 constant sa-fremd 13 constant sa-fremd-ende

: satzart-code ( a u -- n )
  2dup s" con0" compare 0= if 2drop sa-con0 exit then
  2dup s" besa" compare 0= if 2drop sa-besa exit then
  2dup s" rvsa" compare 0= if 2drop sa-rvsa exit then
  2dup s" adt0" compare 0= if 2drop sa-adt0 exit then
  2dup s" 0101" compare 0= if 2drop sa-0101 exit then
  2dup s" 0102" compare 0= if 2drop sa-0102 exit then
  2dup s" 0103" compare 0= if 2drop sa-0103 exit then
  2dup s" 0104" compare 0= if 2drop sa-0104 exit then
  2dup s" adt9" compare 0= if 2drop sa-adt9 exit then
  2dup s" con9" compare 0= if 2drop sa-con9 exit then
  2dup s" kad0,sad0,hdrg0" in-liste? if 2drop sa-fremd-kopf exit then
  2dup s" kad9,sad9,hdrg9" in-liste? if 2drop sa-fremd-ende exit then
  s" 0109,sad1,sad2,sad3,hdrg1" in-liste? if sa-fremd exit then
  -1 ;

: fall? ( sa -- f )  sa-0101 sa-0104 1+ within ;

: saetze-bilden ( -- )
  0 #saetze !
  #felder @ 0 ?do
    i f-fk 8000 = if
      #saetze @ if i #saetze @ 1- satz cell+ ! then
      i #saetze @ satz !  i f$ satzart-code #saetze @ satz 2 cells + !
      1 #saetze +!
    else #saetze @ 0= if
      i ort 9004 s" Feld vor dem ersten Satz (jeder Satz beginnt mit 8000)" fehler!
    then then
  loop
  #saetze @ if #felder @ #saetze @ 1- satz cell+ ! then ;

\ -------------------------------------------------------------------
\  Anordnung der Saetze (Kap. 2.1 Container, Kap. 3.3 ADT)
\ -------------------------------------------------------------------
\  Zustaende: 0 Anfang  1 nach con0  2 nach besa  3 nach rvsa
\             4 im ADT-Paket  5 nach Paket  6 nach con9  7 in fremdem Paket

variable zustand  variable adt-gesehen
: folge-fehler ( s -- )
  s-anfang ort 9005 s" Satzart an dieser Stelle nicht zulaessig" fehler! ;

: satzfolge-pruefen ( -- )
  0 zustand !  0 adt-gesehen !
  -1 con0-satz !  -1 besa-satz !  -1 rvsa-satz !  -1 adt0-satz !
  #saetze @ 0 ?do
    i s-art case
      sa-con0 of zustand @ 0 <> if i folge-fehler then  i con0-satz !  1 zustand ! endof
      sa-besa of zustand @ 1 <> if i folge-fehler then  i besa-satz !  2 zustand ! endof
      sa-rvsa of zustand @ 2 <> if i folge-fehler then  i rvsa-satz !  3 zustand ! endof
      sa-adt0 of zustand @ 2 4 within 0= adt-gesehen @ or if i folge-fehler then
                 i adt0-satz !  true adt-gesehen !  4 zustand ! endof
      sa-adt9 of zustand @ 4 <> if i folge-fehler then  5 zustand ! endof
      sa-con9 of zustand @ 4 = if i s-anfang ort
                   9005 s" ADT-Datenpaket ohne Abschlusssatz adt9" fehler!
                 else zustand @ dup 2 4 within swap 5 = or 0= if i folge-fehler then then
                 6 zustand ! endof
      sa-fremd-kopf of zustand @ 2 6 within 0= if i folge-fehler then
                 i s-anfang ort
                 9005 s" Datenpaket wird von diesem Pruefer nicht geprueft (nur ADT)" hinweis!
                 7 zustand ! endof
      sa-fremd-ende of zustand @ 7 <> if i folge-fehler then 5 zustand ! endof
      sa-fremd of zustand @ 7 <> if i folge-fehler then endof
      dup fall? if zustand @ 4 <> if i folge-fehler then
              else i s-anfang ort zustand @ 4 = if 175 else 209 then
                   s" unbekannte Satzart (erlaubter Inhalt von 8000)" fehler! then
    endcase
  loop
  zustand @ 4 = if #zeilen @ 1+ ort-zeile ! -1 ort-fk ! 0 ort-u !
    9005 s" ADT-Datenpaket ohne Abschlusssatz adt9" fehler! then
  zustand @ 6 <> if #zeilen @ 1+ ort-zeile ! -1 ort-fk ! 0 ort-u !
    9005 s" Datei endet nicht mit Container-Abschluss con9" fehler! then ;

\ -------------------------------------------------------------------
\  Satztabellen: zulaessige Felder, Reihenfolge, Mussfelder
\ -------------------------------------------------------------------

variable st-tab
: st-fuer ( sa -- adr | 0 )
  case
    sa-con0 of st-con0 endof  sa-con9 of st-con9 endof
    sa-besa of st-besa endof  sa-rvsa of st-rvsa endof
    sa-adt0 of st-adt0 endof  sa-adt9 of st-adt9 endof
    sa-0101 of st-0101 endof  sa-0102 of st-0102 endof
    sa-0103 of st-0103 endof  sa-0104 of st-0104 endof
    0 swap
  endcase ;

: st-e     ( k -- e )  4 cells * st-tab @ + ;
: st-fk    ( k -- fk ) st-e @ ;
: st-vork  ( k -- n )  st-e cell+ @ ;          \ 1 = einmal, 0 = n-mal
: st-art   ( k -- c )  st-e 2 cells + @ ;
: st-zone  ( k -- z )  st-e 3 cells + @ ;
: st-ende? ( k -- f )  st-fk 0= ;

: st-erste ( fk -- k | -1 )
  0 begin dup st-ende? 0= while
    2dup st-fk = if nip exit then 1+
  repeat 2drop -1 ;
: st-vor ( fk von -- k | -1 )             \ ab Position "von" vorwaerts
  begin dup st-ende? 0= while
    2dup st-fk = if nip exit then 1+
  repeat 2drop -1 ;
: st-zurueck ( fk von zone -- k | -1 )    \ ab "von" rueckwaerts in derselben Zone
  >r begin dup 0< 0= if dup st-zone r@ = else false then while
    2dup st-fk = if nip r> drop exit then 1-
  repeat r> drop 2drop -1 ;

variable st-pos
: reihenfolge-pruefen ( -- )
  0 st-pos !
  re @ rs @ 1+ ?do
    i f-fk 9901 <> if
      i f-fk st-erste  dup 0< if
        drop i ort 9004 s" Feld in dieser Satzart nicht zulaessig (Satztabelle)" fehler!
      else
        st-zone  st-pos @ st-zone  < if
          i ort 9004 s" Reihenfolge: Feld gehoert vor den Leistungs-/Diagnoseteil" fehler!
        else
          i f-fk st-erste st-vork 0= if        \ Vorkommen n: Wiederholung erlaubt
            i f-fk st-pos @ dup st-zone st-zurueck
            dup 0< if drop i f-fk st-pos @ st-vor then
          else i f-fk st-pos @ 1+ st-vor then
          dup 0< if drop i ort
            9004 s" Reihenfolge laut Satztabelle verletzt oder Feld mehrfach (Vorkommen 1)" fehler!
          else st-pos ! then
        then
      then
    then
  loop ;

: mussfelder-pruefen ( -- )
  0 begin dup st-ende? 0= while
    dup st-art [char] M = if
      dup st-fk hat? 0= if
        dup st-zone case
          0 of true endof
          1 of dup st-fk 5000 = if true else 5001 hat? then endof
          2 of 6001 hat? endof
          3 of 3673 hat? endof
          false swap
        endcase
        if satz-ort  dup st-fk ort-fk !  0 ort-u !
           9006 s" Mussfeld (M) fehlt laut Satztabelle" fehler! then
      then
    then 1+
  repeat drop ;

: leistungstage-pruefen ( -- )      \ Kap. 1.5.2: Leistungstage aufsteigend
  0 re @ rs @ ?do
    i f-fk 5000 = if
      i f$ zahl  2dup > if i ort
        9004 s" Leistungstage (5000) nicht in aufsteigender Reihenfolge" fehler! then
      nip
    then
  loop drop ;

\ -------------------------------------------------------------------
\  Container-Regeln
\ -------------------------------------------------------------------

: lanr-felder ( i -- )   \ 0212/4299: Regel 050 F, 762 F
  dup f$ s" 888888800" compare 0= if dup ort 762 s" Ersatzwert 888888800 ist obsolet" fehler! then
  dup f$ lanr? 0= if dup ort 050 s" LANR: Format nnnnnnmff oder Pruefziffer falsch" fehler! then
  drop ;

: container-feldregeln ( i -- )
  dup ort
  dup f-fk case
    0201 of dup f$ bsnr-819? 0= if 819 s" (N)BSNR passt nicht zu Formatregel 049/061/062" fehler! then endof
    0212 of dup lanr-felder endof
    0222 of dup f$ asv? 0= if 059 s" ASV-Teamnummer: Format 00nnnnnnP oder Pruefziffer falsch" hinweis! then endof
    0223 of dup f$ pseudo-lanr? 0= if 063 s" Pseudo-LANR: Format 555555nff" fehler! then endof
  endcase drop ;

variable zaehler
: besa-gruppe ( -- )                \ eine (N)BSNR- bzw. Krankenhaus-IK-Gruppe
  re @ rs @ ?do
    i f-fk 0212 = if
      re @ i 1+ ?do
        i f-fk 0212 = if j f$ i f$ compare 0= if i ort
          875 s" LANR mehrfach unter derselben (N)BSNR" fehler! then then
      loop
    then
  loop
  s" 1,3,4,5,6,7,8,9,10,11,12,13" begin dup while
    2dup [char] , zeichen-suchen  2swap 2over nip -        ( rest-a rest-u w-a w-u )
    0 zaehler !  re @ rs @ ?do i f-fk 0225 = if 2dup i f$ compare 0= if 1 zaehler +! then then loop
    zaehler @
    rs @ ort
    dup 0= if >r 2dup liste-u ! liste-a !  ['] .liste extra-xt !
      858 s" TI-Fachanwendung (0225) fehlt fuer diese (N)BSNR, Wert:" fehler! r> then
    1 > if 862 s" TI-Fachanwendung (0225) mehrfach fuer diese (N)BSNR" fehler! then
    2drop  dup if 1 /string then
  repeat 2drop
  0224 hat? if
    0227 hat? 0= if 0224 fk-ort 872 s" 0224 vorhanden, aber 0227 fehlt" fehler! then
    0228 hat? 0= if 0224 fk-ort 874 s" 0224 vorhanden, aber 0228 fehlt" warnung! then
  then ;

variable b-rs  variable b-re  variable g-start
: besa-regeln ( -- )
  0201 hat? 0213 hat? or 0= if satz-ort 857 s" weder 0201 noch 0213 vorhanden" fehler! then
  0212 hat? 0223 hat? or 0= if satz-ort 836 s" weder 0212 noch 0223 vorhanden" fehler! then
  re @ rs @ ?do
    i f-fk 0201 = if
      re @ i 1+ ?do i f-fk 0201 = if j f$ i f$ compare 0= if i ort
        878 s" (N)BSNR mehrfach im besa-Satz" warnung! then then loop
    then
  loop
  rs @ b-rs !  re @ b-re !  -1 g-start !        \ Gruppen ab 0201 / 0213
  b-re @ b-rs @ ?do
    i f-fk dup 0201 = swap 0213 = or if
      g-start @ 0< 0= if g-start @ rs !  i re !  besa-gruppe then
      i g-start !
    then
  loop
  g-start @ 0< 0= if g-start @ rs !  b-re @ re !  besa-gruppe then
  b-rs @ rs !  b-re @ re ! ;

: rvsa-regeln ( -- )
  0300 s" 1" wert= if
    0301 hat? 0= 0304 hat? 0= or if 0300 fk-ort 740 s" 0300 = 1: 0301 und mind. ein 0304 erforderlich" warnung! then
  then
  0301 s" 1,2" wert-in? 0302 hat? 0= and if 0301 fk-ort 741 s" 0301 = 1 oder 2: 0302 erforderlich" warnung! then
  0305 s" 2" wert= 0302 hat? 0= and if 0305 fk-ort 748 s" 0305 = 2: mind. ein 0302 erforderlich" warnung! then
  re @ rs @ ?do
    i f-fk 0201 = if
      0201 i f$ besa-satz @ wert-im-satz? 0= if i ort
        750 s" (N)BSNR nicht im besa-Satz" warnung! then
    then
  loop ;

: adt0-regeln ( -- )
  re @ rs @ ?do
    i ort
    i f-fk case
      0105 of i f$ pruefnummer052? 0= if 052 s" Pruefnummer: Format a/n[n][n]/JJMM/nn/aaa" fehler! then endof
      9250 of i f$ pruefnummer052? 0= if 052 s" Pruefnummer: Format a/n[n][n]/JJMM/nn/aaa" fehler! then endof
      9251 of i f$ pruefnummer052? 0= if 052 s" Pruefnummer: Format a/n[n][n]/JJMM/nn/aaa" fehler! then endof
      9204 of i f$ qjjjj? 0= if 016 s" Format QJJJJ" fehler! then endof
      9212 of i f$ version031? 0= if 031 s" Format [a]aaaMMJJ.nn" fehler!
              else i f$ s" ADT0726.01,ADT1026.01" in-liste? 0= if
                813 s" keine aktuell gueltige Version (ADT0726.01, ADT1026.01)" warnung! then then endof
    endcase
  loop
  9260 hat? 9261 hat? and if
    9261 wert@ zahl 9260 wert@ zahl > if 9261 fk-ort 738 s" Abrechnungsteil groesser als Anzahl Teilabrechnungen" fehler! then
  then ;

\ -------------------------------------------------------------------
\  ADT-Regeln fuer Behandlungsfaelle (0101-0104)
\ -------------------------------------------------------------------

variable satzart   variable gr-anfang  variable gr-ende

: sug ( -- a u )  4239 wert@ ;
: sug-in? ( liste-a liste-u -- f )  4239 -rot wert-in? ;
: q-von ( -- n )  4101 wert@ 2dup qjjjj? if qjjjj>n else 2drop 0 then ;

: einzelfelder-fall ( i -- )     \ Formatregeln je Feld
  dup ort
  dup f-fk case
    3006 of dup f$ cdm055? 0= if 055 s" Format n[n][n].n[n][n].n[n][n]" fehler! then endof
    3010 of dup f$ zeitstempel? 0= if 060 s" Format JJJJMMTThhmmss" fehler! then endof
    3105 of dup f$ dup 6 13 within >r digits? r> and 0= if 053 s" Format: 6 bis 12 Ziffern" fehler! then endof
    3103 of dup f$ datum021? 0= if 021 s" Format JJJJMMTT (oder JJJJMM00, JJJJ0000, 00000000)" fehler! then endof
    3119 of dup f$ versid? 0= if 054 s" Versicherten-ID: Format oder Pruefziffer falsch" fehler! then
            dup f$ s" T555558879" compare 0= if 537 s" unzulaessige Versicherten-ID" fehler! then endof
    3673 of dup f$ icd? 0= if 022 s" ICD-Code: Format ann, ann.n, ann.nn, ann.n-" fehler! then
            dup f$ s" Z01.7" compare 0= if 860 s" Dauerdiagnose darf nicht Z01.7 sein" warnung! then endof
    6001 of dup f$ icd? 0= if 022 s" ICD-Code: Format ann, ann.n, ann.nn, ann.n-" fehler! then endof
    4101 of dup f$ qjjjj? 0= if 016 s" Format QJJJJ" fehler! then endof
    4104 of dup f$ vknr017? 0= if 017 s" VKNR: Format nnmmm mit gueltiger KV-Kennung" fehler! then
            dup f$ s" 74799" compare 0= if 212 s" VKNR 74799 darf nicht an die KV uebermittelt werden" fehler! then endof
    4125 of dup f$ zwei-daten? 0= if 058 s" Format JJJJMMTTJJJJMMTT" fehler! then endof
    4233 of dup f$ zwei-daten? 0= if 058 s" Format JJJJMMTTJJJJMMTT" fehler! then endof
    4225 of dup f$ asv? 0= if 059 s" ASV-Teamnummer: Format oder Pruefziffer falsch" hinweis! then endof
    4226 of dup f$ asv? 0= if 059 s" ASV-Teamnummer: Format oder Pruefziffer falsch" hinweis! then endof
    5100 of dup f$ asv? 0= if 059 s" ASV-Teamnummer: Format oder Pruefziffer falsch" hinweis! then endof
    4241 of dup f$ lanr? 0= if 056 s" LANR: Format oder Pruefziffer falsch" warnung! then
            dup f$ s" 888888800" compare 0= if 762 s" Ersatzwert 888888800 ist obsolet" fehler! then endof
    4242 of dup f$ lanr? 0= if 056 s" LANR: Format oder Pruefziffer falsch" warnung! then
            dup f$ s" 888888800" compare 0= if 762 s" Ersatzwert 888888800 ist obsolet" fehler! then endof
    4299 of dup lanr-felder endof
    4248 of dup f$ pseudo-lanr? 0= if 064 s" Pseudo-LANR: Format 555555nff" warnung! then endof
    4249 of dup f$ pseudo-lanr? 0= if 064 s" Pseudo-LANR: Format 555555nff" warnung! then endof
    5101 of dup f$ pseudo-lanr? 0= if 063 s" Pseudo-LANR: Format 555555nff" fehler! then endof
    4253 of dup f$ gnr042? 0= if 042 s" GNR: Format nnnnn[G-alpha]" fehler! then endof
    4256 of dup f$ gnr042? 0= if 042 s" GNR: Format nnnnn[G-alpha]" fehler! then endof
    5036 of dup f$ gnr042? 0= if 042 s" GNR: Format nnnnn[G-alpha]" fehler! then endof
    5003 of dup f$ bsnr049? 0= if 049 s" (N)BSNR: Format kknnnnnmm" fehler! then endof
    5006 of dup f$ hhmm? 0= if 005 s" Uhrzeit: Format HHMM (0000-2359)" fehler! then endof
    5012 of dup f$ zahl 999999 > if 710 s" Sachkosten unrealistisch hoch (> 999999 Cent)" warnung! then endof
    5021 of dup f$ dup 4 = if digits? else 2drop false then 0= if 027 s" Format JJJJ" fehler! then endof
    5001 of 4121 s" 3" wert= if
              dup f$ gnr035? 0= if 496 s" GOAe-Nummer: Format gemaess Regel 035" fehler! then
            else 4121 s" 0,1,2" wert-in? if
              dup f$ gnr042? 0= if 497 s" GNR: Format nnnnn[G-alpha] (Regel 042)" fehler! then
            then then endof
    5041 of q-von 20261 < if dup f$ s" R,L,B" in-liste? 0= if 706 s" Seitenlokalisation: R, L oder B" fehler! then
            else dup f$ s" L,R" in-liste? 0= if 706 s" Seitenlokalisation ab Q1/2026: L oder R" fehler! then then endof
    5098 of dup f$ bsnr-819? 0= if 823 s" (N)BSNR passt nicht zu Formatregel 049/061/062" fehler! then endof
    5099 of dup f$ s" 888888800" compare 0= if 762 s" Ersatzwert 888888800 ist obsolet" fehler! then
            s" 28" sug-in? if dup f$ lanr? 0= if 725 s" LANR: Formatregel 056 verletzt" warnung! then
            else dup f$ lanr? 0= if 725 s" LANR: Formatregel 050 verletzt" fehler! then then endof
  endcase drop ;

: z01.7? ( i -- f )  f$ s" Z01.7" compare 0= ;
: diagnose-ausser-z017? ( -- f )   \ mind. ein 6001 mit Inhalt <> Z01.7
  false re @ rs @ ?do i f-fk 6001 = if i z01.7? 0= or then loop ;

\ Leistungsgruppen: ab 5001 bis zum naechsten 5001/5000 oder Ende des Leistungsteils
: gr-anzahl ( fk -- n )  0 gr-ende @ gr-anfang @ ?do over i f-fk = if 1+ then loop nip ;
: gruppe-ende ( i -- j )
  1+ begin dup re @ < if dup f-fk dup 5001 <> over 5000 <> and swap 6001 <> and
                           over f-fk 3673 <> and else false then
     while 1+ repeat ;

: gop-regeln ( -- )               \ Regeln je GNR (Feld 5001)
  gr-anfang @ f$
  2dup s" 11511,11512,11516,11517,11518,11521,11721,11722,11723,11728,11730,11731" gop-in? if
    5077 gr-anzahl 1 <> 5079 gr-anzahl 1 <> or if
      770 s" 5077 und 5079 muessen genau einmal vorhanden sein" warnung! then then
  2dup s" 11233" gop-in? if
    5079 gr-anzahl 0= if 816 s" 5079 erforderlich" warnung! then
    5077 gr-anzahl 5078 gr-anzahl + if 816 s" 5077/5078 duerfen nicht vorhanden sein" warnung! then then
  2dup s" 11302,11303,11305,19402" gop-in? if
    diagnose-ausser-z017? 0= if 828 s" mind. ein 6001 ungleich Z01.7 erforderlich" warnung! then
    5077 gr-anzahl 5079 gr-anzahl + if 828 s" 5077/5079 duerfen nicht uebertragen werden" warnung! then then
  2dup s" 19421,19451,19452" gop-in? if
    5077 gr-anzahl 1 <> diagnose-ausser-z017? 0= or if
      829 s" genau ein 5077 je GNR und ein 6001 ungleich Z01.7 erforderlich" warnung! then
    5079 gr-anzahl 1 <> if 847 s" 5079 sollte genau einmal vorhanden sein" hinweis! then then
  2dup s" 19424,19453,19456" gop-in? if
    5077 gr-anzahl 0= diagnose-ausser-z017? 0= or if
      830 s" mind. ein 5077 je GNR und ein 6001 ungleich Z01.7 erforderlich" warnung! then
    5079 gr-anzahl 0= if 848 s" 5079 sollte mind. einmal vorhanden sein" hinweis! then then
  2dup s" 11522,11513,11724,11725,11726,11727,11732" gop-in? if
    5077 gr-anzahl 0= 5079 gr-anzahl 0= or if 834 s" 5077 und 5079 je GNR erforderlich" warnung! then then
  2dup s" 32901,32902,32904,32906,32908,32910,32911" gop-in? if
    diagnose-ausser-z017? 0= 5077 gr-anzahl 5079 gr-anzahl + or if
      843 s" 6001 ungleich Z01.7 erforderlich, 5077/5079 unzulaessig" warnung! then then
  2dup s" 32915,32916,32917,32918" gop-in? if
    diagnose-ausser-z017? 0= 5077 gr-anzahl 5079 gr-anzahl + or if
      854 s" 6001 ungleich Z01.7 erforderlich, 5077/5079 unzulaessig" warnung! then then
  2drop
  5050 gr-anzahl 0<>  5005 gr-anzahl 0<> and if
    5050 gr-anzahl  0 gr-ende @ gr-anfang @ ?do i f-fk 5005 = if drop i f$ zahl then loop
    <> if 894 s" Anzahl 5050 muss dem Multiplikator 5005 entsprechen" warnung! then then
  5042 gr-anzahl 0<> 5005 gr-anzahl 0<> and if 707 s" bei 5042 ist kein Multiplikator 5005 zulaessig" warnung! then
  gr-ende @ gr-anfang @ ?do                   \ 891/893 je HGNC-Gensymbol
    i f-fk 5077 = if
      0 gr-ende @ i 1+ ?do i f-fk 5077 = if leave then i f-fk 5078 = if 1+ then loop
      i f$ s" 999999" compare 0= if 0= if i ort 891 s" 5077 = 999999: 5078 erforderlich" warnung! then
      else if i ort 893 s" 5077 <> 999999: 5078 unzulaessig" warnung! then then
    then
  loop ;

: leistungen-regeln ( -- )
  0 re @ rs @ ?do                             \ ( letzter-5000 )
    i f-fk 5000 = if drop i then
    i f-fk 5001 = if
      i gr-anfang !  i gruppe-ende gr-ende !
      i ort  dup 0= if 9004 s" GNR ohne vorangehenden Leistungstag 5000" fehler! then
      gop-regeln
    then
  loop drop ;

variable heute-v  variable frueh
: tag-regeln ( -- )               \ Regeln mit Bezug auf die Leistungstage
  heute heute-v !  0 frueh !
  re @ rs @ ?do
    i f-fk 5000 = if
      i ort  i f$ zahl                                      ( tag )
      heute-v @ if dup heute-v @ > if 304 s" Leistungstag liegt in der Zukunft" fehler! then then
      3103 hat? if 3103 wert@ zahl over > if 313 s" Leistungstag vor dem Geburtsdatum" fehler! then then
      4110 hat? if 4110 wert@ zahl over < if 315 s" Leistungstag nach Ende des Versicherungsschutzes 4110" warnung! then then
      q-von if dup jjjjq q-von <> if 324 s" Leistungstag nicht im Quartal 4101" fehler! then then
      4125 hat? if 4125 wert@ 16 = if dup 8 zahl swap 8 + 8 zahl  2 pick -rot 1+ within 0=
                     if 363 s" Leistungstag ausserhalb Gueltigkeitszeitraum 4125" fehler! then
                   else drop then then
      frueh @ 0= over frueh @ < or if dup frueh ! then
      drop
    then
  loop
  frueh @ if                       \ 886, 899, 900: gegen den fruehesten Leistungstag
    4115 hat? if 4115 fk-ort frueh @ tage 4115 wert@ zahl tage - 60 >
      if 886 s" Tag der Terminvermittlung mehr als 60 Tage vor dem Leistungstag" warnung! then then
    4214 hat? if 4214 fk-ort 4214 wert@ zahl frueh @ >
      if 899 s" 4214 liegt nach dem Leistungstag" warnung! then
      frueh @ tage 4214 wert@ zahl tage - 60 >
      if 900 s" 4214 liegt mehr als 60 Tage vor dem Leistungstag" warnung! then then
  then
  heute-v @ if
    3103 hat? if 3103 wert@ zahl heute-v @ > if 3103 fk-ort 304 s" Geburtsdatum liegt in der Zukunft" fehler! then then
    4115 hat? if 4115 wert@ zahl heute-v @ > if 4115 fk-ort 887 s" 4115 liegt nach dem Systemdatum" warnung! then then
  then ;

: diagnosen-regeln ( -- )
  re @ rs @ ?do
    i f-fk 6001 = if i z01.7? if         \ 856: zugehoerige Diagnosensicherheit
      i ort  false
      re @ i 1+ ?do i f-fk dup 6001 = swap 3673 = or if leave then
        i f-fk 6003 = if drop true i f$ s" G" compare if i ort 856 s" bei Z01.7 muss 6003 = G sein" warnung! then then
      loop
      0= if 856 s" bei Z01.7 muss 6003 = G vorhanden sein" warnung! then
    then then
  loop ;

variable in-gruppe
: konnektor-zu-bsnr? ( a u -- f )   \ hat die (N)BSNR a u im besa-Satz ein Feld 0224?
  besa-satz @ dup 0< if drop 2drop false exit then
  false in-gruppe !
  dup s-ende swap s-anfang ?do
    i f-fk dup 0201 = swap 0213 = or if 2dup i f$ compare 0= in-gruppe ! then
    i f-fk 0224 = in-gruppe @ and if 2drop true unloop exit then
  loop 2drop false ;

: kontext-regeln ( -- )
  satzart @ >r
  \ --- Satzart und Scheinuntergruppe
  4239 fk-ort  4239 hat? if
  r@ sa-0101 = if s" 00" sug-in? 0= if 331 s" Satzart 0101: SUG 00 erwartet" fehler! then then
  r@ sa-0102 = if s" 20,21,23,24,26,27,28" sug-in? 0= if 356 s" Satzart 0102: SUG 20, 21, 23, 24, 26, 27, 28" fehler! then then
  r@ sa-0103 = if s" 30,31,32" sug-in? 0= if 426 s" Satzart 0103: SUG 30, 31, 32" fehler! then then
  r@ sa-0104 = if s" 41-46" sug-in? 0= if 427 s" Satzart 0104: SUG 41-46" fehler! then then
  then
  \ --- Diagnosen, Adresse, Arzt, Ort der Leistungserbringung
  6001 hat? 3673 hat? or 0= if satz-ort 486 s" weder 6001 noch 3673 vorhanden" fehler! then
  3112 hat? 3121 hat? or 0= if
    4109 hat? 3114 s" D" wert= 0= 3114 hat? and 3124 s" D" wert= 0= 3124 hat? and or and 0= if
      satz-ort 479 s" weder PLZ 3112 noch Postfach-PLZ 3121 vorhanden" fehler! then then
  r@ sa-0103 <> if
    5099 hat? 5101 hat? or 0= if satz-ort 837 s" weder 5099 noch 5101 vorhanden" fehler! then
    5098 hat? 5102 hat? or 0= if satz-ort 859 s" weder 5098 noch 5102 vorhanden" fehler! then
  then
  \ --- eGK / Versichertenstammdaten
  4109 hat? 3119 hat? and s" 21,27,28" sug-in? 0= and 3006 hat? 0= and if satz-ort 307 s" 3006 (CDM-Version) erforderlich" warnung! then
  3006 hat? if 3006 fk-ort
    3006 wert@ cdm-vergleich 0> if 308 s" CDM-Version groesser 5.2.0" warnung! then
    3006 wert@ cdm-vergleich 0< 0= 3116 hat? 0= and if 774 s" ab CDM 5.2.0 ist WOP 3116 erforderlich" fehler! then
    3012 hat? if 3006 s" 5.2.0" wert= if 3012 s" 99999" wert= if 3012 fk-ort 309 s" 3012 darf nicht 99999 sein" warnung! then
                 else 3012 s" 99999" wert= 0= if 3012 fk-ort 310 s" 3012 muss 99999 sein" warnung! then then then
  then
  3010 hat? if 3010 fk-ort
    4109 hat? 0= if 876 s" 3010 vorhanden, aber 4109 fehlt" warnung! then
    q-von if 3010 wert@ drop 8 zahl jjjjq q-von <> if 864 s" 3010 nicht im Quartal 4101" warnung! then then
  then
  4109 hat? if 4109 fk-ort
    3006 hat? if 4133 hat? 0= if 775 s" 4133 (Versicherungsschutz-Beginn) erforderlich" fehler! then
                 3114 hat? 3124 hat? or 0= if 784 s" 3114 und/oder 3124 erforderlich" fehler! then then
    s" 21,27,28" sug-in? 0= 4134 hat? 0= and if 777 s" Kostentraegername 4134 erforderlich" fehler! then
    4104 wert@ dup 5 = if drop 2 + 3 zahl else 2drop 0 then dup 800 < if
      3119 hat? 0= if 776 s" 3119 erforderlich (4104 Stellen 3-5 < 800)" fehler! then
    else 3105 hat? 3119 hat? or 0= if 776 s" 3105 oder 3119 erforderlich" fehler! then then
    3006 hat? 0= if 800 < if 790 s" ohne 3006: 4104 Stellen 3-5 muessen >= 800 sein" fehler! then else drop then
    q-von if 4109 wert@ zahl jjjjq q-von <> if 480 s" 4109 nicht im Quartal 4101" warnung! then then
  then
  4112 hat? if 4109 hat? 3010 hat? or 4108 hat? or 3006 hat? or if 4112 fk-ort
    895 s" bei eEB (4112) sind 4109, 3010, 4108, 3006 unzulaessig" warnung! then then
  \ --- Kostentraeger
  4131 s" 07,08" wert-in? 4106 s" 01,09" wert-in? 0= and if 4106 fk-ort 778 s" bei 4131 = 07/08 muss 4106 = 01 oder 09 sein" fehler! then
  4131 s" 06" wert= 4106 s" 02,09" wert-in? 0= and if 4106 fk-ort 779 s" bei 4131 = 06 muss 4106 = 02 oder 09 sein" fehler! then
  4131 s" 04" wert= 4106 s" 00,09" wert-in? 0= and if 4106 fk-ort 780 s" bei 4131 = 04 muss 4106 = 00 oder 09 sein" fehler! then
  4131 s" 09" wert= 4106 s" 00,09" wert-in? 0= and if 4106 fk-ort 818 s" bei 4131 = 09 soll 4106 = 00 oder 09 sein" warnung! then
  4109 hat? 4131 s" 00" wert= and 4106 s" 00,09" wert-in? 0= and if 4106 fk-ort 827 s" 4106 muss 00 oder 09 sein" warnung! then
  \ --- Scheinuntergruppen-abhaengige Regeln
  s" 27,28" sug-in? 4221 hat? <> if 4239 fk-ort 404 s" 4221 genau dann erforderlich, wenn SUG 27/28" fehler! then
  s" 27,28" sug-in? 4102 hat? 0= and if 4239 fk-ort 405 s" bei SUG 27/28 ist 4102 erforderlich" fehler! then
  s" 28" sug-in? 4221 s" 3" wert= and if 4221 fk-ort 754 s" bei SUG 28 darf 4221 nicht 3 sein" fehler! then
  s" 21" sug-in? 4205 hat? 0= and if 4239 fk-ort 744 s" bei SUG 21 ist 4205 erforderlich" fehler! then
  s" 27" sug-in? 4205 hat? 0= and if 4239 fk-ort 755 s" bei SUG 27 ist 4205 erforderlich" fehler! then
  s" 31" sug-in? if 4218 hat? 4205 hat? 4207 hat? or 4208 hat? or and 0= if
    4239 fk-ort 746 s" bei SUG 31: 4218 und 4205/4207/4208 erforderlich" fehler! then then
  s" 28" sug-in? 0= 4209 hat? and if 4209 fk-ort 756 s" 4209 nur bei SUG 28 zulaessig" warnung! then
  s" 30" sug-in? 4233 hat? 0= and if 4239 fk-ort 354 s" bei SUG 30 ist 4233 erforderlich" fehler! then
  4217 hat? 4225 hat? or if s" 27" sug-in? 0= 4217 hat? 4225 hat? and or if
    4217 fk-ort 431 s" 4217 oder 4225 nur bei SUG 27 und nur eines davon" fehler! then then
  4229 hat? s" 27,28" sug-in? 0= and if 4229 fk-ort 432 s" 4229 nur bei SUG 27/28 zulaessig" fehler! then
  \ --- Ueberweisung
  r@ sa-0102 = if
    4218 hat? 1 and 4219 hat? 1 and + 4226 hat? 1 and + 1 <> if satz-ort
      328 s" genau eines der Felder 4218, 4219, 4226 erforderlich" fehler! then
    s" 27,28" sug-in? 0= 4220 hat? 0= and if satz-ort 320 s" 4220 (Ueberweisung an) erforderlich" fehler! then
    4217 hat? if 4217 fk-ort 4217 wert@ over 2 s" 35" compare 0= if bsnr061? else bsnr049? then 0= if
      820 s" (N)BSNR des Erstveranlassers: Formatregel 049/061" fehler! then then
    4218 hat? if 4218 fk-ort 4218 wert@ over 2 s" 35" compare 0= if bsnr061?
      else over 2 s" 77" compare 0= if s" 777777700" compare 0= else bsnr049? then then 0= if
      821 s" (N)BSNR des Ueberweisers: Formatregel 049/061 bzw. 777777700" fehler! then then
  then
  r@ sa-0103 = 4218 hat? and if 4218 fk-ort 4218 wert@ bsnr049? 0= if
    822 s" (N)BSNR des Ueberweisers: Formatregel 049" fehler! then then
  4217 hat? 4218 hat? and if 4217 wert@ 4218 wert@ compare 0= if 4218 fk-ort
    319 s" 4218 darf nicht gleich 4217 sein" fehler! then then
  4225 hat? 4241 hat? 4248 hat? or 0= and if 4225 fk-ort 844 s" zu 4225 ist 4241 oder 4248 erforderlich" warnung! then
  4226 hat? if s" 28" sug-in? if 4242 hat? 0= 4249 hat? or if 4226 fk-ort 845 s" bei SUG 28: 4242 erforderlich, 4249 unzulaessig" warnung! then
              else 4242 hat? 4249 hat? or 0= if 4226 fk-ort 845 s" zu 4226 ist 4242 oder 4249 erforderlich" warnung! then then then
  4103 s" 3" wert= if r@ sa-0102 <> if 4103 fk-ort 870 s" Vermittlungsart 3 nur in Satzart 0102" fehler! then
                      4115 hat? 0= if 4103 fk-ort 877 s" bei 4103 = 3 ist 4115 erforderlich" warnung! then then
  \ --- Psychotherapie
  4252 hat? 4253 hat? <> if 4252 fk-ort 850 s" 4252 und 4253 nur gemeinsam" fehler! then
  4255 hat? 4256 hat? <> if 4255 fk-ort 851 s" 4255 und 4256 nur gemeinsam" fehler! then
  4255 hat? 4252 hat? 0= and if 4255 fk-ort 852 s" zu 4255 ist 4252 erforderlich" fehler! then
  4235 hat? 4252 hat? 4253 hat? and 0= and if 4235 fk-ort 897 s" zu 4235 sollten 4252 und 4253 vorhanden sein" warnung! then
  re @ rs @ ?do i f-fk dup 4253 = swap 4256 = or if
      re @ i 1+ ?do i f-fk j f-fk = if i f$ j f$ compare 0= if i ort 853 s" GNR mehrfach uebertragen" fehler! then then loop
  then loop
  \ --- Abgleich mit dem besa-Satz und SUG 28
  re @ rs @ ?do
    i f-fk 5098 = if i ort
      s" 28" sug-in? if 4218 hat? if 4218 wert@ i f$ compare if 720 s" bei SUG 28 muss 5098 = 4218 sein" fehler! then then then
      vorquartal? 0= if 0201 i f$ besa-satz @ wert-im-satz? 0= if
        s" 28" sug-in? if 724 s" (N)BSNR nicht im besa-Satz" warnung! else 716 s" (N)BSNR nicht im besa-Satz" fehler! then then then
    then
    i f-fk 5099 = if i ort
      s" 28" sug-in? if 4242 hat? if 4242 wert@ i f$ compare if 721 s" bei SUG 28 muss 5099 = 4242 sein" fehler! then then then
      i f$ s" 999999900" compare if 0212 i f$ besa-satz @ wert-im-satz? 0= if
        s" 28" sug-in? if 723 s" LANR nicht im besa-Satz" warnung! else vorquartal? 0= if 715 s" LANR nicht im besa-Satz" fehler! then then then then
    then
    i f-fk 5100 = if i ort
      s" 28" sug-in? 4226 hat? and if 4226 wert@ i f$ compare if 838 s" bei SUG 28 muss 5100 = 4226 sein" hinweis! then then
      vorquartal? 0= if 0222 i f$ besa-satz @ wert-im-satz? 0= if 789 s" ASV-Teamnummer nicht im besa-Satz" warnung! then then
    then
    i f-fk 5101 = if i ort
      vorquartal? 0= if 0223 i f$ besa-satz @ wert-im-satz? 0= if 839 s" Pseudo-LANR nicht im besa-Satz" hinweis! then then
    then
  loop
  \ 869: eGK mit CDM 5.2.0 -> Konnektor-Produkttypversion (0224) zur (N)BSNR im besa
  vorquartal? 0= 3010 hat? and 3006 s" 5.2.0" wert= and if
    false re @ rs @ ?do i f-fk 5098 = if i f$ konnektor-zu-bsnr? or then loop
    0= if 3010 fk-ort 869 s" zu keiner (N)BSNR aus 5098 gibt es 0224 im besa-Satz" warnung! then
  then
  r> drop ;

: fall-pruefen ( s -- )
  dup s-art satzart !
  re @ rs @ ?do i einzelfelder-fall loop
  re @ rs @ ?do i wertelisten-pruefen loop
  leistungstage-pruefen
  leistungen-regeln
  tag-regeln
  diagnosen-regeln
  kontext-regeln
  drop ;

\ -------------------------------------------------------------------
\  Ablauf
\ -------------------------------------------------------------------

: satz-pruefen ( s -- )
  dup im-satz
  dup s-art st-fuer ?dup if
    st-tab !
    re @ rs @ ?do i ort i feldtabelle-pruefen loop
    reihenfolge-pruefen  mussfelder-pruefen
  then
  dup s-art case
    sa-con0 of re @ rs @ ?do i wertelisten-pruefen loop endof
    sa-besa of re @ rs @ ?do i container-feldregeln  i wertelisten-pruefen loop besa-regeln endof
    sa-rvsa of re @ rs @ ?do i container-feldregeln  i wertelisten-pruefen loop rvsa-regeln endof
    sa-adt0 of re @ rs @ ?do i wertelisten-pruefen loop adt0-regeln endof
    dup fall? if over fall-pruefen then
  endcase
  drop ;

: hat-wert? ( fk a u -- f )   \ irgendein Feld fk im Satz mit Inhalt a u?
  re @ rs @ ?do 2 pick i f-fk = if 2dup i f$ compare 0= if 2drop drop true unloop exit then then loop
  2drop drop false ;

: datei-regeln ( -- )             \ satzuebergreifend
  con0-satz @ 0< if exit then
  con0-satz @ im-satz  9132 s" 1" hat-wert?        ( adt-angekuendigt )
  dup besa-satz @ 0< 0= and if
    besa-satz @ im-satz  0201 hat? rvsa-satz @ 0< and if satz-ort
      743 s" ADT-Datenpaket und (N)BSNR vorhanden: rvsa-Satz erforderlich" warnung! then
  then
  adt0-satz @ 0< 0= swap 0= and if con0-satz @ im-satz 9132 fk-ort
    9007 s" ADT-Datenpaket vorhanden, aber 9132 = 1 fehlt" warnung! then ;

: abschnitt-406 ( -- )            \ Regel 406: 4102 <= Erstellungsdatum (con0 9103)
  #saetze @ 0 ?do i s-art fall? if i im-satz
    4102 hat? if 9103 con0-satz @ wert-aus-satz dup if zahl 4102 wert@ zahl < if
      4102 fk-ort 406 s" Ausstellungsdatum nach dem Erstellungsdatum der Datei" fehler! then
    else 2drop then then
  then loop ;

: zuruecksetzen ( -- )
  0 #F !  0 #W !  0 #I !  0 #treffer !  0 #felder !  0 #saetze ! ;

: .zusammenfassung ( a u -- )
  cr ." Datei:     " type cr
  ." Zeilen:    " #zeilen @ . ."   Saetze: " #saetze @ .
  ."   Behandlungsfaelle: " 0 #saetze @ 0 ?do i s-art fall? if 1+ then loop . cr
  ." Meldungen: " #F @ . ." Fehler, " #W @ . ." Warnungen, " #I @ . ." Hinweise" cr
  #F @ if ." Ergebnis:  FEHLERHAFT" else ." Ergebnis:  OK" then
  #W @ if ."  (mit Warnungen)" then cr ;

: pruefe ( a u -- )
  zuruecksetzen
  2dup datei-laden
  zeilen-lesen
  saetze-bilden
  satzfolge-pruefen
  #saetze @ 0 ?do i satz-pruefen loop
  datei-regeln
  abschnitt-406
  puffer @ free throw  felder @ free throw  saetze @ free throw
  still @ if 2drop else .zusammenfassung then ;
