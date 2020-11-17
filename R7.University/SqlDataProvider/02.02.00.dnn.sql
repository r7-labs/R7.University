-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}University_GetEmployees_ByDivisionID]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE {databaseOwner}[{objectQualifier}University_GetEmployees_ByDivisionID]
GO

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}University_GetEmployees_ByDivisionID_Recursive]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE {databaseOwner}[{objectQualifier}University_GetEmployees_ByDivisionID_Recursive]
GO

-- IF NOT EXISTS (select * from sys.indexes where name = N'UN_{objectQualifier}University_Divisions_DivisionTermID')
--    ALTER TABLE {databaseOwner}[{objectQualifier}University_Divisions] ADD CONSTRAINT
--        [UN_{objectQualifier}University_Divisions_DivisionTermID] UNIQUE ([DivisionTermID])

UPDATE {databaseOwner}[{objectQualifier}TabModuleSettings]
	SET SettingValue = N'0' WHERE SettingName = N'EmployeeList_SortType' AND SettingValue = N'1'

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Employees]') and name = N'ScienceIndexAuthorID')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Employees]
        ADD [ScienceIndexAuthorID] int NULL
END
GO

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Employees]') and name = N'AltPhotoFileID')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_Employees]
        ADD [AltPhotoFileID] int NULL
END
GO

IF EXISTS (select * from {databaseOwner}[{objectQualifier}University_DocumentTypes] where Type = N'Contingent')
BEGIN
	DELETE FROM {databaseOwner}[{objectQualifier}University_Documents]
		WHERE DocumentTypeID = (select DocumentTypeID from {databaseOwner}[{objectQualifier}University_DocumentTypes] where Type = N'Contingent')

	DELETE FROM {databaseOwner}[{objectQualifier}University_DocumentTypes] WHERE Type = N'Contingent'
END
GO

IF EXISTS (select * from {databaseOwner}[{objectQualifier}University_DocumentTypes] where Type = N'ContingentMovement')
BEGIN
	DELETE FROM {databaseOwner}[{objectQualifier}University_Documents]
		WHERE DocumentTypeID = (select DocumentTypeID from {databaseOwner}[{objectQualifier}University_DocumentTypes] where Type = N'ContingentMovement')

	DELETE FROM {databaseOwner}[{objectQualifier}University_DocumentTypes] WHERE Type = N'ContingentMovement'
END
GO
