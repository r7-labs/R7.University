-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Documents]') and name = N'EduProgramID')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Documents]
        ADD [EduProgramID] int NULL,
            [EduProgramProfileID] int NULL
        CONSTRAINT [FK_{objectQualifier}University_Documents_EduPrograms] FOREIGN KEY (EduProgramID)
            REFERENCES {databaseOwner}[{objectQualifier}University_EduPrograms] (EduProgramID) ON DELETE CASCADE,
        CONSTRAINT [FK_{objectQualifier}University_Documents_EduProgramProfiles] FOREIGN KEY (EduProgramProfileID)
            REFERENCES {databaseOwner}[{objectQualifier}University_EduProgramProfiles] (EduProgramProfileID) ON DELETE CASCADE
END
GO

IF EXISTS (select * from sys.indexes where object_id = object_id (N'{databaseOwner}[{objectQualifier}University_Documents]') and name = N'IX_{objectQualifier}University_Documents_Items')
    DROP INDEX [IX_{objectQualifier}University_Documents_Items] ON {databaseOwner}[{objectQualifier}University_Documents]
GO

IF EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Documents]') and name = N'ItemID')
BEGIN
    UPDATE {databaseOwner}[{objectQualifier}University_Documents] SET EduProgramID =
            CAST (REPLACE (ItemID, N'EduProgramID=', N'') AS int)
        WHERE ItemID LIKE N'EduProgramID=%'

    UPDATE {databaseOwner}[{objectQualifier}University_Documents] SET EduProgramProfileID =
            CAST (REPLACE (ItemID, N'EduProgramProfileID=', N'') AS int)
        WHERE ItemID LIKE N'EduProgramProfileID=%'

    ALTER TABLE {databaseOwner}[{objectQualifier}University_Documents]
        DROP COLUMN [ItemID]
END
GO

-- Re-add index

IF NOT EXISTS (select * from sys.indexes where object_id = object_id (N'{databaseOwner}[{objectQualifier}University_Documents]') and name = N'IX_{objectQualifier}University_Documents_Items')
    CREATE NONCLUSTERED INDEX [IX_{objectQualifier}University_Documents_Items]
        ON {databaseOwner}[{objectQualifier}University_Documents] (EduProgramProfileID ASC, EduProgramID ASC)
GO

-- Drop views

IF EXISTS (select * from sys.objects where object_id = object_id(N'{databaseOwner}[{objectQualifier}vw_University_EmployeeDisciplines]') and type in (N'V'))
    DROP VIEW {databaseOwner}[{objectQualifier}vw_University_EmployeeDisciplines]
GO

-- Drop stored procedures

IF EXISTS (select * from sys.objects where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_GetHeadEmployee]') and type in (N'P'))
    DROP PROCEDURE {databaseOwner}[{objectQualifier}University_GetHeadEmployee]
GO
