@REM BATCH FILE
@ECHO off

rem =========================================================
rem Please run this BAT script on CMD prompt as Administrator
rem For Installingl/Uninstsalling C# .NET 5+ Services only
rem
rem Declare global variables here.
rem =========================================================

set SERVICE_EXE="C:\Users\sp\source\repos\LinuxService\LinuxService\bin\Release\net5.0\publish\LinuxService.exe"
set SERVICE_DES="Sample Windows/Linux Service"
set SERVICE_SID="Sample .NET 5 Working Service"

rem =========================================================
rem Install / Uninstall Windows Service
rem =========================================================

echo Checking system service: [%SERVICE_SID%]
sc query %SERVICE_SID% | findstr /c:"RUNNING" /c:"STOPPED"
if "%ERRORLEVEL%"=="0" goto UninstallPrompt

rem ====== Install Service ===========================
echo Service not installed. Install and start service?
set /P c=Answer [y/n]? 
if /I "%c%" EQU "Y" goto :Install
goto:eof

:Install
sc create %SERVICE_SID% binpath= %SERVICE_EXE% start= delayed-auto
sc description %SERVICE_SID% %SERVICE_DES%
sc start %SERVICE_SID%
goto:eof

rem ====== Uninstall Service ========================
:UninstallPrompt
echo Service installed. Stop and uninstall service?
set /P c=Answer [y/n]? 
if /I "%c%" EQU "Y" goto :Uninstall
goto:eof

:Uninstall
sc stop %SERVICE_SID%
sc delete %SERVICE_SID%
goto:eof