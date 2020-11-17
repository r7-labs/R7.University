﻿-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

-- EduProgramDivisions

IF NOT EXISTS (select * from sys.tables where name = N'{objectQualifier}University_EduProgramDivisions')
BEGIN
    CREATE TABLE {databaseOwner}[{objectQualifier}University_EduProgramDivisions] (
        EduProgramDivisionID bigint IDENTITY (1,1) NOT NULL,
        DivisionID int NOT NULL,
        EduProgramID int NULL,
        EduProgramProfileID int NULL,
        DivisionRole nvarchar (255) NULL
        CONSTRAINT [PK_{objectQualifier}University_EduProgramDivisions] PRIMARY KEY (EduProgramDivisionID)
        CONSTRAINT [FK_{objectQualifier}University_EduProgramDivisions_Divisions] FOREIGN KEY (DivisionID)
            REFERENCES {databaseOwner}[{objectQualifier}University_Divisions] (DivisionID) ON DELETE CASCADE,
        CONSTRAINT [FK_{objectQualifier}University_EduProgramDivisions_EduPrograms] FOREIGN KEY (EduProgramID)
            REFERENCES {databaseOwner}[{objectQualifier}University_EduPrograms] (EduProgramID) ON DELETE SET NULL,
        CONSTRAINT [FK_{objectQualifier}University_EduProgramDivisions_EduProgramProfiles] FOREIGN KEY (EduProgramProfileID)
            REFERENCES {databaseOwner}[{objectQualifier}University_EduProgramProfiles] (EduProgramProfileID) ON DELETE SET NULL,
        CONSTRAINT [UN_{objectQualifier}University_EduProgramDivisions] UNIQUE (DivisionID, EduProgramID, EduProgramProfileID)
    )

    INSERT INTO {databaseOwner}[{objectQualifier}University_EduProgramDivisions] (DivisionID, EduProgramID)
        SELECT EP.DivisionID, EP.EduProgramID FROM {databaseOwner}[{objectQualifier}University_EduPrograms] EP
            WHERE EP.DivisionID IS NOT NULL

    INSERT INTO {databaseOwner}[{objectQualifier}University_EduProgramDivisions] (DivisionID, EduProgramProfileID)
        SELECT EPP.DivisionID, EPP.EduProgramProfileID FROM {databaseOwner}[{objectQualifier}University_EduProgramProfiles] EPP
            WHERE EPP.DivisionID IS NOT NULL
END
GO

IF NOT EXISTS (select * from sys.indexes where name = N'IX_{objectQualifier}University_EduProgramDivisions')
BEGIN
    CREATE NONCLUSTERED INDEX [IX_{objectQualifier}University_EduProgramDivisions]
        ON {databaseOwner}[{objectQualifier}University_EduProgramDivisions] (DivisionID, EduProgramID, EduProgramProfileID)
END
GO

IF EXISTS (select * from sys.foreign_keys where name = N'FK_{objectQualifier}University_EduPrograms_Divisions')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduPrograms]
        DROP CONSTRAINT [FK_{objectQualifier}University_EduPrograms_Divisions]
END
GO

IF EXISTS (select * from sys.foreign_keys where name = N'FK_{objectQualifier}University_EduProgramProfiles_Divisions')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfiles]
        DROP CONSTRAINT [FK_{objectQualifier}University_EduProgramProfiles_Divisions]
END
GO

IF EXISTS (select * from sys.columns where object_id = object_id (N'{databaseOwner}[{objectQualifier}University_EduPrograms]') and name = N'DivisionID')
BEGIN
    IF EXISTS (select * from sys.indexes where name = N'IX_{objectQualifier}University_EduPrograms_Divisions')
        DROP INDEX [IX_{objectQualifier}University_EduPrograms_Divisions] ON {databaseOwner}[{objectQualifier}University_EduPrograms]

    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduPrograms]
        DROP COLUMN DivisionID
END
GO

IF EXISTS (select * from sys.columns where object_id = object_id (N'{databaseOwner}[{objectQualifier}University_EduProgramProfiles]') and name = N'DivisionID')
BEGIN
    IF EXISTS (select * from sys.indexes where name = N'IX_{objectQualifier}University_EduProgramProfiles_Divisions')
        DROP INDEX [IX_{objectQualifier}University_EduProgramProfiles_Divisions] ON {databaseOwner}[{objectQualifier}University_EduProgramProfiles]

    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfiles]
        DROP COLUMN DivisionID
END
GO

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EduProgramProfileForms]') and name = N'TimeToLearnHours')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfileForms]
        ADD TimeToLearnHours int NOT NULL DEFAULT (0)

    EXECUTE sp_executesql N'UPDATE {databaseOwner}[{objectQualifier}University_EduProgramProfileForms]
        SET TimeToLearnHours = TimeToLearn, TimeToLearn = 0
        WHERE TimeToLearnUnit = N''h'''

    EXECUTE {databaseOwner}[{objectQualifier}r7_DnnExtensions_DropDefaultConstraint] @tableName = N'University_EduProgramProfileForms', @columnName = N'TimeToLearnUnit'

    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfileForms]
        DROP COLUMN TimeToLearnUnit
END
GO
