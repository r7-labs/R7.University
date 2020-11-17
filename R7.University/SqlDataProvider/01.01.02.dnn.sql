-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

-- Alter tables

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Employees]') and name = N'Disciplines')
	ALTER TABLE {databaseOwner}[{objectQualifier}University_Employees]
		ADD [Disciplines] [nvarchar](max) NULL
GO

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_Positions]') and name = N'IsTeacher')
	ALTER TABLE {databaseOwner}[{objectQualifier}University_Positions]
		ADD [IsTeacher] [bit] NOT NULL CONSTRAINT [DF_{objectQualifier}University_Positions_IsTeacher] DEFAULT ((0))
GO
