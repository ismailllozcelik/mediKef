# 📚 MediKef LBYS - Dokümantasyon İndeksi

## 🎯 Hızlı Başlangıç

Projeye yeni başlıyorsanız, dokümanları şu sırayla okumanızı öneririz:

1. **[Proje Özeti](PROJE_OZETI.md)** - Projeye genel bakış (15 dakika)
2. **[Teknik Mimari](TEKNIK_MIMARI.md)** - Sistem mimarisi ve diyagramlar (20 dakika)
3. **[Kurulum ve Deployment](KURULUM_VE_DEPLOYMENT.md)** - Projeyi çalıştırma (30 dakika)
4. **[API Endpoints](API_ENDPOINTS.md)** - API kullanımı (20 dakika)

---

## 📖 Tüm Dokümanlar

### 1. 📋 Analiz Dokümanı
**Dosya:** [ANALIZ_DOKUMANI.md](ANALIZ_DOKUMANI.md)  
**Boyut:** ~1070 satır  
**Okuma Süresi:** 60-90 dakika  

**İçerik:**
- Proje genel bakış ve hedefler
- Sistem mimarisi
- Teknoloji stack detayları
- Veritabanı tasarımı (ERD)
- API endpoint spesifikasyonları
- LisBox entegrasyon detayları
- Frontend tasarım mockup'ları
- Güvenlik gereksinimleri
- Deployment stratejileri
- Test yaklaşımları
- Proje zaman çizelgesi
- Risk analizi
- Maliyet tahmini

**Kimler İçin:**
- Proje yöneticileri
- Sistem analistleri
- Yazılım mimarları
- Müşteri / Stakeholder'lar

---

### 2. 🏗️ Teknik Mimari
**Dosya:** [TEKNIK_MIMARI.md](TEKNIK_MIMARI.md)  
**Boyut:** ~150 satır  
**Okuma Süresi:** 20-30 dakika  

**İçerik:**
- Genel sistem mimarisi diyagramı
- Veri akış diyagramı (sequence diagram)
- Veritabanı ilişki diyagramı (ERD)
- Katmanlı mimari açıklaması
- Component diyagramları

**Kimler İçin:**
- Yazılım mimarları
- Backend geliştiriciler
- Frontend geliştiriciler
- DevOps mühendisleri

---

### 3. 🔌 API Endpoints
**Dosya:** [API_ENDPOINTS.md](API_ENDPOINTS.md)  
**Boyut:** ~650 satır  
**Okuma Süresi:** 30-40 dakika  

**İçerik:**
- Tüm API endpoint'leri
- Request/Response örnekleri
- HTTP status code'ları
- Authentication detayları
- Error handling
- Kullanım örnekleri

**Endpoint Kategorileri:**
1. LisBox Integration API
2. Patients API
3. Samples API
4. Tests API
5. Devices API
6. Authentication API (Planned)

**Kimler İçin:**
- Backend geliştiriciler
- Frontend geliştiriciler
- API tüketicileri
- Test mühendisleri

---

### 4. 🧪 Test Senaryoları
**Dosya:** [TEST_SENARYOLARI.md](TEST_SENARYOLARI.md)  
**Boyut:** ~660 satır  
**Okuma Süresi:** 40-50 dakika  

**İçerik:**
- Unit test senaryoları (xUnit)
- Integration test senaryoları
- End-to-End test senaryoları
- Performans testleri (Load, Stress, Endurance)
- Güvenlik testleri (SQL Injection, XSS, CSRF)
- Test metrikleri ve başarı kriterleri
- Test araçları

**Kimler İçin:**
- Test mühendisleri
- QA ekibi
- Backend geliştiriciler
- DevOps mühendisleri

---

### 5. 🚀 Kurulum ve Deployment
**Dosya:** [KURULUM_VE_DEPLOYMENT.md](KURULUM_VE_DEPLOYMENT.md)  
**Boyut:** ~677 satır  
**Okuma Süresi:** 45-60 dakika  

**İçerik:**
- Gereksinimler (yazılım, donanım)
- Development ortamı kurulumu
  - PostgreSQL kurulumu (Docker, Windows, macOS, Linux)
  - Backend kurulumu
  - Frontend kurulumu
  - LisBox Simulator kurulumu
- Production deployment
  - IIS deployment (Windows Server)
  - Nginx + Kestrel (Linux)
  - SSL/TLS konfigürasyonu
- Docker deployment
  - Dockerfile'lar
  - Docker Compose
- Veritabanı yönetimi
  - Migration'lar
  - Backup/Restore
  - Otomatik backup (Cron)
- Monitoring ve Logging
  - Serilog
  - Health checks
  - Prometheus metrics

**Kimler İçin:**
- DevOps mühendisleri
- Sistem yöneticileri
- Backend geliştiriciler
- IT operasyon ekibi

---

### 6. 📊 Proje Özeti
**Dosya:** [PROJE_OZETI.md](PROJE_OZETI.md)  
**Boyut:** ~300 satır  
**Okuma Süresi:** 15-20 dakika  

**İçerik:**
- Proje amacı ve hedefler
- Sistem mimarisi özeti
- Teknoloji stack tablosu
- Veritabanı tasarımı özeti
- API endpoints özeti
- İş akışı
- Özellikler (mevcut ve planlanan)
- Proje durumu
- Hedef kullanıcılar
- İş modeli ve rekabet avantajları

**Kimler İçin:**
- Yeni ekip üyeleri
- Proje yöneticileri
- Müşteri / Stakeholder'lar
- Satış ekibi

---

### 7. 🔧 Postman Collection
**Dosya:** [MediKef_LBYS.postman_collection.json](MediKef_LBYS.postman_collection.json)  
**Format:** JSON  

**İçerik:**
- Tüm API endpoint'leri için hazır request'ler
- Örnek request body'ler
- Environment variables
- Test script'leri (planned)

**Kullanım:**
```bash
# Postman'e import et
File → Import → MediKef_LBYS.postman_collection.json

# Newman ile çalıştır (CLI)
newman run MediKef_LBYS.postman_collection.json
```

**Kimler İçin:**
- Backend geliştiriciler
- Frontend geliştiriciler
- Test mühendisleri
- API tüketicileri

---

## 🗂️ Diğer Dokümanlar

### Database Schema
**Dosya:** [../database/schema.sql](../database/schema.sql)  
**İçerik:** PostgreSQL veritabanı şeması (8 tablo)

### Seed Data
**Dosya:** [../database/seed-data.sql](../database/seed-data.sql)  
**İçerik:** Demo veriler (hastalar, cihazlar, testler, numuneler)

### README
**Dosya:** [../README.md](../README.md)  
**İçerik:** Proje ana sayfası, hızlı başlangıç

---

## 📊 Doküman İstatistikleri

| Doküman | Satır Sayısı | Okuma Süresi | Güncelleme |
|---------|--------------|--------------|------------|
| Analiz Dokümanı | ~1070 | 60-90 dk | 2024-12-28 |
| Teknik Mimari | ~150 | 20-30 dk | 2024-12-28 |
| API Endpoints | ~650 | 30-40 dk | 2024-12-28 |
| Test Senaryoları | ~660 | 40-50 dk | 2024-12-28 |
| Kurulum ve Deployment | ~677 | 45-60 dk | 2024-12-28 |
| Proje Özeti | ~300 | 15-20 dk | 2024-12-28 |
| **TOPLAM** | **~3507** | **~4-5 saat** | - |

---

## 🎯 Rol Bazlı Okuma Önerileri

### Proje Yöneticisi
1. Proje Özeti
2. Analiz Dokümanı (Bölüm 1-3, 11-13)
3. Kurulum ve Deployment (Bölüm 3)

### Yazılım Mimarı
1. Teknik Mimari
2. Analiz Dokümanı (Bölüm 2-4)
3. API Endpoints

### Backend Geliştirici
1. Teknik Mimari
2. API Endpoints
3. Kurulum ve Deployment (Bölüm 2)
4. Test Senaryoları (Bölüm 1-2)

### Frontend Geliştirici
1. API Endpoints
2. Analiz Dokümanı (Bölüm 7)
3. Kurulum ve Deployment (Bölüm 2.5)

### DevOps Mühendisi
1. Kurulum ve Deployment
2. Teknik Mimari
3. Test Senaryoları (Bölüm 4)

### Test Mühendisi
1. Test Senaryoları
2. API Endpoints
3. Kurulum ve Deployment (Bölüm 2)

### Müşteri / Stakeholder
1. Proje Özeti
2. Analiz Dokümanı (Bölüm 1, 7, 11-13)

---

## 🔄 Doküman Güncelleme Politikası

- **Versiyon:** Semantic Versioning (1.0.0)
- **Güncelleme Sıklığı:** Her major feature sonrası
- **Sorumluluk:** Development Team Lead
- **Review:** Proje Yöneticisi onayı

---

## 📞 Destek

Dokümanlarla ilgili sorularınız için:
- **Email:** docs@medikef.com
- **Slack:** #medikef-docs
- **Wiki:** https://wiki.medikef.com

---

**Son Güncelleme:** 28 Aralık 2024  
**Doküman Versiyonu:** 1.0  
**Hazırlayan:** MediKef Development Team

