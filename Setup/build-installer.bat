@echo off
echo Building Student Management System Installer...

REM Compile the installer
"C:\Program Files (x86)\Inno Setup 6\ISCC.exe" "setup.iss"

if errorlevel 1 (
    echo Error building installer
    pause
    exit /b 1
)

echo Installer built successfully!
echo Output location: Output\StudentManagementSystem_Setup.exe
pause 