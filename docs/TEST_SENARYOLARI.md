# 🧪 MediKef LBYS - Test Senaryoları

## 📋 İçindekiler
1. [Birim Testleri (Unit Tests)](#1-birim-testleri)
2. [Entegrasyon Testleri (Integration Tests)](#2-entegrasyon-testleri)
3. [End-to-End Testleri](#3-end-to-end-testleri)
4. [Performans Testleri](#4-performans-testleri)
5. [Güvenlik Testleri](#5-güvenlik-testleri)

---

## 1. Birim Testleri (Unit Tests)

### 1.1 Backend - LisBoxController Tests

#### Test 1.1.1: Geçerli API Key ile Test Sonucu Alma
```csharp
[Fact]
public async Task ReceiveResults_WithValidApiKey_ReturnsSuccess()
{
    // Arrange
    var controller = new LisBoxController(_context, _configuration);
    var request = new LisBoxResultDto
    {
        DeviceId = "COBAS_C311_01",
        SampleBarcode = "BAR2024001",
        TestResults = new List<TestResultItemDto>
        {
            new TestResultItemDto
            {
                TestCode = "GLU",
                TestName = "Glukoz",
                ResultValue = "95",
                ResultNumeric = 95.0m,
                Unit = "mg/dL",
                ReferenceRange = "70-110",
                Flag = "N"
            }
        }
    };
    
    // Act
    var result = await controller.ReceiveResults(request);
    
    // Assert
    Assert.IsType<OkObjectResult>(result.Result);
    var okResult = result.Result as OkObjectResult;
    var response = okResult.Value as LisBoxResponseDto;
    Assert.True(response.Success);
    Assert.Equal(1, response.ProcessedTests);
}
```

#### Test 1.1.2: Geçersiz API Key ile Test Sonucu Alma
```csharp
[Fact]
public async Task ReceiveResults_WithInvalidApiKey_ReturnsUnauthorized()
{
    // Arrange
    var controller = new LisBoxController(_context, _configuration);
    // Set invalid API key in request header
    
    // Act
    var result = await controller.ReceiveResults(validRequest);
    
    // Assert
    Assert.IsType<UnauthorizedResult>(result.Result);
}
```

#### Test 1.1.3: Olmayan Cihaz ID ile Test Sonucu Alma
```csharp
[Fact]
public async Task ReceiveResults_WithNonExistentDevice_ReturnsNotFound()
{
    // Arrange
    var request = new LisBoxResultDto
    {
        DeviceId = "INVALID_DEVICE_ID",
        SampleBarcode = "BAR2024001",
        TestResults = new List<TestResultItemDto>()
    };
    
    // Act
    var result = await controller.ReceiveResults(request);
    
    // Assert
    Assert.IsType<NotFoundObjectResult>(result.Result);
}
```

#### Test 1.1.4: Olmayan Barkod ile Test Sonucu Alma
```csharp
[Fact]
public async Task ReceiveResults_WithNonExistentBarcode_ReturnsNotFound()
{
    // Arrange
    var request = new LisBoxResultDto
    {
        DeviceId = "COBAS_C311_01",
        SampleBarcode = "INVALID_BARCODE",
        TestResults = new List<TestResultItemDto>()
    };
    
    // Act
    var result = await controller.ReceiveResults(request);
    
    // Assert
    Assert.IsType<NotFoundObjectResult>(result.Result);
}
```

### 1.2 Backend - PatientsController Tests

#### Test 1.2.1: Hasta Oluşturma - Başarılı
```csharp
[Fact]
public async Task CreatePatient_WithValidData_ReturnsCreated()
{
    // Arrange
    var dto = new CreatePatientDto
    {
        TcNo = "12345678901",
        FirstName = "Test",
        LastName = "User",
        BirthDate = new DateTime(1990, 1, 1),
        Gender = "M"
    };
    
    // Act
    var result = await controller.CreatePatient(dto);
    
    // Assert
    Assert.IsType<CreatedAtActionResult>(result.Result);
    var createdResult = result.Result as CreatedAtActionResult;
    var patient = createdResult.Value as Patient;
    Assert.NotNull(patient.PatientId);
    Assert.StartsWith("P2024", patient.PatientId);
}
```

#### Test 1.2.2: Hasta Oluşturma - Geçersiz TC No
```csharp
[Fact]
public async Task CreatePatient_WithInvalidTcNo_ReturnsBadRequest()
{
    // Arrange
    var dto = new CreatePatientDto
    {
        TcNo = "123", // Invalid TC No (must be 11 digits)
        FirstName = "Test",
        LastName = "User"
    };
    
    // Act & Assert
    await Assert.ThrowsAsync<ValidationException>(
        () => controller.CreatePatient(dto)
    );
}
```

#### Test 1.2.3: Hasta Arama - İsim ile
```csharp
[Fact]
public async Task SearchPatients_ByName_ReturnsMatchingPatients()
{
    // Arrange
    var searchTerm = "Ahmet";
    
    // Act
    var result = await controller.GetPatients(searchTerm);
    
    // Assert
    Assert.IsType<OkObjectResult>(result.Result);
    var patients = (result.Result as OkObjectResult).Value as List<Patient>;
    Assert.All(patients, p => 
        Assert.Contains(searchTerm, p.FirstName, StringComparison.OrdinalIgnoreCase)
    );
}
```

### 1.3 Backend - SamplesController Tests

#### Test 1.3.1: Numune Oluşturma - Otomatik Barkod
```csharp
[Fact]
public async Task CreateSample_AutoGeneratesBarcode()
{
    // Arrange
    var dto = new CreateSampleDto
    {
        PatientId = 1,
        SampleType = "Serum",
        TestIds = new List<int> { 1, 2, 3 }
    };
    
    // Act
    var result = await controller.CreateSample(dto);
    
    // Assert
    var sample = (result.Result as CreatedAtActionResult).Value as Sample;
    Assert.NotNull(sample.Barcode);
    Assert.StartsWith("BAR2024", sample.Barcode);
    Assert.Matches(@"^BAR\d{10}$", sample.Barcode);
}
```

#### Test 1.3.2: Numune Oluşturma - Test İlişkileri
```csharp
[Fact]
public async Task CreateSample_CreatesTestRelationships()
{
    // Arrange
    var dto = new CreateSampleDto
    {
        PatientId = 1,
        SampleType = "Serum",
        TestIds = new List<int> { 1, 2, 3 }
    };
    
    // Act
    var result = await controller.CreateSample(dto);
    
    // Assert
    var sample = await _context.Samples
        .Include(s => s.SampleTests)
        .FirstOrDefaultAsync(s => s.Id == sample.Id);
    
    Assert.Equal(3, sample.SampleTests.Count);
}
```

---

## 2. Entegrasyon Testleri (Integration Tests)

### 2.1 LisBox Entegrasyon Testi - Tam Akış

#### Senaryo: Cihazdan Sonuç Alma ve Kaydetme
```
1. Veritabanında test verisi oluştur:
   - Hasta: Ahmet Yılmaz (TC: 12345678901)
   - Numune: BAR2024001 (Serum, Pending)
   - Testler: GLU, CREA, BUN

2. LisBox Simulator'dan test sonucu gönder:
   POST /api/LisBox/receive-results
   {
     "DeviceId": "COBAS_C311_01",
     "SampleBarcode": "BAR2024001",
     "TestResults": [...]
   }

3. Doğrulama:
   - HTTP 200 OK dönmeli
   - test_results tablosuna kayıt eklenmeli
   - sample_tests.status = "Completed" olmalı
   - sample.status = "Completed" olmalı (tüm testler tamamsa)
   - lisbox_logs tablosuna log kaydı eklenmeli

4. Sonuçları API'den çek:
   GET /api/Samples/1

5. Doğrulama:
   - Numune detayları dönmeli
   - Test sonuçları dönmeli
   - Cihaz bilgisi dönmeli
```

### 2.2 Hasta-Numune-Test Akışı

#### Senaryo: Yeni Hasta Kaydı ve Numune Oluşturma
```
1. Yeni hasta oluştur:
   POST /api/Patients
   {
     "tcNo": "98765432109",
     "firstName": "Ayşe",
     "lastName": "Demir",
     ...
   }

2. Doğrulama:
   - HTTP 201 Created
   - PatientId otomatik oluşturulmalı (P2024XXXXXX)

3. Numune oluştur:
   POST /api/Samples
   {
     "patientId": <yeni_hasta_id>,
     "sampleType": "Serum",
     "testIds": [1, 2, 3]
   }

4. Doğrulama:
   - HTTP 201 Created
   - SampleId ve Barcode otomatik oluşturulmalı
   - sample_tests tablosuna 3 kayıt eklenmeli
   - Status = "Pending" olmalı

5. Numune detaylarını çek:
   GET /api/Samples/<sample_id>

6. Doğrulama:
   - Hasta bilgileri dönmeli
   - 3 test dönmeli
   - Her test için status = "Pending" olmalı
```

### 2.3 Çoklu Cihaz Testi

#### Senaryo: Farklı Cihazlardan Aynı Numuneye Sonuç Gelme
```
1. Numune oluştur (Biyokimya + Hemogram testleri):
   - GLU, CREA (Biyokimya - Cobas c 311)
   - WBC, RBC (Hemogram - Sysmex XN-550)

2. Cobas'tan sonuç gönder:
   POST /api/LisBox/receive-results
   DeviceId: COBAS_C311_01
   Tests: GLU, CREA

3. Doğrulama:
   - Biyokimya testleri "Completed"
   - Hemogram testleri hala "Pending"
   - Sample.status = "InProgress"

4. Sysmex'ten sonuç gönder:
   POST /api/LisBox/receive-results
   DeviceId: SYSMEX_XN550_01
   Tests: WBC, RBC

5. Doğrulama:
   - Tüm testler "Completed"
   - Sample.status = "Completed"
```

---

## 3. End-to-End Testleri

### 3.1 Tam İş Akışı Testi

#### Senaryo: Hasta Kabulden Sonuç Raporuna
```
ADIM 1: RESEPSIYON - Hasta Kaydı
- Frontend: Hasta kayıt formu doldur
- Backend: POST /api/Patients
- Doğrulama: Hasta ID oluşturuldu

ADIM 2: RESEPSIYON - Numune Kabul
- Frontend: Hasta seç, testleri seç
- Backend: POST /api/Samples
- Doğrulama: Barkod yazdırıldı (BAR2024XXX)

ADIM 3: LAB TEKNİSYENİ - Numune Hazırlık
- Numuneyi santrifüj et
- Barkodu cihaza okut
- Cihaza yerleştir

ADIM 4: CİHAZ - Test Çalıştır
- Cihaz testi çalıştırır
- HL7 mesajı oluşturur
- LisBox'a gönderir

ADIM 5: LISBOX - Veri İşleme
- HL7 parse et
- JSON'a dönüştür
- Backend API'ye POST et

ADIM 6: BACKEND - Sonuç Kaydet
- API Key doğrula
- Cihaz ve numune kontrol et
- Test sonuçlarını kaydet
- Numune durumunu güncelle

ADIM 7: FRONTEND - Sonuç Görüntüleme
- Dashboard'da yeni sonuç bildirimi
- Sonuç detay sayfasını aç
- Test sonuçlarını görüntüle
- Referans aralıkları kontrol et
- Anormal değerleri vurgula

ADIM 8: DOKTOR - Onay ve Rapor
- Sonuçları incele
- Onay ver
- Rapor yazdır/PDF oluştur
```

### 3.2 Hata Senaryoları

#### Senaryo 3.2.1: Yanlış Barkod
```
1. Cihazdan yanlış barkod ile sonuç gelir
2. Backend: 404 Not Found döner
3. LisBox: Retry mekanizması devreye girer
4. lisbox_logs: Hata kaydedilir
5. Frontend: Admin panelinde hata bildirimi
```

#### Senaryo 3.2.2: Cihaz Bağlantı Hatası
```
1. Cihaz offline olur
2. LisBox: Connection timeout
3. LisBox: Retry (3 deneme)
4. LisBox: Hata logla
5. Frontend: Cihaz durumu "Offline" göster
```

#### Senaryo 3.2.3: Duplicate Sonuç
```
1. Aynı test için 2. kez sonuç gelir
2. Backend: Mevcut sonucu günceller (overwrite)
3. lisbox_logs: "Duplicate result" kaydı
4. Frontend: Güncelleme bildirimi
```

---

## 4. Performans Testleri

### 4.1 Yük Testi (Load Testing)

#### Test 4.1.1: Eşzamanlı LisBox İstekleri
```
Senaryo: 10 cihazdan aynı anda sonuç gelme

Araç: Apache JMeter / k6

Konfigürasyon:
- Thread Count: 10
- Ramp-up Period: 5 saniye
- Loop Count: 100

Beklenen Sonuç:
- Ortalama Response Time: < 500ms
- 95th Percentile: < 1000ms
- Error Rate: < 0.1%
- Throughput: > 100 req/sec
```

#### Test 4.1.2: Hasta Arama Performansı
```
Senaryo: 10,000 hasta kaydı ile arama

Test Adımları:
1. 10,000 hasta kaydı oluştur
2. 100 eşzamanlı kullanıcı arama yapsın
3. Farklı arama terimleri kullan

Beklenen Sonuç:
- Arama süresi: < 200ms
- Database query optimization
- Index kullanımı doğrulanmalı
```

### 4.2 Stres Testi (Stress Testing)

#### Test 4.2.1: Maksimum Kapasite
```
Senaryo: Sistemin kırılma noktasını bul

Adımlar:
1. 10 kullanıcı ile başla
2. Her 30 saniyede 10 kullanıcı ekle
3. Hata oranı %5'i geçene kadar devam et

Ölçümler:
- Maksimum eşzamanlı kullanıcı sayısı
- CPU kullanımı
- Memory kullanımı
- Database connection pool
```

### 4.3 Dayanıklılık Testi (Endurance Testing)

#### Test 4.3.1: 24 Saat Kesintisiz Çalışma
```
Senaryo: Sistem 24 saat boyunca orta yük altında çalışsın

Konfigürasyon:
- Sabit 50 kullanıcı
- Süre: 24 saat
- İşlemler: Hasta kaydı, numune oluşturma, sonuç alma

Ölçümler:
- Memory leak kontrolü
- Database connection leak
- Response time degradation
- Error rate artışı
```

---

## 5. Güvenlik Testleri

### 5.1 Authentication & Authorization

#### Test 5.1.1: API Key Doğrulama
```
Test Case 1: Geçerli API Key
- Header: X-API-Key: LISBOX_SECRET_KEY_2024
- Beklenen: 200 OK

Test Case 2: Geçersiz API Key
- Header: X-API-Key: INVALID_KEY
- Beklenen: 401 Unauthorized

Test Case 3: API Key Yok
- Header yok
- Beklenen: 401 Unauthorized

Test Case 4: Boş API Key
- Header: X-API-Key:
- Beklenen: 401 Unauthorized
```

#### Test 5.1.2: JWT Token Doğrulama (Planned)
```
Test Case 1: Geçerli Token
- Authorization: Bearer <valid_token>
- Beklenen: 200 OK

Test Case 2: Expired Token
- Authorization: Bearer <expired_token>
- Beklenen: 401 Unauthorized

Test Case 3: Manipulated Token
- Authorization: Bearer <tampered_token>
- Beklenen: 401 Unauthorized
```

### 5.2 SQL Injection

#### Test 5.2.1: Hasta Arama SQL Injection
```
Test Case 1: Basic SQL Injection
- Input: search='; DROP TABLE patients; --
- Beklenen: Parameterized query kullanıldığı için güvenli

Test Case 2: Union-based Injection
- Input: search=' UNION SELECT * FROM users --
- Beklenen: Güvenli

Doğrulama:
- Entity Framework parameterized queries kullanıyor
- Raw SQL kullanılmıyor
```

### 5.3 XSS (Cross-Site Scripting)

#### Test 5.3.1: Hasta Adı XSS
```
Test Case 1: Script Tag
- Input: firstName=<script>alert('XSS')</script>
- Beklenen: Encode edilmeli, çalıştırılmamalı

Test Case 2: Event Handler
- Input: firstName=<img src=x onerror=alert('XSS')>
- Beklenen: Sanitize edilmeli

Doğrulama:
- Angular otomatik sanitization
- Backend validation
```

### 5.4 CSRF (Cross-Site Request Forgery)

#### Test 5.4.1: CSRF Token Kontrolü
```
Test: Farklı origin'den istek
- Origin: http://malicious-site.com
- Beklenen: CORS policy tarafından bloklanmalı

Doğrulama:
- CORS sadece http://localhost:4200 izin veriyor
- Production'da sadece gerçek domain
```

### 5.5 Rate Limiting (Planned)

#### Test 5.5.1: Brute Force Koruması
```
Test: 100 istek/dakika limiti
- 100 istek gönder (1 dakika içinde)
- 101. istek: 429 Too Many Requests
- 1 dakika bekle
- Yeni istek: 200 OK
```

---

## 📊 Test Metrikleri

### Başarı Kriterleri

| Metrik | Hedef | Kritik |
|--------|-------|--------|
| Unit Test Coverage | > 80% | > 60% |
| Integration Test Coverage | > 70% | > 50% |
| API Response Time (avg) | < 500ms | < 1000ms |
| API Response Time (p95) | < 1000ms | < 2000ms |
| Error Rate | < 0.1% | < 1% |
| Uptime | > 99.9% | > 99% |

### Test Araçları

**Backend Testing:**
- xUnit (Unit & Integration Tests)
- Moq (Mocking)
- FluentAssertions (Assertions)
- Bogus (Test Data Generation)

**API Testing:**
- Postman (Manual Testing)
- Newman (Automated Postman Tests)
- k6 (Load Testing)

**Frontend Testing:**
- Jasmine (Unit Tests)
- Karma (Test Runner)
- Protractor / Cypress (E2E Tests)

**Security Testing:**
- OWASP ZAP (Vulnerability Scanning)
- SonarQube (Code Quality & Security)

---

## 🚀 Test Çalıştırma

### Backend Unit Tests
```bash
cd src/Backend/MediKef.Api.Tests
dotnet test --collect:"XPlat Code Coverage"
```

### Backend Integration Tests
```bash
cd src/Backend/MediKef.Api.IntegrationTests
dotnet test
```

### API Tests (Postman)
```bash
newman run docs/MediKef_LBYS.postman_collection.json \
  --environment docs/MediKef_LBYS.postman_environment.json
```

### Load Tests (k6)
```bash
k6 run tests/load-test.js
```

---

**Doküman Versiyonu:** 1.0
**Son Güncelleme:** 28 Aralık 2024
**Hazırlayan:** MediKef Development Team

