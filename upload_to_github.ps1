# Script untuk upload Video Duplicate Finder ke GitHub
# Pastikan Anda sudah login ke GitHub dan membuat repository terlebih dahulu

Write-Host "=== UPLOAD KE GITHUB ===" -ForegroundColor Cyan
Write-Host ""

# Cek apakah sudah ada remote
$remoteExists = git remote -v 2>&1
if ($remoteExists -match "origin") {
    Write-Host "Remote 'origin' sudah ada:" -ForegroundColor Yellow
    git remote -v
    Write-Host ""
    Write-Host "Apakah Anda ingin mengubah remote URL? (Y/N)" -ForegroundColor Cyan
    $changeRemote = Read-Host
    if ($changeRemote -eq "Y" -or $changeRemote -eq "y") {
        Write-Host "Masukkan URL repository GitHub:" -ForegroundColor Yellow
        Write-Host "Contoh: https://github.com/USERNAME/REPO_NAME.git" -ForegroundColor White
        $repoUrl = Read-Host
        git remote set-url origin $repoUrl
        Write-Host "Remote URL telah diubah" -ForegroundColor Green
    }
} else {
    Write-Host "Remote belum dikonfigurasi." -ForegroundColor Yellow
    Write-Host ""
    Write-Host "Masukkan URL repository GitHub:" -ForegroundColor Yellow
    Write-Host "Contoh: https://github.com/USERNAME/REPO_NAME.git" -ForegroundColor White
    Write-Host "Atau tekan Enter untuk skip (Anda bisa menambahkannya manual nanti)" -ForegroundColor Gray
    $repoUrl = Read-Host
    
    if ($repoUrl) {
        git remote add origin $repoUrl
        Write-Host "Remote 'origin' telah ditambahkan" -ForegroundColor Green
    } else {
        Write-Host "Remote tidak ditambahkan. Tambahkan manual dengan:" -ForegroundColor Yellow
        Write-Host "git remote add origin https://github.com/USERNAME/REPO_NAME.git" -ForegroundColor White
    }
}

Write-Host ""
Write-Host "=== STATUS REPOSITORY ===" -ForegroundColor Cyan
git status
Write-Host ""

# Cek branch
$currentBranch = git branch --show-current
Write-Host "Current branch: $currentBranch" -ForegroundColor White

if ($currentBranch -eq "master") {
    Write-Host ""
    Write-Host "Mengubah branch dari 'master' ke 'main'..." -ForegroundColor Yellow
    git branch -M main
    Write-Host "Branch telah diubah ke 'main'" -ForegroundColor Green
}

Write-Host ""
Write-Host "=== SIAP UNTUK PUSH ===" -ForegroundColor Cyan
Write-Host ""
Write-Host "Untuk push ke GitHub, jalankan:" -ForegroundColor Yellow
Write-Host "git push -u origin main" -ForegroundColor White
Write-Host ""
Write-Host "Atau jika branch masih 'master':" -ForegroundColor Yellow
Write-Host "git push -u origin master" -ForegroundColor White
Write-Host ""
Write-Host "Apakah Anda ingin push sekarang? (Y/N)" -ForegroundColor Cyan
$pushNow = Read-Host

if ($pushNow -eq "Y" -or $pushNow -eq "y") {
    Write-Host ""
    Write-Host "Pushing ke GitHub..." -ForegroundColor Yellow
    
    $branch = git branch --show-current
    git push -u origin $branch
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host ""
        Write-Host "=== BERHASIL ===" -ForegroundColor Green
        Write-Host "Repository berhasil diupload ke GitHub!" -ForegroundColor Green
    } else {
        Write-Host ""
        Write-Host "=== ERROR ===" -ForegroundColor Red
        Write-Host "Gagal push ke GitHub. Periksa:" -ForegroundColor Red
        Write-Host "1. Apakah repository sudah dibuat di GitHub?" -ForegroundColor Yellow
        Write-Host "2. Apakah Anda sudah login ke GitHub?" -ForegroundColor Yellow
        Write-Host "3. Apakah URL remote sudah benar?" -ForegroundColor Yellow
        Write-Host ""
        Write-Host "Untuk login ke GitHub via Git:" -ForegroundColor Cyan
        Write-Host "git config --global credential.helper wincred" -ForegroundColor White
        Write-Host "Kemudian push lagi, Git akan meminta username dan password/token" -ForegroundColor White
    }
} else {
    Write-Host ""
    Write-Host "Push dibatalkan. Jalankan manual saat siap:" -ForegroundColor Yellow
    Write-Host "git push -u origin main" -ForegroundColor White
}

Write-Host ""

