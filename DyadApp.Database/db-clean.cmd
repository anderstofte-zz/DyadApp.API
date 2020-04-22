@echo off
:choice
set /P c=Are you sure you want to drop the database [y/n]? 
if /I "%c%" EQU "y" goto :run_flyway_clean
if /I "%c%" EQU "n" goto :quit
goto :choice

:run_flyway_clean
call flyway clean
pause 
exit

:quit
