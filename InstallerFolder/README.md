# Student Management System Installer

This installer package contains everything needed to set up the Student Management System, including the application, MySQL database, and all necessary configurations.

## Contents

- `MAB_EDP_Csharp_Demo_Installer.iss` - Inno Setup script for creating the installer
- `mysql-installer-community-8.0.42.0.msi` - MySQL Server installer
- `setup_mysql.bat` - Script to automate MySQL configuration
- `Database/` - Contains database initialization scripts
  - `init_db.sql` - Initial database setup
  - `studentmanagement.sql` - Full database schema
- `Config/` - Contains configuration files
  - `connection.config` - Database connection settings

## Installation Process

The installer will:
1. Install the Student Management System application
2. Install MySQL Server 8.0
3. Configure MySQL with the following credentials:
   - Root password: `mike`
   - Application database user: `studentapp`
   - Application database password: `studentpass`
4. Create and initialize the database
5. Set up the connection configuration
6. Create desktop and start menu shortcuts

## Database Configuration

### MySQL Root Access
- Username: `root`
- Password: `mike`

### Application Database Access
- Database: `studentmanagement`
- Username: `studentapp`
- Password: `studentpass`

## Building the Installer

To build the installer:
1. Open Inno Setup Compiler
2. Open `MAB_EDP_Csharp_Demo_Installer.iss`
3. Click "Build" > "Compile"
4. The installer will be created in the `Output` folder

## Requirements

- Windows 10 or later
- .NET 6.0 Runtime
- Administrator privileges for installation

## Notes

- The MySQL root password is set to 'mike'
- The application uses a separate database user for security
- All database scripts are included in the installation
- The installer will automatically configure all necessary settings

## Troubleshooting

If you encounter any issues during installation:
1. Ensure you have administrator privileges
2. Check that MySQL is not already installed
3. Verify that port 3306 is available
4. Check the Windows Event Viewer for any error messages

## Support

For any issues or questions, please contact your system administrator or Professor Mike. 