@echo off
REM -- MySQL Database Initialization Script --
REM This script sets up the database with the required password and schema

set MYSQL_PATH="C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql.exe"

echo Initializing Student Management System Database...
echo This will:
echo 1. Set the root password to 'mike'
echo 2. Create the studentmanagement database
echo 3. Import the schema and sample data
echo.
echo Please ensure MySQL Server is running before continuing.
echo.
pause

echo.
echo Setting up database...
%MYSQL_PATH% -u root --connect-expired-password < "%~dp0init_db.sql"

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Database initialization completed successfully!
    echo The database is now ready to use with:
    echo - Username: root
    echo - Password: mike
    echo - Database: studentmanagement
) else (
    echo.
    echo Error: Database initialization failed.
    echo Please check that:
    echo 1. MySQL Server is running
    echo 2. You have administrator privileges
    echo 3. The MySQL path is correct
)

echo.
pause 