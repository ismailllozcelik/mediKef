# 🔌 MediKef LBYS - API Endpoints Dokümanı

## 📋 İçindekiler
1. [LisBox Integration API](#1-lisbox-integration-api)
2. [Patients API](#2-patients-api)
3. [Samples API](#3-samples-api)
4. [Tests API](#4-tests-api)
5. [Devices API](#5-devices-api)
6. [Authentication](#6-authentication)

---

## Base URL
```
Development: http://localhost:5218/api
Production: https://api.medikef.com/api
```

## Common Headers
```
Content-Type: application/json
Accept: application/json
```

---

## 1. LisBox Integration API

### 1.1 Receive Test Results from LisBox

**Endpoint:** `POST /LisBox/receive-results`

**Description:** LisBox middleware'den cihaz test sonuçlarını alır.

**Headers:**
```
X-API-Key: LISBOX_SECRET_KEY_2024
Content-Type: application/json
```

**Request Body:**
```json
{
  "DeviceId": "COBAS_C311_01",
  "SampleBarcode": "BAR2024001",
  "TestResults": [
    {
      "TestCode": "GLU",
      "TestName": "Glukoz",
      "ResultValue": "95",
      "ResultNumeric": 95.0,
      "Unit": "mg/dL",
      "ReferenceRange": "70-110",
      "Flag": "N",
      "ResultDateTime": "2024-12-28T10:30:00Z"
    },
    {
      "TestCode": "CREA",
      "TestName": "Kreatinin",
      "ResultValue": "0.9",
      "ResultNumeric": 0.9,
      "Unit": "mg/dL",
      "ReferenceRange": "0.7-1.3",
      "Flag": "N",
      "ResultDateTime": "2024-12-28T10:30:00Z"
    }
  ],
  "Status": "Final",
  "Timestamp": "2024-12-28T10:30:00Z"
}
```

**Response (200 OK):**
```json
{
  "success": true,
  "message": "Test sonuçları başarıyla kaydedildi",
  "sampleId": "S2024000001",
  "barcode": "BAR2024001",
  "processedTests": 2,
  "timestamp": "2024-12-28T10:30:05Z"
}
```

**Response (401 Unauthorized):**
```json
{
  "success": false,
  "message": "Geçersiz API Key"
}
```

**Response (404 Not Found):**
```json
{
  "success": false,
  "message": "Cihaz bulunamadı: COBAS_C311_01"
}
```

**Response (404 Not Found - Sample):**
```json
{
  "success": false,
  "message": "Numune bulunamadı: BAR2024001"
}
```

**Response (400 Bad Request):**
```json
{
  "success": false,
  "message": "Test bulunamadı: INVALID_CODE"
}
```

---

## 2. Patients API

### 2.1 Get All Patients

**Endpoint:** `GET /Patients`

**Query Parameters:**
- `search` (optional): Hasta adı, soyadı veya TC No ile arama

**Request:**
```
GET /api/Patients?search=Ahmet
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "patientId": "P2024000001",
    "tcNo": "12345678901",
    "firstName": "Ahmet",
    "lastName": "Yılmaz",
    "birthDate": "1985-05-15",
    "gender": "M",
    "phone": "05551234567",
    "email": "ahmet.yilmaz@example.com",
    "address": "İstanbul, Türkiye",
    "createdAt": "2024-12-28T09:00:00Z",
    "updatedAt": "2024-12-28T09:00:00Z"
  }
]
```

### 2.2 Get Patient by ID

**Endpoint:** `GET /Patients/{id}`

**Request:**
```
GET /api/Patients/1
```

**Response (200 OK):**
```json
{
  "id": 1,
  "patientId": "P2024000001",
  "tcNo": "12345678901",
  "firstName": "Ahmet",
  "lastName": "Yılmaz",
  "birthDate": "1985-05-15",
  "gender": "M",
  "phone": "05551234567",
  "email": "ahmet.yilmaz@example.com",
  "address": "İstanbul, Türkiye",
  "createdAt": "2024-12-28T09:00:00Z",
  "updatedAt": "2024-12-28T09:00:00Z",
  "samples": []
}
```

**Response (404 Not Found):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404
}
```

### 2.3 Create Patient

**Endpoint:** `POST /Patients`

**Request Body:**
```json
{
  "tcNo": "98765432109",
  "firstName": "Ayşe",
  "lastName": "Demir",
  "birthDate": "1990-08-20",
  "gender": "F",
  "phone": "05559876543",
  "email": "ayse.demir@example.com",
  "address": "Ankara, Türkiye"
}
```

**Response (201 Created):**
```json
{
  "id": 6,
  "patientId": "P2024000006",
  "tcNo": "98765432109",
  "firstName": "Ayşe",
  "lastName": "Demir",
  "birthDate": "1990-08-20",
  "gender": "F",
  "phone": "05559876543",
  "email": "ayse.demir@example.com",
  "address": "Ankara, Türkiye",
  "createdAt": "2024-12-28T11:00:00Z",
  "updatedAt": "2024-12-28T11:00:00Z"
}
```

**Response (400 Bad Request):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "TcNo": ["TC No 11 karakter olmalıdır"],
    "FirstName": ["Ad alanı zorunludur"]
  }
}
```

### 2.4 Update Patient

**Endpoint:** `PUT /Patients/{id}`

**Request:**
```
PUT /api/Patients/6
```

**Request Body:**
```json
{
  "tcNo": "98765432109",
  "firstName": "Ayşe",
  "lastName": "Demir",
  "birthDate": "1990-08-20",
  "gender": "F",
  "phone": "05559999999",
  "email": "ayse.demir.new@example.com",
  "address": "İzmir, Türkiye"
}
```

**Response (204 No Content)**

**Response (404 Not Found):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404
}
```

### 2.5 Delete Patient

**Endpoint:** `DELETE /Patients/{id}`

**Request:**
```
DELETE /api/Patients/6
```

**Response (204 No Content)**

**Response (404 Not Found):**
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404
}
```

---

## 3. Samples API

### 3.1 Get All Samples

**Endpoint:** `GET /Samples`

**Query Parameters:**
- `patientId` (optional): Hasta ID'sine göre filtreleme
- `status` (optional): Durum filtresi (Pending, InProgress, Completed, Cancelled)

**Request:**
```
GET /api/Samples?patientId=1&status=Pending
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "sampleId": "S2024000001",
    "barcode": "BAR2024001",
    "patientId": 1,
    "patient": {
      "id": 1,
      "patientId": "P2024000001",
      "firstName": "Ahmet",
      "lastName": "Yılmaz"
    },
    "sampleType": "Serum",
    "collectionDate": "2024-12-28T08:00:00Z",
    "receivedDate": "2024-12-28T08:30:00Z",
    "status": "Pending",
    "priority": "Normal",
    "notes": null,
    "createdBy": "user1",
    "createdAt": "2024-12-28T08:30:00Z",
    "updatedAt": "2024-12-28T08:30:00Z"
  }
]
```

### 3.2 Get Sample by ID

**Endpoint:** `GET /Samples/{id}`

**Request:**
```
GET /api/Samples/1
```

**Response (200 OK):**
```json
{
  "id": 1,
  "sampleId": "S2024000001",
  "barcode": "BAR2024001",
  "patientId": 1,
  "patient": {
    "id": 1,
    "patientId": "P2024000001",
    "tcNo": "12345678901",
    "firstName": "Ahmet",
    "lastName": "Yılmaz",
    "birthDate": "1985-05-15",
    "gender": "M"
  },
  "sampleType": "Serum",
  "collectionDate": "2024-12-28T08:00:00Z",
  "receivedDate": "2024-12-28T08:30:00Z",
  "status": "Completed",
  "priority": "Normal",
  "notes": null,
  "createdBy": "user1",
  "createdAt": "2024-12-28T08:30:00Z",
  "updatedAt": "2024-12-28T10:35:00Z",
  "sampleTests": [
    {
      "id": 1,
      "testId": 1,
      "test": {
        "id": 1,
        "testCode": "GLU",
        "testName": "Glukoz",
        "testCategory": "Biyokimya",
        "unit": "mg/dL",
        "referenceRange": "70-110"
      },
      "status": "Completed",
      "testResults": [
        {
          "id": 1,
          "resultValue": "95",
          "resultNumeric": 95.0,
          "unit": "mg/dL",
          "referenceRange": "70-110",
          "flag": "N",
          "resultStatus": "Final",
          "resultDate": "2024-12-28T10:30:00Z",
          "deviceId": 1,
          "device": {
            "deviceId": "COBAS_C311_01",
            "deviceName": "Cobas c 311",
            "manufacturer": "Roche"
          }
        }
      ]
    }
  ]
}
```

### 3.3 Create Sample

**Endpoint:** `POST /Samples`

**Request Body:**
```json
{
  "patientId": 1,
  "sampleType": "Serum",
  "collectionDate": "2024-12-28T09:00:00Z",
  "priority": "Urgent",
  "notes": "Açlık testi",
  "testIds": [1, 2, 3, 4, 5]
}
```

**Response (201 Created):**
```json
{
  "id": 4,
  "sampleId": "S2024000004",
  "barcode": "BAR2024004",
  "patientId": 1,
  "sampleType": "Serum",
  "collectionDate": "2024-12-28T09:00:00Z",
  "receivedDate": "2024-12-28T11:15:00Z",
  "status": "Pending",
  "priority": "Urgent",
  "notes": "Açlık testi",
  "createdBy": "user1",
  "createdAt": "2024-12-28T11:15:00Z",
  "updatedAt": "2024-12-28T11:15:00Z"
}
```

### 3.4 Update Sample Status

**Endpoint:** `PUT /Samples/{id}/status`

**Request:**
```
PUT /api/Samples/1/status
```

**Request Body:**
```json
{
  "status": "InProgress"
}
```

**Response (204 No Content)**

---

## 4. Tests API

### 4.1 Get All Tests

**Endpoint:** `GET /Tests`

**Query Parameters:**
- `category` (optional): Test kategorisi (Biyokimya, Hemogram, Hormon)

**Request:**
```
GET /api/Tests?category=Biyokimya
```

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "testCode": "GLU",
    "testName": "Glukoz",
    "testCategory": "Biyokimya",
    "unit": "mg/dL",
    "referenceRange": "70-110",
    "sampleType": "Serum",
    "price": 15.00,
    "isActive": true,
    "createdAt": "2024-12-28T00:00:00Z",
    "updatedAt": "2024-12-28T00:00:00Z"
  },
  {
    "id": 2,
    "testCode": "CREA",
    "testName": "Kreatinin",
    "testCategory": "Biyokimya",
    "unit": "mg/dL",
    "referenceRange": "0.7-1.3",
    "sampleType": "Serum",
    "price": 12.00,
    "isActive": true,
    "createdAt": "2024-12-28T00:00:00Z",
    "updatedAt": "2024-12-28T00:00:00Z"
  }
]
```

### 4.2 Get Test Categories

**Endpoint:** `GET /Tests/categories`

**Response (200 OK):**
```json
[
  "Biyokimya",
  "Hemogram",
  "Hormon"
]
```

### 4.3 Get Test by ID

**Endpoint:** `GET /Tests/{id}`

**Request:**
```
GET /api/Tests/1
```

**Response (200 OK):**
```json
{
  "id": 1,
  "testCode": "GLU",
  "testName": "Glukoz",
  "testCategory": "Biyokimya",
  "unit": "mg/dL",
  "referenceRange": "70-110",
  "sampleType": "Serum",
  "price": 15.00,
  "isActive": true,
  "createdAt": "2024-12-28T00:00:00Z",
  "updatedAt": "2024-12-28T00:00:00Z"
}
```

---

## 5. Devices API

### 5.1 Get All Devices

**Endpoint:** `GET /Devices`

**Response (200 OK):**
```json
[
  {
    "id": 1,
    "deviceId": "COBAS_C311_01",
    "deviceName": "Cobas c 311",
    "manufacturer": "Roche",
    "model": "c 311",
    "serialNumber": "SN-COBAS-2024-001",
    "deviceType": "Biyokimya",
    "protocol": "HL7",
    "connectionType": "TCP/IP",
    "ipAddress": "192.168.1.101",
    "port": 5000,
    "isActive": true,
    "createdAt": "2024-12-28T00:00:00Z",
    "updatedAt": "2024-12-28T00:00:00Z"
  }
]
```

---

## 6. Authentication

### 6.1 Login (Planned)

**Endpoint:** `POST /Auth/login`

**Request Body:**
```json
{
  "username": "admin",
  "password": "Admin123!"
}
```

**Response (200 OK):**
```json
{
  "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "expiresAt": "2024-12-29T11:00:00Z",
  "user": {
    "id": 1,
    "username": "admin",
    "fullName": "Admin User",
    "role": "Admin"
  }
}
```

---

## 📊 HTTP Status Codes

| Code | Meaning | Description |
|------|---------|-------------|
| 200 | OK | İstek başarılı |
| 201 | Created | Kaynak oluşturuldu |
| 204 | No Content | İstek başarılı, içerik yok |
| 400 | Bad Request | Geçersiz istek |
| 401 | Unauthorized | Kimlik doğrulama gerekli |
| 403 | Forbidden | Erişim reddedildi |
| 404 | Not Found | Kaynak bulunamadı |
| 500 | Internal Server Error | Sunucu hatası |

---

## 🔐 Security

### API Key Authentication (LisBox)
LisBox endpoint'i için `X-API-Key` header'ı zorunludur:
```
X-API-Key: LISBOX_SECRET_KEY_2024
```

### JWT Authentication (Planned)
Diğer endpoint'ler için JWT token kullanılacak:
```
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

---

## 📝 Notes

1. Tüm tarih/saat değerleri **ISO 8601** formatında ve **UTC** timezone'da gönderilmelidir.
2. Pagination henüz implement edilmemiştir (gelecek versiyonlarda eklenecek).
3. Rate limiting henüz aktif değildir.
4. API versiyonlama henüz yapılmamıştır.

---

**Doküman Versiyonu:** 1.0
**Son Güncelleme:** 28 Aralık 2024
**Hazırlayan:** MediKef Development Team

