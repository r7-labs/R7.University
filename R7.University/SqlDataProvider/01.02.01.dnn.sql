-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

-- Alter tables

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EduPrograms]') and name = N'ProfileCode')
	ALTER TABLE {databaseOwner}[{objectQualifier}University_EduPrograms]
		ADD [ProfileCode] nvarchar (64)
GO

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EduPrograms]') and name = N'ProfileTitle')
	ALTER TABLE {databaseOwner}[{objectQualifier}University_EduPrograms]
		ADD [ProfileTitle] nvarchar (250)
GO

-- Drop views

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}vw_University_EmployeeEduPrograms]') and OBJECTPROPERTY(id, N'IsView') = 1)
	DROP VIEW {databaseOwner}[{objectQualifier}vw_University_EmployeeEduPrograms]
GO

-- Create views

CREATE VIEW {databaseOwner}[{objectQualifier}vw_University_EmployeeEduPrograms] AS
	SELECT EEP.*, EP.Code, EP.Title, EP.ProfileCode, EP.ProfileTitle FROM {databaseOwner}[{objectQualifier}University_EmployeeEduPrograms] AS EEP
		INNER JOIN {databaseOwner}[{objectQualifier}University_EduPrograms] AS EP
			ON EEP.EduProgramID = EP.EduProgramID
GO
