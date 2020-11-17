﻿-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Documents]') and name = N'CreatedOnDate')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Documents]
        ADD CreatedOnDate datetime NOT NULL DEFAULT (GETDATE ()),
        LastModifiedOnDate datetime NOT NULL DEFAULT (GETDATE ()),
        CreatedByUserID int NOT NULL DEFAULT (-1),
        LastModifiedByUserID int NOT NULL DEFAULT (-1)
END
GO

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Divisions]') and name = N'IsGoverning')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Divisions]
        ADD IsGoverning bit NOT NULL DEFAULT (0)
END
GO

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EduProgramProfiles]') and name = N'IsAdopted')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfiles]
        ADD IsAdopted bit NOT NULL DEFAULT (0)
END
GO

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EduProgramProfiles]') and name = N'ELearning')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfiles]
        ADD ELearning bit NOT NULL DEFAULT (0)
END
GO

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EduProgramProfiles]') and name = N'DistanceEducation')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfiles]
        ADD DistanceEducation bit NOT NULL DEFAULT (0)
END
GO

IF EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Divisions]') and name = N'IsVirtual')
BEGIN
    EXECUTE sp_rename N'{objectQualifier}University_Divisions.IsVirtual', N'IsSingleEntity', N'COLUMN';
END
GO

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EduForms]') and name = N'SortIndex')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduForms]
        ADD SortIndex int NOT NULL DEFAULT (0)

    EXECUTE sp_executesql N'UPDATE {databaseOwner}[{objectQualifier}University_EduForms] SET SortIndex = 10 WHERE Title = N''FullTime'''
    EXECUTE sp_executesql N'UPDATE {databaseOwner}[{objectQualifier}University_EduForms] SET SortIndex = 20 WHERE Title = N''Extramural'''
    EXECUTE sp_executesql N'UPDATE {databaseOwner}[{objectQualifier}University_EduForms] SET SortIndex = 30 WHERE Title = N''PartTime'''
END
GO

IF NOT EXISTS (select * from sys.indexes where name = N'UN_{objectQualifier}University_EduProgramProfileForms')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfileForms]
        ALTER COLUMN EduProgramProfileFormID int NOT NULL

    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfileForms]
        DROP CONSTRAINT [PK_{objectQualifier}University_EduProgramProfileForms]

    DROP INDEX [IX_{objectQualifier}University_EduProgramProfileForms] ON {databaseOwner}[{objectQualifier}University_EduProgramProfileForms]

    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfileForms]
        ADD CONSTRAINT [PK_{objectQualifier}University_EduProgramProfileForms] PRIMARY KEY (EduProgramProfileFormID)

    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfileForms]
        ADD CONSTRAINT [UN_{objectQualifier}University_EduProgramProfileForms] UNIQUE (EduProgramProfileID, EduFormID)
END
GO

IF NOT EXISTS (select * from sys.tables where name = N'{objectQualifier}University_Years')
BEGIN
    CREATE TABLE {databaseOwner}[{objectQualifier}University_Years] (
        YearID int IDENTITY (1,1) NOT NULL,
        [Year] int NOT NULL,
        AdmissionIsOpen bit NOT NULL
        CONSTRAINT [PK_{objectQualifier}University_Years] PRIMARY KEY (YearID)
    )
END
GO

IF NOT EXISTS (select * from sys.tables where name = N'{objectQualifier}University_EduProgramProfileFormYears')
BEGIN
    CREATE TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfileFormYears] (
        EduProgramProfileFormYearID int IDENTITY (1,1) NOT NULL,
        EduProgramProfileID int NOT NULL,
        EduFormID int NOT NULL,
        YearID int NULL,
        StartDate datetime NULL,
        EndDate datetime NULL
        CONSTRAINT [PK_{objectQualifier}University_EduProgramProfileFormYears] PRIMARY KEY (EduProgramProfileFormYearID)
        CONSTRAINT [FK_{objectQualifier}University_EduProgramProfileFormYears_EduProgramProfiles]
            FOREIGN KEY (EduProgramProfileID) REFERENCES {databaseOwner}[{objectQualifier}University_EduProgramProfiles] (EduProgramProfileID)
                ON DELETE CASCADE,
        CONSTRAINT [FK_{objectQualifier}University_EduProgramProfileFormYears_EduForms]
            FOREIGN KEY (EduFormID) REFERENCES {databaseOwner}[{objectQualifier}University_EduForms] (EduFormID)
                ON DELETE NO ACTION,
        CONSTRAINT [FK_{objectQualifier}University_EduProgramProfileFormYears_Years]
            FOREIGN KEY (YearID) REFERENCES {databaseOwner}[{objectQualifier}University_Years] (YearID)
                ON DELETE NO ACTION,
        CONSTRAINT [UN_{objectQualifier}EduProgramProfileFormYears_Years] UNIQUE (EduProgramProfileID, EduFormID, YearID)
    )

    EXECUTE sp_executesql N'INSERT INTO {databaseOwner}[{objectQualifier}University_EduProgramProfileFormYears] (YearID, EduFormID, EduProgramProfileID)
		SELECT NULL AS YearID, EduFormID, EduProgramProfileID
		FROM {databaseOwner}[{objectQualifier}University_EduProgramProfileForms]'
END
GO

IF NOT EXISTS (select * from sys.tables where name = N'{objectQualifier}University_EduVolume')
BEGIN
    CREATE TABLE {databaseOwner}[{objectQualifier}University_EduVolume] (
        EduVolumeID int NOT NULL,
        TimeToLearnHours int NOT NULL,
        TimeToLearnMonths int NOT NULL,
        Year1Cu int NULL,
        Year2Cu int NULL,
        Year3Cu int NULL,
        Year4Cu int NULL,
        Year5Cu int NULL,
        Year6Cu int NULL,
        PracticeType1Cu int NULL,
        PracticeType2Cu int NULL,
        PracticeType3Cu int NULL
        CONSTRAINT [PK_{objectQualifier}University_EduVolume] PRIMARY KEY (EduVolumeID)
        CONSTRAINT [FK_{objectQualifier}University_EduVolume_EduProgramProfileFormYears]
            FOREIGN KEY (EduVolumeID) REFERENCES {databaseOwner}[{objectQualifier}University_EduProgramProfileFormYears] (EduProgramProfileFormYearID)
                ON DELETE CASCADE
    )

    EXECUTE sp_executesql N'INSERT INTO {databaseOwner}[{objectQualifier}University_EduVolume] (EduVolumeID, TimeToLearnMonths, TimeToLearnHours)
		SELECT EduProgramProfileFormYearID AS EduVolumeID, EPPF.TimeToLearn AS TimeToLearnMonths, EPPF.TimeToLearnHours
		FROM {databaseOwner}[{objectQualifier}University_EduProgramProfileFormYears] AS EPPFY
		INNER JOIN {databaseOwner}[{objectQualifier}University_EduProgramProfileForms] AS EPPF
			ON EPPFY.EduProgramProfileID = EPPF.EduProgramProfileID AND EPPFY.EduFormID = EPPF.EduFormID'
END
GO

IF NOT EXISTS (select * from sys.tables where name = N'{objectQualifier}University_Contingent')
BEGIN
    CREATE TABLE {databaseOwner}[{objectQualifier}University_Contingent] (
        ContingentID int NOT NULL,

        AvgAdmScore decimal (18,3) NULL,
        AdmittedFB int NULL,
        AdmittedRB int NULL,
        AdmittedMB int NULL,
        AdmittedBC int NULL,

        ActualFB int NULL,
        ActualRB int NULL,
        ActualMB int NULL,
        ActualBC int NULL,

        VacantFB int NULL,
        VacantRB int NULL,
        VacantMB int NULL,
        VacantBC int NULL,

        MovedIn int NULL,
        MovedOut int NULL,
        Restored int NULL,
        Expelled int NULL

        CONSTRAINT [PK_{objectQualifier}University_Contingent] PRIMARY KEY (ContingentID)
        CONSTRAINT [FK_{objectQualifier}University_Contingent_EduProgramProfileFormYears]
            FOREIGN KEY (ContingentID) REFERENCES {databaseOwner}[{objectQualifier}University_EduProgramProfileFormYears] (EduProgramProfileFormYearID)
                ON DELETE CASCADE
    )
END
GO

IF NOT EXISTS (select * from sys.tables where name = N'{objectQualifier}University_Science')
BEGIN
    CREATE TABLE {databaseOwner}[{objectQualifier}University_Science] (
        ScienceID int NOT NULL,
        Directions nvarchar (max),
        Base nvarchar (max),
        Scientists int,
        Students int,
        Monographs int,
        Articles int,
        ArticlesForeign int,
        Patents int,
        PatentsForeign int,
        Certificates int,
        CertificatesForeign int,
        FinancingByScientist decimal (18,3)
        CONSTRAINT [PK_{objectQualifier}University_Science] PRIMARY KEY (ScienceID)
        CONSTRAINT [FK_{objectQualifier}University_Science_EduPrograms]
            FOREIGN KEY (ScienceID) REFERENCES {databaseOwner}[{objectQualifier}University_EduPrograms] (EduProgramID)
                ON DELETE CASCADE
    )
END
GO

