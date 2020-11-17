-- NOTE: To manually execute this script, you must replace {databaseOwner} and {objectQualifier} with real values:
-- defaults is "dbo." for {databaseOwner} and "" for {objectQualifier} 

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Contingent]') and name = N'ActualForeign')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Contingent]
        ADD [ActualForeign] int NULL
END
GO

IF EXISTS (select * from sys.indexes where name = N'UN_{objectQualifier}University_OccupiedPositions')
BEGIN
	ALTER TABLE {databaseOwner}[{objectQualifier}University_OccupiedPositions]
		DROP CONSTRAINT UN_{objectQualifier}University_OccupiedPositions
END
GO