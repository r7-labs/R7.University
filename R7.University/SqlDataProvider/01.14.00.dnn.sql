﻿-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_DocumentTypes]') and name = N'FilenameFormat')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_DocumentTypes]
        ADD [FilenameFormat] nvarchar (255) NULL
GO

IF NOT EXISTS (select * from {databaseOwner}[{objectQualifier}University_DocumentTypes] where [FilenameFormat] is not null)
BEGIN
    UPDATE {databaseOwner}[{objectQualifier}University_DocumentTypes]
        SET [FilenameFormat] = N'prikaz_priem_[a-z0-9_]+_\d{8}\.pdf' WHERE [Type] = N'OrderEnrollment'

    UPDATE {databaseOwner}[{objectQualifier}University_DocumentTypes]
        SET [FilenameFormat] = N'prikaz_otch_[a-z0-9_]+_\d{8}\.pdf' WHERE [Type] = N'OrderExpulsion'

    UPDATE {databaseOwner}[{objectQualifier}University_DocumentTypes]
        SET [FilenameFormat] = N'prikaz_vosst_[a-z0-9_]+_\d{8}\.pdf' WHERE [Type] = N'OrderRestoration'

    UPDATE {databaseOwner}[{objectQualifier}University_DocumentTypes]
        SET [FilenameFormat] = N'prikaz_perevod_[a-z0-9_]+_\d{8}\.pdf' WHERE [Type] = N'OrderTransfer'

    UPDATE {databaseOwner}[{objectQualifier}University_DocumentTypes]
        SET [FilenameFormat] = N'prikaz_academ_[a-z0-9_]+_\d{8}\.pdf' WHERE [Type] = N'OrderAcademicLeave'

    UPDATE {databaseOwner}[{objectQualifier}University_DocumentTypes]
        SET [FilenameFormat] = N'standart_[a-z0-9_]+\.pdf' WHERE [Type] = N'EduStandard'

    UPDATE {databaseOwner}[{objectQualifier}University_DocumentTypes]
        SET [FilenameFormat] = N'prof_standart_[a-z0-9_]+\.pdf' WHERE [Type] = N'ProfStandard'

    UPDATE {databaseOwner}[{objectQualifier}University_DocumentTypes]
        SET [FilenameFormat] = N'oop_[a-z0-9_]+_\d{8}\.pdf' WHERE [Type] = N'EduProgram'

    UPDATE {databaseOwner}[{objectQualifier}University_DocumentTypes]
        SET [FilenameFormat] = N'graf_[a-z0-9_]+_\d{8}\.pdf' WHERE [Type] = N'EduSchedule'

    UPDATE {databaseOwner}[{objectQualifier}University_DocumentTypes]
        SET [FilenameFormat] = N'ucheb_plan_[a-z0-9_]+_\d{8}\.pdf' WHERE [Type] = N'EduPlan'

    UPDATE {databaseOwner}[{objectQualifier}University_DocumentTypes]
        SET [FilenameFormat] = N'metod_[a-z0-9_]+_\d{8}\.pdf' WHERE [Type] = N'EduMaterial'

    UPDATE {databaseOwner}[{objectQualifier}University_DocumentTypes]
        SET [FilenameFormat] = N'annot_[a-z0-9_]+_\d{8}\.pdf' WHERE [Type] = N'WorkProgramAnnotation'

    UPDATE {databaseOwner}[{objectQualifier}University_DocumentTypes]
        SET [FilenameFormat] = N'rp_praktika_[a-z0-9_]+_\d{8}\.pdf' WHERE [Type] = N'WorkProgramOfPractice'

    UPDATE {databaseOwner}[{objectQualifier}University_DocumentTypes]
        SET [FilenameFormat] = N'chislen_[a-z0-9_]+_\d{8}\.pdf' WHERE [Type] = N'Contingent'

    UPDATE {databaseOwner}[{objectQualifier}University_DocumentTypes]
        SET [FilenameFormat] = N'perevod_[a-z0-9_]+_\d{8}\.pdf' WHERE [Type] = N'ContingentMovement'

    UPDATE {databaseOwner}[{objectQualifier}University_DocumentTypes]
        SET [FilenameFormat] = N'nir_[a-z0-9_]+_\d{8}\.(pdf|odt|docx?)' WHERE [Type] = N'ScienceInfo'
END
GO

IF NOT EXISTS (select * from sys.objects where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_AchievementTypes]') and type in (N'U'))
    CREATE TABLE {databaseOwner}[{objectQualifier}University_AchievementTypes] (
        AchievementTypeID int IDENTITY (1,1) NOT NULL,
        Type nvarchar(64) NOT NULL,
        Description nvarchar(255) NULL,
        IsSystem bit NOT NULL,
        TmpAchievementType nchar (1) NOT NULL
        CONSTRAINT [PK_{objectQualifier}University_AchievementTypes] PRIMARY KEY (AchievementTypeID)
    )
GO

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Achievements]') and name = N'AchievementTypeID')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Achievements]
        ADD [AchievementTypeID] int NULL
GO

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EmployeeAchievements]') and name = N'AchievementTypeID')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_EmployeeAchievements]
        ADD [AchievementTypeID] int NULL
GO

IF (select count (*) from {databaseOwner}[{objectQualifier}University_AchievementTypes]) = 0
BEGIN
    EXECUTE sp_executesql N'INSERT INTO {databaseOwner}[{objectQualifier}University_AchievementTypes] (Type, IsSystem, TmpAchievementType) VALUES
        (N''Education'', 1, N''E''),
        (N''ProfTraining'', 1, N''+''),
        (N''Training'', 1, N''T''),
        (N''ProfRetraining'', 1, N''+''),
        (N''Work'', 1, N''W''),
        (N''AcademicTitle'', 1, N''R''),
        (N''AcademicDegree'', 1, N''D''),
        (N''Title'', 1, N''+''),
        (N''Authorship'', 1, N''+'')'

    EXECUTE sp_executesql N'UPDATE {databaseOwner}[{objectQualifier}University_Achievements]
        SET AchievementTypeID = (select [AchievementTypeID]
            from {databaseOwner}[{objectQualifier}University_AchievementTypes] AS AT
                where A.AchievementType = AT.TmpAchievementType)
        FROM {databaseOwner}[{objectQualifier}University_Achievements] AS A'

    EXECUTE sp_executesql N'UPDATE {databaseOwner}[{objectQualifier}University_EmployeeAchievements]
        SET AchievementTypeID = (select [AchievementTypeID]
            from {databaseOwner}[{objectQualifier}University_AchievementTypes] AS AT
                where EA.AchievementType = AT.TmpAchievementType)
        FROM {databaseOwner}[{objectQualifier}University_EmployeeAchievements] AS EA'
END
GO

IF NOT EXISTS (select * from sys.foreign_keys where name = N'FK_{objectQualifier}University_Achievements_AchievementTypes')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Achievements]
        ADD CONSTRAINT [FK_{objectQualifier}University_Achievements_AchievementTypes] FOREIGN KEY (AchievementTypeID)
            REFERENCES {databaseOwner}[{objectQualifier}University_AchievementTypes] (AchievementTypeID) ON DELETE NO ACTION
GO

IF NOT EXISTS (select * from sys.foreign_keys where name = N'FK_{objectQualifier}University_EmployeeAchievements_AchievementTypes')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_EmployeeAchievements]
        ADD CONSTRAINT [FK_{objectQualifier}University_EmployeeAchievements_AchievementTypes] FOREIGN KEY (AchievementTypeID)
            REFERENCES {databaseOwner}[{objectQualifier}University_AchievementTypes] (AchievementTypeID) ON DELETE NO ACTION
GO

IF EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_AchievementTypes]') and name = N'TmpAchievementType')
    EXECUTE sp_executesql N'ALTER TABLE {databaseOwner}[{objectQualifier}University_AchievementTypes]
        DROP COLUMN [TmpAchievementType]'
GO

IF EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Achievements]') and name = N'AchievementType')
    EXECUTE sp_executesql N'ALTER TABLE {databaseOwner}[{objectQualifier}University_Achievements]
        DROP COLUMN [AchievementType]'
GO

IF EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EmployeeAchievements]') and name = N'AchievementType')
    EXECUTE sp_executesql N'ALTER TABLE {databaseOwner}[{objectQualifier}University_EmployeeAchievements]
        DROP COLUMN [AchievementType]'
GO
