-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

-- Alter phone number columns
ALTER TABLE {databaseOwner}[{objectQualifier}University_Employees]
ALTER COLUMN [Phone] [nvarchar](64) NULL
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_Employees]
ALTER COLUMN [CellPhone] [nvarchar](64) NULL
GO

ALTER TABLE {databaseOwner}[{objectQualifier}University_Divisions]
ALTER COLUMN [Phone] [nvarchar](64) NULL
GO

-- Add University_WorkingHours vocabulary
IF NOT EXISTS (SELECT [VocabularyID]
	FROM {databaseOwner}[{objectQualifier}Taxonomy_Vocabularies]
		WHERE [Name] = N'University_WorkingHours')
BEGIN
	INSERT INTO {databaseOwner}[{objectQualifier}Taxonomy_Vocabularies]
		([VocabularyTypeID], [Name], [ScopeTypeID], [IsSystem],
		[CreatedByUserID], [LastModifiedByUserID], [CreatedOnDate], [LastModifiedOnDate],
		[Description])
		VALUES (1, N'University_WorkingHours', 1, 1,
			-1, -1, GETDATE(), GETDATE(),
			N'System vocabulary for frequently used employee / division working hours')
END
GO

-- Rename Structure vocabulary to University_Structure, if exists
UPDATE {databaseOwner}[{objectQualifier}Taxonomy_Vocabularies]
	SET [Name] = N'University_Structure',
		[Description] = N'Host-level vocabulary for organizational structure'
	WHERE [Name] = N'Structure';

-- Add or rename University_Structure vocabulary
IF NOT EXISTS (SELECT [VocabularyID]
	FROM {databaseOwner}[{objectQualifier}Taxonomy_Vocabularies]
		WHERE [Name] = N'University_Structure')
BEGIN
	INSERT INTO {databaseOwner}[{objectQualifier}Taxonomy_Vocabularies]
		([VocabularyTypeID], [Name], [ScopeTypeID], [IsSystem],
		[CreatedByUserID], [LastModifiedByUserID], [CreatedOnDate], [LastModifiedOnDate],
		[Description])
		VALUES (2, N'University_Structure', 1, 0,
			-1, -1, GETDATE(), GETDATE(),
			N'Host-level vocabulary for organization structure')
END
GO
