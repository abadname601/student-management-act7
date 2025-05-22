@echo off
echo Building Student Management System...

REM Create necessary directories
if not exist "Setup\Output" mkdir "Setup\Output"
if not exist "bin\Release\net6.0-windows\win-x64\publish" mkdir "bin\Release\net6.0-windows\win-x64\publish"

REM Build the application
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true

REM Build the installer
"C:\Program Files (x86)\Inno Setup 6\ISCC.exe" "Setup\setup.iss"

echo.
echo Build completed!
echo The installer can be found in Setup\Output\StudentManagementSystem_Setup.exe
echo.
pause 