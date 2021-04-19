-- NOTE: To manually execute this script, you must replace {databaseOwner} and {objectQualifier} with real values:
-- defaults is "dbo." for {databaseOwner} and "" for {objectQualifier}

-- Contingent-related changes

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Contingent]') and name = N'ActualForeignFB')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Contingent]
        ADD [ActualForeignFB] int NULL
GO

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Contingent]') and name = N'ActualForeignRB')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Contingent]
        ADD [ActualForeignRB] int NULL
GO

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Contingent]') and name = N'ActualForeignMB')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Contingent]
        ADD [ActualForeignMB] int NULL
GO

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Contingent]') and name = N'ActualForeignBC')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Contingent]
        ADD [ActualForeignBC] int NULL
GO

-- Science-related changes

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Science]') and name = N'Results')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Science]
        ADD [Results] nvarchar(max)
GO
