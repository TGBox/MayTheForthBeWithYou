@echo off
rem Prueft eine oder mehrere KVDT-Dateien:  pruefe.bat testdaten\00_gueltig.kvdt
chcp 65001 >nul
cd /d "%~dp0"
if not defined GFORTHPATH set "GFORTHPATH=.:/cygdrive/c/Program Files (x86)/gforth:/cygdrive/c/Program Files/gforth"
gforth pruefe.fs %*
