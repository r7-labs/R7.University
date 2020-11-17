-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

-- EduProgramProfiles

-- Add accreditaton dates for educational program profiles,
-- leaving education programs AccreditedToDate field as is, as it could be used later.

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EduProgramProfiles]') and name = N'AccreditedToDate')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfiles]
        ADD [Languages] nvarchar(250) NULL,
            [AccreditedToDate] date NULL,
            [CommunityAccreditedToDate] date NULL
END
GO

-- EduLevels

IF EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EduLevels]') and name = N'Type')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduLevels]
        DROP COLUMN [Type]
END
GO

-- EduForms

IF NOT EXISTS (select * from sys.objects where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EduForms]') and type in (N'U'))
BEGIN
    CREATE TABLE {databaseOwner}[{objectQualifier}University_EduForms] (
        [EduFormID] int IDENTITY(1,1) NOT NULL,
        [Title] nvarchar(250) NOT NULL,
        [ShortTitle] nvarchar(64) NULL,
        [IsSystem] bit NOT NULL
        CONSTRAINT [PK_{objectQualifier}University_EduForms] PRIMARY KEY CLUSTERED (EduFormID)
    )

    -- must be same as SystemEduForm enum members
    INSERT INTO {databaseOwner}[{objectQualifier}University_EduForms] (Title, IsSystem)
        VALUES
            (N'FullTime', 1),
            (N'PartTime', 1),
            (N'Extramural', 1)
END
GO

-- EduProgramProfileForms

IF NOT EXISTS (select * from sys.objects where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EduProgramProfileForms]') and type in (N'U'))
BEGIN
    CREATE TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfileForms] (
        [EduProgramProfileFormID] bigint IDENTITY(1,1) NOT NULL,
        [EduProgramProfileID] int NOT NULL,
        [EduFormID] int NOT NULL,
        [TimeToLearn] int NOT NULL,
        [IsAdmissive] bit NOT NULL
        CONSTRAINT [PK_{objectQualifier}University_EduProgramProfileForms] PRIMARY KEY CLUSTERED (EduProgramProfileID, EduFormID)
        CONSTRAINT [FK_{objectQualifier}University_EduProgramProfileForms_EduProgramProfiles] FOREIGN KEY (EduProgramProfileID)
            REFERENCES {databaseOwner}[{objectQualifier}University_EduProgramProfiles](EduProgramProfileID) ON DELETE CASCADE,
        CONSTRAINT [FK_{objectQualifier}University_EduProgramProfileForms_EduForms] FOREIGN KEY (EduFormID)
            REFERENCES {databaseOwner}[{objectQualifier}University_EduForms](EduFormID) ON DELETE CASCADE
    )
END
GO

-- Refresh views

EXECUTE sp_refreshview N'{databaseOwner}[{objectQualifier}vw_University_EduProgramProfiles]';
GO
