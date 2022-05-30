﻿CREATE TABLE [dbo].[SystemUser]
(
	[SystemUserId] INT NOT NULL,
	[Name] NVARCHAR(50) NOT NULL,
	[Surname] NVARCHAR(50) NOT NULL,
	[Email] NVARCHAR(50) NOT NULL,
	[Password] NVARCHAR(50) NOT NULL,
	[IsAdmin] BIT NOT NULL,
	PRIMARY KEY CLUSTERED ([SystemUserId] ASC)
)