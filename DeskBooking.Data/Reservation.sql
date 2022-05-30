CREATE TABLE [dbo].[Reservation]
(
	[ReservationId] INT NOT NULL,
	[DeskId] INT NOT NULL FOREIGN KEY REFERENCES Desk([DeskId]),
	[UserId] INT NOT NULL FOREIGN KEY REFERENCES SystemUser([SystemUserId]),
	[StartDate] DATETIME NOT NULL,
	[EndDate] DATETIME NOT NULL,
	PRIMARY KEY CLUSTERED ([ReservationId] ASC)
)
