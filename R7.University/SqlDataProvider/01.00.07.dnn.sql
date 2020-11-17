-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

-- Set all Employee modules cache time to 0
UPDATE {databaseOwner}[{objectQualifier}TabModules] SET CacheTime = 0
WHERE ModuleID IN (
	SELECT ModuleID
		FROM {databaseOwner}[{objectQualifier}Modules] AS M
		INNER JOIN {databaseOwner}[{objectQualifier}ModuleDefinitions] AS MD
			ON M.ModuleDefID = MD.ModuleDefID
		INNER JOIN {databaseOwner}[{objectQualifier}DesktopModules] AS DM
			ON MD.DesktopModuleID = DM.DesktopModuleID
		WHERE DM.ModuleName = N'R7.University.Employee'
)
GO

-- Set all EmployeeList modules cache time to 0
UPDATE {databaseOwner}[{objectQualifier}TabModules] SET CacheTime = 0
WHERE ModuleID IN (
	SELECT ModuleID
		FROM {databaseOwner}[{objectQualifier}Modules] AS M
		INNER JOIN {databaseOwner}[{objectQualifier}ModuleDefinitions] AS MD
			ON M.ModuleDefID = MD.ModuleDefID
		INNER JOIN {databaseOwner}[{objectQualifier}DesktopModules] AS DM
			ON MD.DesktopModuleID = DM.DesktopModuleID
		WHERE DM.ModuleName = N'R7.University.EmployeeList'
)
GO
