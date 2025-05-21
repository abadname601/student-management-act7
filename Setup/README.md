# Student Management System Installer

This installer packages the Student Management System application along with MySQL configuration.

## Prerequisites

1. Windows 10 or later
2. .NET 6.0 Runtime or later
3. MySQL Server 8.0 or later

## Installation Steps

1. Download the installer (`StudentManagementSystem_Setup.exe`)
2. Run the installer as administrator
3. Follow the installation wizard:
   - Choose installation directory
   - Configure MySQL connection details
   - Create desktop shortcut (optional)

## Post-Installation Setup

1. The installer will create a configuration file with your MySQL connection details
2. Database scripts will be installed in the `Database` folder
3. Run the application and log in with default credentials:
   - Admin: username = 'admin', password = 'admin'
   - Teacher: username = 'teacher1', password = 'teacher1'
   - Student: username = 'student1', password = 'student1'

## Troubleshooting

If you encounter any issues:
1. Check MySQL service is running
2. Verify MySQL connection details in `Config/connection.config`
3. Ensure database is properly initialized
4. Check application logs in the installation directory

## Support

For support, please create an issue in the GitHub repository or contact the system administrator. 