-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

-- EduPrograms

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EduPrograms]') and name = N'Generation')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduPrograms]
        ADD [Generation] nvarchar (16) NULL,
        [AccreditedToDate] datetime NULL,
        [StartDate] datetime NULL,
        [EndDate] datetime NULL,
        [CreatedOnDate] datetime NOT NULL DEFAULT ((GETDATE())),
        [CreatedByUserID] int NOT NULL DEFAULT ((-1)),
        [LastModifiedOnDate] datetime NOT NULL DEFAULT ((GETDATE())),
        [LastModifiedByUserID] int NOT NULL DEFAULT ((-1))
END
GO

-- EduProgramProfiles

IF NOT EXISTS (select * from sys.objects where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EduProgramProfiles]') and type in (N'U'))
BEGIN
    CREATE TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfiles] (
        [EduProgramProfileID] int IDENTITY(1,1) NOT NULL,
        [EduProgramID] int NOT NULL,
        [OldEduProgramID] int,
        [ProfileCode] nvarchar (64) NULL,
        [ProfileTitle] nvarchar (250) NULL,
        [DivisionID] int NULL,
        [StartDate] datetime NULL,
        [EndDate] datetime NULL,
        [CreatedOnDate] datetime NOT NULL DEFAULT ((GETDATE())),
        [CreatedByUserID] int NOT NULL DEFAULT ((-1)),
        [LastModifiedOnDate] datetime NOT NULL DEFAULT ((GETDATE())),
        [LastModifiedByUserID] int NOT NULL DEFAULT ((-1))
        CONSTRAINT [PK_{objectQualifier}University_EduProgramProfiles] PRIMARY KEY CLUSTERED (EduProgramProfileID)
        CONSTRAINT [FK_{objectQualifier}University_EduProgramProfiles_EduPrograms] FOREIGN KEY (EduProgramID)
            REFERENCES {databaseOwner}[{objectQualifier}University_EduPrograms] (EduProgramID) ON DELETE CASCADE
    )

    INSERT INTO {databaseOwner}[{objectQualifier}University_EduProgramProfiles] ([EduProgramID], [OldEduProgramID], [ProfileCode], [ProfileTitle]) SELECT
        (SELECT TOP(1) EduProgramID FROM {databaseOwner}[{objectQualifier}University_EduPrograms] WHERE Code = EP.Code ORDER BY EduProgramID ASC) AS EduProgramID,
        EP.EduProgramID AS OldEduProgramID, EP.ProfileCode, EP.ProfileTitle
            FROM {databaseOwner}[{objectQualifier}University_EduPrograms] AS EP
END
GO

-- EmployeeDisciplines

IF NOT EXISTS (select * from sys.objects where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EmployeeDisciplines]') and type in (N'U'))
BEGIN
    CREATE TABLE {databaseOwner}[{objectQualifier}University_EmployeeDisciplines] (
        [EmployeeDisciplineID] bigint IDENTITY(1,1) NOT NULL,
        [EmployeeID] int NOT NULL,
        [EduProgramProfileID] int NOT NULL,
        [Disciplines] nvarchar(max) NULL
        CONSTRAINT [PK_{objectQualifier}University_EmployeeDisciplines] PRIMARY KEY CLUSTERED ([EmployeeDisciplineID])
        CONSTRAINT [FK_{objectQualifier}University_EmployeeDisciplines_Employees] FOREIGN KEY([EmployeeID])
            REFERENCES {databaseOwner}[{objectQualifier}University_Employees]([EmployeeID]) ON DELETE CASCADE,
        CONSTRAINT [FK_{objectQualifier}University_EmployeeDisciplines_EduProgramProfiles] FOREIGN KEY([EduProgramProfileID])
            REFERENCES {databaseOwner}[{objectQualifier}University_EduProgramProfiles]([EduProgramProfileID]) ON DELETE CASCADE,
        CONSTRAINT [UN_{objectQualifier}University_EmployeeDisciplines] UNIQUE ([EmployeeID], [EduProgramProfileID])
    )

    INSERT INTO {databaseOwner}[{objectQualifier}University_EmployeeDisciplines] SELECT EEP.EmployeeID,
        (SELECT EPP.EduProgramProfileID FROM dbo.University_EduProgramProfiles AS EPP WHERE EPP.OldEduProgramID = EEP.EduProgramID) AS EduProgramProfileID,
        EEP.Disciplines FROM dbo.University_EmployeeEduPrograms AS EEP
END
GO

-- Cleanup

IF EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EduPrograms]') and name = N'ProfileCode')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduPrograms]
        DROP COLUMN ProfileCode, ProfileTitle
GO

IF EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EduProgramProfiles]') and name = N'OldEduProgramID')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfiles]
        DROP COLUMN OldEduProgramID
GO

IF EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Employees]') and name = N'NamePrefix')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Employees]
        DROP COLUMN NamePrefix, Disciplines, AcademicTitle, AcademicDegree
GO

IF EXISTS (select * from sys.objects where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EmployeeEduPrograms]') and type in (N'U'))
    DROP TABLE {databaseOwner}[{objectQualifier}University_EmployeeEduPrograms]
GO

-- Clean orphaned educational programs

DELETE FROM {databaseOwner}[{objectQualifier}University_EduPrograms]
    WHERE EduProgramID NOT IN (SELECT DISTINCT EduProgramID FROM {databaseOwner}[{objectQualifier}University_EduProgramProfiles])
GO

-- Drop views

IF EXISTS (select * from sys.objects where object_id = object_id(N'{databaseOwner}[{objectQualifier}vw_University_EmployeeEduPrograms]') and type in (N'V'))
    DROP VIEW {databaseOwner}[{objectQualifier}vw_University_EmployeeEduPrograms]
GO

IF EXISTS (select * from sys.objects where object_id = object_id(N'{databaseOwner}[{objectQualifier}vw_University_EmployeeDisciplines]') and type in (N'V'))
    DROP VIEW {databaseOwner}[{objectQualifier}vw_University_EmployeeDisciplines]
GO

IF EXISTS (select * from sys.objects where object_id = object_id(N'{databaseOwner}[{objectQualifier}vw_University_EduProgramProfiles]') and type in (N'V'))
    DROP VIEW {databaseOwner}[{objectQualifier}vw_University_EduProgramProfiles]
GO

-- Create views

CREATE VIEW {databaseOwner}[{objectQualifier}vw_University_EmployeeDisciplines] AS
    SELECT ED.*, EP.Code, EP.Title, EPP.ProfileCode, EPP.ProfileTitle FROM {databaseOwner}[{objectQualifier}University_EmployeeDisciplines] AS ED
        INNER JOIN {databaseOwner}[{objectQualifier}University_EduProgramProfiles] AS EPP
            ON ED.EduProgramProfileID = EPP.EduProgramProfileID
        INNER JOIN {databaseOwner}[{objectQualifier}University_EduPrograms] AS EP
            ON EPP.EduProgramID = EP.EduProgramID
GO

CREATE VIEW {databaseOwner}[{objectQualifier}vw_University_EduProgramProfiles] AS
    SELECT EPP.*, EP.Code, EP.Title FROM {databaseOwner}[{objectQualifier}University_EduProgramProfiles] AS EPP
        INNER JOIN {databaseOwner}[{objectQualifier}University_EduPrograms] AS EP
            ON EPP.EduProgramID = EP.EduProgramID
GO
