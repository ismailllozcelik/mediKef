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

### 3.4 Linux Deployment (Ubuntu Server)

#### 3.4.1 Nginx + Kestrel

```bash
# .NET Runtime kur
wget https://packages.microsoft.com/config/ubuntu/22.04/packages-microsoft-prod.deb
sudo dpkg -i packages-microsoft-prod.deb
sudo apt update
sudo apt install -y aspnetcore-runtime-9.0

# Nginx kur
sudo apt install -y nginx

# Backend dosyalarını kopyala
sudo mkdir -p /var/www/medikef-api
sudo cp -r ./publish/* /var/www/medikef-api/

# Systemd service oluştur
sudo nano /etc/systemd/system/medikef-api.service
```

**/etc/systemd/system/medikef-api.service:**
```ini
[Unit]
Description=MediKef LBYS API
After=network.target

[Service]
WorkingDirectory=/var/www/medikef-api
ExecStart=/usr/bin/dotnet /var/www/medikef-api/MediKef.Api.dll
Restart=always
RestartSec=10
KillSignal=SIGINT
SyslogIdentifier=medikef-api
User=www-data
Environment=ASPNETCORE_ENVIRONMENT=Production
Environment=DOTNET_PRINT_TELEMETRY_MESSAGE=false

[Install]
WantedBy=multi-user.target
```

```bash
# Service'i başlat
sudo systemctl enable medikef-api
sudo systemctl start medikef-api
sudo systemctl status medikef-api

# Nginx konfigürasyonu
sudo nano /etc/nginx/sites-available/medikef-api
```

**/etc/nginx/sites-available/medikef-api:**
```nginx
server {
    listen 80;
    server_name api.medikef.com;

    location / {
        proxy_pass http://localhost:5218;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection keep-alive;
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
        proxy_set_header X-Forwarded-For $proxy_add_x_forwarded_for;
        proxy_set_header X-Forwarded-Proto $scheme;
    }
}
```

```bash
# Nginx konfigürasyonunu aktifleştir
sudo ln -s /etc/nginx/sites-available/medikef-api /etc/nginx/sites-enabled/
sudo nginx -t
sudo systemctl restart nginx
```

#### 3.4.2 SSL/TLS (Let's Encrypt)

```bash
# Certbot kur
sudo apt install -y certbot python3-certbot-nginx

# SSL sertifikası al
sudo certbot --nginx -d api.medikef.com -d medikef.com

# Otomatik yenileme
sudo certbot renew --dry-run
```

---

## 4. Docker Deployment

### 4.1 Dockerfile (Backend)

**src/Backend/MediKef.Api/Dockerfile:**
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["MediKef.Api/MediKef.Api.csproj", "MediKef.Api/"]
RUN dotnet restore "MediKef.Api/MediKef.Api.csproj"
COPY . .
WORKDIR "/src/MediKef.Api"
RUN dotnet build "MediKef.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MediKef.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MediKef.Api.dll"]
```

### 4.2 Dockerfile (Frontend)

**src/Frontend/medikef-web/Dockerfile:**
```dockerfile
# Stage 1: Build
FROM node:18-alpine AS build
WORKDIR /app
COPY package*.json ./
RUN npm ci
COPY . .
RUN npm run build -- --configuration production

# Stage 2: Serve with Nginx
FROM nginx:alpine
COPY --from=build /app/dist/medikef-web /usr/share/nginx/html
COPY nginx.conf /etc/nginx/conf.d/default.conf
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
```

**nginx.conf:**
```nginx
server {
    listen 80;
    server_name localhost;
    root /usr/share/nginx/html;
    index index.html;

    location / {
        try_files $uri $uri/ /index.html;
    }

    location /api {
        proxy_pass http://backend:80;
        proxy_http_version 1.1;
        proxy_set_header Upgrade $http_upgrade;
        proxy_set_header Connection 'upgrade';
        proxy_set_header Host $host;
        proxy_cache_bypass $http_upgrade;
    }
}
```

### 4.3 Docker Compose (Full Stack)

**docker-compose.production.yml:**
```yaml
version: '3.8'

services:
  postgres:
    image: postgres:16-alpine
    container_name: medikef-postgres
    environment:
      POSTGRES_DB: medikef_db
      POSTGRES_USER: medikef_user
      POSTGRES_PASSWORD: ${DB_PASSWORD}
    volumes:
      - postgres_data:/var/lib/postgresql/data
      - ./database/schema.sql:/docker-entrypoint-initdb.d/01-schema.sql
      - ./database/seed-data.sql:/docker-entrypoint-initdb.d/02-seed-data.sql
    ports:
      - "5432:5432"
    networks:
      - medikef-network
    restart: unless-stopped

  backend:
    build:
      context: ./src/Backend
      dockerfile: MediKef.Api/Dockerfile
    container_name: medikef-backend
    environment:
      ASPNETCORE_ENVIRONMENT: Production
      ConnectionStrings__DefaultConnection: "Host=postgres;Database=medikef_db;Username=medikef_user;Password=${DB_PASSWORD}"
      LisBox__ApiKey: ${LISBOX_API_KEY}
    ports:
      - "5218:80"
    depends_on:
      - postgres
    networks:
      - medikef-network
    restart: unless-stopped

  frontend:
    build:
      context: ./src/Frontend/medikef-web
      dockerfile: Dockerfile
    container_name: medikef-frontend
    ports:
      - "80:80"
    depends_on:
      - backend
    networks:
      - medikef-network
    restart: unless-stopped

volumes:
  postgres_data:

networks:
  medikef-network:
    driver: bridge
```

**.env:**
```env
DB_PASSWORD=your_secure_password_here
LISBOX_API_KEY=your_lisbox_api_key_here
```

### 4.4 Docker Deployment Komutları

```bash
# Build ve başlat
docker-compose -f docker-compose.production.yml up -d --build

# Logları izle
docker-compose -f docker-compose.production.yml logs -f

# Durdur
docker-compose -f docker-compose.production.yml down

# Veritabanı dahil tamamen temizle
docker-compose -f docker-compose.production.yml down -v
```

---

## 5. Veritabanı Yönetimi

### 5.1 Migration Oluşturma

```bash
cd src/Backend/MediKef.Api

# Yeni migration oluştur
dotnet ef migrations add MigrationName

# Migration'ı uygula
dotnet ef database update

# Migration'ı geri al
dotnet ef database update PreviousMigrationName

# Migration'ı sil
dotnet ef migrations remove
```

### 5.2 Backup ve Restore

#### Backup
```bash
# PostgreSQL backup
pg_dump -h localhost -U medikef_user -d medikef_db -F c -b -v -f medikef_backup_$(date +%Y%m%d).dump

# SQL formatında backup
pg_dump -h localhost -U medikef_user -d medikef_db > medikef_backup_$(date +%Y%m%d).sql
```

#### Restore
```bash
# Custom format restore
pg_restore -h localhost -U medikef_user -d medikef_db -v medikef_backup_20241228.dump

# SQL format restore
psql -h localhost -U medikef_user -d medikef_db < medikef_backup_20241228.sql
```

### 5.3 Otomatik Backup (Cron Job)

```bash
# Crontab düzenle
crontab -e

# Her gün saat 02:00'de backup al
0 2 * * * /usr/bin/pg_dump -h localhost -U medikef_user -d medikef_db -F c -b -v -f /backups/medikef_backup_$(date +\%Y\%m\%d).dump

# Eski backupları sil (30 günden eski)
0 3 * * * find /backups -name "medikef_backup_*.dump" -mtime +30 -delete
```

---

## 6. Monitoring ve Logging

### 6.1 Application Logging (Serilog)

**appsettings.Production.json:**
```json
{
  "Serilog": {
    "Using": ["Serilog.Sinks.Console", "Serilog.Sinks.File"],
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "System": "Warning"
      }
    },
    "WriteTo": [
      {
        "Name": "Console"
      },
      {
        "Name": "File",
        "Args": {
          "path": "/var/log/medikef/api-.log",
          "rollingInterval": "Day",
          "retainedFileCountLimit": 30,
          "outputTemplate": "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
        }
      }
    ]
  }
}
```

### 6.2 Health Checks

**Program.cs:**
```csharp
builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString, name: "database");

app.MapHealthChecks("/health");
```

**Kullanım:**
```bash
curl http://localhost:5218/health
# Response: Healthy
```

### 6.3 Prometheus Metrics (Planned)

```bash
# Prometheus endpoint
GET /metrics

# Örnek metrikler:
# - http_requests_total
# - http_request_duration_seconds
# - database_connections_active
# - lisbox_results_received_total
```

---

## 📊 Deployment Checklist

### Pre-Deployment
- [ ] Tüm testler başarılı
- [ ] Code review tamamlandı
- [ ] Security scan yapıldı
- [ ] Database migration hazır
- [ ] Backup alındı
- [ ] Rollback planı hazır

### Deployment
- [ ] Production build oluşturuldu
- [ ] Environment variables ayarlandı
- [ ] Database migration uygulandı
- [ ] Application deploy edildi
- [ ] Health check başarılı
- [ ] Smoke test yapıldı

### Post-Deployment
- [ ] Monitoring aktif
- [ ] Logging çalışıyor
- [ ] Performance metrikleri normal
- [ ] Error rate normal
- [ ] Kullanıcı bildirimi yapıldı
- [ ] Dokümantasyon güncellendi

---

**Doküman Versiyonu:** 1.0
**Son Güncelleme:** 28 Aralık 2024
**Hazırlayan:** MediKef Development Team

