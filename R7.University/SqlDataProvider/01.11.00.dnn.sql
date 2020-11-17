-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

IF NOT EXISTS (select * from sys.columns where object_id = object_id(N'{databaseOwner}[{objectQualifier}University_EduProgramProfileForms]') and name = N'TimeToLearnUnit')
BEGIN
    ALTER TABLE {databaseOwner}[{objectQualifier}University_EduProgramProfileForms]
        ADD [TimeToLearnUnit] nchar (1) NOT NULL DEFAULT (N'M')
END
GO
