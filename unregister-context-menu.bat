@echo off
setlocal
echo Removing "Open with NoteTxtMd" from Windows Explorer context menu...

reg delete "HKCU\Software\Classes\Directory\shell\NoteTxtMd" /f >nul 2>&1
reg delete "HKCU\Software\Classes\Directory\Background\shell\NoteTxtMd" /f >nul 2>&1
reg delete "HKCU\Software\Classes\*\shell\NoteTxtMd" /f >nul 2>&1

echo.
echo [SUCCESS] "Open with NoteTxtMd" context menu items removed.
pause
endlocal
