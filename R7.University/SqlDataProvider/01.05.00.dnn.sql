-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

-- Divisions

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Divisions]') and name = N'IsVirtual')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Divisions]
        ADD [IsVirtual] bit NOT NULL DEFAULT ((0)),
        [HeadPositionID] int NULL,
        [StartDate] datetime NULL,
        [EndDate] datetime NULL

        CONSTRAINT [FK_{objectQualifier}University_Divisions_Positions] FOREIGN KEY([HeadPositionID])
            REFERENCES {databaseOwner}[{objectQualifier}University_Positions]([PositionID]) ON DELETE SET NULL
END
GO

-- Drop existing stored procedures & functions

IF EXISTS (select * from sys.objects where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_GetHeadEmployee]') and type in (N'P'))
    DROP PROCEDURE {databaseOwner}[{objectQualifier}University_GetHeadEmployee]
GO

IF EXISTS (select * from sys.objects where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_DivisionsHierarchy]') and type in (N'IF'))
    DROP FUNCTION {databaseOwner}[{objectQualifier}University_DivisionsHierarchy]
GO

-- Create stored procedures

CREATE PROCEDURE {databaseOwner}[{objectQualifier}University_GetHeadEmployee]
    @divisionId int,
    @headPositionId int
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    SELECT E.* FROM {databaseOwner}[{objectQualifier}University_Employees] AS E
        INNER JOIN {databaseOwner}[{objectQualifier}University_OccupiedPositions] AS EOP
            ON E.EmployeeID = EOP.EmployeeID
        WHERE EOP.DivisionID = @divisionId AND EOP.PositionID = @headPositionId AND E.IsPublished = 1
END
GO

-- Create functions

CREATE FUNCTION {databaseOwner}[{objectQualifier}University_DivisionsHierarchy]
(
    @divisionId int
)
RETURNS TABLE
AS
RETURN
(
    WITH DivisionsHierachy (DivisionID, ParentDivisionID, [Level], [Path])
    AS
    (
        SELECT DivisionID, ParentDivisionID, 0 AS [Level], CONVERT (nvarchar(4000), N'/') AS [Path]
            FROM {databaseOwner}[{objectQualifier}University_Divisions] AS D
                WHERE D.DivisionID = @divisionId -- insert parameter here
            UNION ALL
            SELECT D.DivisionID, D.ParentDivisionID, DH.[Level] + 1, DH.[Path] + CONVERT(nvarchar(4000), RIGHT(N'000000' + CONVERT(nvarchar, D.DivisionID), 6) + N'/')
                FROM {databaseOwner}[{objectQualifier}University_Divisions] AS D
                    INNER JOIN DivisionsHierachy AS DH
                        ON D.ParentDivisionID = DH.DivisionID
    )
    SELECT * FROM DivisionsHierachy
)
GO
