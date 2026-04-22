 -- Create Client Table
CREATE TABLE Clients (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(100) NOT NULL,
    ContactDetails NVARCHAR(255),
    Region NVARCHAR(100)
);

-- Create Contract Table
CREATE TABLE Contracts (
    Id INT PRIMARY KEY IDENTITY(1,1),
    ContractName NVARCHAR(100) NOT NULL,
    ClientId INT NOT NULL,
    StartDate DATETIME NOT NULL,
    EndDate DATETIME NOT NULL,
    Status NVARCHAR(50) DEFAULT 'Draft', --[DRAFT/ACTIVE/EXPIRED]
    ServiceLevel NVARCHAR(50),
    SignedConsentPath NVARCHAR(MAX),
    CONSTRAINT FK_Contracts_Clients FOREIGN KEY (ClientId) 
        REFERENCES Clients(Id) ON DELETE CASCADE
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
        REFERENCES Contracts(Id) ON DELETE CASCADE
);


-- Insert Dummy Clients
INSERT INTO Clients (Name, ContactDetails, Region)
VALUES 
('John Smith', 'john.smith@email.com', 'Gauteng'),
('Sarah Connor', '555-0199', 'Western Cape'),
('Thabo Mokoena', 'thabo.m@provider.co.za', 'KwaZulu-Natal');

-- Insert Dummy Contracts
-- (Assuming IDs 1, 2, and 3 were generated for the patients above)
INSERT INTO Contracts ( ClientId, ContractName, StartDate, EndDate, Status, ServiceLevel)
VALUES 
( 1, 'Contract', '2025-01-01', '2026-01-01', 'Active', 'Gold'),
( 1, 'ContractTwo', '2024-06-01', '2025-06-01', 'Draft', 'Silver'),
(2, 'ContractThree', '2025-03-15', '2026-03-15', 'Active', 'Platinum'),
( 3, 'ContractFour', '2025-01-01', '2025-12-31', 'Active', 'Bronze');

-- Insert Dummy ServiceRequest
-- (Linking to the Contract IDs created above)
INSERT INTO ServiceRequests (ContractId, Description, CurrencyFrom, Amount, Cost, Status)
VALUES 
(1, 'Annual Physical Examination','USD', 1200.00, 1200.00, 'Completed'),
(1, 'Blood Work - Routine Check', 'JPY', 450.50, 450.50, 'Pending'),
(3, 'Dental Wisdom Tooth Extraction', 'USD', 3500.00, 3500.00, 'Confirmed'),
(4, 'Optometry Consultation', 'USD', 850.00, 850.00, 'Cancelled');


DROP TABLE ServiceRequests;
DROP TABLE Contracts;
DROP TABLE Clients;

SELECT DB_NAME() AS CurrentDB;
SELECT * FROM Clients;