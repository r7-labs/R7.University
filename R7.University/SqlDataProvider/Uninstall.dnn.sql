-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

-- Drop foreign key constraints

ALTER TABLE {databaseOwner}[{objectQualifier}University_Divisions]
	DROP CONSTRAINT [FK_{objectQualifier}University_Divisions_Taxonomy_Terms]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_Divisions]
    DROP CONSTRAINT [FK_{objectQualifier}University_Divisions_Positions]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_Employees]
    DROP CONSTRAINT [FK_{objectQualifier}University_Employees_Users]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_Employees]
    DROP CONSTRAINT [FK_{objectQualifier}University_Employees_PhotoFile]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_OccupiedPositions]
	DROP CONSTRAINT [FK_{objectQualifier}University_OccupiedPositions_University_Divisions]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_OccupiedPositions]
	DROP CONSTRAINT [FK_{objectQualifier}University_OccupiedPositions_University_Employees]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_OccupiedPositions]
	DROP CONSTRAINT [FK_{objectQualifier}University_OccupiedPositions_University_Positions]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_EmployeeAchievements]
	DROP CONSTRAINT [FK_{objectQualifier}University_EmployeeAchievements_University_Achievements]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_EmployeeAchievements]
    DROP CONSTRAINT [FK_{objectQualifier}University_EmployeeAchievements_AchievementTypes]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_Achievements]
    DROP CONSTRAINT [FK_{objectQualifier}University_Achievements_AchievementTypes]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_EduLevels]
    DROP CONSTRAINT [FK_{objectQualifier}University_EduLevels_ParentEduLevels]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_EduPrograms]
	DROP CONSTRAINT [FK_{objectQualifier}University_EduPrograms_EduLevels]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfiles]
    DROP CONSTRAINT [FK_{objectQualifier}University_EduProgramProfiles_EduLevels]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfiles]
    DROP CONSTRAINT [FK_{objectQualifier}University_EduProgramProfiles_EduPrograms]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfileForms]
    DROP CONSTRAINT [FK_{objectQualifier}University_EduProgramProfileForms_EduForms]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfileForms]
    DROP CONSTRAINT [FK_{objectQualifier}University_EduProgramProfileForms_EduProgramProfiles]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_EmployeeDisciplines]
	DROP CONSTRAINT [FK_{objectQualifier}University_EmployeeDisciplines_Employees]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_EmployeeDisciplines]
	DROP CONSTRAINT [FK_{objectQualifier}University_EmployeeDisciplines_EduProgramProfiles]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramDivisions]
    DROP CONSTRAINT [FK_{objectQualifier}University_EduProgramDivisions_Divisions]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramDivisions]
    DROP CONSTRAINT [FK_{objectQualifier}University_EduProgramDivisions_EduPrograms]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramDivisions]
    DROP CONSTRAINT [FK_{objectQualifier}University_EduProgramDivisions_EduProgramProfiles]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfileFormYears]
    DROP CONSTRAINT [FK_{objectQualifier}University_EduProgramProfileFormYears_EduProgramProfileForms]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfileFormYears]
    DROP CONSTRAINT [FK_{objectQualifier}University_EduProgramProfileFormYears_Years]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_EduVolume]
    DROP CONSTRAINT [FK_{objectQualifier}University_EduVolume_EduProgramProfileFormYears]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_Contingent]
    DROP CONSTRAINT [FK_{objectQualifier}University_Contingent_EduProgramProfileFormYears]
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_Science]
    DROP CONSTRAINT [FK_{objectQualifier}University_Science_EduPrograms]
GO

-- Drop stored procedures & functions

DROP PROCEDURE {databaseOwner}[{objectQualifier}University_FindEmployees]
GO

DROP PROCEDURE {databaseOwner}[{objectQualifier}University_FindDivisions]
GO

DROP FUNCTION {databaseOwner}[{objectQualifier}University_DivisionsHierarchy]
GO

-- Drop views

DROP VIEW {databaseOwner}[{objectQualifier}vw_University_OccupiedPositions]
GO

DROP VIEW {databaseOwner}[{objectQualifier}vw_University_EmployeesMaxWeight]
GO

DROP VIEW {databaseOwner}[{objectQualifier}vw_University_EmployeesTotalWeight]
GO

DROP VIEW {databaseOwner}[{objectQualifier}vw_University_EmployeeAchievements]
GO

-- Drop tables

DROP TABLE {databaseOwner}[{objectQualifier}University_EduProgramDivisions]
GO

DROP TABLE {databaseOwner}[{objectQualifier}University_OccupiedPositions]
GO

DROP TABLE {databaseOwner}[{objectQualifier}University_Positions]
GO

DROP TABLE {databaseOwner}[{objectQualifier}University_EmployeeAchievements]
GO

DROP TABLE {databaseOwner}[{objectQualifier}University_Achievements]
GO

DROP TABLE {databaseOwner}[{objectQualifier}University_EmployeeDisciplines]
GO

DROP TABLE {databaseOwner}[{objectQualifier}University_EduForms]
GO

DROP TABLE {databaseOwner}[{objectQualifier}University_EduLevels]
GO

DROP TABLE {databaseOwner}[{objectQualifier}University_Documents]
GO

DROP TABLE {databaseOwner}[{objectQualifier}University_DocumentTypes]
GO

DROP TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfileForms]
GO

DROP TABLE {databaseOwner}[{objectQualifier}University_Divisions]
GO

DROP TABLE {databaseOwner}[{objectQualifier}University_Employees]
GO

DROP TABLE {databaseOwner}[{objectQualifier}University_AchievementTypes]
GO

DROP TABLE {databaseOwner}[{objectQualifier}University_EduVolume]
GO

DROP TABLE {databaseOwner}[{objectQualifier}University_Contingent]
GO

DROP TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfileFormYears]
GO

DROP TABLE {databaseOwner}[{objectQualifier}University_Years]
GO

DROP TABLE {databaseOwner}[{objectQualifier}University_Science]
GO

DROP TABLE {databaseOwner}[{objectQualifier}University_EduPrograms]
GO

DROP TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfiles]
GO
