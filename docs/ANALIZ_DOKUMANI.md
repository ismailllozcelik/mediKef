# 🔬 MediKef LBYS - Detaylı Analiz Dokümanı

## 📋 İÇİNDEKİLER

1. [Proje Özeti](#1-proje-özeti)
2. [Sistem Mimarisi](#2-sistem-mimarisi)
3. [Teknoloji Stack](#3-teknoloji-stack)
4. [Veritabanı Tasarımı](#4-veritabanı-tasarımı)
5. [Backend API Detayları](#5-backend-api-detayları)
6. [LisBox Entegrasyonu](#6-lisbox-entegrasyonu)
7. [Frontend Tasarımı](#7-frontend-tasarımı)
8. [Güvenlik](#8-güvenlik)
9. [Deployment](#9-deployment)
10. [Test Stratejisi](#10-test-stratejisi)

---

## 1. PROJE ÖZETİ

### 1.1 Proje Adı
**MediKef LBYS** (Laboratuvar Bilgi Yönetim Sistemi)

### 1.2 Proje Amacı
Tıbbi laboratuvarlarda kullanılan cihazlardan (Cobas, Sysmex, vb.) otomatik olarak test sonuçlarını alıp,
veritabanına kaydeden ve kullanıcılara web arayüzü üzerinden sunan bir LBYS yazılımı geliştirmek.

### 1.3 Hedef Kullanıcılar
- **Laboratuvar Teknisyenleri**: Numune kabul, test talep etme
- **Doktorlar**: Test sonuçlarını görüntüleme, onaylama
- **Resepsiyonistler**: Hasta kaydı, numune barkod basımı
- **Sistem Yöneticileri**: Cihaz yönetimi, kullanıcı yönetimi

### 1.4 Temel Özellikler
- ✅ Hasta kayıt ve yönetimi
- ✅ Numune kabul ve barkod sistemi
- ✅ Test katalog yönetimi (Biyokimya, Hemogram, Hormon, vb.)
- ✅ **LisBox entegrasyonu ile otomatik cihaz sonuç alma**
- ✅ Test sonuçlarını görüntüleme ve raporlama
- ✅ Kalite kontrol (QC) takibi
- ✅ Kullanıcı yetkilendirme sistemi
- ✅ Audit log (tüm işlemlerin kaydı)

### 1.5 Proje Kapsamı Dışı (Out of Scope)
- ❌ HBYS (Hastane Bilgi Sistemi) entegrasyonu (gelecek versiyonda)
- ❌ Fatura/Muhasebe modülü
- ❌ Mobil uygulama
- ❌ E-imza entegrasyonu (gelecek versiyonda)

---

## 2. SİSTEM MİMARİSİ

### 2.1 Genel Mimari

```
┌─────────────────────────────────────────────────────────────────┐
│                    LABORATUVAR CİHAZLARI                        │
│  (Cobas c 311, Sysmex XN-550, Cobas e 411, vb.)               │
└────────────────────────┬────────────────────────────────────────┘
                         │ HL7 / ASTM
                         ▼
┌─────────────────────────────────────────────────────────────────┐
│                      LisBox Middleware                          │
│  - Cihazdan veri okuma (Serial/TCP-IP)                         │
│  - HL7/ASTM parsing                                             │
│  - JSON'a dönüştürme                                            │
└────────────────────────┬────────────────────────────────────────┘
                         │ HTTP POST (JSON)
                         │ X-API-Key: LISBOX_SECRET_KEY_2024
                         ▼
┌─────────────────────────────────────────────────────────────────┐
│                   MediKef Backend API                           │
│  - ASP.NET Core 9.0 Web API                                    │
│  - Entity Framework Core 9.0                                    │
│  - RESTful API Endpoints                                        │
│  - Swagger/OpenAPI Documentation                                │
└────────────────────────┬────────────────────────────────────────┘
                         │ Npgsql
                         ▼
┌─────────────────────────────────────────────────────────────────┐
│                   PostgreSQL Database                           │
│  - Patients, Devices, Tests, Samples                           │
│  - TestResults, LisBoxLogs                                      │
└─────────────────────────────────────────────────────────────────┘
                         ▲
                         │ HTTP REST API
                         │
┌─────────────────────────────────────────────────────────────────┐
│                   Angular Frontend                              │
│  - Angular 17+                                                  │
│  - Angular Material UI                                          │
│  - TypeScript                                                   │
│  - RxJS for reactive programming                                │
└─────────────────────────────────────────────────────────────────┘
```

### 2.2 Veri Akışı

#### 2.2.1 Numune Kabul İş Akışı
```
1. Resepsiyon → Hasta kaydı oluştur
2. Resepsiyon → Numune oluştur + Test seç
3. Sistem → Barkod üret (BAR2024XXXXXX)
4. Resepsiyon → Barkod etiket yazdır
5. Teknisyen → Barkodu numuneye yapıştır
6. Teknisyen → Numuneyi cihaza yerleştir
```

#### 2.2.2 Test Sonuç Alma İş Akışı
```
1. Cihaz → Test tamamlandı (HL7/ASTM mesajı)
2. LisBox → Mesajı parse et
3. LisBox → JSON'a dönüştür
4. LisBox → POST /api/LisBox/receive-results
5. Backend → Barkod ile numune bul
6. Backend → Test sonuçlarını kaydet
7. Backend → Numune durumunu güncelle (InProgress/Completed)
8. Frontend → Real-time güncelleme (SignalR - gelecek)
9. Doktor → Sonuçları görüntüle ve onayla
```

### 2.3 Deployment Mimarisi

```
┌─────────────────────────────────────────────────────────────────┐
│                        Docker Host                              │
│                                                                 │
│  ┌──────────────┐  ┌──────────────┐  ┌──────────────┐         │
│  │  PostgreSQL  │  │  Backend API │  │   Frontend   │         │
│  │  Container   │  │  Container   │  │  Container   │         │
│  │  Port: 5432  │  │  Port: 5000  │  │  Port: 4200  │         │
│  └──────────────┘  └──────────────┘  └──────────────┘         │
│         │                  │                  │                 │
│         └──────────────────┴──────────────────┘                 │
│                    medikef-network                              │
└─────────────────────────────────────────────────────────────────┘
```

---

## 3. TEKNOLOJİ STACK

### 3.1 Backend

| Teknoloji | Versiyon | Kullanım Amacı |
|-----------|----------|----------------|
| **.NET** | 9.0 | Backend framework |
| **ASP.NET Core** | 9.0 | Web API |
| **Entity Framework Core** | 9.0 | ORM (Object-Relational Mapping) |
| **Npgsql** | 9.0.2 | PostgreSQL provider |

| Alan | Tip | Açıklama | Örnek |
|------|-----|----------|-------|
| id | SERIAL | Primary key | 1 |
| patient_id | VARCHAR(50) | Sistem hasta ID (unique) | P2024000001 |
| tc_no | VARCHAR(11) | TC Kimlik No (unique) | 12345678901 |
| first_name | VARCHAR(100) | Ad | Ahmet |
| last_name | VARCHAR(100) | Soyad | Yılmaz |
| birth_date | DATE | Doğum tarihi | 1985-05-15 |
| gender | VARCHAR(10) | Cinsiyet | Erkek/Kadın |
| phone | VARCHAR(20) | Telefon | 05551234567 |
| email | VARCHAR(100) | E-posta | ahmet@example.com |
| address | TEXT | Adres | İstanbul, Türkiye |
| created_at | TIMESTAMP | Oluşturulma zamanı | 2024-12-28 10:00:00 |
| updated_at | TIMESTAMP | Güncellenme zamanı | 2024-12-28 10:00:00 |

**İş Kuralları:**
- `patient_id` otomatik üretilir: `P{YIL}{6 haneli sıra no}`
- `tc_no` 11 haneli olmalı ve unique
- `birth_date` bugünden küçük olmalı
- `gender` sadece "Erkek", "Kadın", "Diğer" değerlerini alabilir

#### 4.2.2 devices (Cihazlar)

| Alan | Tip | Açıklama | Örnek |
|------|-----|----------|-------|
| id | SERIAL | Primary key | 1 |
| device_id | VARCHAR(50) | Cihaz ID (unique) | COBAS_C311_01 |
| device_name | VARCHAR(100) | Cihaz adı | Cobas c 311 |
| manufacturer | VARCHAR(100) | Üretici | Roche |
| model | VARCHAR(100) | Model | c 311 |
| serial_number | VARCHAR(100) | Seri no | SN123456 |
| device_type | VARCHAR(50) | Cihaz tipi | Biyokimya |
| protocol | VARCHAR(20) | Protokol | HL7/ASTM |
| connection_type | VARCHAR(20) | Bağlantı tipi | Serial/TCP-IP |
| ip_address | VARCHAR(50) | IP adresi | 192.168.1.100 |
| port | INTEGER | Port | 5000 |
| is_active | BOOLEAN | Aktif mi? | true |

**İş Kuralları:**
- `device_id` unique olmalı
- `protocol` sadece "HL7", "ASTM", "HL7v2", "ASTM E1394" değerlerini alabilir
- `connection_type` sadece "Serial", "TCP-IP" değerlerini alabilir
- `is_active = false` olan cihazlardan sonuç alınamaz

#### 4.2.3 tests (Testler)

| Alan | Tip | Açıklama | Örnek |
|------|-----|----------|-------|
| id | SERIAL | Primary key | 1 |
| test_code | VARCHAR(20) | Test kodu (unique) | GLU |
| test_name | VARCHAR(200) | Test adı | Glukoz |
| test_category | VARCHAR(100) | Kategori | Biyokimya |
| unit | VARCHAR(50) | Birim | mg/dL |
| reference_range_min | DECIMAL | Referans min | 70 |
| reference_range_max | DECIMAL | Referans max | 110 |
| reference_range_text | VARCHAR(200) | Referans metin | 70-110 mg/dL |
| sample_type | VARCHAR(50) | Numune tipi | Serum |
| price | DECIMAL | Fiyat | 15.50 |
| is_active | BOOLEAN | Aktif mi? | true |

**Test Kategorileri:**
- Biyokimya (Glukoz, BUN, Kreatinin, ALT, AST, vb.)
- Hemogram (WBC, RBC, HGB, HCT, PLT, vb.)
- Hormon (TSH, FT3, FT4, Kortizol, vb.)
- Mikrobiyoloji
- İmmünoloji
- Koagülasyon

#### 4.2.4 samples (Numuneler)

| Alan | Tip | Açıklama | Örnek |
|------|-----|----------|-------|
| id | SERIAL | Primary key | 1 |
| sample_id | VARCHAR(50) | Numune ID (unique) | S2024000001 |
| barcode | VARCHAR(50) | Barkod (unique) | BAR2024000001 |
| patient_id | INTEGER | Hasta ID (FK) | 1 |
| sample_type | VARCHAR(50) | Numune tipi | Serum |
| collection_date | TIMESTAMP | Alınma zamanı | 2024-12-28 09:00:00 |
| received_date | TIMESTAMP | Kabul zamanı | 2024-12-28 09:15:00 |
| status | VARCHAR(50) | Durum | Pending/InProgress/Completed |
| priority | VARCHAR(20) | Öncelik | Normal/Urgent/STAT |
| notes | TEXT | Notlar | Açlık kan şekeri |
| created_by | VARCHAR(100) | Oluşturan | user@example.com |

**Numune Durumları:**
- `Pending`: Beklemede (henüz cihaza konmadı)
- `InProgress`: İşlemde (bazı testler tamamlandı)
- `Completed`: Tamamlandı (tüm testler tamamlandı)
- `Cancelled`: İptal edildi

**Öncelik Seviyeleri:**
- `Normal`: Normal öncelik
- `Urgent`: Acil
- `STAT`: Çok acil (15 dk içinde)

#### 4.2.5 sample_tests (Numune-Test İlişkisi)

| Alan | Tip | Açıklama | Örnek |
|------|-----|----------|-------|
| id | SERIAL | Primary key | 1 |
| sample_id | INTEGER | Numune ID (FK) | 1 |
| test_id | INTEGER | Test ID (FK) | 5 |
| status | VARCHAR(50) | Durum | Pending/Completed |

**İş Kuralları:**
- Bir numune için aynı test birden fazla kez talep edilemez (unique constraint)
- Test tamamlandığında `status = 'Completed'` olur

#### 4.2.6 test_results (Test Sonuçları)

| Alan | Tip | Açıklama | Örnek |
|------|-----|----------|-------|
| id | SERIAL | Primary key | 1 |
| sample_test_id | INTEGER | Numune-Test ID (FK) | 1 |
| device_id | INTEGER | Cihaz ID (FK) | 1 |
| result_value | VARCHAR(200) | Sonuç değeri (metin) | 95 |
| result_numeric | DECIMAL | Sonuç değeri (sayısal) | 95.0 |
| unit | VARCHAR(50) | Birim | mg/dL |
| reference_range | VARCHAR(200) | Referans aralık | 70-110 |
| flag | VARCHAR(10) | Bayrak | N/H/L/HH/LL/A |
| result_status | VARCHAR(50) | Sonuç durumu | Preliminary/Final |
| result_date | TIMESTAMP | Sonuç zamanı | 2024-12-28 10:30:00 |
| validated_by | VARCHAR(100) | Onaylayan | dr.ayse@example.com |
| validated_at | TIMESTAMP | Onay zamanı | 2024-12-28 11:00:00 |
| notes | TEXT | Notlar | Tekrar edildi |

**Flag Değerleri:**
- `N`: Normal (referans aralıkta)
- `H`: High (yüksek)
- `L`: Low (düşük)
- `HH`: Very High (çok yüksek)
- `LL`: Very Low (çok düşük)
- `A`: Abnormal (anormal)

**Result Status:**
- `Preliminary`: Ön sonuç (henüz onaylanmadı)
- `Final`: Nihai sonuç (onaylandı)
- `Corrected`: Düzeltilmiş
- `Cancelled`: İptal edilmiş

#### 4.2.7 lisbox_logs (LisBox Logları)

| Alan | Tip | Açıklama | Örnek |
|------|-----|----------|-------|
| id | SERIAL | Primary key | 1 |
| device_id | INTEGER | Cihaz ID (FK) | 1 |
| sample_barcode | VARCHAR(50) | Numune barkodu | BAR2024000001 |
| raw_data | TEXT | Ham veri (HL7/ASTM) | MSH|^~\&|... |
| parsed_data | JSONB | Parse edilmiş veri | {"DeviceId": "..."} |
| status | VARCHAR(50) | Durum | Success/Failed |
| error_message | TEXT | Hata mesajı | Numune bulunamadı |
| created_at | TIMESTAMP | Oluşturulma zamanı | 2024-12-28 10:30:00 |

**Kullanım Amacı:**
- Debugging (hata ayıklama)
- Audit trail (denetim izi)
- Cihaz iletişim sorunlarını tespit etme
- İstatistiksel analiz

### 4.3 İndeksler

```sql
-- Performans için önemli indeksler
CREATE INDEX idx_samples_barcode ON samples(barcode);
CREATE INDEX idx_samples_patient_id ON samples(patient_id);
CREATE INDEX idx_samples_status ON samples(status);
CREATE INDEX idx_samples_created_at ON samples(created_at);

CREATE INDEX idx_test_results_sample_test_id ON test_results(sample_test_id);
CREATE INDEX idx_test_results_result_date ON test_results(result_date);
CREATE INDEX idx_test_results_result_status ON test_results(result_status);

CREATE INDEX idx_lisbox_logs_created_at ON lisbox_logs(created_at);
CREATE INDEX idx_lisbox_logs_status ON lisbox_logs(status);
```

---

## 5. BACKEND API DETAYLARI

### 5.1 API Endpoint'leri

#### 5.1.1 LisBox Controller (EN ÖNEMLİ!)

**POST /api/LisBox/receive-results**
- **Açıklama**: LisBox'tan cihaz sonuçlarını alır
- **Authentication**: X-API-Key header
- **Request Body**:
```json
{
  "DeviceId": "COBAS_C311_01",
  "SampleBarcode": "BAR2024000001",
  "TestResults": [
    {
      "TestCode": "GLU",
      "TestName": "Glukoz",
      "ResultValue": "95",
      "ResultNumeric": 95.0,
      "Unit": "mg/dL",
      "ReferenceRange": "70-110",
      "Flag": "N",
      "ResultDateTime": "2024-12-28T10:30:00"
    }
  ],
  "Status": "Final",
  "Timestamp": "2024-12-28T10:30:00"
}
```
- **Response (Success)**:
```json
{
  "Success": true,
  "Message": "Sonuçlar başarıyla kaydedildi",
  "SampleId": "S2024000001",
  "ProcessedResults": 1
}
```
- **Response (Error)**:
```json
{
  "Success": false,
  "Message": "Numune bulunamadı: BAR2024000001"
}
```

**İş Akışı:**
1. API Key kontrolü (güvenlik)
2. Cihaz ID kontrolü (cihaz kayıtlı mı?)
3. Barkod ile numune bulma
4. Test kodlarını eşleştirme
5. Test sonuçlarını kaydetme
6. Numune durumunu güncelleme
7. Log kaydetme

#### 5.1.2 Patients Controller

**GET /api/Patients**
- Query params: `?search=ahmet`
- Response: Hasta listesi

**GET /api/Patients/{id}**
- Response: Hasta detayı (numuneler dahil)

**POST /api/Patients**
- Request: Hasta bilgileri
- Response: Oluşturulan hasta

**PUT /api/Patients/{id}**
- Request: Güncellenmiş hasta bilgileri

**DELETE /api/Patients/{id}**
- Soft delete önerilir (is_deleted flag)

#### 5.1.3 Samples Controller

**GET /api/Samples**
- Query params: `?status=Pending`
- Response: Numune listesi

**GET /api/Samples/{id}**
- Response: Numune detayı (test sonuçları dahil)

**GET /api/Samples/barcode/{barcode}**
- Response: Barkod ile numune bulma

**POST /api/Samples**
- Request:
```json
{
  "PatientId": 1,
  "SampleType": "Serum",
  "Priority": "Normal",
  "TestIds": [1, 2, 3],
  "CreatedBy": "user@example.com"
}
```
- Response: Oluşturulan numune (barkod dahil)

**PUT /api/Samples/{id}/status**
- Request: `"Completed"`
- Manuel durum güncelleme

#### 5.1.4 Tests Controller

**GET /api/Tests**
- Query params: `?category=Biyokimya`
- Response: Test listesi

**GET /api/Tests/categories**
- Response: Test kategorileri

**GET /api/Tests/{id}**
- Response: Test detayı

### 5.2 Authentication & Authorization

**Gelecek Versiyon için Planlanan:**
- JWT Token based authentication
- Role-based authorization (Admin, Doctor, Technician, Receptionist)
- Refresh token mekanizması

**Şu anki Versiyon:**
- LisBox için API Key authentication
- Diğer endpoint'ler için authentication yok (development)

### 5.3 Error Handling

**Standart Error Response:**
```json
{
  "StatusCode": 404,
  "Message": "Numune bulunamadı",
  "Details": "Barkod: BAR2024000001",
  "Timestamp": "2024-12-28T10:30:00"
}
```

**HTTP Status Codes:**
- 200: Success
- 201: Created
- 400: Bad Request
- 401: Unauthorized
- 404: Not Found
- 500: Internal Server Error

---

## 6. LİSBOX ENTEGRASYONU

### 6.1 LisBox Nedir?

LisBox, laboratuvar cihazları ile LBYS arasında köprü görevi gören bir **middleware** yazılımıdır.

**Görevleri:**
1. Cihazdan veri okuma (Serial port veya TCP/IP)
2. HL7/ASTM mesajlarını parse etme
3. JSON formatına dönüştürme
4. LBYS API'sine HTTP POST ile gönderme

### 6.2 Desteklenen Protokoller

| Protokol | Açıklama | Kullanım |
|----------|----------|----------|
| **HL7 v2.x** | Health Level Seven | Sağlık sistemleri arası veri alışverişi |
| **ASTM E1394-97** | Laboratory Instrument Standard | Laboratuvar cihaz standartı |
| **ASTM E1381-95** | Laboratory Data Transfer | Laboratuvar veri transferi |

### 6.3 HL7 Mesaj Örneği

```
MSH|^~\&|COBAS_C311|LAB|LBYS|HOSPITAL|20241228103000||ORU^R01|MSG001|P|2.5
PID|1||P2024000001||YILMAZ^AHMET||19850515|M
OBR|1||BAR2024000001|GLU^Glukoz^LN|||20241228093000
OBX|1|NM|GLU^Glukoz^LN||95|mg/dL|70-110|N|||F
```

**Açıklama:**
- MSH: Message Header
- PID: Patient Identification
- OBR: Observation Request
- OBX: Observation Result

### 6.4 LisBox Konfigürasyonu

**LisBox'ta yapılması gerekenler:**
1. Cihaz tanımlama (Cobas c 311, Sysmex XN-550, vb.)
2. Bağlantı ayarları (Serial/TCP-IP)
3. API endpoint URL: `http://localhost:5000/api/LisBox/receive-results`
4. API Key: `LISBOX_SECRET_KEY_2024`
5. Veri formatı: JSON
6. HTTP Method: POST

### 6.5 LisBox Simulator

Gerçek cihaz olmadan test etmek için **LisBox Simulator** geliştirildi.

**Özellikler:**
- 3 farklı cihaz simülasyonu (Biyokimya, Hemogram, Hormon)
- Rastgele test sonucu üretme
- HTTP POST ile API'ye gönderme
- Detaylı log çıktısı

**Kullanım:**
```bash
cd src/Backend/LisBoxSimulator
dotnet run
```

---

## 7. FRONTEND TASARIMI

### 7.1 Sayfa Yapısı

```
MediKef LBYS
│
├── Login (Giriş)
│
├── Dashboard (Ana Sayfa)
│   ├── Bugünkü istatistikler
│   ├── Bekleyen numuneler
│   ├── Tamamlanan testler
│   └── Grafik ve raporlar
│
├── Patients (Hastalar)
│   ├── Hasta listesi
│   ├── Hasta ekle
│   ├── Hasta düzenle
│   └── Hasta detay
│
├── Samples (Numuneler)
│   ├── Numune listesi
│   ├── Numune kabul
│   ├── Barkod yazdır
│   └── Numune detay
│
├── Results (Sonuçlar)
│   ├── Sonuç listesi
│   ├── Sonuç detay
│   ├── Sonuç onaylama
│   └── Rapor yazdır
│
├── Tests (Testler)
│   ├── Test kataloğu
│   ├── Test kategorileri
│   └── Test fiyatları
│
├── Devices (Cihazlar)
│   ├── Cihaz listesi
│   ├── Cihaz ekle/düzenle
│   └── Cihaz durumu
│
└── Settings (Ayarlar)
    ├── Kullanıcı yönetimi
    ├── Sistem ayarları
    └── Log görüntüleme
```

### 7.2 UI/UX Tasarım Prensipleri

**Renk Paleti (Infomed tarzı):**
- Primary: #1976D2 (Mavi)
- Accent: #FF9800 (Turuncu)
- Success: #4CAF50 (Yeşil)
- Warning: #FFC107 (Sarı)
- Error: #F44336 (Kırmızı)

**Tipografi:**
- Font: Roboto
- Başlıklar: 24px, Bold
- Alt başlıklar: 18px, Medium
- Metin: 14px, Regular

**Bileşenler:**
- Angular Material components
- Responsive design (mobil uyumlu)
- Dark mode desteği (gelecek)

### 7.3 Örnek Ekran Tasarımları

#### 7.3.1 Numune Kabul Ekranı

```
┌─────────────────────────────────────────────────────────────┐
│  MediKef LBYS > Numune Kabul                        [Logout] │
├─────────────────────────────────────────────────────────────┤
│                                                               │
│  Hasta Bilgileri                                             │
│  ┌─────────────────────────────────────────────────────┐    │
│  │ TC No: [___________] [Ara]                          │    │
│  │                                                      │    │
│  │ Ad Soyad: Ahmet YILMAZ                              │    │
│  │ Doğum Tarihi: 15.05.1985                            │    │
│  │ Cinsiyet: Erkek                                      │    │
│  └─────────────────────────────────────────────────────┘    │
│                                                               │
│  Test Seçimi                                                 │
│  ┌─────────────────────────────────────────────────────┐    │
│  │ Kategori: [Biyokimya ▼]                             │    │
│  │                                                      │    │
│  │ ☑ Glukoz (GLU)                    15.50 TL          │    │
│  │ ☑ Üre (BUN)                       12.00 TL          │    │
│  │ ☑ Kreatinin (CREA)                18.00 TL          │    │
│  │ ☐ ALT                             20.00 TL          │    │
│  │ ☐ AST                             20.00 TL          │    │
│  │                                                      │    │
│  │ Toplam: 45.50 TL                                    │    │
│  └─────────────────────────────────────────────────────┘    │
│                                                               │
│  Numune Bilgileri                                            │
│  ┌─────────────────────────────────────────────────────┐    │
│  │ Numune Tipi: [Serum ▼]                              │    │
│  │ Öncelik: [Normal ▼]                                 │    │
│  │ Notlar: [Açlık kan şekeri]                          │    │
│  └─────────────────────────────────────────────────────┘    │
│                                                               │
│  [İptal]                                    [Kaydet ve Yazdır]│
└─────────────────────────────────────────────────────────────┘
```

#### 7.3.2 Sonuç Görüntüleme Ekranı

```
┌─────────────────────────────────────────────────────────────┐
│  MediKef LBYS > Test Sonuçları                      [Logout] │
├─────────────────────────────────────────────────────────────┤
│                                                               │
│  Hasta: Ahmet YILMAZ (P2024000001)                          │
│  Numune: BAR2024000001                                       │
│  Tarih: 28.12.2024 10:30                                    │
│                                                               │
│  ┌─────────────────────────────────────────────────────┐    │
│  │ Test      Sonuç  Birim   Referans   Flag  Durum    │    │
│  ├─────────────────────────────────────────────────────┤    │
│  │ Glukoz    95     mg/dL   70-110     N     ✓ Final  │    │
│  │ Üre       28     mg/dL   10-50      N     ✓ Final  │    │
│  │ Kreatinin 0.9    mg/dL   0.6-1.2    N     ✓ Final  │    │
│  └─────────────────────────────────────────────────────┘    │
│                                                               │
│  Cihaz: Cobas c 311 (COBAS_C311_01)                         │
│  Onaylayan: Dr. Ayşe DEMİR                                  │
│  Onay Tarihi: 28.12.2024 11:00                              │
│                                                               │
│  [Yazdır]  [PDF İndir]  [E-posta Gönder]          [Onayla] │
└─────────────────────────────────────────────────────────────┘
```

### 7.4 Angular Modül Yapısı

```typescript
src/
├── app/
│   ├── core/                    // Singleton servisler
│   │   ├── services/
│   │   │   ├── auth.service.ts
│   │   │   ├── api.service.ts
│   │   │   └── notification.service.ts
│   │   ├── guards/
│   │   │   └── auth.guard.ts
│   │   └── interceptors/
│   │       └── http.interceptor.ts
│   │
│   ├── shared/                  // Paylaşılan bileşenler
│   │   ├── components/
│   │   │   ├── header/
│   │   │   ├── sidebar/
│   │   │   └── loading/
│   │   ├── pipes/
│   │   │   └── date-format.pipe.ts
│   │   └── directives/
│   │
│   ├── features/                // Özellik modülleri
│   │   ├── dashboard/
│   │   ├── patients/
│   │   ├── samples/
│   │   ├── results/
│   │   ├── tests/
│   │   └── devices/
│   │
│   ├── app.component.ts
│   ├── app.routes.ts
│   └── app.config.ts
│
└── environments/
    ├── environment.ts
    └── environment.prod.ts
```

---

## 8. GÜVENLİK

### 8.1 API Güvenliği

**LisBox API Key:**
- Header: `X-API-Key: LISBOX_SECRET_KEY_2024`
- Ortam değişkeninde saklanmalı
- Periyodik olarak değiştirilmeli

**JWT Authentication (Gelecek):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "...",
  "expiresIn": 3600
}
```

### 8.2 Veri Güvenliği

**Şifreleme:**
- HTTPS kullanımı (TLS 1.3)
- Veritabanı şifreleri hash'lenmeli (bcrypt)
- Hassas veriler encrypted olmalı

**SQL Injection Koruması:**
- Entity Framework parametreli sorgular kullanır
- Raw SQL kullanımından kaçınılmalı

**XSS Koruması:**
- Angular otomatik sanitization yapar
- HTML binding'de dikkatli olunmalı

### 8.3 Audit Trail

**Tüm kritik işlemler loglanmalı:**
- Hasta ekleme/düzenleme/silme
- Numune oluşturma
- Test sonucu onaylama
- Kullanıcı giriş/çıkış

**Log Formatı:**
```json
{
  "Timestamp": "2024-12-28T10:30:00",
  "UserId": "user@example.com",
  "Action": "SAMPLE_CREATED",
  "EntityType": "Sample",
  "EntityId": 123,
  "IpAddress": "192.168.1.100",
  "UserAgent": "Mozilla/5.0..."
}
```

---

## 9. DEPLOYMENT

### 9.1 Docker Deployment

**docker-compose.yml:**
```yaml
version: '3.8'

services:
  postgres:
    image: postgres:16-alpine
    environment:
      POSTGRES_DB: medikef_db
      POSTGRES_USER: medikef_user
      POSTGRES_PASSWORD: medikef_pass_2024
    ports:
      - "5432:5432"
    volumes:
      - postgres_data:/var/lib/postgresql/data

  backend:
    build: ./src/Backend/MediKef.Api
    ports:
      - "5000:80"
    depends_on:
      - postgres
    environment:
      ConnectionStrings__DefaultConnection: "Host=postgres;Database=medikef_db;Username=medikef_user;Password=medikef_pass_2024"

  frontend:
    build: ./src/Frontend
    ports:
      - "4200:80"
    depends_on:
      - backend

volumes:
  postgres_data:
```

**Deployment Komutları:**
```bash
# Build
docker-compose build

# Start
docker-compose up -d

# Stop
docker-compose down

# Logs
docker-compose logs -f
```

### 9.2 Production Deployment

**Gereksinimler:**
- Ubuntu 22.04 LTS
- Docker 24+
- Nginx (reverse proxy)
- SSL sertifikası (Let's Encrypt)

**Nginx Konfigürasyonu:**
```nginx
server {
    listen 80;
    server_name medikef.example.com;
    return 301 https://$server_name$request_uri;
}

server {
    listen 443 ssl http2;
    server_name medikef.example.com;

    ssl_certificate /etc/letsencrypt/live/medikef.example.com/fullchain.pem;
    ssl_certificate_key /etc/letsencrypt/live/medikef.example.com/privkey.pem;

    location /api/ {
        proxy_pass http://localhost:5000/api/;
        proxy_set_header Host $host;
        proxy_set_header X-Real-IP $remote_addr;
    }

    location / {
        proxy_pass http://localhost:4200/;
        proxy_set_header Host $host;
    }
}
```

### 9.3 Backup Stratejisi

**PostgreSQL Backup:**
```bash
# Daily backup
0 2 * * * pg_dump -h localhost -U medikef_user medikef_db > /backup/medikef_$(date +\%Y\%m\%d).sql

# Retention: 30 days
find /backup -name "medikef_*.sql" -mtime +30 -delete
```

**Restore:**
```bash
psql -h localhost -U medikef_user medikef_db < /backup/medikef_20241228.sql
```

---

## 10. TEST STRATEJİSİ

### 10.1 Unit Tests

**Backend (xUnit):**
```csharp
[Fact]
public async Task ReceiveResults_ValidData_ReturnsSuccess()
{
    // Arrange
    var controller = new LisBoxController(_context, _logger, _config);
    var dto = new LisBoxResultDto { ... };

    // Act
    var result = await controller.ReceiveResults(dto);

    // Assert
    Assert.IsType<OkObjectResult>(result.Result);
}
```

**Frontend (Jasmine/Karma):**
```typescript
describe('SampleService', () => {
  it('should create sample with barcode', () => {
    const sample = service.createSample(patientId, testIds);
    expect(sample.barcode).toMatch(/^BAR\d{10}$/);
  });
});
```

### 10.2 Integration Tests

**API Integration Test:**
```csharp
[Fact]
public async Task EndToEnd_CreateSampleAndReceiveResults()
{
    // 1. Create patient
    var patient = await CreatePatient();

    // 2. Create sample
    var sample = await CreateSample(patient.Id);

    // 3. Send device results
    var result = await SendDeviceResults(sample.Barcode);

    // 4. Verify results saved
    var testResults = await GetTestResults(sample.Id);
    Assert.NotEmpty(testResults);
}
```

### 10.3 Performance Tests

**Load Testing (k6):**
```javascript
import http from 'k6/http';

export let options = {
  vus: 100,
  duration: '30s',
};

export default function() {
  http.post('http://localhost:5000/api/LisBox/receive-results',
    JSON.stringify({...}),
    { headers: { 'X-API-Key': 'LISBOX_SECRET_KEY_2024' } }
  );
}
```

**Hedefler:**
- Response time < 200ms (95th percentile)
- Throughput > 1000 req/sec
- Error rate < 0.1%

### 10.4 Manual Test Scenarios

**Senaryo 1: Numune Kabul ve Sonuç Alma**
1. Hasta kaydı oluştur
2. Numune oluştur (3 test seç)
3. Barkod yazdır
4. LisBox Simulator ile sonuç gönder
5. Sonuçları görüntüle
6. Sonuçları onayla

**Senaryo 2: Hata Durumları**
1. Olmayan barkod ile sonuç gönder → 404 hatası
2. Geçersiz API Key ile istek → 401 hatası
3. Talep edilmemiş test sonucu gönder → Log'a kaydedilmeli

---

## 11. PROJE PLANI

### 11.1 Faz 1: Backend (Tamamlandı ✅)
- [x] Veritabanı tasarımı
- [x] Entity Framework modelleri
- [x] API Controller'ları
- [x] LisBox entegrasyonu
- [x] LisBox Simulator
- [x] Migration ve seed data

### 11.2 Faz 2: Frontend (Devam Ediyor 🔄)
- [ ] Angular proje kurulumu
- [ ] Routing ve layout
- [ ] Dashboard sayfası
- [ ] Hasta yönetimi sayfaları
- [ ] Numune kabul sayfası
- [ ] Sonuç görüntüleme sayfası
- [ ] Test katalog sayfası
- [ ] Cihaz yönetimi sayfası

### 11.3 Faz 3: Test ve Deployment (Beklemede ⏳)
- [ ] Unit testler
- [ ] Integration testler
- [ ] Performance testler
- [ ] Docker deployment
- [ ] Production deployment
- [ ] Dokümantasyon

### 11.4 Faz 4: İyileştirmeler (Gelecek 🔮)
- [ ] Real-time updates (SignalR)
- [ ] Rapor modülü (PDF, Excel)
- [ ] E-imza entegrasyonu
- [ ] HBYS entegrasyonu
- [ ] Mobil uygulama
- [ ] Dark mode
- [ ] Multi-language support

---

## 12. SONUÇ

MediKef LBYS, modern teknolojiler kullanılarak geliştirilmiş, **LisBox entegrasyonu** ile laboratuvar cihazlarından otomatik veri alabilen, kullanıcı dostu bir LBYS yazılımıdır.

**Temel Avantajlar:**
- ✅ Otomatik veri transferi (manuel giriş hatası yok)
- ✅ Hızlı sonuç raporlama
- ✅ Barkod sistemi ile izlenebilirlik
- ✅ Modern ve kullanıcı dostu arayüz
- ✅ Ölçeklenebilir mimari
- ✅ Açık kaynak teknolojiler (maliyet avantajı)

**Hedef Pazar:**
- Özel laboratuvarlar
- Hastane laboratuvarları
- Tıp merkezleri
- Klinikler

**Rekabet Avantajı:**
- Infomed benzeri kullanıcı deneyimi
- LisBox ile kolay entegrasyon
- Uygun fiyat
- Türkçe destek
- Özelleştirilebilir

---

## 13. EK KAYNAKLAR

### 13.1 Dokümantasyon
- [README.md](../README.md) - Proje genel bilgiler
- [database/schema.sql](../database/schema.sql) - Veritabanı şeması
- [database/seed-data.sql](../database/seed-data.sql) - Demo veriler

### 13.2 API Dokümantasyonu
- Swagger UI: `http://localhost:5218/swagger`

### 13.3 Referanslar
- [HL7 v2.x Specification](https://www.hl7.org/)
- [ASTM E1394-97 Standard](https://www.astm.org/)
- [Entity Framework Core Docs](https://docs.microsoft.com/ef/core/)
- [Angular Material](https://material.angular.io/)
- [PostgreSQL Documentation](https://www.postgresql.org/docs/)

---

**Doküman Versiyonu:** 1.0
**Son Güncelleme:** 28 Aralık 2024
**Hazırlayan:** MediKef Development Team
**İletişim:** info@medikef.com

| Teknoloji | Versiyon | Kullanım Amacı |
|-----------|----------|----------------|
| **Angular** | 17+ | Frontend framework |
| **TypeScript** | 5.0+ | Programlama dili |
| **Angular Material** | 17+ | UI component library |
| **RxJS** | 7.8+ | Reactive programming |
| **Chart.js** | 4.0+ | Grafik ve raporlama |

### 3.3 Database

| Teknoloji | Versiyon | Kullanım Amacı |
|-----------|----------|----------------|
| **PostgreSQL** | 16 | İlişkisel veritabanı |
| **pgAdmin** | 4 | Database yönetim aracı |

### 3.4 DevOps

| Teknoloji | Versiyon | Kullanım Amacı |
|-----------|----------|----------------|
| **Docker** | 24+ | Containerization |
| **Docker Compose** | 2.0+ | Multi-container orchestration |
| **Git** | 2.0+ | Version control |

---

## 4. VERİTABANI TASARIMI

### 4.1 Entity Relationship Diagram (ERD)

```
┌─────────────────┐
│    patients     │
├─────────────────┤
│ id (PK)         │
│ patient_id (UK) │
│ tc_no (UK)      │
│ first_name      │
│ last_name       │
│ birth_date      │
│ gender          │
│ phone           │
│ email           │
│ address         │
└────────┬────────┘
         │ 1
         │
         │ N
┌────────▼────────┐
│    samples      │
├─────────────────┤
│ id (PK)         │
│ sample_id (UK)  │
│ barcode (UK)    │
│ patient_id (FK) │
│ sample_type     │
│ collection_date │
│ status          │
│ priority        │
└────────┬────────┘
         │ 1
         │
         │ N
┌────────▼────────┐       ┌─────────────────┐
│  sample_tests   │ N   1 │     tests       │
├─────────────────┤───────├─────────────────┤
│ id (PK)         │       │ id (PK)         │
│ sample_id (FK)  │       │ test_code (UK)  │
│ test_id (FK)    │       │ test_name       │
│ status          │       │ test_category   │
└────────┬────────┘       │ unit            │
         │ 1              │ reference_range │
         │                └─────────────────┘
         │ N
┌────────▼────────┐
│  test_results   │       ┌─────────────────┐
├─────────────────┤ N   1 │    devices      │
│ id (PK)         │───────├─────────────────┤
│ sample_test_id  │       │ id (PK)         │
│ device_id (FK)  │       │ device_id (UK)  │
│ result_value    │       │ device_name     │
│ result_numeric  │       │ manufacturer    │
│ unit            │       │ model           │
│ flag            │       │ protocol        │
│ result_status   │       │ connection_type │
│ result_date     │       └─────────────────┘
│ validated_by    │
└─────────────────┘

┌─────────────────┐
│  lisbox_logs    │
├─────────────────┤
│ id (PK)         │
│ device_id (FK)  │
│ sample_barcode  │
│ raw_data        │
│ parsed_data     │
│ status          │
│ error_message   │
└─────────────────┘
```

### 4.2 Tablo Detayları

#### 4.2.1 patients (Hastalar)

