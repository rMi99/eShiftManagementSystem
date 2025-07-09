-- e-Shift Management System Database Schema
-- MySQL/MariaDB Database Schema
-- Version: 1.0.0

-- Create database if not exists
CREATE DATABASE IF NOT EXISTS eshift_db 
CHARACTER SET utf8mb4 
COLLATE utf8mb4_unicode_ci;

USE eshift_db;

-- Set foreign key checks
SET foreign_key_checks = 0;

-- Drop tables if they exist (for clean reinstall)
DROP TABLE IF EXISTS audit_logs;
DROP TABLE IF EXISTS load_products;
DROP TABLE IF EXISTS job_status_history;
DROP TABLE IF EXISTS loads;
DROP TABLE IF EXISTS quotes;
DROP TABLE IF EXISTS jobs;
DROP TABLE IF EXISTS transport_units;
DROP TABLE IF EXISTS assistants;
DROP TABLE IF EXISTS drivers;
DROP TABLE IF EXISTS staff;
DROP TABLE IF EXISTS containers;
DROP TABLE IF EXISTS vehicles;
DROP TABLE IF EXISTS vehicle_types;
DROP TABLE IF EXISTS products;
DROP TABLE IF EXISTS product_categories;
DROP TABLE IF EXISTS customers;
DROP TABLE IF EXISTS user_roles;
DROP TABLE IF EXISTS users;
DROP TABLE IF EXISTS roles;

-- Enable foreign key checks
SET foreign_key_checks = 1;

-- Create roles table
CREATE TABLE roles (
    role_id INT AUTO_INCREMENT PRIMARY KEY,
    role_name VARCHAR(50) NOT NULL UNIQUE,
    description TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Create users table
CREATE TABLE users (
    user_id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(50) NOT NULL UNIQUE,
    email VARCHAR(100) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL,
    role VARCHAR(50) NOT NULL DEFAULT 'customer',
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    last_login TIMESTAMP NULL,
    INDEX idx_username (username),
    INDEX idx_email (email),
    INDEX idx_role (role)
);

-- Create user_roles table (for future RBAC enhancement)
CREATE TABLE user_roles (
    user_role_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    role_id INT NOT NULL,
    assigned_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    assigned_by INT,
    FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE,
    FOREIGN KEY (role_id) REFERENCES roles(role_id) ON DELETE CASCADE,
    FOREIGN KEY (assigned_by) REFERENCES users(user_id) ON DELETE SET NULL,
    UNIQUE KEY unique_user_role (user_id, role_id)
);

-- Create customers table
CREATE TABLE customers (
    customer_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    phone VARCHAR(20),
    address TEXT,
    city VARCHAR(100),
    postal_code VARCHAR(20),
    country VARCHAR(100) DEFAULT 'Pakistan',
    registration_date DATE DEFAULT (CURRENT_DATE),
    is_active BOOLEAN DEFAULT TRUE,
    FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE,
    INDEX idx_name (first_name, last_name),
    INDEX idx_city (city)
);

-- Create product_categories table
CREATE TABLE product_categories (
    category_id INT AUTO_INCREMENT PRIMARY KEY,
    category_name VARCHAR(100) NOT NULL UNIQUE,
    description TEXT,
    is_fragile BOOLEAN DEFAULT FALSE,
    default_handling_cost DECIMAL(10,2) DEFAULT 0.00,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP
);

-- Create products table
CREATE TABLE products (
    product_id INT AUTO_INCREMENT PRIMARY KEY,
    category_id INT NOT NULL,
    product_name VARCHAR(100) NOT NULL,
    description TEXT,
    unit_weight DECIMAL(10,2),
    unit_volume DECIMAL(10,2),
    fragile BOOLEAN DEFAULT FALSE,
    special_handling_required BOOLEAN DEFAULT FALSE,
    base_cost_per_unit DECIMAL(10,2) DEFAULT 0.00,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (category_id) REFERENCES product_categories(category_id) ON DELETE RESTRICT,
    INDEX idx_category (category_id),
    INDEX idx_name (product_name)
);

-- Create vehicle_types table
CREATE TABLE vehicle_types (
    vehicle_type_id INT AUTO_INCREMENT PRIMARY KEY,
    type_name VARCHAR(50) NOT NULL UNIQUE,
    max_weight_capacity DECIMAL(10,2) NOT NULL,
    max_volume_capacity DECIMAL(10,2) NOT NULL,
    cost_per_km DECIMAL(10,2) DEFAULT 0.00,
    description TEXT
);

-- Create vehicles table
CREATE TABLE vehicles (
    vehicle_id INT AUTO_INCREMENT PRIMARY KEY,
    vehicle_type_id INT NOT NULL,
    registration_number VARCHAR(50) NOT NULL UNIQUE,
    make VARCHAR(50) NOT NULL,
    model VARCHAR(50) NOT NULL,
    year INT NOT NULL,
    mileage DECIMAL(10,2) DEFAULT 0.00,
    last_service_date DATE,
    next_service_date DATE,
    is_available BOOLEAN DEFAULT TRUE,
    fuel_capacity DECIMAL(10,2),
    insurance_expiry DATE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (vehicle_type_id) REFERENCES vehicle_types(vehicle_type_id) ON DELETE RESTRICT,
    INDEX idx_registration (registration_number),
    INDEX idx_availability (is_available)
);

-- Create containers table
CREATE TABLE containers (
    container_id INT AUTO_INCREMENT PRIMARY KEY,
    container_number VARCHAR(50) NOT NULL UNIQUE,
    type VARCHAR(50) NOT NULL,
    max_weight DECIMAL(10,2) NOT NULL,
    max_volume DECIMAL(10,2) NOT NULL,
    is_available BOOLEAN DEFAULT TRUE,
    condition_status VARCHAR(50) DEFAULT 'good',
    last_inspection_date DATE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    INDEX idx_container_number (container_number),
    INDEX idx_availability (is_available)
);

-- Create staff table
CREATE TABLE staff (
    staff_id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT NOT NULL,
    first_name VARCHAR(50) NOT NULL,
    last_name VARCHAR(50) NOT NULL,
    phone VARCHAR(20),
    address TEXT,
    position VARCHAR(100) NOT NULL,
    hire_date DATE DEFAULT (CURRENT_DATE),
    salary DECIMAL(10,2) DEFAULT 0.00,
    is_active BOOLEAN DEFAULT TRUE,
    emergency_contact_name VARCHAR(100),
    emergency_contact_phone VARCHAR(20),
    FOREIGN KEY (user_id) REFERENCES users(user_id) ON DELETE CASCADE,
    INDEX idx_name (first_name, last_name),
    INDEX idx_position (position)
);

-- Create drivers table
CREATE TABLE drivers (
    driver_id INT AUTO_INCREMENT PRIMARY KEY,
    staff_id INT NOT NULL,
    license_number VARCHAR(50) NOT NULL UNIQUE,
    license_type VARCHAR(50) NOT NULL,
    license_expiry_date DATE NOT NULL,
    experience_years INT DEFAULT 0,
    vehicle_type_preference VARCHAR(100),
    is_available BOOLEAN DEFAULT TRUE,
    current_vehicle_id INT NULL,
    rating DECIMAL(3,2) DEFAULT 0.00,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (staff_id) REFERENCES staff(staff_id) ON DELETE CASCADE,
    FOREIGN KEY (current_vehicle_id) REFERENCES vehicles(vehicle_id) ON DELETE SET NULL,
    INDEX idx_license (license_number),
    INDEX idx_availability (is_available)
);

-- Create assistants table
CREATE TABLE assistants (
    assistant_id INT AUTO_INCREMENT PRIMARY KEY,
    staff_id INT NOT NULL,
    specialization VARCHAR(100),
    is_available BOOLEAN DEFAULT TRUE,
    current_job_id INT NULL,
    rating DECIMAL(3,2) DEFAULT 0.00,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (staff_id) REFERENCES staff(staff_id) ON DELETE CASCADE,
    INDEX idx_availability (is_available)
);

-- Create transport_units table
CREATE TABLE transport_units (
    transport_unit_id INT AUTO_INCREMENT PRIMARY KEY,
    unit_name VARCHAR(100) NOT NULL,
    vehicle_id INT NOT NULL,
    driver_id INT,
    assistant_id INT,
    container_id INT,
    is_active BOOLEAN DEFAULT TRUE,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (vehicle_id) REFERENCES vehicles(vehicle_id) ON DELETE CASCADE,
    FOREIGN KEY (driver_id) REFERENCES drivers(driver_id) ON DELETE SET NULL,
    FOREIGN KEY (assistant_id) REFERENCES assistants(assistant_id) ON DELETE SET NULL,
    FOREIGN KEY (container_id) REFERENCES containers(container_id) ON DELETE SET NULL,
    INDEX idx_vehicle (vehicle_id),
    INDEX idx_active (is_active)
);

-- Create jobs table
CREATE TABLE jobs (
    job_id INT AUTO_INCREMENT PRIMARY KEY,
    customer_id INT NOT NULL,
    job_number VARCHAR(50) NOT NULL UNIQUE,
    pickup_address TEXT NOT NULL,
    pickup_city VARCHAR(100) NOT NULL,
    pickup_postal_code VARCHAR(20),
    destination_address TEXT NOT NULL,
    destination_city VARCHAR(100) NOT NULL,
    destination_postal_code VARCHAR(20),
    requested_pickup_date DATE NOT NULL,
    requested_delivery_date DATE,
    actual_pickup_date DATE,
    actual_delivery_date DATE,
    status VARCHAR(50) DEFAULT 'pending',
    total_estimated_weight DECIMAL(10,2),
    total_estimated_volume DECIMAL(10,2),
    total_actual_weight DECIMAL(10,2),
    total_actual_volume DECIMAL(10,2),
    special_instructions TEXT,
    transport_unit_id INT,
    estimated_cost DECIMAL(12,2),
    final_cost DECIMAL(12,2),
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (customer_id) REFERENCES customers(customer_id) ON DELETE RESTRICT,
    FOREIGN KEY (transport_unit_id) REFERENCES transport_units(transport_unit_id) ON DELETE SET NULL,
    INDEX idx_job_number (job_number),
    INDEX idx_customer (customer_id),
    INDEX idx_status (status),
    INDEX idx_pickup_date (requested_pickup_date)
);

-- Create quotes table
CREATE TABLE quotes (
    quote_id INT AUTO_INCREMENT PRIMARY KEY,
    customer_id INT NOT NULL,
    job_id INT,
    quote_number VARCHAR(50) NOT NULL UNIQUE,
    estimated_cost DECIMAL(12,2) NOT NULL,
    description TEXT,
    breakdown_details JSON,
    valid_until DATE NOT NULL,
    status VARCHAR(50) DEFAULT 'pending',
    approved_by INT,
    approved_at TIMESTAMP NULL,
    declined_reason TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (customer_id) REFERENCES customers(customer_id) ON DELETE RESTRICT,
    FOREIGN KEY (job_id) REFERENCES jobs(job_id) ON DELETE CASCADE,
    FOREIGN KEY (approved_by) REFERENCES users(user_id) ON DELETE SET NULL,
    INDEX idx_quote_number (quote_number),
    INDEX idx_customer (customer_id),
    INDEX idx_status (status),
    INDEX idx_valid_until (valid_until)
);

-- Create loads table
CREATE TABLE loads (
    load_id INT AUTO_INCREMENT PRIMARY KEY,
    job_id INT NOT NULL,
    load_number VARCHAR(50) NOT NULL,
    description TEXT,
    total_weight DECIMAL(10,2),
    total_volume DECIMAL(10,2),
    is_fragile BOOLEAN DEFAULT FALSE,
    special_handling_required BOOLEAN DEFAULT FALSE,
    loading_instructions TEXT,
    status VARCHAR(50) DEFAULT 'pending',
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
    FOREIGN KEY (job_id) REFERENCES jobs(job_id) ON DELETE CASCADE,
    INDEX idx_job (job_id),
    INDEX idx_load_number (load_number),
    INDEX idx_status (status)
);

-- Create load_products table (many-to-many relationship)
CREATE TABLE load_products (
    load_product_id INT AUTO_INCREMENT PRIMARY KEY,
    load_id INT NOT NULL,
    product_id INT NOT NULL,
    quantity INT NOT NULL DEFAULT 1,
    unit_weight DECIMAL(10,2),
    unit_volume DECIMAL(10,2),
    total_weight DECIMAL(10,2),
    total_volume DECIMAL(10,2),
    condition_notes TEXT,
    FOREIGN KEY (load_id) REFERENCES loads(load_id) ON DELETE CASCADE,
    FOREIGN KEY (product_id) REFERENCES products(product_id) ON DELETE RESTRICT,
    UNIQUE KEY unique_load_product (load_id, product_id),
    INDEX idx_load (load_id),
    INDEX idx_product (product_id)
);

-- Create job_status_history table
CREATE TABLE job_status_history (
    history_id INT AUTO_INCREMENT PRIMARY KEY,
    job_id INT NOT NULL,
    old_status VARCHAR(50),
    new_status VARCHAR(50) NOT NULL,
    changed_by INT NOT NULL,
    change_reason TEXT,
    changed_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    additional_notes TEXT,
    FOREIGN KEY (job_id) REFERENCES jobs(job_id) ON DELETE CASCADE,
    FOREIGN KEY (changed_by) REFERENCES users(user_id) ON DELETE RESTRICT,
    INDEX idx_job (job_id),
    INDEX idx_changed_at (changed_at)
);

-- Create audit_logs table
CREATE TABLE audit_logs (
    audit_id INT AUTO_INCREMENT PRIMARY KEY,
    table_name VARCHAR(100) NOT NULL,
    record_id INT NOT NULL,
    action_type VARCHAR(20) NOT NULL, -- INSERT, UPDATE, DELETE
    old_values JSON,
    new_values JSON,
    changed_by INT,
    ip_address VARCHAR(45),
    user_agent TEXT,
    created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (changed_by) REFERENCES users(user_id) ON DELETE SET NULL,
    INDEX idx_table_record (table_name, record_id),
    INDEX idx_action_type (action_type),
    INDEX idx_changed_by (changed_by),
    INDEX idx_created_at (created_at)
);

-- Insert default roles
INSERT INTO roles (role_name, description) VALUES 
('super_admin', 'Super Administrator with full system access'),
('admin', 'Administrator with management access'),
('customer', 'Customer with limited access to their data'),
('driver', 'Driver with access to assigned jobs'),
('staff', 'Staff member with operational access');

-- Insert default vehicle types
INSERT INTO vehicle_types (type_name, max_weight_capacity, max_volume_capacity, cost_per_km, description) VALUES
('Small Van', 1000.00, 8.00, 15.00, 'Small delivery van for household goods'),
('Medium Truck', 3000.00, 20.00, 25.00, 'Medium truck for apartment moves'),
('Large Truck', 8000.00, 50.00, 40.00, 'Large truck for house moves'),
('Container Truck', 25000.00, 100.00, 60.00, 'Large container truck for commercial moves');

-- Insert default product categories
INSERT INTO product_categories (category_name, description, is_fragile, default_handling_cost) VALUES
('Furniture', 'Household furniture items', FALSE, 50.00),
('Electronics', 'Electronic devices and appliances', TRUE, 100.00),
('Glassware', 'Glass items and fragile decoratives', TRUE, 75.00),
('Clothing', 'Clothes and textiles', FALSE, 10.00),
('Books', 'Books and documents', FALSE, 15.00),
('Kitchen Items', 'Kitchen utensils and cookware', FALSE, 25.00),
('Artwork', 'Paintings and artwork', TRUE, 150.00),
('Sports Equipment', 'Sports and recreational equipment', FALSE, 30.00);

-- Insert sample products
INSERT INTO products (category_id, product_name, description, unit_weight, unit_volume, fragile, base_cost_per_unit) VALUES
(1, 'Sofa', 'Standard 3-seater sofa', 80.00, 3.50, FALSE, 200.00),
(1, 'Dining Table', 'Wooden dining table', 60.00, 2.00, FALSE, 150.00),
(1, 'Wardrobe', 'Large wooden wardrobe', 120.00, 4.00, FALSE, 300.00),
(2, 'Refrigerator', 'Standard household refrigerator', 100.00, 1.80, TRUE, 400.00),
(2, 'Washing Machine', 'Front loading washing machine', 85.00, 1.20, TRUE, 350.00),
(2, 'Television', 'LED TV with stand', 25.00, 0.80, TRUE, 250.00),
(3, 'Crockery Set', 'Complete dinner set', 15.00, 0.50, TRUE, 100.00),
(4, 'Clothing Box', 'Box of assorted clothing', 20.00, 1.00, FALSE, 50.00);

-- Create indexes for performance optimization
CREATE INDEX idx_jobs_date_status ON jobs(requested_pickup_date, status);
CREATE INDEX idx_loads_job_status ON loads(job_id, status);
CREATE INDEX idx_audit_logs_composite ON audit_logs(table_name, action_type, created_at);

-- Add foreign key for assistants.current_job_id (after jobs table is created)
ALTER TABLE assistants ADD CONSTRAINT fk_assistants_current_job 
FOREIGN KEY (current_job_id) REFERENCES jobs(job_id) ON DELETE SET NULL;

-- Enable foreign key checks
SET foreign_key_checks = 1;

-- Grant permissions (adjust as needed for your environment)
-- GRANT ALL PRIVILEGES ON eshift_db.* TO 'eshift_user'@'localhost' IDENTIFIED BY 'your_password';
-- FLUSH PRIVILEGES;