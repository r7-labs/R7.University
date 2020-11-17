-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

DELETE FROM {databaseOwner}[{objectQualifier}PackageDependencies]
	WHERE PackageID = (select PackageID from {databaseOwner}[{objectQualifier}Packages] where Name = N'R7.University')
	AND (PackageName = N'R7.DotNetNuke.Extensions' OR PackageName = N'DotNetNuke.R7')
GO

IF EXISTS (select * from {databaseOwner}[{objectQualifier}University_DocumentTypes] where Type = N'ScienceInfo')
BEGIN
	DELETE FROM {databaseOwner}[{objectQualifier}University_Documents]
		WHERE DocumentTypeID = (select DocumentTypeID from {databaseOwner}[{objectQualifier}University_DocumentTypes] where Type = N'ScienceInfo')

	DELETE FROM {databaseOwner}[{objectQualifier}University_DocumentTypes] WHERE Type = N'ScienceInfo'
END
GO
