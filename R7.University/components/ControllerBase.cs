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
using R7.University;
using DotNetNuke.Entities.Tabs;

namespace R7.University
{
	public abstract class ControllerBase : ModuleSearchBase
	{
		#region Common methods

		/// <summary>
		/// Adds a new T object into the database
		/// </summary>
		/// <param name='info'></param>
		public void Add<T> (T info) where T: class
		{
			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<T> ();
				repo.Insert (info);
			}
		}

		/// <summary>
		/// Get single object from the database
		/// </summary>
		/// <returns>
		/// The object
		/// </returns>
		/// <param name='itemId'>
		/// Item identifier.
		/// </param>
		public T Get<T> (int itemId) where T: class
		{
			T info;

			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<T> ();
				info = repo.GetById (itemId);
			}

			return info;
		}

		/// <summary>
		/// Get single object from the database
		/// </summary>
		/// <returns>
		/// The object
		/// </returns>
		/// <param name='itemId'>
		/// Item identifier.
		/// </param>
		/// <param name='scopeId'>
		/// Scope identifier (like moduleId)
		/// </param>
		public T Get<T> (int itemId, int scopeId) where T: class
		{
			T info;

			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<T> ();
				info = repo.GetById (itemId, scopeId);
			}

			return info;
		}

		/// <summary>
		/// Updates an object already stored in the database
		/// </summary>
		/// <param name='info'>
		/// Info.
		/// </param>
		public void Update<T> (T info) where T: class
		{
			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<T> ();
				repo.Update (info);
			}
		}

		/// <summary>
		/// Gets all objects for items matching scopeId
		/// </summary>
		/// <param name='scopeId'>
		/// Scope identifier (like moduleId)
		/// </param>
		/// <returns></returns>
		public IEnumerable<T> GetObjects<T> (int scopeId) where T: class
		{
			IEnumerable<T> infos = null;

			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<T> ();
				infos = repo.Get (scopeId);
				
				// Without [Scope("ModuleID")] it should be like:
				// infos = repo.Find ("WHERE ModuleID = @0", moduleId);
			}

			return infos;
		}

		/// <summary>
		/// Gets all objects of type T from database
		/// </summary>
		/// <returns></returns>
		public IEnumerable<T> GetObjects<T> () where T: class
		{
			IEnumerable<T> infos = null;

			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<T> ();
				infos = repo.Get ();
			}

			return infos;
		}

		/// <summary>
		/// Gets the all objects of type T from result of a dynamic sql query
		/// </summary>
		/// <returns>Enumerable with objects of type T</returns>
		/// <param name="sqlCondition">SQL command condition.</param>
		/// <param name="args">SQL command arguments.</param>
		/// <typeparam name="T">Type of objects.</typeparam>
		public IEnumerable<T> GetObjects<T> (string sqlConditon, params object[] args) where T: class
		{
			IEnumerable<T> infos = null;
			
			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<T> ();
				infos = repo.Find (sqlConditon, args);
			}
			
			return infos;
		}

		/// <summary>
		/// Gets the all objects of type T from result of a dynamic sql query
		/// </summary>
		/// <returns>Enumerable with objects of type T</returns>
		/// <param name="cmdType">Type of an SQL command.</param>
		/// <param name="sql">SQL command.</param>
		/// <param name="args">SQL command arguments.</param>
		/// <typeparam name="T">Type of objects.</typeparam>
		public IEnumerable<T> GetObjects<T> (System.Data.CommandType cmdType, string sql, params object[] args) where T: class
		{
			IEnumerable<T> infos = null;
			
			using (var ctx = DataContext.Instance ())
			{
				infos = ctx.ExecuteQuery<T>	(cmdType, sql, args);
			}
			
			return infos;
		}

		/// <summary>
		/// Gets one page of objects of type T
		/// </summary>
		/// <param name="scopeId">Scope identifier (like moduleId)</param>
		/// <param name="index">a page index</param>
		/// <param name="size">a page size</param>
		/// <returns>A paged list of T objects</returns>
		public IPagedList<T> GetPage<T> (int scopeId, int index, int size) where T: class
		{
			IPagedList<T> infos;

			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<T> ();
				infos = repo.GetPage (scopeId, index, size);
			}

			return infos;
		}

		/// <summary>
		/// Gets one page of objects of type T
		/// </summary>
		/// <param name="index">a page index</param>
		/// <param name="size">a page size</param>
		/// <returns>A paged list of T objects</returns>
		public IPagedList<T> GetPage<T> (int index, int size) where T: class
		{
			IPagedList<T> infos;

			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<T> ();
				infos = repo.GetPage (index, size);
			}

			return infos;
		}

		/// <summary>
		/// Delete a given item from the database by instance
		/// </summary>
		/// <param name='info'></param>
		public void Delete<T> (T info) where T: class
		{
			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<T> ();
				repo.Delete (info);
		
			}
		}

		/// <summary>
		/// Delete a given item from the database by ID
		/// </summary>
		/// <param name='itemId'></param>
		public void Delete<T> (int itemId) where T: class
		{
			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<T> ();
				repo.Delete (repo.GetById (itemId));
			}
		}

		/// <summary>
		/// Delete some item from the database using SQL condition
		/// </summary>
		/// <param name='sqlConditon'>SQL condition</param>
		/// <param name='args'>Optional arguments</param>
		public void Delete<T> (string sqlConditon, params object[] args) where T: class
		{
			using (var ctx = DataContext.Instance ())
			{
				var repo = ctx.GetRepository<T> ();
				repo.Delete (sqlConditon, args);
			}
		}

		/*
		public T ExecReader <T> (string spName, params object [] args) where T: class
		{
			using (var ctx = DataContext.Instance ())
				return ctx.ExecuteQuery<T> (CommandType.StoredProcedure, spName, args);
		}

		public T ExecScalar <T> (string spName, params object [] args) where T: class
		{
			using (var ctx = DataContext.Instance ())
				return ctx.ExecuteQuery<T> (CommandType.StoredProcedure, spName, args);
		}
		*/

		#endregion

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
		                        List<OccupiedPositionInfo> occupiedPositions, List<EmployeeAchievementInfo> achievements)
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
            List<EmployeeEduProgramInfo> eduPrograms)
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

                    var employeeEduProgramIDs = eduPrograms.Select (a => a.EmployeeEduProgramID.ToString ());
                    if (employeeEduProgramIDs.Any())
                    {
                        // delete those not in current list
                        Delete<EmployeeEduProgramInfo> (
                            string.Format ("WHERE [EmployeeID] = {0} AND [EmployeeEduProgramID] NOT IN ({1})", 
                                employee.EmployeeID, Utils.FormatList (", ", employeeEduProgramIDs))); 
                    }
                    else
                    {
                        // delete all employee edu programs
                        Delete<EmployeeEduProgramInfo> ("WHERE [EmployeeID] = @0", employee.EmployeeID);
                    }

                    // add new employee edu programs
                    foreach (var ep in eduPrograms)
                    {
                        ep.EmployeeID = employee.EmployeeID;

                        if (ep.EmployeeEduProgramID <= 0)
                            Add<EmployeeEduProgramInfo> (ep);
                        else
                            Update<EmployeeEduProgramInfo> (ep);
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

        public IEnumerable<EmployeeInfo> GetTeachersByEduProgram (int eduProgramId)
        {
            var teachers = GetObjects<EmployeeInfo> (CommandType.Text,
                @"SELECT DISTINCT E.* FROM dbo.University_Employees AS E
                    INNER JOIN dbo.vw_University_OccupiedPositions AS OP
                        ON E.EmployeeID = OP.EmployeeID
                    INNER JOIN dbo.University_EmployeeEduPrograms AS EEP
                        ON E.EmployeeID = EEP.EmployeeID
                WHERE EEP.EduProgramID = @0 AND OP.IsTeacher = 1 AND E.IsPublished = 1
                ORDER BY E.LastName, E.FirstName", eduProgramId);

            return teachers ?? Enumerable.Empty<EmployeeInfo> ();
        }

        public IEnumerable<EmployeeInfo> GetTeachersWithoutEduPrograms ()
        {
            var teachers = GetObjects<EmployeeInfo> (CommandType.Text,
                @"SELECT DISTINCT E.* FROM dbo.University_Employees AS E
                    INNER JOIN dbo.vw_University_OccupiedPositions AS OP
                        ON E.EmployeeID = OP.EmployeeID
                    WHERE OP.IsTeacher = 1 AND E.IsPublished = 1 AND E.EmployeeID NOT IN 
                        (SELECT DISTINCT EmployeeID FROM dbo.University_EmployeeEduPrograms)");

            return teachers ?? Enumerable.Empty<EmployeeInfo> ();
        }

        public IEnumerable<DivisionInfo> GetSubDivisions (int divisionId)
        {
            var subDivisions = GetObjects<DivisionInfo> (CommandType.Text,
                @"SELECT DISTINCT D.*, DH.[Level] FROM dbo.University_Divisions AS D 
                    INNER JOIN dbo.University_DivisionsHierarchy (@0) AS DH
                        ON D.DivisionID = DH.DivisionID
                    ORDER BY DH.[Level], D.Title", divisionId);

            return subDivisions ?? Enumerable.Empty<DivisionInfo> ();
        }

        public IEnumerable<DivisionInfo> GetRootDivisions ()
        {
            var rootDivisions = GetObjects<DivisionInfo> ("WHERE [ParentDivisionID] IS NULL");
            return rootDivisions ?? Enumerable.Empty<DivisionInfo> ();
        }

		#endregion
	}
}

