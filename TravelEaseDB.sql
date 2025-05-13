CREATE DATABASE TravelEase
USE TravelEase

-- //////////////////////////////////////////////////////////////////////////////

-- Step 1: Find the name of the constraint on the Status column
SELECT name
FROM sys.check_constraints
WHERE parent_object_id = OBJECT_ID('Booking');

-- Suppose the constraint name is CK_Booking_Status

-- Step 2: Drop the existing constraint
ALTER TABLE Booking DROP CONSTRAINT CK__Booking__Status__68487DD7; -- YE CHEEZ TERE LAPTOP PE DIFF HOGI GPT SE POOCH LEIN IS ERROR KA

-- Step 3: Add the new constraint including 'Unpaid'
ALTER TABLE Booking
ADD CONSTRAINT CK_Booking_Status
CHECK (Status IN ('Cancelled', 'Paid', 'Unpaid'));

-- //////////////////////////////////////////////////////////////////////////////

ALTER TABLE [User] ADD Status varchar(50) CHECK (Status IN ('Approved', 'Unapproved')) DEFAULT 'Unapproved'

ALTER TABLE TripResource ADD Status varchar(50) CHECK (Status IN ('Approved', 'Unapproved')) DEFAULT 'Unapproved'
select * from TripResource

-- //////////////////////////////////////////////////////////////////////////////

UPDATE [User]
SET Status = CASE 
    WHEN ABS(CAST(CAST(NEWID() AS VARBINARY) AS INT)) % 2 = 0 THEN 'Approved'
    ELSE 'Unapproved'
END
WHERE Status IS NULL;

SELECT* FROM Booking
INSERT INTO Booking (BookingDate, Status, TotalAmount, TripID, UserID)
VALUES
('2024-01-05', 'Unpaid', 35000.00, 1, 9),
('2024-02-10', 'Unpaid', 28000.00, 4, 10);

-- //////////////////////////////////////////////////////////////////////////////

-- Update half to 'Approved' and the rest to 'Unapproved' randomly
UPDATE TripResource
SET Status = CASE 
    WHEN ABS(CAST(CAST(NEWID() AS VARBINARY) AS INT)) % 2 = 0 THEN 'Approved'
    ELSE 'Unapproved'
END
WHERE Status IS NULL;

-- //////////////////////////////////////////////////////////////////////////////

CREATE TABLE [User] (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(100) NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Phone VARCHAR(20) NOT NULL,
    Password VARCHAR(100) NOT NULL,
    Role VARCHAR(50) CHECK(Role In ('Service Provider', 'Traveler', 'Admin', 'Operator'))
);

CREATE TABLE Traveler (
    TravelerID INT PRIMARY KEY,
    Age INT CHECK (Age > 0),
    Gender VARCHAR(10) CHECK (Gender IN ('Male', 'Female')),
    Nationality VARCHAR(50) NOT NULL,
    FOREIGN KEY (TravelerID) REFERENCES [User](UserID)
        ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Operator (
    OperatorID INT PRIMARY KEY NOT NULL,
    LicenseNumber VARCHAR(50) UNIQUE NOT NULL,
    CompanyName VARCHAR(100) NOT NULL,
    Description VARCHAR(100) NOT NULL,
    FOREIGN KEY (OperatorID) REFERENCES [User](UserID)
        ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Admin (
    AdminID INT PRIMARY KEY,
    AssignedRegion VARCHAR(100) NOT NULL,
    FOREIGN KEY (AdminID) REFERENCES [User](UserID)
        ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE ServiceProvider (
    ProviderID INT PRIMARY KEY,
    ContactInfo VARCHAR(100) NOT NULL,
    Type VARCHAR(50) NOT NULL,
    BusinessName VARCHAR(100) NOT NULL,
    FOREIGN KEY (ProviderID) REFERENCES [User](UserID)
        ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Destination (
    DestinationID INT PRIMARY KEY IDENTITY(1,1),
    Name VARCHAR(100) NOT NULL,
    Country VARCHAR(50) NOT NULL,
    Region VARCHAR(100) NOT NULL
);

CREATE TABLE Trip (
    TripID INT PRIMARY KEY IDENTITY(1,1),
    Capacity INT CHECK (Capacity > 0),
    Description VARCHAR(100) NOT NULL,
    Duration INT CHECK (Duration > 0),
    PricePerPerson DECIMAL(10, 2) NOT NULL,
    OperatorID INT,
    DestinationID INT,
    FOREIGN KEY (OperatorID) REFERENCES Operator(OperatorID)
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (DestinationID) REFERENCES Destination(DestinationID)
        ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Review (
    ReviewID INT PRIMARY KEY IDENTITY(1,1),
    Rating INT CHECK (Rating BETWEEN 1 AND 10) NOT NULL,
    ReviewDate DATE NOT NULL,
    Comment VARCHAR(100) NOT NULL,
    TripID INT,
    UserID INT,
    FOREIGN KEY (TripID) REFERENCES Trip(TripID)
		ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (UserID) REFERENCES [User](UserID)
);

CREATE TABLE Booking (
    BookingID INT PRIMARY KEY IDENTITY(1,1),
    BookingDate DATE NOT NULL,
    Status VARCHAR(50) CHECK (Status IN ('Cancelled', 'Paid')) NOT NULL,
    TotalAmount DECIMAL(10, 2) NOT NULL,
    TripID INT,
    UserID INT,
    FOREIGN KEY (TripID) REFERENCES Trip(TripID)
		ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (UserID) REFERENCES [User](UserID)
);



CREATE TABLE Paid (
    PaymentID INT PRIMARY KEY,
    PaymentDate DATE NOT NULL,
    PaymentMethod VARCHAR(50) CHECK (PaymentMethod IN ('Credit/Debit Card', 'Cash', 'Online')),
    Amount DECIMAL(10,2) CHECK (Amount > 0) NOT NULL,
    TripID INT,
    ProviderID INT,
    UserID INT,
    FOREIGN KEY (TripID) REFERENCES Trip(TripID)	
		ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (ProviderID) REFERENCES ServiceProvider(ProviderID),
    FOREIGN KEY (UserID) REFERENCES [User](UserID)
);

CREATE TABLE Cancelled (
    CancellationID INT PRIMARY KEY,
    RefundAmount DECIMAL(10, 2) NOT NULL,
    Reason VARCHAR(100) NOT NULL,
    BookingID INT,
    FOREIGN KEY (BookingID) REFERENCES Booking(BookingID)
        ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE TripResource (
    ResourceID INT PRIMARY KEY IDENTITY(1,1),
    ResourceType VARCHAR(50) NOT NULL,
    ProviderID INT,
    TripID INT,
    FOREIGN KEY (ProviderID) REFERENCES ServiceProvider(ProviderID)
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (TripID) REFERENCES Trip(TripID)
);

CREATE TABLE ResourceProfile (
    ProviderID INT PRIMARY KEY,
    Availability VARCHAR(100) CHECK (Availability IN ('Seasonal', 'Year-round')),
    Details VARCHAR(100) NOT NULL,
    Rating DECIMAL(3, 2) CHECK(Rating BETWEEN 1 AND 10) NOT NULL,
    FOREIGN KEY (ProviderID) REFERENCES ServiceProvider(ProviderID)
        ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE Registration (
    RegistrationID INT PRIMARY KEY IDENTITY(1,1),
    AdminID INT,
    TravelerID INT,
    FOREIGN KEY (AdminID) REFERENCES Admin(AdminID)
        ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (TravelerID) REFERENCES Traveler(TravelerID)
);

SELECT* FROM [User]
INSERT INTO [User] (Name, Email, Phone, Password, Role)
VALUES
-- Admins (8)
('Ali Khan', 'ali.khan@email.com', '03001234567', 'adminPass123', 'Admin'),
('Sara Ahmed', 'sara.ahmed@email.com', '03011234568', 'adminPass456', 'Admin'),
('Bilal Raza', 'bilal.raza@email.com', '03121234569', 'adminPass789', 'Admin'),
('Hina Malik', 'hina.malik@email.com', '03221234570', 'adminPass321', 'Admin'),
('Usman Sheikh', 'usman.sheikh@email.com', '03331234571', 'adminPass654', 'Admin'),
('Noor Fatima', 'noor.fatima@email.com', '03441234572', 'adminPass987', 'Admin'),
('Owais Iqbal', 'owais.iqbal@email.com', '03551234573', 'adminPass159', 'Admin'),
('Fiza Khan', 'fiza.khan@email.com', '03661234574', 'adminPass357', 'Admin'),

-- Travelers (25)
('Ahmed Raza', 'ahmed.raza@email.com', '03009876541', 'travelerPass1', 'Traveler'),
('Ayesha Khan', 'ayesha.khan@email.com', '03019876542', 'travelerPass2', 'Traveler'),
('Imran Ali', 'imran.ali@email.com', '03129876543', 'travelerPass3', 'Traveler'),
('Mehwish Qureshi', 'mehwish.qureshi@email.com', '03239876544', 'travelerPass4', 'Traveler'),
('Haris Malik', 'haris.malik@email.com', '03349876545', 'travelerPass5', 'Traveler'),
('Sadia Ahmed', 'sadia.ahmed@email.com', '03459876546', 'travelerPass6', 'Traveler'),
('Farhan Khan', 'farhan.khan@email.com', '03569876547', 'travelerPass7', 'Traveler'),
('Zainab Riaz', 'zainab.riaz@email.com', '03679876548', 'travelerPass8', 'Traveler'),
('Danish Ali', 'danish.ali@email.com', '03789876549', 'travelerPass9', 'Traveler'),
('Sanaullah Khan', 'sanaullah.khan@email.com', '03889876540', 'travelerPass10', 'Traveler'),
('Yasir Sheikh', 'yasir.sheikh@email.com', '03001234575', 'travelerPass11', 'Traveler'),
('Sundus Malik', 'sundus.malik@email.com', '03011234576', 'travelerPass12', 'Traveler'),
('Amna Bibi', 'amna.bibi@email.com', '03121234577', 'travelerPass13', 'Traveler'),
('Owais Raza', 'owais.raza@email.com', '03221234578', 'travelerPass14', 'Traveler'),
('Nimra Khan', 'nimra.khan@email.com', '03331234579', 'travelerPass15', 'Traveler'),
('Fahad Ali', 'fahad.ali@email.com', '03441234580', 'travelerPass16', 'Traveler'),
('Iqra Malik', 'iqra.malik@email.com', '03551234581', 'travelerPass17', 'Traveler'),
('Sameer Ahmed', 'sameer.ahmed@email.com', '03661234582', 'travelerPass18', 'Traveler'),
('Hafsa Khan', 'hafsa.khan@email.com', '03771234583', 'travelerPass19', 'Traveler'),
('Talha Riaz', 'talha.riaz@email.com', '03881234584', 'travelerPass20', 'Traveler'),
('Kiran Malik', 'kiran.malik@email.com', '03001234585', 'travelerPass21', 'Traveler'),
('Sana Khan', 'sana.khan@email.com', '03011234586', 'travelerPass22', 'Traveler'),
('Haroon Ali', 'haroon.ali@email.com', '03121234587', 'travelerPass23', 'Traveler'),
('Ayesha Siddiqui', 'ayesha.siddiqui@email.com', '03221234588', 'travelerPass24', 'Traveler'),
('Bilal Ahmed', 'bilal.ahmed@email.com', '03331234589', 'travelerPass25', 'Traveler'),

-- Operators (10)
('Adventure Pakistan', 'adventure.pk@email.com', '03441234590', 'operatorPass1', 'Operator'),
('Travel Explorers', 'travel.explorers@email.com', '03551234591', 'operatorPass2', 'Operator'),
('Northern Trips', 'northern.trips@email.com', '03661234592', 'operatorPass3', 'Operator'),
('Karachi Tours', 'karachi.tours@email.com', '03771234593', 'operatorPass4', 'Operator'),
('Punjab Travels', 'punjab.travels@email.com', '03881234594', 'operatorPass5', 'Operator'),
('Sindh Adventures', 'sindh.adventures@email.com', '03001234595', 'operatorPass6', 'Operator'),
('Balochistan Trips', 'balochistan.trips@email.com', '03011234596', 'operatorPass7', 'Operator'),
('KPK Tours', 'kpk.tours@email.com', '03121234597', 'operatorPass8', 'Operator'),
('Gilgit Explorers', 'gilgit.explorers@email.com', '03221234598', 'operatorPass9', 'Operator'),
('Kashmir Travels', 'kashmir.travels@email.com', '03331234599', 'operatorPass10', 'Operator'),

-- Service Providers (7)
('Islamabad Hotel', 'islamabad.hotel@email.com', '03441234600', 'providerPass1', 'Service Provider'),
('Lahore Transport', 'lahore.transport@email.com', '03551234601', 'providerPass2', 'Service Provider'),
('Karachi Guides', 'karachi.guides@email.com', '03661234602', 'providerPass3', 'Service Provider'),
('Peshawar Rides', 'peshawar.rides@email.com', '03771234603', 'providerPass4', 'Service Provider'),
('Quetta Transport', 'quetta.transport@email.com', '03881234604', 'providerPass5', 'Service Provider'),
('Multan Hotel', 'multan.hotel@email.com', '03001234605', 'providerPass6', 'Service Provider'),
('Sialkot Guides', 'sialkot.guides@email.com', '03011234606', 'providerPass7', 'Service Provider');

INSERT INTO Admin (AdminID, AssignedRegion)
VALUES
(1, 'Punjab'),
(2, 'Sindh'),
(3, 'KPK'),
(4, 'Balochistan'),
(5, 'Gilgit-Baltistan'),
(6, 'Islamabad'),
(7, 'Azad Kashmir'),
(8, 'International');

INSERT INTO Traveler (TravelerID, Age, Gender, Nationality)
VALUES
(9, 25, 'Male', 'Pakistani'),
(10, 28, 'Female', 'Pakistani'),
(11, 30, 'Male', 'Pakistani'),
(12, 22, 'Female', 'Pakistani'),
(13, 35, 'Male', 'Pakistani'),
(14, 27, 'Female', 'Pakistani'),
(15, 32, 'Male', 'Pakistani'),
(16, 29, 'Female', 'Pakistani'),
(17, 40, 'Male', 'Pakistani'),
(18, 26, 'Female', 'Pakistani'),
(19, 33, 'Male', 'Pakistani'),
(20, 24, 'Female', 'Pakistani'),
(21, 31, 'Male', 'Pakistani'),
(22, 23, 'Female', 'Pakistani'),
(23, 34, 'Male', 'Pakistani'),
(24, 25, 'Female', 'Pakistani'),
(25, 36, 'Male', 'Pakistani'),
(26, 27, 'Female', 'Pakistani'),
(27, 38, 'Male', 'Pakistani'),
(28, 29, 'Female', 'Pakistani'),
(29, 42, 'Male', 'Pakistani'),
(30, 26, 'Female', 'Pakistani'),
(31, 33, 'Male', 'Pakistani'),
(32, 24, 'Female', 'Pakistani'),
(33, 35, 'Male', 'Pakistani');

INSERT INTO Operator (OperatorID, LicenseNumber, CompanyName, Description)
VALUES
(34, 'LIC-OP001', 'Adventure Pakistan', 'Specializing in adventure tours across Pakistan'),
(35, 'LIC-OP002', 'Travel Explorers', 'Exploring hidden gems of Pakistan'),
(36, 'LIC-OP003', 'Northern Trips', 'Northern areas tour specialists'),
(37, 'LIC-OP004', 'Karachi Tours', 'Cultural and historical tours of Karachi'),
(38, 'LIC-OP005', 'Punjab Travels', 'Exploring Punjab region'),
(39, 'LIC-OP006', 'Sindh Adventures', 'Adventure tours in Sindh'),
(40, 'LIC-OP007', 'Balochistan Trips', 'Exploring Balochistan'),
(41, 'LIC-OP008', 'KPK Tours', 'Tour packages in Khyber Pakhtunkhwa'),
(42, 'LIC-OP009', 'Gilgit Explorers', 'Gilgit-Baltistan specialists'),
(43, 'LIC-OP010', 'Kashmir Travels', 'Azad Kashmir tour packages');

INSERT INTO ServiceProvider (ProviderID, ContactInfo, Type, BusinessName)
VALUES
(44, '0300-1234567', 'Hotel', 'Islamabad Grand Hotel'),
(45, '0301-1234568', 'Transport', 'Lahore Luxury Transport'),
(46, '0312-1234569', 'Guide', 'Karachi Professional Guides'),
(47, '0322-1234570', 'Transport', 'Peshawar VIP Rides'),
(48, '0333-1234571', 'Transport', 'Quetta Express Transport'),
(49, '0344-1234572', 'Hotel', 'Multan Comfort Inn'),
(50, '0355-1234573', 'Guide', 'Sialkot Tour Guides');

SELECT* FROM Destination
INSERT INTO Destination (Name, Country, Region)
VALUES
('Islamabad', 'Pakistan', 'Punjab'),
('Lahore', 'Pakistan', 'Punjab'),
('Karachi', 'Pakistan', 'Sindh'),
('Peshawar', 'Pakistan', 'KPK'),
('Quetta', 'Pakistan', 'Balochistan'),
('Murree', 'Pakistan', 'Punjab'),
('Hunza Valley', 'Pakistan', 'Gilgit-Baltistan'),
('Skardu', 'Pakistan', 'Gilgit-Baltistan'),
('Naran', 'Pakistan', 'KPK'),
('Swat Valley', 'Pakistan', 'KPK'),
('Malam Jabba', 'Pakistan', 'KPK'),
('Mohenjo-Daro', 'Pakistan', 'Sindh'),
('Multan', 'Pakistan', 'Punjab'),
('Faisalabad', 'Pakistan', 'Punjab'),
('Bahawalpur', 'Pakistan', 'Punjab'),
('Sialkot', 'Pakistan', 'Punjab'),
('Gujranwala', 'Pakistan', 'Punjab'),
('Hyderabad', 'Pakistan', 'Sindh'),
('Sukkur', 'Pakistan', 'Sindh'),
('Gilgit', 'Pakistan', 'Gilgit-Baltistan'),
('Fairy Meadows', 'Pakistan', 'Gilgit-Baltistan'),
('Deosai Plains', 'Pakistan', 'Gilgit-Baltistan'),
('Kaghan Valley', 'Pakistan', 'KPK'),
('Neelum Valley', 'Pakistan', 'Azad Kashmir'),
('Muzaffarabad', 'Pakistan', 'Azad Kashmir'),
('Rawalakot', 'Pakistan', 'Azad Kashmir'),
('Maldives', 'Maldives', 'Indian Ocean'),
('Dubai', 'UAE', 'Middle East'),
('Istanbul', 'Turkey', 'Europe/Asia'),
('Bangkok', 'Thailand', 'Southeast Asia'),
('Paris', 'France', 'Western Europe'),
('Rome', 'Italy', 'Southern Europe'),
('London', 'UK', 'Western Europe'),
('New York', 'USA', 'North America'),
('Tokyo', 'Japan', 'East Asia'),
('Sydney', 'Australia', 'Oceania'),
('Cape Town', 'South Africa', 'Africa'),
('Rio de Janeiro', 'Brazil', 'South America'),
('Beijing', 'China', 'East Asia'),
('Moscow', 'Russia', 'Eastern Europe'),
('Toronto', 'Canada', 'North America'),
('Barcelona', 'Spain', 'Southern Europe'),
('Amsterdam', 'Netherlands', 'Western Europe'),
('Vienna', 'Austria', 'Central Europe'),
('Zurich', 'Switzerland', 'Central Europe'),
('Stockholm', 'Sweden', 'Northern Europe'),
('Oslo', 'Norway', 'Northern Europe'),
('Helsinki', 'Finland', 'Northern Europe'),
('Copenhagen', 'Denmark', 'Northern Europe'),
('Dublin', 'Ireland', 'Western Europe')

SELECT *FROM Trip
INSERT INTO Trip (Capacity, Description, Duration, PricePerPerson, OperatorID, DestinationID)
VALUES
(10, 'Skardu Adventure', 5, 28000.00, 34, 1),
(20, 'Murree Family Tour', 3, 15000.00, 35, 2),
(12, 'Swat Valley Retreat', 4, 22000.00, 36, 3),
(8, 'Fairy Meadows Trek', 6, 40000.00, 37, 4),
(15, 'Karachi Heritage Tour', 2, 12000.00, 38, 5),
(10, 'Lahore Food Tour', 1, 8000.00, 39, 6),
(25, 'Islamabad City Tour', 1, 7000.00, 40, 7),
(12, 'Maldives Luxury Getaway', 7, 120000.00, 41, 8),
(15, 'Dubai Shopping Tour', 5, 90000.00, 42, 9),
(10, 'Istanbul Cultural Trip', 6, 85000.00, 43, 10),
(8, 'Bangkok Adventure', 5, 75000.00, 34, 11),
(20, 'Neelum Valley Retreat', 4, 25000.00, 35, 12),
(15, 'Deosai Plains Safari', 3, 32000.00, 36, 13),
(10, 'Mohenjo-Daro Historical Tour', 2, 18000.00, 37, 14),
(15, 'Paris Romantic Getaway', 5, 95000.00, 38, 15),
(20, 'Rome Historical Tour', 4, 88000.00, 39, 16),
(25, 'London City Experience', 3, 82000.00, 40, 17),
(10, 'New York Skyscraper Tour', 7, 110000.00, 41, 18),
(12, 'Tokyo Cultural Immersion', 6, 105000.00, 42, 19),
(15, 'Sydney Coastal Adventure', 5, 98000.00, 43, 20),
(10, 'Cape Town Safari', 8, 115000.00, 34, 21),
(12, 'Rio Carnival Experience', 6, 102000.00, 35, 22),
(15, 'Beijing Great Wall Tour', 4, 92000.00, 36, 23),
(20, 'Moscow Red Square Tour', 3, 85000.00, 37, 24),
(25, 'Toronto Niagara Falls', 2, 78000.00, 38, 25),
(15, 'Barcelona Architecture Tour', 4, 88000.00, 39, 26),
(10, 'Amsterdam Canal Cruise', 3, 82000.00, 40, 27),
(12, 'Vienna Music Tour', 5, 95000.00, 41, 28),
(15, 'Zurich Alpine Adventure', 6, 105000.00, 42, 29),
(10, 'Stockholm Archipelago Tour', 4, 92000.00, 43, 30),
(12, 'Oslo Fjord Exploration', 5, 98000.00, 34, 31),
(15, 'Helsinki Design Tour', 3, 85000.00, 35, 32),
(20, 'Copenhagen Food Tour', 4, 88000.00, 36, 33),
(25, 'Dublin Pub Crawl', 2, 75000.00, 37, 34),
(15, 'Edinburgh Castle Tour', 3, 82000.00, 38, 35),
(10, 'Manchester Football Experience', 1, 68000.00, 39, 36),
(12, 'Birmingham Industrial Tour', 2, 72000.00, 40, 37),
(15, 'Glasgow Whisky Trail', 3, 78000.00, 41, 38),
(20, 'Liverpool Beatles Tour', 2, 75000.00, 42, 39),
(25, 'Belfast Titanic Experience', 1, 70000.00, 43, 40),
(15, 'Cardiff Castle Tour', 2, 72000.00, 34, 41),
(10, 'Newcastle Nightlife Tour', 1, 65000.00, 35, 42),
(12, 'Leeds Shopping Tour', 2, 70000.00, 36, 43),
(15, 'Sheffield Peak District Tour', 3, 75000.00, 37, 44),
(20, 'Multan Sufi Tour', 2, 18000.00, 38, 45),
(25, 'Faisalabad Industrial Tour', 1, 15000.00, 39, 46),
(15, 'Bahawalpur Desert Safari', 3, 22000.00, 40, 47),
(10, 'Sialkot Sports Tour', 2, 20000.00, 41, 48),
(12, 'Gujranwala Food Tour', 1, 12000.00, 42, 49),
(15, 'Quetta Mountain Tour', 4, 26000.00, 43, 50);

SELECT* FROM Booking
INSERT INTO Booking (BookingDate, Status, TotalAmount, TripID, UserID)
VALUES
('2024-01-05', 'Paid', 35000.00, 1, 9),
('2024-02-10', 'Paid', 28000.00, 2, 10),
('2024-03-15', 'Paid', 15000.00, 3, 11),
('2024-04-20', 'Cancelled', 22000.00, 4, 12),
('2024-05-25', 'Cancelled', 40000.00, 5, 13),
('2024-06-30', 'Cancelled', 12000.00, 6, 14),
('2024-07-10', 'Paid', 8000.00, 7, 15),
('2024-08-15', 'Paid', 7000.00, 8, 16),
('2024-09-20', 'Paid', 120000.00, 9, 17),
('2024-10-25', 'Paid', 90000.00, 10, 18),
('2024-11-30', 'Cancelled', 85000.00, 11, 19),
('2025-01-05', 'Cancelled', 75000.00, 12, 20),
('2025-02-10', 'Paid', 25000.00, 13, 21),
('2025-03-15', 'Paid', 32000.00, 14, 22),
('2025-04-20', 'Paid', 18000.00, 15, 23),
('2025-05-25', 'Paid', 35000.00, 16, 24),
('2025-06-30', 'Paid', 28000.00, 17, 25),
('2025-07-10', 'Paid', 15000.00, 18, 26),
('2025-08-15', 'Cancelled', 22000.00, 19, 27),
('2025-09-20', 'Paid', 40000.00, 20, 28),
('2024-01-10', 'Paid', 95000.00, 21, 29),
('2024-02-15', 'Cancelled', 88000.00, 22, 30),
('2024-03-20', 'Paid', 82000.00, 23, 31),
('2024-04-25', 'Paid', 110000.00, 24, 32),
('2024-05-30', 'Paid', 105000.00, 25, 33),
('2024-06-05', 'Paid', 98000.00, 26, 9),
('2024-07-15', 'Paid', 115000.00, 27, 10),
('2024-08-20', 'Paid', 102000.00, 28, 11),
('2024-09-25', 'Paid', 92000.00, 29, 12),
('2024-10-30', 'Cancelled', 85000.00, 30, 13),
('2024-11-05', 'Cancelled', 78000.00, 31, 14),
('2025-01-10', 'Paid', 88000.00, 32, 15),
('2025-02-15', 'Cancelled', 82000.00, 33, 16),
('2025-03-20', 'Paid', 95000.00, 34, 17),
('2025-04-25', 'Cancelled', 105000.00, 35, 18),
('2025-05-30', 'Paid', 92000.00, 36, 19),
('2025-06-05', 'Cancelled', 98000.00, 37, 20),
('2025-07-15', 'Paid', 85000.00, 38, 21),
('2025-08-20', 'Paid', 88000.00, 39, 22),
('2025-09-25', 'Cancelled', 75000.00, 40, 23),
('2025-10-30', 'Paid', 82000.00, 40, 24),
('2024-01-15', 'Paid', 72000.00, 42, 25),
('2024-02-20', 'Paid', 78000.00, 43, 26),
('2024-03-25', 'Paid', 75000.00, 44, 27),
('2024-04-30', 'Paid', 70000.00, 45, 28),
('2024-05-05', 'Paid', 72000.00, 46, 29),
('2024-06-15', 'Paid', 65000.00, 47, 30),
('2024-07-20', 'Paid', 70000.00, 48, 31),
('2024-08-25', 'Cancelled', 75000.00, 49, 32),
('2024-07-25', 'Paid', 70000.00, 50, 33)


SELECT* FROM Paid
INSERT INTO Paid (PaymentID, PaymentDate, PaymentMethod, Amount, TripID, ProviderID, UserID)
VALUES
(1, '2024-01-06', 'Credit/Debit Card', 35000.00, 1, 44, 9),
(2, '2024-02-11', 'Online', 28000.00, 2, 45, 10),
(3, '2024-03-16', 'Cash', 15000.00, 3, 46, 11),
(4, '2024-04-21', 'Credit/Debit Card', 22000.00, 4, 47, 12),
(5, '2024-05-26', 'Online', 40000.00, 5, 48, 13),
(6, '2024-07-01', 'Credit/Debit Card', 12000.00, 6, 49, 14),
(7, '2024-07-11', 'Online', 8000.00, 7, 50, 15),
(8, '2024-08-16', 'Cash', 7000.00, 8, 44, 16),
(9, '2024-09-21', 'Credit/Debit Card', 120000.00, 9, 45, 17),
(10, '2024-10-23', 'Online', 90000.00, 10, 46, 18),
(11, '2024-10-25', 'Cash', 94000.00, 11, 47, 19),
(12, '2024-10-28', 'Online', 99000.00, 12, 48, 20),
(13, '2025-02-11', 'Credit/Debit Card', 25000.00, 13, 49, 21),
(14, '2025-03-16', 'Online', 32000.00, 14, 50, 22),
(15, '2025-04-21', 'Cash', 18000.00, 15, 44, 23),
(16, '2025-05-26', 'Credit/Debit Card', 35000.00, 16, 45, 24),
(17, '2025-07-01', 'Online', 28000.00, 17, 46, 25),
(18, '2025-07-11', 'Credit/Debit Card', 15000.00, 18, 47, 26),
(19, '2025-08-16', 'Online', 22000.00, 19, 48, 27),
(20, '2025-09-21', 'Cash', 40000.00, 20, 49, 28),
(21, '2024-01-11', 'Credit/Debit Card', 95000.00, 21, 50, 29),
(22, '2024-02-16', 'Online', 88000.00, 22, 44, 30),
(23, '2024-03-21', 'Cash', 82000.00, 23, 45, 31),
(24, '2024-04-26', 'Credit/Debit Card', 110000.00, 24, 46, 32),
(25, '2024-05-31', 'Online', 105000.00, 25, 47, 33),
(26, '2024-06-06', 'Credit/Debit Card', 98000.00, 26, 48, 9),
(27, '2024-07-16', 'Online', 115000.00, 27, 49, 10),
(28, '2024-08-21', 'Cash', 102000.00, 28, 50, 11),
(29, '2024-09-26', 'Credit/Debit Card', 92000.00, 29, 44, 12),
(30, '2024-10-31', 'Online', 85000.00, 30, 45, 13),
(31, '2024-11-06', 'Credit/Debit Card', 78000.00, 31, 46, 14),
(32, '2025-01-11', 'Online', 88000.00, 32, 47, 15),
(33, '2025-02-16', 'Cash', 82000.00, 33, 48, 16),
(34, '2025-03-21', 'Credit/Debit Card', 95000.00, 34, 49, 17),
(35, '2025-04-26', 'Online', 105000.00, 35, 50, 18),
(36, '2025-05-31', 'Credit/Debit Card', 92000.00, 36, 44, 19),
(37, '2025-06-06', 'Online', 98000.00, 37, 45, 20),
(38, '2025-07-16', 'Cash', 85000.00, 38, 46, 21),
(39, '2025-08-21', 'Credit/Debit Card', 88000.00, 39, 47, 22),
(40, '2025-09-26', 'Online', 75000.00, 40, 48, 23),
(41, '2025-10-31', 'Credit/Debit Card', 82000.00, 41, 49, 24),
(42, '2025-11-06', 'Online', 68000.00, 42, 50, 25),
(43, '2024-01-16', 'Cash', 72000.00, 43, 44, 26),
(44, '2024-02-21', 'Credit/Debit Card', 78000.00, 44, 45, 27),
(45, '2024-03-26', 'Online', 75000.00, 45, 46, 28),
(46, '2024-04-30', 'Credit/Debit Card', 70000.00, 46, 47, 29),
(47, '2024-05-06', 'Online', 72000.00, 47, 48, 30),
(48, '2024-06-16', 'Cash', 65000.00, 48, 49, 31),
(49, '2024-07-21', 'Credit/Debit Card', 70000.00, 49, 50, 32),
(50, '2024-08-26', 'Online', 75000.00, 50, 44, 33);

SELECT* FROM Cancelled
INSERT INTO Cancelled (CancellationID, RefundAmount, Reason, BookingID)
VALUES
(1, 42500.00, 'Changed travel plans', 1),
(2, 37500.00, 'Personal emergency', 2),
(3, 47500.00, 'Flight cancellation', 3),
(4, 44000.00, 'Health issues', 4),
(5, 41000.00, 'Work commitment', 5),
(6, 55000.00, 'Family emergency', 6),
(7, 52500.00, 'Changed destination', 7),
(8, 49000.00, 'Financial reasons', 8),
(9, 57500.00, 'Travel restrictions', 9),
(10, 51000.00, 'Personal reasons', 10),
(11, 41000.00, 'Changed mind', 11),
(12, 52500.00, 'Weather concerns', 12),
(13, 46000.00, 'Schedule conflict', 13),
(14, 49000.00, 'Found better deal', 14),
(15, 42500.00, 'Lost interest', 15),
(16, 44000.00, 'Travel companion cancelled', 16),
(17, 37500.00, 'Visa issues', 17),
(18, 41000.00, 'Accommodation issues', 18),
(19, 36000.00, 'Transportation issues', 19),
(20, 32500.00, 'Event cancellation', 20),
(21, 39000.00, 'Insurance issues', 21),
(22, 35000.00, 'Passport issues', 22),
(23, 41000.00, 'Political instability', 23),
(24, 36000.00, 'Natural disaster', 24),
(25, 32500.00, 'Terrorism concerns', 25),
(26, 39000.00, 'Economic reasons', 26),
(27, 35000.00, 'Currency issues', 27),
(28, 41000.00, 'Language barrier concerns', 28),
(29, 36000.00, 'Cultural concerns', 29),
(30, 32500.00, 'Food concerns', 30),
(31, 39000.00, 'Climate concerns', 31),
(32, 35000.00, 'Safety concerns', 32),
(33, 41000.00, 'Health advisory', 33),
(34, 36000.00, 'Political advisory', 34),
(35, 32500.00, 'Travel advisory', 35),
(36, 39000.00, 'Vaccination requirements', 36),
(37, 35000.00, 'Quarantine requirements', 37),
(38, 41000.00, 'Testing requirements', 38),
(39, 36000.00, 'Documentation issues', 39),
(40, 32500.00, 'Booking error', 40),
(41, 39000.00, 'Double booking', 41),
(42, 35000.00, 'System error', 42),
(43, 41000.00, 'Website issues', 43),
(44, 36000.00, 'App issues', 44),
(45, 32500.00, 'Payment issues', 45),
(46, 39000.00, 'Refund policy', 46),
(47, 35000.00, 'Cancellation policy', 47),
(48, 41000.00, 'Change of plans', 48),
(49, 36000.00, 'No longer interested', 49),
(50, 32500.00, 'Found alternative', 50);

SELECT* FROM Review
INSERT INTO Review (Rating, ReviewDate, Comment, TripID, UserID)
VALUES
(9, '2024-01-15', 'Amazing experience in Hunza!', 1, 9),
(8, '2024-02-20', 'Beautiful scenery in Skardu', 2, 10),
(7, '2024-03-25', 'Family enjoyed Murree trip', 3, 11),
(9, '2024-04-30', 'Swat Valley was breathtaking', 4, 12),
(10, '2024-05-30', 'Best trekking experience ever', 5, 13),
(8, '2024-07-05', 'Learned a lot about Karachi history', 6, 14),
(7, '2024-07-15', 'Delicious food in Lahore', 7, 15),
(6, '2024-08-20', 'Islamabad tour was okay', 8, 16),
(9, '2024-09-25', 'Maldives was paradise', 9, 17),
(8, '2024-10-30', 'Great shopping in Dubai', 10, 18),
(8, '2024-02-10', 'Loved the scenic beauty of Swat Valley.', 11, 19),
(10, '2024-03-05', 'The trip to Skardu was unforgettable!', 12, 20),
(9, '2025-02-15', 'Neelum Valley is heaven on earth', 13, 21),
(8, '2025-03-20', 'Saw brown bears in Deosai', 14, 22),
(7, '2025-04-25', 'Fascinating historical site', 15, 23),
(9, '2024-01-20', 'Paris was magical!', 16, 24),
(8, '2024-02-25', 'Rome history came alive', 17, 25),
(7, '2024-03-30', 'London was rainy but fun', 18, 26),
(10, '2024-05-05', 'New York skyline amazing', 19, 27),
(9, '2024-06-10', 'Tokyo culture fascinating', 20, 28),
(8, '2024-07-20', 'Sydney beaches perfect', 21, 29),
(9, '2024-08-25', 'Cape Town safari unforgettable', 22, 30),
(7, '2024-09-30', 'Rio carnival energetic', 23, 31),
(8, '2024-11-10', 'Great Wall impressive', 24, 32),
(9, '2025-01-15', 'Red Square magnificent', 25, 33),
(7, '2025-02-20', 'Niagara Falls powerful', 26, 9),
(8, '2025-03-25', 'Barcelona architecture stunning', 27, 10),
(9, '2025-04-30', 'Amsterdam canals charming', 28, 11),
(7, '2025-06-10', 'Vienna music delightful', 29, 12),
(8, '2025-07-20', 'Zurich mountains breathtaking', 30, 13),
(9, '2025-08-25', 'Stockholm archipelago serene', 31, 14),
(7, '2025-09-30', 'Oslo fjords peaceful', 32, 15),
(8, '2025-11-10', 'Helsinki design innovative', 33, 16),
(9, '2024-01-25', 'Copenhagen food delicious', 34, 17),
(7, '2024-03-05', 'Dublin pubs lively', 35, 18),
(8, '2024-04-10', 'Edinburgh castle historic', 36, 19),
(9, '2024-05-15', 'Manchester football exciting', 37, 20),
(7, '2024-06-20', 'Birmingham industrial interesting', 38, 21),
(8, '2024-07-25', 'Glasgow whisky smooth', 39, 22),
(9, '2024-08-30', 'Liverpool Beatles nostalgic', 40, 23),
(7, '2024-10-10', 'Belfast Titanic informative', 41, 24),
(8, '2024-11-15', 'Cardiff castle medieval', 42, 25),
(9, '2025-01-20', 'Newcastle nightlife vibrant', 43, 26),
(7, '2025-02-25', 'Leeds shopping excellent', 44, 27),
(8, '2025-04-05', 'Sheffield countryside beautiful', 45, 28),
(9, '2025-05-10', 'Multan Sufi spiritual', 46, 29),
(7, '2025-06-15', 'Faisalabad industrial impressive', 47, 30),
(8, '2025-07-20', 'Bahawalpur desert vast', 48, 31),
(9, '2025-08-25', 'Sialkot sports quality', 49, 32),
(7, '2025-09-30', 'Gujranwala food tasty', 50, 33);

SELECT* FROM TripResource
INSERT INTO TripResource (ResourceType, ProviderID, TripID)
VALUES
('Hotel', 44, 1),
('Transport', 45, 2),
('Guide', 46, 3),
('Hotel', 44, 4),
('Transport', 45, 5),
('Hotel', 49, 6),
('Transport', 47, 7),
('Hotel', 44, 8),
('Transport', 48, 9),
('Hotel', 44, 10),
('Transport', 45, 11),
('Hotel', 49, 12),
('Transport', 47, 13),
('Hotel', 44, 14),
('Transport', 45, 15),
('Hotel', 44, 16),
('Transport', 45, 17),
('Hotel', 44, 18),
('Transport', 45, 19),
('Hotel', 49, 20),
('Transport', 48, 21),
('Hotel', 44, 22),
('Transport', 45, 23),
('Guide', 46, 24),
('Hotel', 47, 25),
('Transport', 48, 26),
('Guide', 49, 27),
('Hotel', 50, 28),
('Transport', 44, 29),
('Guide', 45, 30),
('Hotel', 46, 31),
('Transport', 47, 32),
('Guide', 48, 33),
('Hotel', 49, 34),
('Transport', 50, 35),
('Guide', 44, 36),
('Hotel', 45, 37),
('Transport', 46, 38),
('Guide', 47, 39),
('Hotel', 48, 40),
('Transport', 49, 41),
('Guide', 50, 42),
('Hotel', 44, 43),
('Transport', 45, 44),
('Guide', 46, 45),
('Hotel', 47, 46),
('Transport', 48, 47),
('Guide', 49, 48),
('Hotel', 50, 49),
('Transport', 44, 50);

-- Update half to 'Approved' and the rest to 'Unapproved' randomly
UPDATE TripResource
SET Status = CASE 
    WHEN ABS(CAST(CAST(NEWID() AS VARBINARY) AS INT)) % 2 = 0 THEN 'Approved'
    ELSE 'Unapproved'
END
WHERE Status IS NULL;


SELECT* FROM ResourceProfile
INSERT INTO ResourceProfile (ProviderID, Availability, Details, Rating)
VALUES
(44, 'Year-round', '5-star hotel', 9.2),
(45, 'Year-round', 'Luxury transport service', 8.8),
(46, 'Seasonal', 'Professional tour guides', 9.0),
(47, 'Year-round', 'VIP transport service', 8.5),
(48, 'Year-round', 'Reliable transport service', 8.0),
(49, 'Year-round', 'Comfortable 4-star hotel', 8.5),
(50, 'Seasonal', 'Experienced local guides', 8.7);

SELECT* FROM Registration
INSERT INTO Registration (AdminID, TravelerID)
VALUES
(1, 9),
(2, 10),
(3, 11),
(4, 12),
(5, 13),
(6, 14),
(7, 15),
(8, 16),
(1, 17),
(2, 18),
(3, 19),
(4, 20),
(5, 21),
(6, 22),
(7, 23),
(8, 24),
(1, 25),
(2, 26),
(3, 27),
(4, 28),
(5, 29),
(6, 30),
(7, 31),
(8, 32),
(1, 33),
(2, 9),
(3, 10),
(4, 11),
(5, 12),
(6, 13),
(7, 14),
(8, 15),
(1, 16),
(2, 17),
(3, 18),
(4, 19),
(5, 20),
(6, 21),
(7, 22),
(8, 23),
(1, 24),
(2, 25),
(3, 26),
(4, 27),
(5, 28),
(6, 29),
(7, 30),
(8, 31),
(1, 32),
(2, 33);

CREATE TABLE BookingLog (
    LogID INT PRIMARY KEY IDENTITY(1,1),
    ActionType VARCHAR(50),
    BookingID INT,
    UserID INT,
    TripID INT,
    ActionTimestamp DATETIME DEFAULT GETDATE(),
    PerformedBy SYSNAME DEFAULT SYSTEM_USER
);

CREATE TRIGGER BookingLogInsert
ON Booking
AFTER INSERT
AS
BEGIN
    INSERT INTO BookingLog (ActionType, BookingID, UserID, TripID)
    SELECT 'INSERT', i.BookingID, i.UserID, i.TripID
    FROM inserted i;
END;

CREATE TRIGGER UpdateTripCapacityAfterBooking
ON Booking
AFTER INSERT
AS
BEGIN
    UPDATE Trip
    SET Capacity = Capacity - 1
    FROM Trip t
    JOIN inserted i ON t.TripID = i.TripID
    WHERE t.Capacity > 0;
END;