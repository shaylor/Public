ALTER TABLE [dbo].[Users]
	ADD CONSTRAINT [FK_Users_Roles] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Roles] ([Id])