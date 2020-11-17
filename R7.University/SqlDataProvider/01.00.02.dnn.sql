-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

-- Drop existing stored procedures & functions

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}University_DivisionsHierarchy]') and OBJECTPROPERTY(id, N'IsTableFunction') = 1)
	DROP FUNCTION {databaseOwner}[{objectQualifier}University_DivisionsHierarchy]
GO

-- Create stored procedures & functions

CREATE FUNCTION {databaseOwner}[{objectQualifier}University_DivisionsHierarchy]
(
	@divisionId int
)
RETURNS TABLE
AS
RETURN
(
	WITH DivisionsHierachy (DivisionID, ParentDivisionID, [Level])
	AS
	(
		SELECT DivisionID, ParentDivisionID, 0 AS [Level]
			FROM {databaseOwner}[{objectQualifier}University_Divisions] AS D
				WHERE D.DivisionID = @divisionId -- insert parameter here
			UNION ALL
			SELECT D.DivisionID, D.ParentDivisionID, DH.[Level] + 1
				FROM {databaseOwner}[{objectQualifier}University_Divisions] AS D
					INNER JOIN DivisionsHierachy AS DH
						ON D.ParentDivisionID = DH.DivisionID
	)
	SELECT * FROM DivisionsHierachy
)
GO
