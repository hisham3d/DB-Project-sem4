CREATE DATABASE dummy
USE dummy

CREATE TABLE random_table 
(
    ID INT PRIMARY KEY,
	password VARCHAR(50),
    name VARCHAR(50),
    contact VARCHAR(15)
);

CREATE TABLE [User] (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Phone VARCHAR(20) NOT NULL,
    Password VARCHAR(100) NOT NULL,
    Role VARCHAR(50) CHECK(Role In ('Service Provider', 'Traveler', 'Admin', 'Operator'))
);

CREATE TABLE ServiceProvider (
    ProviderID INT PRIMARY KEY,
    ContactInfo VARCHAR(100) NOT NULL,
    Type VARCHAR(50) NOT NULL,
    BusinessName VARCHAR(100) NOT NULL,
    FOREIGN KEY (ProviderID) REFERENCES [User](UserID)
        ON DELETE CASCADE ON UPDATE CASCADE
);

INSERT INTO random_table (ID, password, name, contact) 
VALUES 
(1, 'pass1', N'Ali Raza', N'03001234567'),
(2, 'pass2', N'Sara Khan', N'03111234567'),
(3, 'pass3', N'Usman Tariq', N'03221234567'),
(4, 'pass4', N'Zara Malik', N'03331234567')
