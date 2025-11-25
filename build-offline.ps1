# Build script untuk mencegah koneksi internet
# Script ini akan build aplikasi tanpa melakukan restore packages

Write-Host "Building VideoDuplicateFinder in offline mode..." -ForegroundColor Green

# Set environment variables untuk mencegah koneksi internet
$env:DOTNET_CLI_TELEMETRY_OPTOUT = "1"
$env:DOTNET_SKIP_FIRST_TIME_EXPERIENCE = "1"
$env:DOTNET_NOLOGO = "1"
$env:NUGET_XMLDOC_MODE = "skip"
$env:DOTNET_CLI_TELEMETRY_SESSIONID = ""
$env:DOTNET_ADD_GLOBAL_TOOLS_TO_PATH = "false"
$env:DOTNET_MULTILEVEL_LOOKUP = "0"

# Build tanpa restore
Write-Host "Building solution without restore..." -ForegroundColor Yellow
dotnet build VideoDuplicateFinder.sln --no-restore --verbosity quiet

if ($LASTEXITCODE -eq 0) {
    Write-Host "Build berhasil!" -ForegroundColor Green
} else {
    Write-Host "Build gagal. Pastikan semua packages sudah ter-restore sebelumnya." -ForegroundColor Red
    Write-Host "Untuk restore packages sekali saja, jalankan:" -ForegroundColor Yellow
    Write-Host "  dotnet restore --no-cache" -ForegroundColor Cyan
    exit 1
}

