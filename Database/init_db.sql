-- Set root password to 'mike'
ALTER USER 'root'@'localhost' IDENTIFIED BY 'mike';

-- Create database if it doesn't exist
CREATE DATABASE IF NOT EXISTS studentmanagement;

-- Use the database
USE studentmanagement;

-- Import the schema and data
SOURCE studentmanagement.sql;

-- Grant privileges
GRANT ALL PRIVILEGES ON studentmanagement.* TO 'root'@'localhost';
FLUSH PRIVILEGES; 