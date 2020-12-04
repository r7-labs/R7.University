-- NOTE: To manually execute this script, you must replace {databaseOwner} and {objectQualifier} with real values:
-- defaults is "dbo." for {databaseOwner} and "" for {objectQualifier}

-- Assume all previously added edu. standards are state ones GH-401

UPDATE {databaseOwner}[{objectQualifier}University_DocumentTypes] SET [Type] = N'StateEduStandard' WHERE [Type] = N'EduStandard'
GO

-- Set proper EduLevelID based on TitleSuffix, clear TitleSuffix GH-398

UPDATE {databaseOwner}[{objectQualifier}University_EmployeeAchievements]
    SET EduLevelID = (select EduLevelID from {databaseOwner}[{objectQualifier}University_EduLevels] where ShortTitle = N'Бакалавриат'),
        TitleSuffix = N''
    WHERE TitleSuffix = N'(%бакалавр%)'
GO

UPDATE {databaseOwner}[{objectQualifier}University_EmployeeAchievements]
    SET EduLevelID = (select EduLevelID from {databaseOwner}[{objectQualifier}University_EduLevels] where ShortTitle = N'Специалитет СПО'),
        TitleSuffix = N''
    WHERE TitleSuffix LIKE N'(%средне%)'
GO

UPDATE {databaseOwner}[{objectQualifier}University_EmployeeAchievements]
    SET EduLevelID = (select EduLevelID from {databaseOwner}[{objectQualifier}University_EduLevels] where ShortTitle = N'Специалитет ВО'),
        TitleSuffix = N''
    WHERE TitleSuffix = N'(%специал%)'
GO

UPDATE {databaseOwner}[{objectQualifier}University_EmployeeAchievements]
    SET EduLevelID = (select EduLevelID from {databaseOwner}[{objectQualifier}University_EduLevels] where ShortTitle = N'Магистратура'),
        TitleSuffix = N''
    WHERE TitleSuffix = N'(%магистр%)'
GO

UPDATE {databaseOwner}[{objectQualifier}University_EmployeeAchievements]
    SET EduLevelID = (select EduLevelID from {databaseOwner}[{objectQualifier}University_EduLevels] where ShortTitle = N'Ординатура'),
        TitleSuffix = N''
    WHERE TitleSuffix = N'(%ординат%)'
GO

UPDATE {databaseOwner}[{objectQualifier}University_EmployeeAchievements]
    SET EduLevelID = (select EduLevelID from {databaseOwner}[{objectQualifier}University_EduLevels] where ShortTitle = N'Аспирантура'),
        TitleSuffix = N''
    WHERE TitleSuffix = N'(%кадр%)'
GO
