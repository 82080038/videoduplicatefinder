# Script untuk restore packages sekali saja
# Gunakan script ini hanya jika packages belum pernah di-restore sebelumnya

Write-Host "Restoring NuGet packages (one-time only)..." -ForegroundColor Green

# Set environment variables untuk mencegah telemetry
$env:DOTNET_CLI_TELEMETRY_OPTOUT = "1"
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = "1"
$env:DOTNET_NOLOGO = "1"
$env:NUGET_XMLDOC_MODE = "skip"

# Enable NuGet.org temporarily untuk restore
Write-Host "Temporarily enabling NuGet.org for package restore..." -ForegroundColor Yellow

# Backup NuGet.config
if (Test-Path NuGet.config) {
    Copy-Item NuGet.config NuGet.config.backup
}

# Create temporary NuGet.config dengan nuget.org enabled
$tempConfig = @"
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <clear />
    <add key="nuget.org" value="https://api.nuget.org/v3/index.json" protocolVersion="3" />
  </packageSources>
  <disabledPackageSources>
    <add key="AvaloniaCI" value="true" />
  </disabledPackageSources>
  <config>
    <add key="repositoryPath" value="packages" />
    <add key="globalPackagesFolder" value="%USERPROFILE%\.nuget\packages" />
  </config>
  <packageRestore>
    <add key="enabled" value="true" />
    <add key="automatic" value="true" />
  </packageRestore>
</configuration>
"@
$tempConfig | Set-Content NuGet.config

try {
    # Restore packages
    Write-Host "Restoring packages..." -ForegroundColor Yellow
    dotnet restore --no-cache
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Package restore berhasil!" -ForegroundColor Green
        Write-Host "Sekarang Anda bisa build dengan: .\build-offline.ps1" -ForegroundColor Cyan
    } else {
        Write-Host "Package restore gagal. Periksa koneksi internet Anda." -ForegroundColor Red
    }
} finally {
    # Restore NuGet.config
    if (Test-Path NuGet.config.backup) {
        Move-Item NuGet.config.backup NuGet.config -Force
        Write-Host "NuGet.config telah dikembalikan ke mode offline." -ForegroundColor Green
    }
}

