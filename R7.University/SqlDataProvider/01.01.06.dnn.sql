-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

-- Drop existing stored procedures

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}University_FindDivisions]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE {databaseOwner}[{objectQualifier}University_FindDivisions]
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}University_GetHeadEmployee]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE {databaseOwner}[{objectQualifier}University_GetHeadEmployee]
GO

-- Create stored procedures

CREATE PROCEDURE {databaseOwner}[{objectQualifier}University_FindDivisions]
    @searchText nvarchar (50),
    -- TODO: Remove @includeSubdivisions as obsolete, should always be 1
    @includeSubdivisions bit,
    @divisionId int = -1
AS
DECLARE
    @searchPattern nvarchar (100)
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    SET @searchPattern = N'%' + @searchText + '%';

    IF (@divisionId <> -1 AND @includeSubdivisions = 1)
    BEGIN
        SELECT D.* FROM {databaseOwner}[{objectQualifier}University_DivisionsHierarchy](@divisionId) AS DH
            INNER JOIN {databaseOwner}[{objectQualifier}University_Divisions] AS D
                ON DH.DivisionID = D.DivisionID
        WHERE (LEN (@searchText) = 0 -- This could be empty
	        OR D.Title LIKE @searchPattern OR D.ShortTitle LIKE @searchPattern
	        OR D.Email LIKE @searchPattern OR D.SecondaryEmail LIKE @searchPattern
	        OR D.Phone LIKE @searchPattern OR D.Fax LIKE @searchPattern
	        OR D.Location LIKE @searchPattern)
       	ORDER BY DH.Level, D.Title
    END
    ELSE
    BEGIN
        SELECT D.* FROM {databaseOwner}[{objectQualifier}University_Divisions] AS D
        WHERE (D.DivisionID = @divisionId OR @divisionId = -1) AND
        	(D.Title LIKE @searchPattern OR D.ShortTitle LIKE @searchPattern
	        OR D.Email LIKE @searchPattern OR D.SecondaryEmail LIKE @searchPattern
	        OR D.Phone LIKE @searchPattern OR D.Fax LIKE @searchPattern
	        OR D.Location LIKE @searchPattern)
       	ORDER BY D.Title
    END
END
GO

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
	ORDER BY VOP.PositionWeight DESC
END
GO
