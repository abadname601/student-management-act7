Student Management System - Installation Guide

Before Installation:
1. Install MySQL Server if not already installed
   - Download from: https://dev.mysql.com/downloads/installer/
   - During installation, you can skip setting a root password
   - Remember the port number (default is 3306)

Installation Steps:
1. Run StudentManagementSystem_Setup.exe
2. Follow the installation wizard
3. When prompted for database configuration:
   - Server: localhost
   - Port: 3306
   - Username: root
   - Password: mike (this will be set automatically)

Database Setup:
1. Open MySQL Command Line or MySQL Workbench
2. Run the initialization script:
   - Location: [Installation Folder]\Database\init.sql
   - This will:
     * Set the root password to 'mike'
     * Create the studentmanagement database
     * Grant necessary privileges
3. Import the database schema and data:
   - Location: [Installation Folder]\Database\studentmanagement.sql
   - This will create all tables and insert initial data

Default Login Credentials:
- Admin: username = 'admin', password = 'admin'


Troubleshooting:
1. If you get database connection errors:
   - Verify MySQL Server is running
   - Check the connection settings in Config/connection.config
   - Make sure the database 'studentmanagement' exists
   - Verify your MySQL credentials (username: root, password: mike)

2. If the application doesn't start:
   - Make sure .NET 6.0 Runtime is installed
   - Check Windows Event Viewer for error messages

For Support:
Contact your system administrator or refer to the project documentation. 