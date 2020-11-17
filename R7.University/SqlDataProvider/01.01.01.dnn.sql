-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

-- Alter tables

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Divisions]') and name = N'DocumentUrl')
	ALTER TABLE {databaseOwner}[{objectQualifier}University_Divisions]
		ADD [DocumentUrl] [nvarchar](250) NULL
GO

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Divisions]') and name = N'WebSiteLabel')
	ALTER TABLE {databaseOwner}[{objectQualifier}University_Divisions]
		ADD [WebSiteLabel] [nvarchar](64) NULL
GO

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Employees]') and name = N'WebSiteLabel')
	ALTER TABLE {databaseOwner}[{objectQualifier}University_Employees]
		ADD [WebSiteLabel] [nvarchar](64) NULL
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_Employees]
ALTER COLUMN [OtherName] [nvarchar](50) NULL
GO

-- Drop existing view

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}vw_University_EmployeesMaxWeight]') and OBJECTPROPERTY(id, N'IsView') = 1)
	DROP VIEW {databaseOwner}[{objectQualifier}vw_University_EmployeesMaxWeight]
GO

-- Create view

CREATE VIEW {databaseOwner}[{objectQualifier}vw_University_EmployeesMaxWeight] AS
	SELECT E.EmployeeID, E.IsPublished, MAX(P.Weight) AS MaxWeight
		FROM {databaseOwner}[{objectQualifier}University_Employees] AS E
		INNER JOIN {databaseOwner}[{objectQualifier}University_OccupiedPositions] AS OP
			ON E.EmployeeID = OP.EmployeeID
		INNER JOIN {databaseOwner}[{objectQualifier}University_Positions] AS P
			ON OP.PositionID = P.PositionID
	GROUP BY E.EmployeeID, E.IsPublished
GO

-- Drop existing stored procedures

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}University_GetEmployeesByDivisionID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE {databaseOwner}[{objectQualifier}University_GetEmployeesByDivisionID]
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}University_GetRecursiveEmployeesByDivisionID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE {databaseOwner}[{objectQualifier}University_GetRecursiveEmployeesByDivisionID]
GO

-- Create stored procedures

CREATE PROCEDURE {databaseOwner}[{objectQualifier}University_GetEmployeesByDivisionID]
	@divisionId int,
	@sortType int,
	@includeNonPublished bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF (@sortType = 1)
	BEGIN
		-- Sort by total (summary) weight of all employee positions
		SELECT DE.* FROM (
			SELECT DISTINCT E.*, EW.TotalWeight
				FROM {databaseOwner}[{objectQualifier}University_Employees] AS E
					INNER JOIN {databaseOwner}[{objectQualifier}University_OccupiedPositions] AS OP
						ON E.EmployeeID = OP.EmployeeID
					INNER JOIN {databaseOwner}[{objectQualifier}vw_University_EmployeesTotalWeight] AS EW
						ON E.EmployeeID = EW.EmployeeID
			WHERE OP.DivisionID = @divisionId AND (E.IsPublished = 1 OR @includeNonPublished = 1)
		) AS DE
		ORDER BY DE.TotalWeight DESC, DE.LastName;
	END
	ELSE IF (@sortType = 2)
	BEGIN
		-- Sort by employee lastname, then firstname
		SELECT DISTINCT E.*
			FROM {databaseOwner}[{objectQualifier}University_Employees] AS E
				INNER JOIN {databaseOwner}[{objectQualifier}University_OccupiedPositions] AS OP
					ON E.EmployeeID = OP.EmployeeID
			WHERE OP.DivisionID = @divisionId AND (E.IsPublished = 1 OR @includeNonPublished = 1)
		ORDER BY E.LastName
	END
	ELSE -- IF (@sortType = 0)
	BEGIN
		-- Sort by max weight of all employee positions (by default)
		SELECT E.* FROM (
			SELECT EW.EmployeeID,
				-- Add some weight to prime positions in the current division
				MAX (EW.MaxWeight + CASE WHEN (OP.DivisionID = @divisionId AND OP.IsPrime = 1) THEN 10 ELSE 0 END) AS MaxWeight
				FROM {databaseOwner}[{objectQualifier}vw_University_EmployeesMaxWeight] AS EW
					INNER JOIN {databaseOwner}[{objectQualifier}University_OccupiedPositions] AS OP
						ON EW.EmployeeID = OP.EmployeeID
			WHERE OP.DivisionID = @divisionId  AND (EW.IsPublished = 1 OR @includeNonPublished = 1)
			GROUP BY EW.EmployeeID
		) AS DE INNER JOIN {databaseOwner}[{objectQualifier}University_Employees] AS E ON DE.EmployeeID = E.EmployeeID
		ORDER BY DE.MaxWeight DESC, E.LastName;
	END
END
GO

CREATE PROCEDURE {databaseOwner}[{objectQualifier}University_GetRecursiveEmployeesByDivisionID]
	-- Add the parameters for the stored procedure here
	@divisionId int,
	@sortType int,
	@includeNonPublished bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	IF (@sortType = 1)
	BEGIN
		SELECT DE.* FROM (
			SELECT DISTINCT E.*, EW.TotalWeight
				FROM {databaseOwner}[{objectQualifier}University_DivisionsHierarchy](@divisionId) AS DH
					INNER JOIN {databaseOwner}[{objectQualifier}University_OccupiedPositions]	AS OP
						ON DH.DivisionID = OP.DivisionID
					INNER JOIN {databaseOwner}[{objectQualifier}.University_Employees] AS E
						ON E.EmployeeID = OP.EmployeeID
					INNER JOIN {databaseOwner}[{objectQualifier}vw_University_EmployeesTotalWeight] AS EW
						ON E.EmployeeID = EW.EmployeeID
			WHERE E.IsPublished = 1 OR @includeNonPublished = 1
		) AS DE
		ORDER BY DE.TotalWeight DESC, DE.LastName;
	END
	ELSE IF (@sortType = 2)
	BEGIN
		-- Sort by employee lastname, then firstname
		SELECT DISTINCT E.*
			FROM {databaseOwner}[{objectQualifier}University_DivisionsHierarchy](@divisionId) AS DH
				INNER JOIN {databaseOwner}[{objectQualifier}University_OccupiedPositions] AS OP
					ON DH.DivisionID = OP.DivisionID
				INNER JOIN {databaseOwner}[{objectQualifier}University_Employees] AS E
					ON E.EmployeeID = OP.EmployeeID
			WHERE OP.DivisionID = @divisionId AND (E.IsPublished = 1 OR @includeNonPublished = 1)
		ORDER BY E.LastName;
	END
	ELSE -- IF (@sortType = 0)
	BEGIN
		-- Sort by max weight of all employee positions (by default)
		SELECT E.* FROM (
			SELECT EW.EmployeeID,
				-- Add some weight to prime positions in the current division
				MAX (EW.MaxWeight + CASE WHEN (OP.DivisionID = @divisionId AND OP.IsPrime = 1) THEN 10 ELSE 0 END) AS MaxWeight
				FROM {databaseOwner}[{objectQualifier}University_DivisionsHierarchy](@divisionId) AS DH
					INNER JOIN {databaseOwner}[{objectQualifier}University_OccupiedPositions] AS OP
						ON DH.DivisionID = OP.DivisionID
					INNER JOIN {databaseOwner}[{objectQualifier}vw_University_EmployeesMaxWeight] AS EW
						ON EW.EmployeeID = OP.EmployeeID
			WHERE EW.IsPublished = 1 OR @includeNonPublished = 1
			GROUP BY EW.EmployeeID
		) AS DE INNER JOIN {databaseOwner}[{objectQualifier}University_Employees] AS E ON DE.EmployeeID = E.EmployeeID
		ORDER BY DE.MaxWeight DESC, E.LastName;
	END
END
GO
