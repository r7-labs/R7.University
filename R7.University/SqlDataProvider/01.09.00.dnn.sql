-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Employees]') and name = N'ShowBarcode')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Employees]
        ADD [ShowBarcode] bit NOT NULL DEFAULT ((1))
END
GO

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EduLevels]') and name = N'ParentEduLevelID')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduLevels]
        ADD [ParentEduLevelID] int NULL
        CONSTRAINT [FK_{objectQualifier}University_EduLevels_ParentEduLevels] FOREIGN KEY (ParentEduLevelID)
            REFERENCES {databaseOwner}[{objectQualifier}University_EduLevels] (EduLevelID) ON DELETE NO ACTION
END
GO

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EduProgramProfiles]') and name = N'EduLevelID')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfiles]
        ADD [EduLevelID] int NULL
GO

IF EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EduProgramProfiles]') and name = N'EduLevelID' and is_nullable = 1)
BEGIN
    UPDATE {databaseOwner}[{objectQualifier}University_EduProgramProfiles] SET EduLevelID =
        (SELECT EP.EduLevelID FROM {databaseOwner}[{objectQualifier}University_EduPrograms] AS EP WHERE EP.EduProgramID = EPP.EduProgramID)
        FROM {databaseOwner}[{objectQualifier}University_EduProgramProfiles] AS EPP

    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfiles]
        ALTER COLUMN [EduLevelID] int NOT NULL

    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfiles]
        ADD CONSTRAINT [FK_{objectQualifier}University_EduProgramProfiles_EduLevels] FOREIGN KEY (EduLevelID)
            REFERENCES {databaseOwner}[{objectQualifier}University_EduLevels] (EduLevelID) ON DELETE NO ACTION
END
GO

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EduProgramProfiles]') and name = N'DivisionID')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfiles]
        ADD [DivisionID] int NULL
        CONSTRAINT [FK_{objectQualifier}University_EduProgramProfiles_Divisions] FOREIGN KEY (DivisionID)
            REFERENCES {databaseOwner}[{objectQualifier}University_Divisions] (DivisionID) ON DELETE SET NULL
END
GO

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EduPrograms]') and name = N'HomePage')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduPrograms]
        ADD [HomePage] nvarchar(255) NULL,
        [DivisionID] int NULL
        CONSTRAINT [FK_{objectQualifier}University_EduPrograms_Divisions] FOREIGN KEY (DivisionID)
            REFERENCES {databaseOwner}[{objectQualifier}University_Divisions] (DivisionID) ON DELETE SET NULL
END
GO

-- Add indexes

IF NOT EXISTS (select * from sys.indexes where object_id = object_id (N'{databaseOwner}[{objectQualifier}University_Divisions]') and name = N'IX_{objectQualifier}University_Divisions_ParentDivisions')
    CREATE NONCLUSTERED INDEX [IX_{objectQualifier}University_Divisions_ParentDivisions]
        ON {databaseOwner}[{objectQualifier}University_Divisions] (ParentDivisionID ASC)
GO

IF NOT EXISTS (select * from sys.indexes where object_id = object_id (N'{databaseOwner}[{objectQualifier}University_Documents]') and name = N'IX_{objectQualifier}University_Documents_DocumentTypes')
    CREATE NONCLUSTERED INDEX [IX_{objectQualifier}University_Documents_DocumentTypes]
        ON {databaseOwner}[{objectQualifier}University_Documents] (DocumentTypeID ASC)
GO

IF NOT EXISTS (select * from sys.indexes where object_id = object_id (N'{databaseOwner}[{objectQualifier}University_EduProgramProfileForms]') and name = N'IX_{objectQualifier}University_EduProgramProfileForms')
    CREATE NONCLUSTERED INDEX [IX_{objectQualifier}University_EduProgramProfileForms]
        ON {databaseOwner}[{objectQualifier}University_EduProgramProfileForms] (EduProgramProfileID ASC, EduFormID ASC)
GO

IF NOT EXISTS (select * from sys.indexes where object_id = object_id (N'{databaseOwner}[{objectQualifier}University_EduProgramProfiles]') and name = N'IX_{objectQualifier}University_EduProgramProfiles_EduPrograms')
    CREATE NONCLUSTERED INDEX [IX_{objectQualifier}University_EduProgramProfiles_EduPrograms]
        ON {databaseOwner}[{objectQualifier}University_EduProgramProfiles] (EduProgramID ASC)
GO

IF NOT EXISTS (select * from sys.indexes where object_id = object_id (N'{databaseOwner}[{objectQualifier}University_EduProgramProfiles]') and name = N'IX_{objectQualifier}University_EduProgramProfiles_Divisions')
    CREATE NONCLUSTERED INDEX [IX_{objectQualifier}University_EduProgramProfiles_Divisions]
        ON {databaseOwner}[{objectQualifier}University_EduProgramProfiles] (DivisionID ASC)
GO

IF NOT EXISTS (select * from sys.indexes where object_id = object_id (N'{databaseOwner}[{objectQualifier}University_EduPrograms]') and name = N'IX_{objectQualifier}University_EduPrograms_EduLevels')
    CREATE NONCLUSTERED INDEX [IX_{objectQualifier}University_EduPrograms_EduLevels]
        ON {databaseOwner}[{objectQualifier}University_EduPrograms] (EduLevelID ASC)
GO

IF NOT EXISTS (select * from sys.indexes where object_id = object_id (N'{databaseOwner}[{objectQualifier}University_EduPrograms]') and name = N'IX_{objectQualifier}University_EduPrograms_Divisions')
    CREATE NONCLUSTERED INDEX [IX_{objectQualifier}University_EduPrograms_Divisions]
        ON {databaseOwner}[{objectQualifier}University_EduPrograms] (DivisionID ASC)
GO

IF NOT EXISTS (select * from sys.indexes where object_id = object_id (N'{databaseOwner}[{objectQualifier}University_EmployeeAchievements]') and name = N'IX_{objectQualifier}University_EmployeeAchievements')
    CREATE NONCLUSTERED INDEX [IX_{objectQualifier}University_EmployeeAchievements]
        ON {databaseOwner}[{objectQualifier}University_EmployeeAchievements] (EmployeeID ASC, AchievementID ASC)
GO

IF NOT EXISTS (select * from sys.indexes where object_id = object_id (N'{databaseOwner}[{objectQualifier}University_Employees]') and name = N'IX_{objectQualifier}University_Employees_Users')
    CREATE NONCLUSTERED INDEX [IX_{objectQualifier}University_Employees_Users]
        ON {databaseOwner}[{objectQualifier}University_Employees] (UserID ASC)
GO
