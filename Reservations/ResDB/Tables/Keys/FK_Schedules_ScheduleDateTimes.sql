ALTER TABLE [dbo].[ScheduleDateTimes]
	ADD CONSTRAINT [FK_Schedules_ScheduleDateTimes] FOREIGN KEY ([ScheduleId])
	REFERENCES [dbo].[Schedules] ([Id])
	ON DELETE CASCADE
	ON UPDATE NO ACTION;