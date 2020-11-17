﻿-- NOTE: To manually execute this script, you must replace {databaseOwner} and {objectQualifier} with real values:
-- defaults is "dbo." for {databaseOwner} and "" for {objectQualifier}

IF EXISTS (select * from dbo.sysobjects where id = object_id(N'{databaseOwner}[{objectQualifier}University_FindEmployees]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
    DROP PROCEDURE {databaseOwner}[{objectQualifier}University_FindEmployees]
GO

CREATE PROCEDURE {databaseOwner}[{objectQualifier}University_FindEmployees]
    @divisionId int,
    @teachersOnly bit
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;

    SELECT E.* FROM (
        SELECT OP.EmployeeID
        FROM {databaseOwner}[{objectQualifier}University_DivisionsHierarchy](@divisionId) AS DH
            INNER JOIN {databaseOwner}[{objectQualifier}University_OccupiedPositions] AS OP
                ON DH.DivisionID = OP.DivisionID
            INNER JOIN {databaseOwner}[{objectQualifier}University_Positions] AS P
                ON OP.PositionID = P.PositionID
        WHERE P.IsTeacher = 1 OR @teachersOnly = 0
    ) AS DE INNER JOIN {databaseOwner}[{objectQualifier}University_Employees] AS E ON DE.EmployeeID = E.EmployeeID;

END
GO

IF NOT EXISTS (select * from {databaseOwner}[{objectQualifier}University_DocumentTypes] where [Type] = N'WorkProgram')
    INSERT INTO {databaseOwner}[{objectQualifier}University_DocumentTypes] (Type, IsSystem, FilenameFormat) VALUES
        (N'WorkProgram', 1, N'rp_[a-z0-9_]+_\d{8}\.pdf')
GO

IF EXISTS (select * from {databaseOwner}[{objectQualifier}University_DocumentTypes] where [Type] like N'Order%')
    DELETE FROM {databaseOwner}[{objectQualifier}University_DocumentTypes] WHERE [Type] = N'OrderEnrollment'
    DELETE FROM {databaseOwner}[{objectQualifier}University_DocumentTypes] WHERE [Type] = N'OrderExpulsion'
    DELETE FROM {databaseOwner}[{objectQualifier}University_DocumentTypes] WHERE [Type] = N'OrderRestoration'
    DELETE FROM {databaseOwner}[{objectQualifier}University_DocumentTypes] WHERE [Type] = N'OrderTransfer'
    DELETE FROM {databaseOwner}[{objectQualifier}University_DocumentTypes] WHERE [Type] = N'OrderAcademicLeave'
GO
