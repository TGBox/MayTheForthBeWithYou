\ Kommandozeilen-Einstieg (gforth):   gforth pruefe.fs datei1.kvdt [datei2.kvdt ...]
\ next-arg ist gforth-spezifisch; der Pruefer selbst (kvdt.fs) ist ANS-Forth.

include kvdt.fs

: main ( -- )
  next-arg dup 0= if 2drop ." Aufruf: gforth pruefe.fs <datei.kvdt> ..." cr exit then
  begin dup while pruefe next-arg repeat 2drop ;

main bye
