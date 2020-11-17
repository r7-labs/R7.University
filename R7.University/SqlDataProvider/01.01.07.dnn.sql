-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

-- Drop existing stored procedures

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}University_GetHeadEmployee]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE {databaseOwner}[{objectQualifier}University_GetHeadEmployee]
GO

-- Create stored procedures

CREATE PROCEDURE {databaseOwner}[{objectQualifier}University_GetHeadEmployee]
    @divisionId int
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    SELECT TOP (1) E.* FROM {databaseOwner}[{objectQualifier}University_Employees] AS E
		INNER JOIN {databaseOwner}[{objectQualifier}vw_University_OccupiedPositions] AS VOP
			ON E.EmployeeID = VOP.EmployeeID AND VOP.DivisionID = @divisionId
		WHERE E.IsPublished = 1
	ORDER BY VOP.PositionWeight DESC
END
GO
