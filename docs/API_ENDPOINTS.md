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


