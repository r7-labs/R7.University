-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

ALTER TABLE {databaseOwner}[{objectQualifier}University_Positions]
ALTER COLUMN [ShortTitle] [nvarchar](64) NULL
GO
