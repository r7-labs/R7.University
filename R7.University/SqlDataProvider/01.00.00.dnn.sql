-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

-- Create tables

IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}University_Positions]') and OBJECTPROPERTY(id, N'IsTable') = 1)
BEGIN
	CREATE TABLE {databaseOwner}[{objectQualifier}University_Positions] (
		[PositionID] [int] IDENTITY(1,1) NOT NULL,
		[Title] [nvarchar](100) NOT NULL,
		[ShortTitle] [nvarchar](50) NULL,
		[Weight] [int] NOT NULL,
		CONSTRAINT [PK_{objectQualifier}University_Duties] PRIMARY KEY CLUSTERED
		(
			[PositionID] ASC
		) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE {databaseOwner}[{objectQualifier}University_Positions] ADD  CONSTRAINT [DF_{objectQualifier}University_Positions_Weight] DEFAULT ((1)) FOR [Weight]

END
GO

IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}University_Divisions]') and OBJECTPROPERTY(id, N'IsTable') = 1)
BEGIN
	CREATE TABLE {databaseOwner}[{objectQualifier}University_Divisions](
		[DivisionID] [int] IDENTITY(1,1) NOT NULL,
		[ParentDivisionID] [int] NULL,
		[DivisionTermID] [int] NULL,
		[Title] [nvarchar](128) NOT NULL,
		[ShortTitle] [nvarchar](64) NULL,
		[HomePage] [nvarchar](128) NULL,
		[WebSite] [nvarchar](128) NULL,
		[Phone] [nvarchar](50) NULL,
		[Email] [nvarchar](250) NULL,
		[SecondaryEmail] [nvarchar](250) NULL,
		[Location] [nvarchar](128) NULL,
		[WorkingHours] [nvarchar](100) NULL,
		[CreatedByUserID] [int] NOT NULL,
		[CreatedOnDate] [datetime] NOT NULL,
		[LastModifiedByUserID] [int] NOT NULL,
		[LastModifiedOnDate] [datetime] NOT NULL,
		[Fax] [nvarchar](50) NULL,
	 CONSTRAINT [PK_{objectQualifier}University_Units] PRIMARY KEY CLUSTERED
	 (
		[DivisionID] ASC
	 ) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE {databaseOwner}[{objectQualifier}University_Divisions] ADD  CONSTRAINT [DF_{objectQualifier}University_Divisions_CreatedByUserID]  DEFAULT ((-1)) FOR [CreatedByUserID]

	ALTER TABLE {databaseOwner}[{objectQualifier}University_Divisions] ADD  CONSTRAINT [DF_{objectQualifier}University_Divisions_CreatedOnDate]  DEFAULT (getdate()) FOR [CreatedOnDate]

	ALTER TABLE {databaseOwner}[{objectQualifier}University_Divisions] ADD  CONSTRAINT [DF_{objectQualifier}University_Divisions_LastModifiedByUserID]  DEFAULT ((-1)) FOR [LastModifiedByUserID]

	ALTER TABLE {databaseOwner}[{objectQualifier}University_Divisions] ADD  CONSTRAINT [DF_{objectQualifier}University_Divisions_LastModifiedOnDate]  DEFAULT (getdate()) FOR [LastModifiedOnDate]

	ALTER TABLE {databaseOwner}[{objectQualifier}University_Divisions]  WITH CHECK ADD  CONSTRAINT [FK_{objectQualifier}University_Divisions_Taxonomy_Terms] FOREIGN KEY([DivisionTermID])
	REFERENCES [dbo].[Taxonomy_Terms] ([TermID])
	ON DELETE SET NULL

	ALTER TABLE {databaseOwner}[{objectQualifier}University_Divisions] CHECK CONSTRAINT [FK_{objectQualifier}University_Divisions_Taxonomy_Terms]

END
GO

IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}University_Employees]') and OBJECTPROPERTY(id, N'IsTable') = 1)
BEGIN
	CREATE TABLE {databaseOwner}[{objectQualifier}University_Employees](
		[EmployeeID] [int] IDENTITY(1,1) NOT NULL,
		[UserID] [int] NULL,
		[PhotoFileID] [int] NULL,
		[Phone] [nvarchar](30) NULL,
		[CellPhone] [nvarchar](30) NULL,
		[Fax] [nvarchar](30) NULL,
		[LastName] [nvarchar](50) NOT NULL,
		[FirstName] [nvarchar](50) NOT NULL,
		[OtherName] [nvarchar](50) NOT NULL,
		[NamePrefix] [nvarchar](50) NULL,
		[Email] [nvarchar](250) NULL,
		[SecondaryEmail] [nvarchar](250) NULL,
		[WebSite] [nvarchar](250) NULL,
		[Messenger] [nvarchar](250) NULL,
		[AcademicDegree] [nvarchar](50) NULL,
		[AcademicTitle] [nvarchar](50) NULL,
		[WorkingPlace] [nvarchar](50) NULL,
		[WorkingHours] [nvarchar](100) NULL,
		[Biography] [nvarchar](max) NULL,
		[ExperienceYears] [int] NULL,
		[ExperienceYearsBySpec] [int] NULL,
		[IsPublished] [bit] NOT NULL,
		[LastModifiedByUserID] [int] NOT NULL,
		[LastModifiedOnDate] [datetime] NOT NULL,
		[CreatedByUserID] [int] NOT NULL,
		[CreatedOnDate] [datetime] NOT NULL,
		[IsDeleted] [bit] NOT NULL,
	 	CONSTRAINT [PK_{objectQualifier}University_Employees] PRIMARY KEY CLUSTERED
		(
			[EmployeeID] ASC
		) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

	ALTER TABLE {databaseOwner}[{objectQualifier}University_Employees] ADD  CONSTRAINT [DF_{objectQualifier}University_Employees_IsPublished]  DEFAULT ((0)) FOR [IsPublished]

	ALTER TABLE {databaseOwner}[{objectQualifier}University_Employees] ADD  CONSTRAINT [DF_{objectQualifier}University_Employees_LastModifiedBy]  DEFAULT ((-1)) FOR [LastModifiedByUserID]

	ALTER TABLE {databaseOwner}[{objectQualifier}University_Employees] ADD  CONSTRAINT [DF_{objectQualifier}University_Employees_LastModifiedOnDate]  DEFAULT (getdate()) FOR [LastModifiedOnDate]

	ALTER TABLE {databaseOwner}[{objectQualifier}University_Employees] ADD  CONSTRAINT [DF_{objectQualifier}University_Employees_CreatedBy]  DEFAULT ((-1)) FOR [CreatedByUserID]

	ALTER TABLE {databaseOwner}[{objectQualifier}University_Employees] ADD  CONSTRAINT [DF_{objectQualifier}University_Employees_CreatedOnDate]  DEFAULT (getdate()) FOR [CreatedOnDate]

	ALTER TABLE {databaseOwner}[{objectQualifier}University_Employees] ADD  CONSTRAINT [DF_{objectQualifier}University_Employees_IsDeleted]  DEFAULT ((0)) FOR [IsDeleted]

	ALTER TABLE {databaseOwner}[{objectQualifier}University_Employees]  WITH CHECK ADD  CONSTRAINT [FK_{objectQualifier}University_Employees_Users] FOREIGN KEY([UserID])
		REFERENCES [dbo].[Users] ([UserID]) ON DELETE SET NULL

	ALTER TABLE {databaseOwner}[{objectQualifier}University_Employees] CHECK CONSTRAINT [FK_{objectQualifier}University_Employees_Users]

END
GO

IF NOT EXISTS (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}University_OccupiedPositions]') and OBJECTPROPERTY(id, N'IsTable') = 1)
BEGIN
	CREATE TABLE {databaseOwner}[{objectQualifier}University_OccupiedPositions](
		[OccupiedPositionID] [int] IDENTITY(1,1) NOT NULL,
		[PositionID] [int] NOT NULL,
		[EmployeeID] [int] NOT NULL,
		[DivisionID] [int] NOT NULL,
		[IsPrime] [bit] NOT NULL,
		CONSTRAINT [PK_{objectQualifier}University_OccupiedPositions] PRIMARY KEY CLUSTERED
		(
			[OccupiedPositionID] ASC
		) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
	 	CONSTRAINT [UN_{objectQualifier}University_OccupiedPositions] UNIQUE NONCLUSTERED
		(
			[DivisionID] ASC,
			[EmployeeID] ASC,
			[PositionID] ASC
		) WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
	) ON [PRIMARY]

	ALTER TABLE {databaseOwner}[{objectQualifier}University_OccupiedPositions] ADD  CONSTRAINT [DF_{objectQualifier}University_OccupiedPositions_IsPrime]  DEFAULT ((1)) FOR [IsPrime]

	ALTER TABLE {databaseOwner}[{objectQualifier}University_OccupiedPositions]  WITH CHECK ADD  CONSTRAINT [FK_{objectQualifier}University_OccupiedPositions_University_Divisions] FOREIGN KEY([DivisionID])
		REFERENCES {databaseOwner}[{objectQualifier}University_Divisions] ([DivisionID]) ON DELETE CASCADE

	ALTER TABLE {databaseOwner}[{objectQualifier}University_OccupiedPositions] CHECK CONSTRAINT [FK_{objectQualifier}University_OccupiedPositions_University_Divisions]

	ALTER TABLE {databaseOwner}[{objectQualifier}University_OccupiedPositions]  WITH CHECK ADD  CONSTRAINT [FK_{objectQualifier}University_OccupiedPositions_University_Employees] FOREIGN KEY([EmployeeID])
		REFERENCES {databaseOwner}[{objectQualifier}University_Employees] ([EmployeeID]) ON DELETE CASCADE

	ALTER TABLE {databaseOwner}[{objectQualifier}University_OccupiedPositions] CHECK CONSTRAINT [FK_{objectQualifier}University_OccupiedPositions_University_Employees]

	ALTER TABLE {databaseOwner}[{objectQualifier}University_OccupiedPositions]  WITH CHECK ADD  CONSTRAINT [FK_{objectQualifier}University_OccupiedPositions_University_Positions] FOREIGN KEY([PositionID])
		REFERENCES {databaseOwner}[{objectQualifier}University_Positions] ([PositionID]) ON DELETE CASCADE

	ALTER TABLE {databaseOwner}[{objectQualifier}University_OccupiedPositions] CHECK CONSTRAINT [FK_{objectQualifier}University_OccupiedPositions_University_Positions]

END
GO

-- Drop existing views

 IF EXISTS (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}vw_University_OccupiedPositions]') and OBJECTPROPERTY(id, N'IsView') = 1)
 	DROP VIEW {databaseOwner}[{objectQualifier}vw_University_OccupiedPositions]
 GO

 IF EXISTS (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}vw_University_EmployeesMaxWeight]') and OBJECTPROPERTY(id, N'IsView') = 1)
 	DROP VIEW {databaseOwner}[{objectQualifier}vw_University_EmployeesMaxWeight]
 GO

 IF EXISTS (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}vw_University_EmployeesTotalWeight]') and OBJECTPROPERTY(id, N'IsView') = 1)
 	DROP VIEW {databaseOwner}[{objectQualifier}vw_University_EmployeesTotalWeight]
 GO

-- Create views

CREATE VIEW {databaseOwner}[{objectQualifier}vw_University_OccupiedPositions] AS
	SELECT OP.OccupiedPositionID, OP.PositionID, OP.EmployeeID,
		OP.IsPrime, OP.DivisionID, P.ShortTitle AS PositionShortTitle,
		D.ShortTitle AS DivisionShortTitle, P.Title AS PositionTitle, D.Title AS DivisionTitle,
		P.Weight AS PositionWeight, D.ParentDivisionID,
		D.HomePage
	FROM {databaseOwner}[{objectQualifier}University_OccupiedPositions] AS OP
		INNER JOIN {databaseOwner}[{objectQualifier}University_Positions] AS P
			ON OP.PositionID = P.PositionID
		INNER JOIN {databaseOwner}[{objectQualifier}University_Divisions] AS D
			ON OP.DivisionID = D.DivisionID
GO

CREATE VIEW {databaseOwner}[{objectQualifier}vw_University_EmployeesMaxWeight] AS
	SELECT E.EmployeeID, MAX(P.Weight) AS MaxWeight
		FROM {databaseOwner}[{objectQualifier}University_Employees] AS E
		INNER JOIN {databaseOwner}[{objectQualifier}University_OccupiedPositions] AS OP
			ON E.EmployeeID = OP.EmployeeID
		INNER JOIN {databaseOwner}[{objectQualifier}University_Positions] AS P
			ON OP.PositionID = P.PositionID
	GROUP BY E.EmployeeID
GO

CREATE VIEW {databaseOwner}[{objectQualifier}vw_University_EmployeesTotalWeight] AS
	SELECT E.EmployeeID, SUM(P.Weight) AS TotalWeight
		FROM {databaseOwner}[{objectQualifier}University_Employees] AS E
			INNER JOIN {databaseOwner}[{objectQualifier}University_OccupiedPositions] AS OP
				ON E.EmployeeID = OP.EmployeeID
			INNER JOIN {databaseOwner}[{objectQualifier}University_Positions] AS P
				ON OP.PositionID = P.PositionID
		GROUP BY E.EmployeeID
GO

-- Drop existing stored procedures & functions

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}University_GetEmployeesByDivisionID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE {databaseOwner}[{objectQualifier}University_GetEmployeesByDivisionID]
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}University_GetRecursiveEmployeesByDivisionID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
	DROP PROCEDURE {databaseOwner}[{objectQualifier}University_GetRecursiveEmployeesByDivisionID]
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}University_DivisionsHierarchy]') and OBJECTPROPERTY(id, N'IsTableFunction') = 1)
	DROP FUNCTION {databaseOwner}[{objectQualifier}University_DivisionsHierarchy]
GO

-- Create stored procedures & functions

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
		SELECT DE.* FROM (
			SELECT DISTINCT E.*, EW.MaxWeight
				FROM {databaseOwner}[{objectQualifier}University_Employees] AS E
					INNER JOIN {databaseOwner}[{objectQualifier}University_OccupiedPositions] AS OP
						ON E.EmployeeID = OP.EmployeeID
					INNER JOIN {databaseOwner}[{objectQualifier}vw_University_EmployeesMaxWeight] AS EW
						ON E.EmployeeID = EW.EmployeeID
			WHERE OP.DivisionID = @divisionId  AND (E.IsPublished = 1 OR @includeNonPublished = 1)
		) AS DE
		ORDER BY DE.MaxWeight DESC, DE.LastName;
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
		SELECT DE.* FROM (
			SELECT DISTINCT E.*, EW.MaxWeight
				FROM {databaseOwner}[{objectQualifier}University_DivisionsHierarchy](@divisionId) AS DH
					INNER JOIN {databaseOwner}[{objectQualifier}University_OccupiedPositions]	AS OP
						ON DH.DivisionID = OP.DivisionID
					INNER JOIN {databaseOwner}[{objectQualifier}University_Employees] AS E
						ON E.EmployeeID = OP.EmployeeID
					INNER JOIN {databaseOwner}[{objectQualifier}vw_University_EmployeesMaxWeight] AS EW
						ON E.EmployeeID = EW.EmployeeID
			WHERE E.IsPublished = 1 OR @includeNonPublished = 1
		) AS DE
		ORDER BY DE.MaxWeight DESC, DE.LastName;
	END
END
GO

CREATE FUNCTION {databaseOwner}[{objectQualifier}University_DivisionsHierarchy]
(
	@divisionId int
)
RETURNS TABLE
AS
RETURN
(
	WITH DivisionsHierachy (DivisionID, ParentDivisionID, [Level])
	AS
	(
		SELECT DivisionID, ParentDivisionID, 0 AS [Level]
			FROM {databaseOwner}[{objectQualifier}University_Divisions] AS D
				WHERE D.DivisionID = @divisionId -- insert parameter here
			UNION ALL
			SELECT D.DivisionID, D.ParentDivisionID, DH.[Level] + 1
				FROM {databaseOwner}[{objectQualifier}University_Divisions] AS D
					INNER JOIN DivisionsHierachy AS DH
						ON DH.ParentDivisionID = D.DivisionID
	)
	SELECT * FROM DivisionsHierachy
)
GO
