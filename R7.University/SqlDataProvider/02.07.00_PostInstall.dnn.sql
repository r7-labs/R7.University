-- NOTE: To manually execute this script, you must replace {databaseOwner} and {objectQualifier} with real values:
-- defaults is "dbo." for {databaseOwner} and "" for {objectQualifier}

-- Assume all previously added edu. standards are state ones GH-401
UPDATE {databaseOwner}[{objectQualifier}University_DocumentTypes] SET [Type] = N'StateEduStandard' WHERE [Type] = N'EduStandard'
GO
