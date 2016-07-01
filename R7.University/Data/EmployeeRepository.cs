//
//  EmployeeRepository.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DotNetNuke.Data;
using R7.DotNetNuke.Extensions.Data;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.Components;
using R7.University.ModelExtensions;
using R7.University.Models;

namespace R7.University.Data
{
    public class EmployeeRepository
    {
        protected Dal2DataProvider DataProvider;

        public EmployeeRepository (Dal2DataProvider dataProvider)
        {
            DataProvider = dataProvider;
        }

        #region Singleton implementation

        private static readonly Lazy<EmployeeRepository> instance = new Lazy<EmployeeRepository> (
            () => new EmployeeRepository (UniversityDataProvider.Instance)
        );

        public static EmployeeRepository Instance
        {
            get { return instance.Value; }
        }

        #endregion

        public EmployeeInfo GetEmployee (int employeeId)
        {
            return DataProvider.Get<EmployeeInfo> (employeeId);
        }

        public EmployeeInfo GetEmployee_ByUserId (int userId)
        {
            return DataProvider.GetObjects<EmployeeInfo> ("WHERE UserId = @0", userId).FirstOrDefault ();
        }

        public IEnumerable<EmployeeInfo> GetEmployees_ByDivisionId (int divisionId, bool includeSubDivisions, int sortType)
        {
            // TODO: Expose weghtMod sp argument
            return DataProvider.GetObjectsFromSp<EmployeeInfo> (
                includeSubDivisions // which SP to use
                    ? "{databaseOwner}[{objectQualifier}University_GetEmployees_ByDivisionID_Recursive]" 
                    : "{databaseOwner}[{objectQualifier}University_GetEmployees_ByDivisionID]", 
                divisionId, sortType);
        }

        public IEnumerable<EmployeeInfo> GetTeachers ()
        {
            return DataProvider.GetObjects<EmployeeInfo> (CommandType.Text,
                @"SELECT DISTINCT E.* FROM {databaseOwner}[{objectQualifier}University_Employees] AS E
                    INNER JOIN {databaseOwner}[{objectQualifier}vw_University_OccupiedPositions] AS OP
                        ON E.EmployeeID = OP.EmployeeID
                WHERE OP.IsTeacher = 1");
        }

        public IEnumerable<EmployeeInfo> FindEmployees (string searchText, bool includeNonPublished, 
            bool teachersOnly, int divisionId)
        {
            // University_FindEmployees SP could return some duplicate records - 
            // not many, so using Distinct() extension method to get rid of them 
            // is looking more sane than further SP SQL code complication.

            // TODO: Remove @includeSubdivisions argument from sp
            return DataProvider.GetObjectsFromSp<EmployeeInfo> ("{databaseOwner}[{objectQualifier}University_FindEmployees]", 
                    searchText, teachersOnly, true, divisionId)
                .Where (e => includeNonPublished || e.IsPublished ())
                .Distinct (new EmployeeEqualityComparer ());
        }

        public void AddEmployee (EmployeeInfo employee, 
            IList<OccupiedPositionInfo> occupiedPositions, 
            IList<EmployeeAchievementInfo> achievements,
            IList<EmployeeDisciplineInfo> eduPrograms)
        {
            using (var ctx = DataContext.Instance ()) {
                ctx.BeginTransaction ();

                try {
                    // add Employee
                    DataProvider.Add<EmployeeInfo> (employee);

                    // add new OccupiedPositions
                    foreach (var op in occupiedPositions) {
                        op.EmployeeID = employee.EmployeeID;
                        DataProvider.Add<OccupiedPositionInfo> (op);
                    }

                    // add new EmployeeAchievements
                    foreach (var ach in achievements) {
                        ach.EmployeeID = employee.EmployeeID;
                        DataProvider.Add<EmployeeAchievementInfo> (ach);
                    }

                    // add new EmployeeEduPrograms
                    foreach (var ep in eduPrograms) {
                        ep.EmployeeID = employee.EmployeeID;
                        DataProvider.Add<EmployeeDisciplineInfo> (ep);
                    }

                    ctx.Commit ();

                    CacheHelper.RemoveCacheByPrefix ("//r7_University");
                }
                catch {
                    ctx.RollbackTransaction ();
                    throw;
                }
            }
        }

        public void UpdateEmployee (EmployeeInfo employee, 
            IList<OccupiedPositionInfo> occupiedPositions, 
            IList<EmployeeAchievementInfo> achievements,
            IList<EmployeeDisciplineInfo> disciplines)
        {
            using (var ctx = DataContext.Instance ()) {
                ctx.BeginTransaction ();

                try {
                    // update Employee
                    DataProvider.Update<EmployeeInfo> (employee);

                    var occupiedPositonIDs = occupiedPositions.Select (op => op.OccupiedPositionID.ToString ());
                    if (occupiedPositonIDs.Any ()) {
                        DataProvider.Delete<OccupiedPositionInfo> (
                            string.Format ("WHERE [EmployeeID] = {0} AND [OccupiedPositionID] NOT IN ({1})", 
                                employee.EmployeeID, TextUtils.FormatList (", ", occupiedPositonIDs))); 
                    }
                    else {
                        // delete all employee occupied positions 
                        DataProvider.Delete<OccupiedPositionInfo> ("WHERE [EmployeeID] = @0", employee.EmployeeID); 
                    }

                    // add new OccupiedPositions
                    foreach (var op in occupiedPositions) {
                        // REVIEW: Do we really need to set EmployeeID here?
                        op.EmployeeID = employee.EmployeeID;

                        if (op.OccupiedPositionID <= 0)
                            DataProvider.Add<OccupiedPositionInfo> (op);
                        else
                            DataProvider.Update<OccupiedPositionInfo> (op);
                    }

                    var employeeAchievementIDs = achievements.Select (a => a.EmployeeAchievementID.ToString ());
                    if (employeeAchievementIDs.Any ()) {
                        // delete those not in current list
                        DataProvider.Delete<EmployeeAchievementInfo> (
                            string.Format ("WHERE [EmployeeID] = {0} AND [EmployeeAchievementID] NOT IN ({1})", 
                                employee.EmployeeID, TextUtils.FormatList (", ", employeeAchievementIDs))); 
                    }
                    else {
                        // delete all employee achievements
                        DataProvider.Delete<EmployeeAchievementInfo> ("WHERE [EmployeeID] = @0", employee.EmployeeID);
                    }

                    // add new EmployeeAchievements
                    foreach (var ach in achievements) {
                        if (ach.AchievementID != null) {
                            // reset linked properties
                            ach.Title = null;
                            ach.ShortTitle = null;
                            ach.AchievementType = null;
                        }

                        ach.EmployeeID = employee.EmployeeID;
                        if (ach.EmployeeAchievementID <= 0)
                            DataProvider.Add<EmployeeAchievementInfo> (ach);
                        else
                            DataProvider.Update<EmployeeAchievementInfo> (ach);
                    }

                    var employeeDisciplineIDs = disciplines.Select (a => a.EmployeeDisciplineID.ToString ());
                    if (employeeDisciplineIDs.Any ()) {
                        // delete those not in current list
                        DataProvider.Delete<EmployeeDisciplineInfo> (
                            string.Format ("WHERE [EmployeeID] = {0} AND [EmployeeDisciplineID] NOT IN ({1})", 
                                employee.EmployeeID, TextUtils.FormatList (", ", employeeDisciplineIDs))); 
                    }
                    else {
                        // delete all employee disciplines
                        DataProvider.Delete<EmployeeDisciplineInfo> ("WHERE [EmployeeID] = @0", employee.EmployeeID);
                    }

                    // add new employee disciplines
                    foreach (var discipline in disciplines) {
                        discipline.EmployeeID = employee.EmployeeID;
                        if (discipline.EmployeeDisciplineID <= 0)
                            DataProvider.Add<EmployeeDisciplineInfo> (discipline);
                        else
                            DataProvider.Update<EmployeeDisciplineInfo> (discipline);
                    }

                    ctx.Commit ();
                    CacheHelper.RemoveCacheByPrefix ("//r7_University");
                }
                catch {
                    ctx.RollbackTransaction ();
                    throw;
                }
            }
        }

        public void DeleteEmployee (int employeeId)
        {
            DataProvider.Delete<EmployeeInfo> (employeeId);
            CacheHelper.RemoveCacheByPrefix ("//r7_University");
        }
    }
}

