 USE master;
GO
-- Create the Farm database only if it does not already exist
IF NOT EXISTS (SELECT name FROM sys.databases WHERE name = 'Medic')
BEGIN
    CREATE DATABASE Medic;
END
GO
-- Switch to the Medic database
USE Medic;
GO
 -- Create Account Table
CREATE TABLE Accounts (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Email NVARCHAR(255) NOT NULL,
    Password NVARCHAR(255) NOT NULL,
    Role NVARCHAR(50) NOT NULL
 
);
-- Create Client Table
CREATE TABLE Client (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    ContactDetails NVARCHAR(255),
    AccountID INT NOT NULL,
    Region NVARCHAR(100),
    CONSTRAINT FK_Clients_Accounts FOREIGN KEY (AccountID) 
    REFERENCES Accounts(Id) ON DELETE CASCADE
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
    Status NVARCHAR(50),
    CONSTRAINT FK_ServiceRequests_Contracts FOREIGN KEY (ContractId) 
        REFERENCES Contract(Id) ON DELETE CASCADE
);
GO
-- Insert Dummy Accounts
INSERT INTO Accounts (Email, Password, Role)
VALUES 
('john.smith@email.com', 'Password@124', 'Client'),
('sarah.connor@email.com', 'Password@124', 'Admin'),
('thabo.mokoena@email.com', 'Password@124', 'Client');
-- Insert Dummy Clients
    INSERT INTO Client (Name, ContactDetails,AccountID , Region)
    VALUES 
('John Smith', 'john.smith@email.com', 1, 'Gauteng'),
('Sarah Connor', '555-0199', 2, 'Western Cape'),
('Thabo Mokoena', 'thabo.m@provider.co.za', 3, 'KwaZulu-Natal');
-- Insert Dummy Accounts
INSERT INTO Accounts (Email, Password, Role)
VALUES 
('john.smith@email.com', 'Password@124', 'Client'),
('sarah.connor@email.com', 'Password@124', 'Admin'),
('thabo.mokoena@email.com', 'Password@124', 'Client');
-- Insert Dummy Contracts
INSERT INTO Contract ( ClientId, ContractName, StartDate, EndDate, Status, ServiceLevel)
VALUES 
( 1, 'Contract', '2025-01-01', '2026-01-01', 'Active', 'Gold'),
(2, 'ContractThree', '2025-03-15', '2026-03-15', 'Active', 'Platinum'),
( 3, 'ContractFour', '2025-01-01', '2025-12-31', 'Active', 'Bronze');
-- Insert Dummy ServiceRequest
INSERT INTO ServiceRequests (ContractId, Description, CurrencyFrom, Amount, Cost, Status)
VALUES 
(1, 'Annual Physical Examination','USD', 1200.00, 1200.00, 'Completed'),
(2, 'Blood Work - Routine Check', 'JPY', 450.50, 450.50, 'Pending'),
(3, 'Dental Wisdom Tooth Extraction', 'USD', 3500.00, 3500.00, 'Confirmed');
GO
 
 
 
 
 
 
 
 
 
 
 





