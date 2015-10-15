using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;
using DotNetNuke.Collections;
using DotNetNuke.Data;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Web.UI.WebControls;
using DotNetNuke.R7;
using R7.University;

namespace R7.University
{
    public abstract class UniversityControllerBase: ControllerBase
	{
		#region Custom methods

		public EmployeeInfo GetEmployeeByUserId (int userId)
		{
			EmployeeInfo employee;

			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<EmployeeInfo> ();
				employee = repo.Find ("WHERE UserId = @0", userId).FirstOrDefault ();
			}

			return employee;
		}

        public IEnumerable<EmployeeInfo> FindEmployees (string searchText, bool includeNonPublished, 
            bool teachersOnly, bool includeSubdivisions, string divisionId)
        {
            // University_FindEmployees SP could return some duplicate records - 
            // not many, so using Distinct() extension method to get rid of them 
            // is looking more sane than further SP SQL code complication.

            return GetObjects<EmployeeInfo> (CommandType.StoredProcedure, 
                "University_FindEmployees", searchText, includeNonPublished, teachersOnly, includeSubdivisions, divisionId)
                    .Distinct (new EmployeeEqualityComparer ());
        }

		public void AddEmployee (EmployeeInfo employee, 
		    List<OccupiedPositionInfo> occupiedPositions, 
            List<EmployeeAchievementInfo> achievements,
            List<EmployeeDisciplineInfo> eduPrograms)
        {
			using (var ctx = DataContext.Instance ())
			{
				ctx.BeginTransaction ();

				try
				{
					// add Employee
					Add<EmployeeInfo> (employee);

					// add new OccupiedPositions
					foreach (var op in occupiedPositions)
					{
						op.EmployeeID = employee.EmployeeID;
						Add<OccupiedPositionInfo> (op);
					}
					
					// add new EmployeeAchievements
					foreach (var ach in achievements)
					{
						ach.EmployeeID = employee.EmployeeID;
						Add<EmployeeAchievementInfo> (ach);
					}

                    // add new EmployeeEduPrograms
                    foreach (var ep in eduPrograms)
                    {
                        ep.EmployeeID = employee.EmployeeID;
                        Add<EmployeeDisciplineInfo> (ep);
                    }
				
					ctx.Commit ();
				}
				catch
				{
					ctx.RollbackTransaction ();
					throw;
				}
			}

		}

		public void UpdateEmployee (EmployeeInfo employee, 
		    List<OccupiedPositionInfo> occupiedPositions, 
            List<EmployeeAchievementInfo> achievements,
            List<EmployeeDisciplineInfo> disciplines)
        {
			using (var ctx = DataContext.Instance ())
			{
				ctx.BeginTransaction ();

				try
				{
					// update Employee
					Update<EmployeeInfo> (employee);

					var occupiedPositonIDs = occupiedPositions.Select (op => op.OccupiedPositionID.ToString ());
					if (occupiedPositonIDs.Any())
					{
						Delete<OccupiedPositionInfo> (
							string.Format ("WHERE [EmployeeID] = {0} AND [OccupiedPositionID] NOT IN ({1})", 
								employee.EmployeeID, Utils.FormatList (", ", occupiedPositonIDs))); 
					}
					else
					{
						// delete all employee occupied positions 
						Delete<OccupiedPositionInfo> ("WHERE [EmployeeID] = @0", employee.EmployeeID); 
					}
					
					// add new OccupiedPositions
					foreach (var op in occupiedPositions)
					{
						// REVIEW: Do we really need to set EmployeeID here?
						op.EmployeeID = employee.EmployeeID;
						
						if (op.OccupiedPositionID <= 0)
							Add<OccupiedPositionInfo> (op);
						else
							Update<OccupiedPositionInfo> (op);
					}
					
					var employeeAchievementIDs = achievements.Select (a => a.EmployeeAchievementID.ToString ());
					if (employeeAchievementIDs.Any())
					{
						// delete those not in current list
						Delete<EmployeeAchievementInfo> (
							string.Format ("WHERE [EmployeeID] = {0} AND [EmployeeAchievementID] NOT IN ({1})", 
								employee.EmployeeID, Utils.FormatList (", ", employeeAchievementIDs))); 
					}
					else
					{
						// delete all employee achievements
						Delete<EmployeeAchievementInfo> ("WHERE [EmployeeID] = @0", employee.EmployeeID);
					}

					// add new EmployeeAchievements
					foreach (var ach in achievements)
					{
						if (ach.AchievementID != null)
						{
							// reset linked properties
							ach.Title = null;
							ach.ShortTitle = null;
							ach.AchievementType = null;
						}
						
						ach.EmployeeID = employee.EmployeeID;
						if (ach.EmployeeAchievementID <= 0)
							Add<EmployeeAchievementInfo> (ach);
						else
							Update<EmployeeAchievementInfo> (ach);
					}

                    var employeeDisciplineIDs = disciplines.Select (a => a.EmployeeDisciplineID.ToString ());
                    if (employeeDisciplineIDs.Any ())
                    {
                        // delete those not in current list
                        Delete<EmployeeDisciplineInfo> (
                            string.Format ("WHERE [EmployeeID] = {0} AND [EmployeeDisciplineID] NOT IN ({1})", 
                                employee.EmployeeID, Utils.FormatList (", ", employeeDisciplineIDs))); 
                    }
                    else
                    {
                        // delete all employee disciplines
                        Delete<EmployeeDisciplineInfo> ("WHERE [EmployeeID] = @0", employee.EmployeeID);
                    }

                    // add new employee disciplines
                    foreach (var discipline in disciplines)
                    {
                        discipline.EmployeeID = employee.EmployeeID;
                        if (discipline.EmployeeDisciplineID <= 0)
                            Add<EmployeeDisciplineInfo> (discipline);
                        else
                            Update<EmployeeDisciplineInfo> (discipline);
                    }

					ctx.Commit ();
				}
				catch
				{
					ctx.RollbackTransaction ();
					throw;
				}
			}
		}

        public IEnumerable<DivisionInfo> FindDivisions (string searchText, bool includeSubdivisions, string divisionId)
        {
            return GetObjects<DivisionInfo> (CommandType.StoredProcedure, 
                "University_FindDivisions", searchText, includeSubdivisions, divisionId);
        }

        public EmployeeInfo GetHeadEmployee (int divisionId)
        {
            return GetObjects<EmployeeInfo> (CommandType.StoredProcedure, 
                "University_GetHeadEmployee", divisionId).FirstOrDefault ();
        }

        public IEnumerable<EmployeeInfo> GetTeachersByEduProgramProfile (int eduProfileId)
        {
            // TODO: Convert to stored procedure
            
            return GetObjects<EmployeeInfo> (CommandType.Text,
                @"SELECT DISTINCT E.* FROM dbo.University_Employees AS E
                    INNER JOIN dbo.vw_University_OccupiedPositions AS OP
                        ON E.EmployeeID = OP.EmployeeID
                    INNER JOIN dbo.University_EmployeeDisciplines AS ED
                        ON E.EmployeeID = ED.EmployeeID
                WHERE ED.EduProgramProfileID = @0 AND OP.IsTeacher = 1 AND E.IsPublished = 1
                ORDER BY E.LastName, E.FirstName", eduProfileId);
        }

        public IEnumerable<EmployeeInfo> GetTeachersWithoutEduPrograms ()
        {
            return GetObjects<EmployeeInfo> (CommandType.Text,
                @"SELECT DISTINCT E.* FROM dbo.University_Employees AS E
                    INNER JOIN dbo.vw_University_OccupiedPositions AS OP
                        ON E.EmployeeID = OP.EmployeeID
                    WHERE OP.IsTeacher = 1 AND E.IsPublished = 1 AND E.EmployeeID NOT IN 
                        (SELECT DISTINCT EmployeeID FROM dbo.University_EmployeeDisciplines)");
        }

        public IEnumerable<DivisionInfo> GetSubDivisions (int divisionId)
        {
            return GetObjects<DivisionInfo> (CommandType.Text,
                @"SELECT DISTINCT D.*, DH.[Level] FROM dbo.University_Divisions AS D 
                    INNER JOIN dbo.University_DivisionsHierarchy (@0) AS DH
                        ON D.DivisionID = DH.DivisionID
                    ORDER BY DH.[Level], D.Title", divisionId);
        }

        public IEnumerable<DivisionInfo> GetRootDivisions ()
        {
            return GetObjects<DivisionInfo> ("WHERE [ParentDivisionID] IS NULL");
        }

		#endregion
	}
}

