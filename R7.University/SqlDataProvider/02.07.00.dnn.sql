-- NOTE: To manually execute this script, you must replace {databaseOwner} and {objectQualifier} with real values:
-- defaults is "dbo." for {databaseOwner} and "" for {objectQualifier}

IF NOT EXISTS (select * from {databaseOwner}[{objectQualifier}University_AchievementTypes] where [Type] = N'ShortTermTraining')
    INSERT INTO {databaseOwner}[{objectQualifier}University_AchievementTypes] (Type, IsSystem) VALUES
        (N'ShortTermTraining', 1)
GO

IF NOT EXISTS (select * from {databaseOwner}[{objectQualifier}University_AchievementTypes] where [Type] = N'Internship')
    INSERT INTO {databaseOwner}[{objectQualifier}University_AchievementTypes] (Type, IsSystem) VALUES
        (N'Internship', 1)
GO

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EmployeeAchievements]') and name = N'Hours')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_EmployeeAchievements]
        ADD [Hours] int NULL
GO

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EmployeeAchievements]') and name = N'EduLevelID')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_EmployeeAchievements]
        ADD [EduLevelID] int NULL
GO

IF NOT EXISTS (select * from sys.foreign_keys where name = N'FK_{objectQualifier}University_EmployeeAchievements_EduLevels')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_EmployeeAchievements]
        ADD CONSTRAINT [FK_{objectQualifier}University_EmployeeAchievements_EduLevels] FOREIGN KEY (EduLevelID)
            REFERENCES {databaseOwner}[{objectQualifier}University_EduLevels] (EduLevelID) ON DELETE SET NULL
GO
