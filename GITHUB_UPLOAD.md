# Cara Upload ke GitHub

## Langkah-langkah:

### 1. Buat Repository di GitHub
1. Buka https://github.com
2. Klik tombol "+" di pojok kanan atas
3. Pilih "New repository"
4. Isi nama repository (contoh: `video-duplicate-finder`)
5. Pilih Public atau Private
6. JANGAN centang "Initialize with README" (karena sudah ada)
7. Klik "Create repository"

### 2. Tambahkan Remote dan Push
Setelah repository dibuat di GitHub, jalankan perintah berikut:

```powershell
cd E:\xampp\htdocs\duplicate\videoduplicatefinder

# Ganti YOUR_USERNAME dan REPO_NAME dengan yang sesuai
git remote add origin https://github.com/YOUR_USERNAME/REPO_NAME.git

# Atau jika menggunakan SSH:
# git remote add origin git@github.com:YOUR_USERNAME/REPO_NAME.git

# Push ke GitHub
git branch -M main
git push -u origin main
```

### 3. Atau Gunakan GitHub CLI (jika terinstall)
```powershell
cd E:\xampp\htdocs\duplicate\videoduplicatefinder

# Login ke GitHub (jika belum)
gh auth login

# Buat repository dan push sekaligus
gh repo create video-duplicate-finder --public --source=. --remote=origin --push
```

## Catatan Penting:

- **Source code saja yang diupload** (folder `videoduplicatefinder`)
- **Compiled files TIDAK diupload** (folder `App-win-x64` terlalu besar dengan banyak DLL)
- **FFmpeg binaries TIDAK diupload** (harus didownload terpisah oleh user)
- File `.gitignore` sudah dikonfigurasi untuk mengabaikan file yang tidak perlu

## Struktur yang Diupload:

```
videoduplicatefinder/
├── README.md
├── .gitignore
├── VideoDuplicateFinder.sln
├── VDF.Core/
│   └── (source code files)
└── VDF.GUI/
    └── (source code files)
```

## Setelah Upload:

1. Tambahkan description di GitHub repository
2. Tambahkan topics/tags: `video`, `duplicate-finder`, `csharp`, `avalonia`, `ffmpeg`
3. Buat Release dengan compiled binaries (opsional)
4. Update README.md jika perlu

