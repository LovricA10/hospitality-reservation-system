-- Users
CREATE TABLE [User] 
(
    IDUser INT PRIMARY KEY IDENTITY(1,1),
    [Name] NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100) UNIQUE NOT NULL,
    [Password] NVARCHAR(50) NOT NULL,
    Phone NVARCHAR(20),
    [Role] NVARCHAR(20) NOT NULL CHECK (Role IN ('User', 'Admin')) DEFAULT 'User'
)

-- Hospitality Type (e.g. restaurant, bar, cafe)
CREATE TABLE HospitalityType
(
    IDType INT PRIMARY KEY IDENTITY(1,1),
    TypeName NVARCHAR(50) NOT NULL UNIQUE
)

-- Hospitality Venue (e.g. a specific restaurant or bar)
CREATE TABLE HospitalityVenue
(
    IDVenue INT PRIMARY KEY IDENTITY(1,1),
    VenueName NVARCHAR(100) NOT NULL,
    [Address] NVARCHAR(255) NOT NULL,
    TypeID INT FOREIGN KEY REFERENCES HospitalityType(IDType)
)

-- Reservation
CREATE TABLE Reservation
(
    IDReservation INT PRIMARY KEY IDENTITY(1,1),
    NumberOfGuests INT NOT NULL CHECK (NumberOfGuests > 0),
    [Status] NVARCHAR(20) NOT NULL CHECK ([Status] IN ('Pending', 'Confirmed', 'Cancelled')) DEFAULT 'Pending',
    ReservationDate DATE NOT NULL,
	UserID INT FOREIGN KEY REFERENCES [User](IDUser),  
    VenueID INT FOREIGN KEY REFERENCES HospitalityVenue(IDVenue)
)

-- Menu (Food, Drink)
CREATE TABLE MenuItem
(
    IDMenuItem INT PRIMARY KEY IDENTITY(1,1),
    ItemName NVARCHAR(100) NOT NULL,
    ItemType NVARCHAR(50) NOT NULL CHECK (ItemType IN ('Food', 'Drink')),
    Price MONEY NOT NULL CHECK (Price > 0)
)

-- M:N Relationship: HospitalityVenue and MenuItem
CREATE TABLE VenueMenuItem
(
    IDVenueMenuItem INT PRIMARY KEY IDENTITY (1,1),
    MenuItemID INT FOREIGN KEY REFERENCES MenuItem(IDMenuItem),
    VenueID INT FOREIGN KEY REFERENCES HospitalityVenue(IDVenue)
)

CREATE TABLE UserReservation
(
    IDUserReservation INT PRIMARY KEY IDENTITY(1,1),
    UserID INT FOREIGN KEY REFERENCES [User](IDUser),
    ReservationID INT FOREIGN KEY REFERENCES Reservation(IDReservation)
)


ALTER TABLE [User]
ADD 
    PwdHash NVARCHAR(256) NOT NULL,
    PwdSalt NVARCHAR(256) NOT NULL

ALTER TABLE [User]
DROP COLUMN [Password]