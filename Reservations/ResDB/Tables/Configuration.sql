﻿CREATE TABLE [dbo].[Configuration]
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[Name] NVARCHAR(100) NOT NULL UNIQUE,
	[Value] NVARCHAR(255) NOT NULL,
)
