@echo off
rem Fuehrt die komplette Testsuite aus.
chcp 65001 >nul
cd /d "%~dp0"
if not defined GFORTHPATH set "GFORTHPATH=.:/cygdrive/c/Program Files (x86)/gforth:/cygdrive/c/Program Files/gforth"
gforth tests.fs
pause
