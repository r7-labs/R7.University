-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

-- Drop existing stored procedures

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}University_FindEmployees]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE {databaseOwner}[{objectQualifier}University_FindEmployees]
GO

-- Create stored procedures

CREATE PROCEDURE {databaseOwner}[{objectQualifier}University_FindEmployees]
    @searchText nvarchar (50),
    @includeNonPublished bit,
    @teachersOnly bit,
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
        SELECT E.* FROM (
            SELECT EW.EmployeeID, MAX (EW.MaxWeight) AS MaxWeight
            FROM {databaseOwner}[{objectQualifier}University_DivisionsHierarchy](@divisionId) AS DH
                INNER JOIN {databaseOwner}[{objectQualifier}vw_University_OccupiedPositions] AS VOP
                    ON DH.DivisionID = VOP.DivisionID
                INNER JOIN {databaseOwner}[{objectQualifier}vw_University_EmployeesMaxWeight] AS EW
                    ON EW.EmployeeID = VOP.EmployeeID
            WHERE (EW.IsPublished = 1 OR @includeNonPublished = 1)
                AND (VOP.IsTeacher = 1 OR @teachersOnly = 0)
            GROUP BY EW.EmployeeID) AS DE
            INNER JOIN {databaseOwner}[{objectQualifier}University_Employees] AS E
                ON DE.EmployeeID = E.EmployeeID
            INNER JOIN {databaseOwner}[{objectQualifier}vw_University_OccupiedPositions] AS VOP
                   ON DE.EmployeeID = VOP.EmployeeID
        WHERE (LEN (@searchText) = 0 -- This could be empty
            OR E.FirstName + ' ' + E.LastName + ' ' + E.OtherName LIKE @searchPattern
            OR E.Email LIKE @searchPattern OR E.SecondaryEmail LIKE @searchPattern
            OR E.Phone LIKE @searchPattern OR E.CellPhone LIKE @searchPattern
            OR E.WorkingPlace LIKE @searchPattern
            OR VOP.PositionTitle + ' ' + VOP.PositionShortTitle LIKE @searchPattern)
        -- Sort by max weight of all employee positions
        ORDER BY DE.MaxWeight DESC, E.LastName;
    END
    ELSE
    BEGIN
        SELECT E.* FROM (
            SELECT EW.EmployeeID, MAX (EW.MaxWeight) AS MaxWeight
            FROM {databaseOwner}[{objectQualifier}vw_University_EmployeesMaxWeight] AS EW
                INNER JOIN {databaseOwner}[{objectQualifier}vw_University_OccupiedPositions] AS VOP
                    ON EW.EmployeeID = VOP.EmployeeID
            WHERE (EW.IsPublished = 1 OR @includeNonPublished = 1)
                AND (VOP.DivisionID = @divisionId OR @divisionId = -1)
                AND (VOP.IsTeacher = 1 OR @teachersOnly = 0)
            GROUP BY EW.EmployeeID) AS EI
            INNER JOIN {databaseOwner}[{objectQualifier}University_Employees] AS E
                ON EI.EmployeeID = E.EmployeeID
            INNER JOIN {databaseOwner}[{objectQualifier}vw_University_OccupiedPositions] AS VOP
                ON EI.EmployeeID = VOP.EmployeeID
        WHERE (E.FirstName + ' ' + E.LastName + ' ' + E.OtherName LIKE @searchPattern
            OR E.Email LIKE @searchPattern OR E.SecondaryEmail LIKE @searchPattern
            OR E.Phone LIKE @searchPattern OR E.CellPhone LIKE @searchPattern
            OR E.WorkingPlace LIKE @searchPattern
            OR VOP.PositionTitle + ' ' + VOP.PositionShortTitle LIKE @searchPattern)
        -- Sort by max weight of all employee positions
        ORDER BY EI.MaxWeight DESC, E.LastName;
    END
END
GO
