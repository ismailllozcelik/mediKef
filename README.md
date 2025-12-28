# MediKef - Laboratuvar Bilgi Yönetim Sistemi (LBYS)

**Infomed tarzı modern LBYS uygulaması** - LisBox middleware entegrasyonu ile cihazlardan otomatik test sonucu alma.

## 🏗️ Mimari

```
┌─────────────────────────────────────────────────────────────┐
│                    MediKef LBYS Sistemi                      │
└─────────────────────────────────────────────────────────────┘

Laboratuvar Cihazı (Simüle)
         │
         │ HL7/ASTM Protokol
         ▼
┌──────────────────────┐
│  LisBox Simulator    │  ← Cihazdan veri alır, parse eder
│  (C# Console App)    │
└──────────────────────┘
         │
         │ HTTP POST (JSON)
         ▼
┌──────────────────────┐
│   Backend API        │  ← REST API (C# .NET 8)
│   (ASP.NET Core)     │  ← Entity Framework Core
└──────────────────────┘  ← PostgreSQL
         │
         │ REST API
         ▼
┌──────────────────────┐
│   Frontend           │  ← Angular 17+
│   (Angular)          │  ← Material Design
└──────────────────────┘
```

## 🚀 Teknoloji Stack

### Backend
- **ASP.NET Core 8.0** - Web API
- **Entity Framework Core** - ORM
- **PostgreSQL 16** - Veritabanı
- **Swagger/OpenAPI** - API Dokümantasyonu

### Frontend
- **Angular 17+** - SPA Framework
- **Angular Material** - UI Components
- **RxJS** - Reactive Programming
- **TypeScript** - Type Safety

### LisBox Simulator
- **C# Console Application** - Cihaz simülatörü
- **HL7/ASTM Parser** - Protokol desteği
- **HTTP Client** - API entegrasyonu

## 📦 Kurulum

### Gereksinimler
- .NET 8 SDK
- Node.js 18+ & npm
- PostgreSQL 16
- Docker & Docker Compose (opsiyonel)

### 1. Docker ile Hızlı Başlangıç (Önerilen)

```bash
# Tüm servisleri başlat
docker-compose up -d

# Frontend: http://localhost:4200
# Backend API: http://localhost:5000
# Swagger: http://localhost:5000/swagger
# PostgreSQL: localhost:5432
```

### 2. Manuel Kurulum

#### PostgreSQL Kurulumu
```bash
# PostgreSQL başlat
# Veritabanı oluştur
createdb medikef_db
```

#### Backend Kurulumu
```bash
cd src/Backend
dotnet restore
dotnet ef database update
dotnet run
```

#### Frontend Kurulumu
```bash
cd src/Frontend
npm install
ng serve
```

#### LisBox Simulator
```bash
cd src/LisBoxSimulator
dotnet run
```

## 🎯 Özellikler

### ✅ Hasta Yönetimi
- Hasta kaydı oluşturma
- Hasta bilgileri güncelleme
- Hasta arama ve filtreleme

### ✅ Numune Yönetimi
- Numune kaydı ve barkod oluşturma
- Test talepleri
- Numune takibi

### ✅ Test Yönetimi
- Test kataloğu
- Test parametreleri
- Referans aralıkları

### ✅ Cihaz Entegrasyonu (LisBox)
- Otomatik sonuç alma
- HL7/ASTM protokol desteği
- Çoklu cihaz desteği
- Real-time veri aktarımı

### ✅ Sonuç Yönetimi
- Test sonuçları görüntüleme
- Sonuç onaylama
- Rapor oluşturma
- Elektronik imza

## 📚 API Dokümantasyonu

Backend çalıştıktan sonra Swagger UI'a erişin:
```
http://localhost:5000/swagger
```

## 🔧 Geliştirme

### Backend Geliştirme
```bash
cd src/Backend
dotnet watch run
```

### Frontend Geliştirme
```bash
cd src/Frontend
ng serve --open
```

### Migration Oluşturma
```bash
cd src/Backend
dotnet ef migrations add MigrationName
dotnet ef database update
```

## 📖 Kullanım

### 1. Hasta Kaydı Oluştur
- Frontend'de "Yeni Hasta" butonuna tıklayın
- Hasta bilgilerini girin
- Kaydet

### 2. Numune ve Test Talebi
- Hasta seçin
- "Yeni Numune" oluşturun
- Testleri seçin
- Barkod yazdırın

### 3. LisBox Simulator ile Test Sonucu Gönder
- LisBox Simulator'ı çalıştırın
- Numune barkodunu girin
- Simülatör otomatik sonuç gönderecek

### 4. Sonuçları Görüntüle
- Frontend'de hasta sonuçlarını görün
- Sonuçları onayla
- Rapor yazdır

## 🔐 Güvenlik

- API Key authentication
- CORS yapılandırması
- SQL Injection koruması (EF Core)
- XSS koruması

## 📝 Lisans

MIT License

## 👥 Katkıda Bulunanlar

- İsmail Hakkı Özçelik

## 📞 İletişim

Sorularınız için issue açabilirsiniz.

