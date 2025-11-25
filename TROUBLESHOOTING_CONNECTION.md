# Troubleshooting: Connection Failed Error

## Masalah
Jika Anda mendapatkan pesan error "Connection failed. If the problem persists, please check your internet connection or VPN" padahal jaringan internet Anda baik-baik saja, ini biasanya disebabkan oleh Visual Studio atau .NET SDK yang mencoba melakukan koneksi ke internet saat build atau restore packages.

## Solusi Cepat (RECOMMENDED)

### Gunakan Script Build Offline

Kami telah menyediakan script PowerShell untuk build tanpa koneksi internet:

**Untuk build tanpa restore (OFFLINE MODE):**
```powershell
.\build-offline.ps1
```

**Untuk restore packages sekali saja (jika diperlukan):**
```powershell
.\restore-once.ps1
```

Script ini akan:
- Menonaktifkan semua koneksi internet secara otomatis
- Build aplikasi tanpa melakukan restore packages
- Mengembalikan konfigurasi ke mode offline setelah restore

## Solusi Manual

### 1. Nonaktifkan Automatic Package Restore di Visual Studio

1. Buka Visual Studio
2. Pergi ke **Tools** → **Options**
3. Pilih **NuGet Package Manager** → **General**
4. **Uncheck** opsi berikut:
   - ✅ Allow NuGet to download missing packages
   - ✅ Automatically check for missing packages during build in Visual Studio
5. Klik **OK**

### 2. Build dengan Offline Mode

Gunakan command berikut untuk build tanpa koneksi internet:

```powershell
dotnet build VideoDuplicateFinder.sln --no-restore
```

Atau untuk restore packages sekali saja (jika diperlukan):

```powershell
dotnet restore --no-cache
dotnet build --no-restore
```

### 3. Konfigurasi Sudah Otomatis

Aplikasi sudah dikonfigurasi untuk menonaktifkan telemetry dan koneksi yang tidak perlu:

**File `Program.cs`:**
- Menonaktifkan semua .NET telemetry
- Menonaktifkan NuGet automatic restore
- Menonaktifkan semua network-related features

**File `NuGet.config`:**
- Semua package sources sudah dinonaktifkan (offline mode)
- Automatic package restore sudah dinonaktifkan

**File `Directory.Build.props` dan `.csproj`:**
- `RestoreOnBuild=false`
- `RestoreOnOpen=false`
- `RestoreOnSave=false`
- `SkipRestorePackages=true`

### 4. Clear NuGet Cache

Jika masih ada masalah, clear NuGet cache:

```powershell
dotnet nuget locals all --clear
```

### 5. Build dengan MSBuild Langsung

Jika menggunakan MSBuild langsung:

```powershell
msbuild VideoDuplicateFinder.sln /p:RestorePackages=false /p:SkipRestorePackages=true /p:RestoreOnBuild=false
```

## Catatan Penting

- **Aplikasi ini TIDAK memerlukan koneksi internet untuk berjalan**
- Semua koneksi yang terjadi biasanya dari proses build/restore di Visual Studio
- Setelah aplikasi di-build, tidak akan ada koneksi jaringan yang dilakukan saat runtime
- File `NuGet.config` sudah dikonfigurasi untuk **OFFLINE MODE** - semua package sources dinonaktifkan
- Jika Anda perlu restore packages, gunakan script `restore-once.ps1` yang akan mengaktifkan NuGet.org sementara

## Verifikasi

Untuk memastikan tidak ada koneksi yang dilakukan:

1. Build aplikasi dengan `.\build-offline.ps1` atau `dotnet build --no-restore`
2. Jalankan aplikasi
3. Monitor network activity menggunakan tools seperti Resource Monitor atau Wireshark
4. Tidak seharusnya ada koneksi HTTP/HTTPS yang dilakukan oleh aplikasi

## Jika Masalah Masih Terjadi

Jika masalah masih terjadi setelah mengikuti langkah-langkah di atas:

1. **Pastikan semua packages sudah ter-restore sebelumnya** - jalankan `.\restore-once.ps1` sekali saja
2. **Build dengan flag `--no-restore`** - gunakan `.\build-offline.ps1`
3. **Periksa Visual Studio Settings** - pastikan automatic restore dinonaktifkan di Tools → Options → NuGet
4. **Periksa firewall atau antivirus** yang mungkin memblokir koneksi
5. **Pastikan tidak ada proxy yang salah dikonfigurasi**
6. **Restart Visual Studio** setelah mengubah konfigurasi NuGet

