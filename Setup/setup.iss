#define MyAppName "Student Management System"
#define MyAppVersion "1.0.0"
#define MyAppPublisher "Your Name"
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

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
; Main application files
Source: "bin\Release\net6.0-windows\publish\*"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

; MySQL configuration file
Source: "Config\my.ini"; DestDir: "{app}\Config"; Flags: ignoreversion

; Database script
Source: "Database\studentmanagement.sql"; DestDir: "{app}\Database"; Flags: ignoreversion

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

[Code]
var
  DatabasePage: TInputQueryWizardPage;
  
procedure InitializeWizard;
begin
  DatabasePage := CreateInputQueryPage(wpSelectDir,
    'Database Configuration', 
    'Configure your MySQL database connection',
    'Please enter your MySQL database connection details below:');
    
  DatabasePage.Add('Server:', False);
  DatabasePage.Add('Port:', False);
  DatabasePage.Add('Username:', False);
  DatabasePage.Add('Password:', True);
  
  DatabasePage.Values[0] := 'localhost';
  DatabasePage.Values[1] := '3307';
  DatabasePage.Values[2] := 'root';
  DatabasePage.Values[3] := '';
end;

function NextButtonClick(CurPageID: Integer): Boolean;
var
  ConfigFile: String;
  Content: TStringList;
begin
  Result := True;
  
  if CurPageID = DatabasePage.ID then
  begin
    ConfigFile := ExpandConstant('{app}\Config\connection.config');
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