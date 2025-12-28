-- MediKef LBYS - Seed Data (Test Verileri)

-- ============================================
-- 1. Test Cihazları
-- ============================================
INSERT INTO devices (device_id, device_name, manufacturer, model, device_type, protocol, connection_type, is_active) VALUES
('COBAS_C311_01', 'Cobas c 311', 'Roche', 'c 311', 'Biyokimya', 'ASTM', 'TCP/IP', true),
('SYSMEX_XN550_01', 'Sysmex XN-550', 'Sysmex', 'XN-550', 'Hemogram', 'HL7', 'TCP/IP', true),
('COBAS_E411_01', 'Cobas e 411', 'Roche', 'e 411', 'Hormon', 'ASTM', 'TCP/IP', true),
('SIMULATOR_01', 'LisBox Simulator', 'MediKef', 'v1.0', 'Simülatör', 'Custom', 'TCP/IP', true);

-- ============================================
-- 2. Test Kataloğu - Biyokimya
-- ============================================
INSERT INTO tests (test_code, test_name, test_category, unit, reference_range_min, reference_range_max, reference_range_text, sample_type, price, is_active) VALUES
-- Biyokimya
('GLU', 'Glukoz', 'Biyokimya', 'mg/dL', 70, 110, '70-110 mg/dL', 'Kan', 15.00, true),
('BUN', 'Üre', 'Biyokimya', 'mg/dL', 10, 50, '10-50 mg/dL', 'Kan', 15.00, true),
('CREA', 'Kreatinin', 'Biyokimya', 'mg/dL', 0.6, 1.2, '0.6-1.2 mg/dL', 'Kan', 15.00, true),
('ALT', 'ALT (SGPT)', 'Biyokimya', 'U/L', 0, 55, '0-55 U/L', 'Kan', 20.00, true),
('AST', 'AST (SGOT)', 'Biyokimya', 'U/L', 0, 40, '0-40 U/L', 'Kan', 20.00, true),
('CHOL', 'Total Kolesterol', 'Biyokimya', 'mg/dL', 0, 200, '<200 mg/dL', 'Kan', 20.00, true),
('TRIG', 'Trigliserit', 'Biyokimya', 'mg/dL', 0, 150, '<150 mg/dL', 'Kan', 20.00, true),
('HDL', 'HDL Kolesterol', 'Biyokimya', 'mg/dL', 40, 60, '>40 mg/dL', 'Kan', 20.00, true),
('LDL', 'LDL Kolesterol', 'Biyokimya', 'mg/dL', 0, 130, '<130 mg/dL', 'Kan', 20.00, true),
('CA', 'Kalsiyum', 'Biyokimya', 'mg/dL', 8.5, 10.5, '8.5-10.5 mg/dL', 'Kan', 15.00, true),
('NA', 'Sodyum', 'Biyokimya', 'mmol/L', 136, 145, '136-145 mmol/L', 'Kan', 15.00, true),
('K', 'Potasyum', 'Biyokimya', 'mmol/L', 3.5, 5.1, '3.5-5.1 mmol/L', 'Kan', 15.00, true),
('CL', 'Klor', 'Biyokimya', 'mmol/L', 98, 107, '98-107 mmol/L', 'Kan', 15.00, true);

-- ============================================
-- 3. Test Kataloğu - Hemogram
-- ============================================
INSERT INTO tests (test_code, test_name, test_category, unit, reference_range_min, reference_range_max, reference_range_text, sample_type, price, is_active) VALUES
('WBC', 'Lökosit', 'Hemogram', '10^3/uL', 4.5, 11.0, '4.5-11.0 10^3/uL', 'Kan', 25.00, true),
('RBC', 'Eritrosit', 'Hemogram', '10^6/uL', 4.5, 5.5, '4.5-5.5 10^6/uL', 'Kan', 25.00, true),
('HGB', 'Hemoglobin', 'Hemogram', 'g/dL', 12.0, 16.0, '12.0-16.0 g/dL', 'Kan', 25.00, true),
('HCT', 'Hematokrit', 'Hemogram', '%', 36.0, 46.0, '36-46 %', 'Kan', 25.00, true),
('MCV', 'MCV', 'Hemogram', 'fL', 80.0, 100.0, '80-100 fL', 'Kan', 25.00, true),
('MCH', 'MCH', 'Hemogram', 'pg', 27.0, 32.0, '27-32 pg', 'Kan', 25.00, true),
('MCHC', 'MCHC', 'Hemogram', 'g/dL', 32.0, 36.0, '32-36 g/dL', 'Kan', 25.00, true),
('PLT', 'Trombosit', 'Hemogram', '10^3/uL', 150.0, 400.0, '150-400 10^3/uL', 'Kan', 25.00, true),
('NEU', 'Nötrofil', 'Hemogram', '%', 40.0, 70.0, '40-70 %', 'Kan', 25.00, true),
('LYM', 'Lenfosit', 'Hemogram', '%', 20.0, 45.0, '20-45 %', 'Kan', 25.00, true);

-- ============================================
-- 4. Test Kataloğu - Hormon
-- ============================================
INSERT INTO tests (test_code, test_name, test_category, unit, reference_range_min, reference_range_max, reference_range_text, sample_type, price, is_active) VALUES
('TSH', 'TSH', 'Hormon', 'uIU/mL', 0.4, 4.0, '0.4-4.0 uIU/mL', 'Kan', 35.00, true),
('FT3', 'Serbest T3', 'Hormon', 'pg/mL', 2.3, 4.2, '2.3-4.2 pg/mL', 'Kan', 35.00, true),
('FT4', 'Serbest T4', 'Hormon', 'ng/dL', 0.8, 1.8, '0.8-1.8 ng/dL', 'Kan', 35.00, true),
('VIT_D', 'Vitamin D', 'Hormon', 'ng/mL', 30.0, 100.0, '30-100 ng/mL', 'Kan', 50.00, true),
('VIT_B12', 'Vitamin B12', 'Hormon', 'pg/mL', 200.0, 900.0, '200-900 pg/mL', 'Kan', 40.00, true),
('FERRITIN', 'Ferritin', 'Hormon', 'ng/mL', 20.0, 250.0, '20-250 ng/mL', 'Kan', 40.00, true);

-- ============================================
-- 5. Demo Hastalar
-- ============================================
INSERT INTO patients (patient_id, tc_no, first_name, last_name, birth_date, gender, phone, email, address) VALUES
('P2024001', '12345678901', 'Ahmet', 'Yılmaz', '1985-05-15', 'Erkek', '0532 123 4567', 'ahmet.yilmaz@email.com', 'Ankara, Çankaya'),
('P2024002', '12345678902', 'Ayşe', 'Demir', '1990-08-22', 'Kadın', '0533 234 5678', 'ayse.demir@email.com', 'İstanbul, Kadıköy'),
('P2024003', '12345678903', 'Mehmet', 'Kaya', '1978-03-10', 'Erkek', '0534 345 6789', 'mehmet.kaya@email.com', 'İzmir, Bornova'),
('P2024004', '12345678904', 'Fatma', 'Şahin', '1995-11-30', 'Kadın', '0535 456 7890', 'fatma.sahin@email.com', 'Bursa, Nilüfer'),
('P2024005', '12345678905', 'Ali', 'Çelik', '1982-07-18', 'Erkek', '0536 567 8901', 'ali.celik@email.com', 'Antalya, Muratpaşa');

-- ============================================
-- 6. Demo Kullanıcılar (Şifre: admin123)
-- ============================================
INSERT INTO users (username, password_hash, full_name, role, is_active) VALUES
('admin', '$2a$11$XYZ...', 'Sistem Yöneticisi', 'Admin', true),
('dr.ayse', '$2a$11$XYZ...', 'Dr. Ayşe Yılmaz', 'Doctor', true),
('teknisyen1', '$2a$11$XYZ...', 'Mehmet Demir', 'Technician', true),
('resepsiyon1', '$2a$11$XYZ...', 'Fatma Kaya', 'Receptionist', true);

-- ============================================
-- 7. Demo Numune ve Test Talepleri
-- ============================================
INSERT INTO samples (sample_id, barcode, patient_id, sample_type, status, priority) VALUES
('S2024001', 'BAR2024001', 1, 'Kan', 'Pending', 'Normal'),
('S2024002', 'BAR2024002', 2, 'Kan', 'Pending', 'Urgent'),
('S2024003', 'BAR2024003', 3, 'Kan', 'Pending', 'Normal');

-- Sample 1 için testler (Biyokimya paneli)
INSERT INTO sample_tests (sample_id, test_id, status) VALUES
(1, 1, 'Pending'),  -- GLU
(1, 2, 'Pending'),  -- BUN
(1, 3, 'Pending'),  -- CREA
(1, 4, 'Pending'),  -- ALT
(1, 5, 'Pending');  -- AST

-- Sample 2 için testler (Hemogram)
INSERT INTO sample_tests (sample_id, test_id, status) VALUES
(2, 14, 'Pending'), -- WBC
(2, 15, 'Pending'), -- RBC
(2, 16, 'Pending'), -- HGB
(2, 17, 'Pending'), -- HCT
(2, 21, 'Pending'); -- PLT

-- Sample 3 için testler (Hormon paneli)
INSERT INTO sample_tests (sample_id, test_id, status) VALUES
(3, 24, 'Pending'), -- TSH
(3, 25, 'Pending'), -- FT3
(3, 26, 'Pending'); -- FT4

