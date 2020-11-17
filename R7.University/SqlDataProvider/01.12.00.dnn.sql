-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Divisions]') and name = N'Address')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Divisions]
        ADD [Address] nvarchar (250) NULL
END
GO

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Divisions]') and name = N'IsInformal')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Divisions]
        ADD [IsInformal] bit NOT NULL DEFAULT (0)
END
GO
