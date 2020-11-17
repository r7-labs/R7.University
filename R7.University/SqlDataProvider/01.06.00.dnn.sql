-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

-- Divisions

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EduLevels]') and name = N'SortIndex')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduLevels]
        ADD [SortIndex] int NOT NULL DEFAULT ((0))
END
GO
