@echo off
setlocal enabledelayedexpansion

REM Set MySQL root password
set MYSQL_ROOT_PASSWORD=mike

REM Wait for MySQL service to start
timeout /t 10 /nobreak

REM Initialize database
"C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql.exe" -u root -p%MYSQL_ROOT_PASSWORD% < "%~dp0Database\init_db.sql"
"C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql.exe" -u root -p%MYSQL_ROOT_PASSWORD% < "%~dp0Database\studentmanagement.sql"

REM Create application user
"C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql.exe" -u root -p%MYSQL_ROOT_PASSWORD% -e "CREATE USER IF NOT EXISTS 'studentapp'@'localhost' IDENTIFIED BY 'studentpass';"
"C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql.exe" -u root -p%MYSQL_ROOT_PASSWORD% -e "GRANT ALL PRIVILEGES ON studentmanagement.* TO 'studentapp'@'localhost';"
"C:\Program Files\MySQL\MySQL Server 8.0\bin\mysql.exe" -u root -p%MYSQL_ROOT_PASSWORD% -e "FLUSH PRIVILEGES;"

REM Update connection config
echo Updating connection configuration...
powershell -Command "(Get-Content '%~dp0Config\connection.config') -replace 'Server=.*;', 'Server=localhost;' -replace 'User=.*;', 'User=studentapp;' -replace 'Password=.*;', 'Password=studentpass;' | Set-Content '%~dp0Config\connection.config'"

echo MySQL setup completed successfully!
pause 