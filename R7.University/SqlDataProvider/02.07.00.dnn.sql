﻿-- NOTE: To manually execute this script, you must replace {databaseOwner} and {objectQualifier} with real values:
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
    BEGIN
        ALTER TABLE {databaseOwner}[{objectQualifier}University_EmployeeAchievements]
        ADD [Hours] int NULL
    END
GO