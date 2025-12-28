-- MediKef LBYS - PostgreSQL Database Schema
-- Infomed tarzı Laboratuvar Bilgi Yönetim Sistemi

-- ============================================
-- 1. PATIENTS (Hastalar)
-- ============================================
CREATE TABLE IF NOT EXISTS patients (
    id SERIAL PRIMARY KEY,
    patient_id VARCHAR(50) UNIQUE NOT NULL,
    tc_no VARCHAR(11) UNIQUE,
    first_name VARCHAR(100) NOT NULL,
    last_name VARCHAR(100) NOT NULL,
    birth_date DATE,
    gender VARCHAR(10) CHECK (gender IN ('Erkek', 'Kadın', 'Diğer')),
    phone VARCHAR(20),
    email VARCHAR(100),
    address TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_patients_tc_no ON patients(tc_no);
CREATE INDEX idx_patients_patient_id ON patients(patient_id);

-- ============================================
-- 2. DEVICES (Cihazlar)
-- ============================================
CREATE TABLE IF NOT EXISTS devices (
    id SERIAL PRIMARY KEY,
    device_id VARCHAR(50) UNIQUE NOT NULL,
    device_name VARCHAR(100) NOT NULL,
    manufacturer VARCHAR(100),
    model VARCHAR(100),
    serial_number VARCHAR(100),
    device_type VARCHAR(50), -- Biyokimya, Hemogram, Hormon, vb.
    protocol VARCHAR(20) CHECK (protocol IN ('HL7', 'ASTM', 'Custom')),
    connection_type VARCHAR(20) CHECK (connection_type IN ('Serial', 'TCP/IP', 'USB')),
    ip_address VARCHAR(50),
    port INTEGER,
    is_active BOOLEAN DEFAULT true,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_devices_device_id ON devices(device_id);

-- ============================================
-- 3. TESTS (Test Kataloğu)
-- ============================================
CREATE TABLE IF NOT EXISTS tests (
    id SERIAL PRIMARY KEY,
    test_code VARCHAR(20) UNIQUE NOT NULL,
    test_name VARCHAR(200) NOT NULL,
    test_category VARCHAR(100), -- Biyokimya, Hemogram, Hormon, vb.
    unit VARCHAR(50),
    reference_range_min DECIMAL(10, 3),
    reference_range_max DECIMAL(10, 3),
    reference_range_text VARCHAR(200),
    sample_type VARCHAR(50), -- Kan, İdrar, vb.
    price DECIMAL(10, 2),
    is_active BOOLEAN DEFAULT true,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_tests_test_code ON tests(test_code);
CREATE INDEX idx_tests_category ON tests(test_category);

-- ============================================
-- 4. SAMPLES (Numuneler)
-- ============================================
CREATE TABLE IF NOT EXISTS samples (
    id SERIAL PRIMARY KEY,
    sample_id VARCHAR(50) UNIQUE NOT NULL,
    barcode VARCHAR(50) UNIQUE NOT NULL,
    patient_id INTEGER NOT NULL REFERENCES patients(id) ON DELETE CASCADE,
    sample_type VARCHAR(50),
    collection_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    received_date TIMESTAMP,
    status VARCHAR(50) DEFAULT 'Pending' CHECK (status IN ('Pending', 'InProgress', 'Completed', 'Cancelled')),
    priority VARCHAR(20) DEFAULT 'Normal' CHECK (priority IN ('Normal', 'Urgent', 'STAT')),
    notes TEXT,
    created_by VARCHAR(100),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_samples_barcode ON samples(barcode);
CREATE INDEX idx_samples_patient_id ON samples(patient_id);
CREATE INDEX idx_samples_status ON samples(status);

-- ============================================
-- 5. SAMPLE_TESTS (Numune Test İlişkisi)
-- ============================================
CREATE TABLE IF NOT EXISTS sample_tests (
    id SERIAL PRIMARY KEY,
    sample_id INTEGER NOT NULL REFERENCES samples(id) ON DELETE CASCADE,
    test_id INTEGER NOT NULL REFERENCES tests(id) ON DELETE CASCADE,
    status VARCHAR(50) DEFAULT 'Pending' CHECK (status IN ('Pending', 'InProgress', 'Completed', 'Cancelled')),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    UNIQUE(sample_id, test_id)
);

CREATE INDEX idx_sample_tests_sample_id ON sample_tests(sample_id);
CREATE INDEX idx_sample_tests_test_id ON sample_tests(test_id);

-- ============================================
-- 6. TEST_RESULTS (Test Sonuçları)
-- ============================================
CREATE TABLE IF NOT EXISTS test_results (
    id SERIAL PRIMARY KEY,
    sample_test_id INTEGER NOT NULL REFERENCES sample_tests(id) ON DELETE CASCADE,
    device_id INTEGER REFERENCES devices(id),
    result_value VARCHAR(200),
    result_numeric DECIMAL(10, 3),
    unit VARCHAR(50),
    reference_range VARCHAR(200),
    flag VARCHAR(10) CHECK (flag IN ('N', 'H', 'L', 'HH', 'LL', 'A')), -- Normal, High, Low, Very High, Very Low, Abnormal
    result_status VARCHAR(50) DEFAULT 'Preliminary' CHECK (result_status IN ('Preliminary', 'Final', 'Corrected', 'Cancelled')),
    result_date TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    validated_by VARCHAR(100),
    validated_at TIMESTAMP,
    notes TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_test_results_sample_test_id ON test_results(sample_test_id);
CREATE INDEX idx_test_results_device_id ON test_results(device_id);
CREATE INDEX idx_test_results_result_status ON test_results(result_status);

-- ============================================
-- 7. LISBOX_LOGS (LisBox Entegrasyon Logları)
-- ============================================
CREATE TABLE IF NOT EXISTS lisbox_logs (
    id SERIAL PRIMARY KEY,
    device_id INTEGER REFERENCES devices(id),
    sample_barcode VARCHAR(50),
    raw_data TEXT,
    parsed_data JSONB,
    status VARCHAR(50) CHECK (status IN ('Success', 'Failed', 'Partial')),
    error_message TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_lisbox_logs_device_id ON lisbox_logs(device_id);
CREATE INDEX idx_lisbox_logs_sample_barcode ON lisbox_logs(sample_barcode);
CREATE INDEX idx_lisbox_logs_created_at ON lisbox_logs(created_at);

-- ============================================
-- 8. USERS (Kullanıcılar - Basit)
-- ============================================
CREATE TABLE IF NOT EXISTS users (
    id SERIAL PRIMARY KEY,
    username VARCHAR(50) UNIQUE NOT NULL,
    password_hash VARCHAR(255) NOT NULL,
    full_name VARCHAR(200) NOT NULL,
    role VARCHAR(50) CHECK (role IN ('Admin', 'Doctor', 'Technician', 'Receptionist')),
    is_active BOOLEAN DEFAULT true,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
);

CREATE INDEX idx_users_username ON users(username);

