CREATE TABLE [dbo].[Desk]
(
	[DeskId] INT NOT NULL,
	[LocationId] INT NOT NULL FOREIGN KEY REFERENCES [dbo].[Location] ([LocationId]),
	[Note] NVARCHAR(MAX) NULL,
	PRIMARY KEY CLUSTERED ([DeskId] ASC)
)
