
CREATE TABLE [User] 
(
	[IDUser] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[LastName] [nvarchar](50) NOT NULL,
	[Email] [nvarchar](100)UNIQUE NOT NULL,
	[Phone] [nvarchar](20) NULL,
	[Role] [nvarchar](20) NOT NULL CHECK (Role IN ('User', 'Admin')) DEFAULT 'User',
	[PwdHash] [nvarchar](256) NOT NULL,
	[PwdSalt] [nvarchar](256) NOT NULL
    
);


CREATE TABLE HospitalityType
(
   [IDType] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
   [TypeName] [nvarchar](50) NOT NULL UNIQUE,
);


CREATE TABLE HospitalityVenue
(
   	[IDVenue] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[VenueName] [nvarchar](100) NOT NULL,
	[Address] [nvarchar](255) NOT NULL,
    [TypeID] [int] FOREIGN KEY REFERENCES HospitalityType(IDType)
);


CREATE TABLE Reservation
(
    [IDReservation] [int] PRIMARY KEY IDENTITY(1,1),
    [NumberOfGuests] [int] NOT NULL CHECK (NumberOfGuests > 0),
    [Status] [nvarchar](20) NOT NULL CHECK ([Status] IN ('Pending', 'Confirmed', 'Cancelled')) DEFAULT 'Pending',
    [ReservationDate] [date] NOT NULL,
    [UserID] [int] FOREIGN KEY REFERENCES [User](IDUser),  
    [VenueID] [int] FOREIGN KEY REFERENCES HospitalityVenue(IDVenue)
);


CREATE TABLE MenuItem
(
   	[IDMenuItem] [int]PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[ItemName] [nvarchar](100) NOT NULL,
    [ItemType] [nvarchar](50) NOT NULL CHECK (ItemType IN ('Food', 'Drink')),
    [Price] [money] NOT NULL CHECK (Price > 0),
    [ImageBase64] [nvarchar] (MAX)
);


CREATE TABLE VenueMenuItem
(
    [IDVenueMenuItem] [int] PRIMARY KEY IDENTITY (1,1),
    [MenuItemID] [int] FOREIGN KEY REFERENCES MenuItem(IDMenuItem),
    [VenueID] [int] FOREIGN KEY REFERENCES HospitalityVenue(IDVenue)
);

CREATE TABLE UserReservation
(
    [IDUserReservation] [int] PRIMARY KEY IDENTITY(1,1),
    [UserID] [int] FOREIGN KEY REFERENCES [User](IDUser),
    [ReservationID] [int] FOREIGN KEY REFERENCES Reservation(IDReservation)
);

CREATE TABLE Logs
(
	[Id] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Message] [nvarchar](max) NOT NULL,
	[Level] [int] NOT NULL,
	[Timestamp] [datetime] NOT NULL
);