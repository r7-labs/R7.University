-- NOTE: To manually execute this script you must
-- replace {databaseOwner} and {objectQualifier} with real values.
-- Defaults is "dbo." for database owner and "" for object qualifier

IF EXISTS (select * from {databaseOwner}[{objectQualifier}ModuleDefinitions] where DefinitionName = N'R7.University.Division')
BEGIN
    -- Rename old definitions
    UPDATE {databaseOwner}[{objectQualifier}ModuleDefinitions]
        SET DefinitionName = N'R7_University_Division' WHERE DefinitionName = N'R7.University.Division'

    UPDATE {databaseOwner}[{objectQualifier}ModuleDefinitions]
        SET DefinitionName = N'R7_University_DivisionDirectory' WHERE DefinitionName = N'R7.University.DivisionDirectory'

    UPDATE {databaseOwner}[{objectQualifier}ModuleDefinitions]
        SET DefinitionName = N'R7_University_EduProgram' WHERE DefinitionName = N'R7.University.EduProgram'

    UPDATE {databaseOwner}[{objectQualifier}ModuleDefinitions]
        SET DefinitionName = N'R7_University_EduProgramDirectory' WHERE DefinitionName = N'R7.University.EduProgramDirectory'

    UPDATE {databaseOwner}[{objectQualifier}ModuleDefinitions]
        SET DefinitionName = N'R7_University_EduProgramProfileDirectory' WHERE DefinitionName = N'R7.University.EduProgramProfileDirectory'

    UPDATE {databaseOwner}[{objectQualifier}ModuleDefinitions]
        SET DefinitionName = N'R7_University_Employee' WHERE DefinitionName = N'R7.University.Employee'

    UPDATE {databaseOwner}[{objectQualifier}ModuleDefinitions]
        SET DefinitionName = N'R7_University_EmployeeDirectory' WHERE DefinitionName = N'R7.University.EmployeeDirectory'

    UPDATE {databaseOwner}[{objectQualifier}ModuleDefinitions]
        SET DefinitionName = N'R7_University_EmployeeList' WHERE DefinitionName = N'R7.University.EmployeeList'

    UPDATE {databaseOwner}[{objectQualifier}ModuleDefinitions]
        SET DefinitionName = N'R7_University_EmployeeDetails' WHERE DefinitionName = N'R7.University.EmployeeDetails'

    UPDATE {databaseOwner}[{objectQualifier}ModuleDefinitions]
        SET DefinitionName = N'R7_University_Launchpad' WHERE DefinitionName = N'R7.University.Launchpad'
END
GO
