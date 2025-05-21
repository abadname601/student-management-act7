# Download Inno Setup
$url = "https://files.jrsoftware.org/is/6/innosetup-6.2.2.exe"
$output = "$PSScriptRoot\innosetup-6.2.2.exe"

Write-Host "Downloading Inno Setup..."
Invoke-WebRequest -Uri $url -OutFile $output

# Install Inno Setup silently
Write-Host "Installing Inno Setup..."
Start-Process -FilePath $output -ArgumentList "/VERYSILENT /SUPPRESSMSGBOXES /NORESTART" -Wait

# Clean up
Remove-Item $output

Write-Host "Inno Setup installation completed!" 