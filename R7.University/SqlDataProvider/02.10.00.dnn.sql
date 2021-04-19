-- NOTE: To manually execute this script, you must replace {databaseOwner} and {objectQualifier} with real values:
-- defaults is "dbo." for {databaseOwner} and "" for {objectQualifier}

-- Cleanup obsolete fields for contingent

IF EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Contingent]') and name = N'ActualForeign')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Contingent]
        DROP COLUMN [ActualForeign]
GO

-- Cleanup obsolete fields for science

IF EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Science]') and name = N'Scientists')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Science]
        DROP COLUMN [Scientists]
GO

IF EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Science]') and name = N'Students')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Science]
        DROP COLUMN [Students]
GO

IF EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Science]') and name = N'Monographs')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Science]
        DROP COLUMN [Monographs]
GO

IF EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Science]') and name = N'Articles')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Science]
        DROP COLUMN [Articles]
GO

IF EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Science]') and name = N'ArticlesForeign')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Science]
        DROP COLUMN [ArticlesForeign]
GO

IF EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Science]') and name = N'Patents')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Science]
        DROP COLUMN [Patents]
GO

IF EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Science]') and name = N'PatentsForeign')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Science]
        DROP COLUMN [PatentsForeign]
GO

IF EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Science]') and name = N'Certificates')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Science]
        DROP COLUMN [Certificates]
GO

IF EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Science]') and name = N'CertificatesForeign')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Science]
        DROP COLUMN [CertificatesForeign]
GO

IF EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Science]') and name = N'FinancingByScientist')
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Science]
        DROP COLUMN [FinancingByScientist]
GO
