@echo off
setlocal
echo Building NoteTxtMd (.NET Framework 4.8)...

set MSBUILD="C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe"
if not exist %MSBUILD% (
    set MSBUILD="C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
)

if not exist %MSBUILD% (
    echo [ERROR] MSBuild for .NET Framework 4.0/4.8 was not found.
    pause
    exit /b 1
)

%MSBUILD% "%~dp0NoteTxtMd.csproj" /p:Configuration=Release /v:m

if %ERRORLEVEL% EQU 0 (
    echo.
    echo [SUCCESS] Build succeeded!
    echo Output binary: "%~dp0bin\Release\NoteTxtMd.exe"
) else (
    echo.
    echo [ERROR] Build failed.
)

endlocal
