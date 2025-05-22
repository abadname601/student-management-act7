#define MyAppName "Student Management System"
#define MyAppVersion "1.0"
#define MyAppPublisher "Your Name"
#define MyAppExeName "StudentManagementSystem.exe"

[Setup]
AppId={{YOUR-UNIQUE-APP-ID-HERE}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
AllowNoIcons=yes
OutputDir=Output
OutputBaseFilename=StudentManagementSystem_Setup
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
; Main application
Source: "bin\Release\net6.0-windows\win-x64\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "bin\Release\net6.0-windows\win-x64\*.dll"; DestDir: "{app}"; Flags: ignoreversion

; MySQL Installer
Source: "mysql-installer-community-8.0.42.0.msi"; DestDir: "{tmp}"; Flags: deleteafterinstall

; Database files
Source: "Database\init_db.sql"; DestDir: "{app}\Database"; Flags: ignoreversion
Source: "Database\studentmanagement.sql"; DestDir: "{app}\Database"; Flags: ignoreversion

; Report templates
Source: "Resources\reportTemplate\*"; DestDir: "{app}\reportTemplate"; Flags: ignoreversion recursesubdirs createallsubdirs

; Configuration
Source: "Config\connection.config"; DestDir: "{app}\Config"; Flags: ignoreversion

; Setup scripts
Source: "setup_mysql.bat"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
; Install MySQL
Filename: "msiexec.exe"; Parameters: "/i ""{tmp}\mysql-installer-community-8.0.42.0.msi"" /qn"; StatusMsg: "Installing MySQL..."; Flags: runhidden

; Run MySQL setup script
Filename: "{app}\setup_mysql.bat"; StatusMsg: "Configuring MySQL..."; Flags: runhidden

; Launch application after installation
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent 