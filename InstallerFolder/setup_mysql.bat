@echo off
setlocal enabledelayedexpansion

REM Set MySQL root password
set MYSQL_ROOT_PASSWORD=mike
set MYSQL_PATH="C:\Program Files\MySQL\MySQL Server 8.0\bin"

REM Wait for MySQL service to be ready
echo Waiting for MySQL service to start...
:WAIT_LOOP
netstat -an | find "3306" > nul
if errorlevel 1 (
    timeout /t 2 /nobreak > nul
    goto WAIT_LOOP
)

REM Initialize database (combined commands for faster execution)
echo Initializing database...
(
    echo CREATE DATABASE IF NOT EXISTS studentmanagement;
    echo USE studentmanagement;
    type "%~dp0Database\init_db.sql"
    type "%~dp0Database\studentmanagement.sql"
    echo CREATE USER IF NOT EXISTS 'studentapp'@'localhost' IDENTIFIED BY 'studentpass';
    echo GRANT ALL PRIVILEGES ON studentmanagement.* TO 'studentapp'@'localhost';
    echo FLUSH PRIVILEGES;
) | "%MYSQL_PATH%\mysql.exe" -u root -p%MYSQL_ROOT_PASSWORD%

REM Update connection config
echo Updating connection configuration...
powershell -Command "(Get-Content '%~dp0Config\connection.config') -replace 'Server=.*;', 'Server=localhost;' -replace 'User=.*;', 'User=studentapp;' -replace 'Password=.*;', 'Password=studentpass;' | Set-Content '%~dp0Config\connection.config'"

echo MySQL setup completed successfully!
pause 