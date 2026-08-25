@echo off
setlocal
echo Registering "Open with NoteTxtMd" in Windows Explorer context menu...

set "EXEPATH=%~dp0bin\Release\NoteTxtMd.exe"

if not exist "%EXEPATH%" (
    echo [ERROR] NoteTxtMd.exe was not found at: "%EXEPATH%"
    echo Please run build.bat first.
    pause
    exit /b 1
)

:: 1. Directory context menu (right-click folder)
reg add "HKCU\Software\Classes\Directory\shell\NoteTxtMd" /ve /d "Open with NoteTxtMd" /f >nul
reg add "HKCU\Software\Classes\Directory\shell\NoteTxtMd" /v "Icon" /d "%EXEPATH%" /f >nul
reg add "HKCU\Software\Classes\Directory\shell\NoteTxtMd\command" /ve /d "\"%EXEPATH%\" \"%%1\"" /f >nul

:: 2. Directory Background context menu (right-click empty space in folder)
reg add "HKCU\Software\Classes\Directory\Background\shell\NoteTxtMd" /ve /d "Open with NoteTxtMd" /f >nul
reg add "HKCU\Software\Classes\Directory\Background\shell\NoteTxtMd" /v "Icon" /d "%EXEPATH%" /f >nul
reg add "HKCU\Software\Classes\Directory\Background\shell\NoteTxtMd\command" /ve /d "\"%EXEPATH%\" \"%%V\"" /f >nul

:: 3. File context menu (right-click any file)
reg add "HKCU\Software\Classes\*\shell\NoteTxtMd" /ve /d "Open with NoteTxtMd" /f >nul
reg add "HKCU\Software\Classes\*\shell\NoteTxtMd" /v "Icon" /d "%EXEPATH%" /f >nul
reg add "HKCU\Software\Classes\*\shell\NoteTxtMd\command" /ve /d "\"%EXEPATH%\" \"%%1\"" /f >nul

echo.
echo [SUCCESS] "Open with NoteTxtMd" successfully added to Windows Explorer context menu!
pause
endlocal
