-- Set root password
ALTER USER 'root'@'localhost' IDENTIFIED BY 'mike';

-- Create the database
CREATE DATABASE IF NOT EXISTS studentmanagement;

-- Grant privileges
GRANT ALL PRIVILEGES ON studentmanagement.* TO 'root'@'localhost';
FLUSH PRIVILEGES;

-- Use the database
USE studentmanagement; 