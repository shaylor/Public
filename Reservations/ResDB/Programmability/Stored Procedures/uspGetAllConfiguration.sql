CREATE PROCEDURE [dbo].[uspGetAllConfiguration]
AS
BEGIN
	SET NOCOUNT ON;
	SELECT 
		[Id],
		[Name],
		[Value]
	FROM 
		[Configuration]
	ORDER BY 
		[Name];
END