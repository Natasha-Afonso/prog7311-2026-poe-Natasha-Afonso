 USE master;
GO
-- Create the Medic database only if it does not already exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Medic')
BEGIN
    CREATE DATABASE Medic;
END
GO
-- Switch to the Medic database
USE Medic;
GO
-- Create Client Table
CREATE TABLE Client (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    ContactDetails NVARCHAR(255),
    Region NVARCHAR(100)
);
 -- Create Account Table
CREATE TABLE Accounts (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(255) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) NOT NULL,
    ClientId INT NULL,
    CONSTRAINT FK_Accounts_Client FOREIGN KEY (ClientId)
        REFERENCES Client(Id) ON DELETE SET NULL
);

-- Create Contract Table
CREATE TABLE Contract (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ContractName NVARCHAR(100) NOT NULL,
    ClientId INT NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    Status NVARCHAR(50) DEFAULT 'Draft', --[DRAFT/ACTIVE/EXPIRED]
    ServiceLevel NVARCHAR(50),
    SignedConsentPath NVARCHAR(MAX),
    CONSTRAINT FK_Contract_Client FOREIGN KEY (ClientId) 
        REFERENCES Client(Id) ON DELETE CASCADE
);

-- Create ServiceRequest Table
CREATE TABLE ServiceRequests (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ContractId INT NOT NULL,
    Description NVARCHAR(MAX),
    CurrencyFrom NVARCHAR(3) NOT NULL,
    Amount DECIMAL(18, 2) NOT NULL,
    Cost DECIMAL(18, 2) NOT NULL,
    PackageSize NVARCHAR (50),
    Condition NVARCHAR (50),
    Speed NVARCHAR (50),
    AssignedVehicleType NVARCHAR (50),
    Status NVARCHAR(50),
    CONSTRAINT FK_ServiceRequests_Contracts FOREIGN KEY (ContractId) 
        REFERENCES Contract(Id) ON DELETE CASCADE
);
GO

-- Insert Dummy Clients
    INSERT INTO Client (Name, ContactDetails , Region)
    VALUES 
('John Smith', 'john.smith@email.com',  'Gauteng'),
('Thabo Mokoena', 'thabo.m@provider.co.za',  'KwaZulu-Natal');
-- Insert Dummy Accounts
INSERT INTO Accounts (Email, Password, Role, ClientId)
VALUES 
('john.smith@email.com', 'Password@124', 'Client',1),
('sarah.connor@email.com', 'Password@124', 'Admin',0),
('thabo.mokoena@email.com', 'Password@124', 'Client',2);
-- Insert Dummy Contracts
INSERT INTO Contract ( ClientId, ContractName, StartDate, EndDate, Status, ServiceLevel)
VALUES 
( 1, 'Contract', '2025-01-01', '2026-01-01', 'Active', 'Premium'),
(2, 'ContractThree', '2025-03-15', '2026-03-15', 'Active', 'Premium'),
( 2, 'ContractFour', '2025-01-01', '2025-12-31', 'Active', 'Basic');
-- Insert Dummy ServiceRequest
INSERT INTO ServiceRequests (ContractId, Description, CurrencyFrom, Amount, Cost, PackageSize, Condition, Speed, AssignedVehicleType, Status)
VALUES 
(1, 'Gallons of blood','USD', 1200.00, 1200.00,'Large','Cold-Chain','Express', 'Express Vehicle', 'Completed'),
(2, 'Blood Work - Routine Check', 'JPY', 450.50, 450.50, 'Large','Cold-Chain','Express', 'Express Vehicle',  'Pending'),
(1, 'Dental Wisdom Tooth Extraction', 'USD', 3500.00, 3500.00, 'Large','Cold-Chain','Standard', 'Heavy Cargo Van',  'Confirmed');
GO
 
 
 
 
 
 
 
 
 
 
 





