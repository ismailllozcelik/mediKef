# 🚀 MediKef LBYS - Kurulum ve Deployment Dokümanı

## 📋 İçindekiler
1. [Gereksinimler](#1-gereksinimler)
2. [Development Ortamı Kurulumu](#2-development-ortamı-kurulumu)
3. [Production Deployment](#3-production-deployment)
4. [Docker Deployment](#4-docker-deployment)
5. [Veritabanı Yönetimi](#5-veritabanı-yönetimi)
6. [Monitoring ve Logging](#6-monitoring-ve-logging)

---

## 1. Gereksinimler

### 1.1 Yazılım Gereksinimleri

#### Backend
- **.NET SDK 9.0** veya üzeri
- **PostgreSQL 16** veya üzeri
- **Git** (versiyon kontrolü)

#### Frontend
- **Node.js 18.x** veya üzeri
- **npm 9.x** veya **yarn 1.22.x**
- **Angular CLI 17.x**

#### Opsiyonel
- **Docker Desktop** (containerized deployment için)
- **Visual Studio 2022** veya **VS Code**
- **pgAdmin 4** (PostgreSQL yönetimi)

### 1.2 Donanım Gereksinimleri

#### Development
- **CPU:** 4 core (önerilen: 8 core)
- **RAM:** 8 GB (önerilen: 16 GB)
- **Disk:** 20 GB boş alan

#### Production
- **CPU:** 8 core (önerilen: 16 core)
- **RAM:** 16 GB (önerilen: 32 GB)
- **Disk:** 100 GB SSD (database için)
- **Network:** 100 Mbps (önerilen: 1 Gbps)

---

## 2. Development Ortamı Kurulumu

### 2.1 Projeyi Klonlama

```bash
# Repository'yi klonla
git clone https://github.com/your-org/medikef-lbys.git
cd medikef-lbys
```

### 2.2 PostgreSQL Kurulumu

#### Seçenek A: Docker ile PostgreSQL
```bash
# Docker Compose ile PostgreSQL başlat
docker-compose up -d postgres

# Bağlantı kontrolü
docker exec -it medikef-postgres psql -U medikef_user -d medikef_db
```

#### Seçenek B: Manuel PostgreSQL Kurulumu

**Windows:**
```powershell
# PostgreSQL 16 indir ve kur
# https://www.postgresql.org/download/windows/

# pgAdmin ile bağlan
# Host: localhost
# Port: 5432
# Database: postgres
# Username: postgres

# Yeni database oluştur
CREATE DATABASE medikef_db;
CREATE USER medikef_user WITH PASSWORD 'medikef_pass_2024';
GRANT ALL PRIVILEGES ON DATABASE medikef_db TO medikef_user;
```

**macOS:**
```bash
# Homebrew ile kur
brew install postgresql@16
brew services start postgresql@16

# Database oluştur
createdb medikef_db
psql medikef_db
CREATE USER medikef_user WITH PASSWORD 'medikef_pass_2024';
GRANT ALL PRIVILEGES ON DATABASE medikef_db TO medikef_user;
```

**Linux (Ubuntu/Debian):**
```bash
# PostgreSQL kur
sudo apt update
sudo apt install postgresql-16 postgresql-contrib

# PostgreSQL başlat
sudo systemctl start postgresql
sudo systemctl enable postgresql

# Database oluştur
sudo -u postgres psql
CREATE DATABASE medikef_db;
CREATE USER medikef_user WITH PASSWORD 'medikef_pass_2024';
GRANT ALL PRIVILEGES ON DATABASE medikef_db TO medikef_user;
\q
```

### 2.3 Backend Kurulumu

```bash
# Backend dizinine git
cd src/Backend/MediKef.Api

# NuGet paketlerini yükle
dotnet restore

# appsettings.Development.json oluştur
cat > appsettings.Development.json << EOF
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=medikef_db;Username=medikef_user;Password=medikef_pass_2024"
  },
  "LisBox": {
    "ApiKey": "LISBOX_SECRET_KEY_2024"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
EOF

# Database migration uygula
dotnet ef database update

# Seed data yükle
psql -h localhost -U medikef_user -d medikef_db -f ../../../database/seed-data.sql

# Uygulamayı çalıştır
dotnet run
```

Backend şimdi çalışıyor: **http://localhost:5218**  
Swagger UI: **http://localhost:5218/swagger**

### 2.4 LisBox Simulator Kurulumu

```bash
# Simulator dizinine git
cd src/Backend/LisBoxSimulator

# Uygulamayı çalıştır
dotnet run
```

### 2.5 Frontend Kurulumu (Planned)

```bash
# Frontend dizinine git
cd src/Frontend/medikef-web

# Dependencies yükle
npm install
# veya
yarn install

# Development server başlat
ng serve

# Tarayıcıda aç
# http://localhost:4200
```

---

## 3. Production Deployment

### 3.1 Backend Production Build

```bash
cd src/Backend/MediKef.Api

# Production build
dotnet publish -c Release -o ./publish

# Publish klasörü oluşturuldu
ls publish/
```

### 3.2 Frontend Production Build

```bash
cd src/Frontend/medikef-web

# Production build
ng build --configuration production

# dist klasörü oluşturuldu
ls dist/medikef-web/
```

### 3.3 IIS Deployment (Windows Server)

#### 3.3.1 IIS Kurulumu
```powershell
# IIS ve ASP.NET Core Hosting Bundle kur
# https://dotnet.microsoft.com/download/dotnet/9.0

# IIS Manager'ı aç
# Yeni Application Pool oluştur: MediKefAppPool
# .NET CLR Version: No Managed Code
```

#### 3.3.2 Backend Deployment
```powershell
# Publish klasörünü kopyala
Copy-Item -Path ".\publish\*" -Destination "C:\inetpub\wwwroot\medikef-api" -Recurse

# IIS'te yeni site oluştur
# Site Name: MediKef API
# Physical Path: C:\inetpub\wwwroot\medikef-api
# Binding: http, Port 80, Host: api.medikef.com

# appsettings.Production.json düzenle
# Connection string'i production database'e yönlendir
```

#### 3.3.3 Frontend Deployment
```powershell
# Angular build'i kopyala
Copy-Item -Path ".\dist\medikef-web\*" -Destination "C:\inetpub\wwwroot\medikef-web" -Recurse

# IIS'te yeni site oluştur
# Site Name: MediKef Web
# Physical Path: C:\inetpub\wwwroot\medikef-web
# Binding: http, Port 80, Host: medikef.com

# URL Rewrite modülü kur (Angular routing için)
# web.config oluştur
```

**web.config (Angular):**
```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <system.webServer>
    <rewrite>
      <rules>
        <rule name="Angular Routes" stopProcessing="true">
          <match url=".*" />
          <conditions logicalGrouping="MatchAll">
            <add input="{REQUEST_FILENAME}" matchType="IsFile" negate="true" />
            <add input="{REQUEST_FILENAME}" matchType="IsDirectory" negate="true" />
          </conditions>
          <action type="Rewrite" url="/" />
        </rule>
      </rules>
    </rewrite>
  </system.webServer>
</configuration>
```


