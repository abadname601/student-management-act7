#define MyAppName "Student Management System"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "Student Management System"
#define MyAppExeName "StudentManagementSystem.exe"

[Setup]
AppId={{YOUR-GUID-HERE}}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
AppPublisher={#MyAppPublisher}
DefaultDirName={autopf}\{#MyAppName}
DefaultGroupName={#MyAppName}
OutputDir=Output
OutputBaseFilename=StudentManagementSystem_Setup
Compression=lzma
SolidCompression=yes
WizardStyle=modern
PrivilegesRequired=admin

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
; Main application files
Source: "..\bin\Release\net6.0-windows\win-x64\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

; MySQL configuration file
Source: "..\Config\my.ini"; DestDir: "{app}\Config"; Flags: ignoreversion

; Database files
Source: "..\Database\init_db.sql"; DestDir: "{app}\Database"; Flags: ignoreversion
Source: "..\Database\init_database.bat"; DestDir: "{app}\Database"; Flags: ignoreversion
Source: "..\Database\studentmanagement.sql"; DestDir: "{app}\Database"; Flags: ignoreversion

; README file with setup instructions
Source: "README.txt"; DestDir: "{app}"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
; Run database initialization
Filename: "{app}\Database\init_database.bat"; Description: "Initialize Database"; Flags: postinstall runascurrentuser waituntilterminated
; Launch the application
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Code]
var
  DatabasePage: TInputQueryWizardPage;
  
procedure InitializeWizard;
begin
  DatabasePage := CreateInputQueryPage(wpSelectDir,
    'Database Configuration', 
    'IMPORTANT: Database Setup Instructions',
    'Please follow these steps carefully:' + #13#10#13#10 +
    '1. The password MUST be set to: mike' + #13#10 +
    '2. This is required for the application to work' + #13#10 +
    '3. The installer will automatically set this password' + #13#10 +
    '4. The database will be initialized after installation' + #13#10#13#10 +
    'Please enter the following database connection details:');
    
  DatabasePage.Add('Server:', False);
  DatabasePage.Add('Port:', False);
  DatabasePage.Add('Username:', False);
  DatabasePage.Add('Password (must be "mike"):', True);
  
  DatabasePage.Values[0] := 'localhost';
  DatabasePage.Values[1] := '3306';
  DatabasePage.Values[2] := 'root';
  DatabasePage.Values[3] := 'mike';
end;

function NextButtonClick(CurPageID: Integer): Boolean;
var
  ConfigFile: String;
  Content: TStringList;
  ConfigDir: String;
begin
  Result := True;
  
  if CurPageID = DatabasePage.ID then
  begin
    if DatabasePage.Values[3] <> 'mike' then
    begin
      MsgBox('The password must be set to "mike" for the application to work properly.', mbError, MB_OK);
      Result := False;
      Exit;
    end;
    
    ConfigDir := ExpandConstant('{app}\Config');
    if not DirExists(ConfigDir) then
      if not CreateDir(ConfigDir) then
      begin
        MsgBox('Failed to create Config directory. Please run the installer as administrator.', mbError, MB_OK);
        Result := False;
        Exit;
      end;
      
    ConfigFile := ConfigDir + '\connection.config';
    Content := TStringList.Create;
    try
      Content.Add('Server=' + DatabasePage.Values[0]);
      Content.Add('Port=' + DatabasePage.Values[1]);
      Content.Add('Username=' + DatabasePage.Values[2]);
      Content.Add('Password=' + DatabasePage.Values[3]);
      Content.SaveToFile(ConfigFile);
    finally
      Content.Free;
    end;
  end;
end; 