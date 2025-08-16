CREATE TABLE [dbo].[ScheduleDateTimes] (
	[Id] UNIQUEIDENTIFIER NOT NULL,
	[ScheduleId] UNIQUEIDENTIFIER NOT NULL,
	[CronExpression] NVARCHAR(MAX) NOT NULL,
	[IsActive] BIT NOT NULL DEFAULT 1,
	[MaxParticipants] INT NULL,
);