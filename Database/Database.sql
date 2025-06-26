
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

INSERT INTO HospitalityType (TypeName) VALUES 
('Beach Bar'),
('Caffe Bar'),
('Konoba'),
('Pizzeria'),
('Restoran');


INSERT INTO [User] (Name, LastName, Email, Phone, Role, PwdHash, PwdSalt) VALUES
('Ante', 'Antic', 'aantic@gmail.com', '0913425483', 'User', '7N+KOaRpV9MID5g6YSlX+w5CLPHiuvUMHzwpLZSPefQ=', 'qwqdpv+H4J+YhI0DXeA+qg=='),
('Mate', 'Lovric', 'lovric@gmail.com', '0913425422', 'User', 'drL2zPsG24mTKRTdPr3fZ8gWiqinYkIsgLHVYukicNk=', 'YqwigaQp1I8ljnqsSoTiwe=='),
('Stjepan', 'Labor', 'labor10@gmail.com', '0913425432', 'Admin', 'wJjmWCqNEfxSllkTfAYfDPa3o9ULHOCwq1duxpl4VM=', 'bgJSK1knEfcu5zm22csuQ2=='),
('Ivan Petar', 'Solic', 'sole@gmail.com', '0913425433', 'Admin', 'bEJcfu2sTA80AW8HdEWZqLfnMUGSSL3k1aBWPhY0HzU=', 'jTXqY0kTrqQ18qRhozyui2k=='),
('Ivan', 'Solic', 'sole23@gmail.com', '0913425433', 'Admin', 'iiSDfZ6fe8zdPkAMnUhr2OrMkDMIX75w0wN9ggnd6RE=', 'dbLkHnDuXNl9oUkLr7mSNA=='),
('Andrej', 'Iovric', 'lovric10@gmail.com', '0913425433', 'Admin', 'bijBqh9+67SgEkWufHn1LcsCXEG2dW8gn3nJyjEjPg=', 'gJXOG3hNlUSCYTiTGly8uA=='),
('Daniel', 'Bele', 'bele@example.com', '0913422511', 'Admin', 'agd8.sx6K4LoMNW2q3mVjmMBUcX04EE+ydh2xsHHd8=', 'zs0hscpZby6xEZBiEjMkJ0=='),
('Leo', 'Messi', 'messi@example.com', '0913425546', 'Admin', 'M5ruEpK0brOtU0Et35xBqicGVTcZqS6Hs2Q++TVmJU=', '/POwAslHKbhSxhBxmmpd4A==');


INSERT INTO HospitalityVenue (VenueName, Address, TypeID) VALUES
('Batak', 'Radnička cesta 37, ZG', 1),
('Stari Kotač', 'Aleja Bologne 18, Zagreb', 2),
('Konoba Vinko', 'Šepurine 12, Šibenik', 2),
('Procaffe', 'Obala Hrvatskog narodnog preporoda 6, Split', 6),
('Mistral', 'Žnjan plaža', 7);




INSERT INTO MenuItem (ItemName, ItemType, Price) VALUES
('Batak piletina', 'Food', 11.75),
('Pečena janjetina', 'Food', 15.50),
('Šarena pljeskavica', 'Food', 9.90),
('Graševina čaša', 'Drink', 3.20),
('Pržene lignje', 'Food', 13.50),
('Skradinski rižot', 'Food', 18.90),
('Bijelo vino kuće', 'Drink', 2.90),
('Cappuccino', 'Drink', 2.00),
('Croissant', 'Food', 2.00),
('Ćevapi sa sirom', 'Food', 9.50),
('Panacota', 'Food', 3.45),
('Koktel Mistral', 'Drink', 3.50),
('Velika toplo', 'Drink', 2.54);

INSERT INTO Reservation (NumberOfGuests, Status, ReservationDate, UserID, VenueID) VALUES
(3, 'Confirmed', '2025-06-03 20:30:00', 3, 3),
(5, 'Pending', '2025-07-04 16:45:00', 13, 4);


INSERT INTO VenueMenuItem (MenuItemID, VenueID) VALUES
(4, 3),
(6, 4),
(7, 4),
(8, 4),
(9, 5),
(10, 5),
(11, 5),
(14, 7),
(15, 7),
(16, 4),
(17, 7),
(21, 9);


INSERT INTO Logs (Message, Level, Timestamp) VALUES
('Test log message', 1, GETDATE());

