//
// EmployeeRepository.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2016 Roman M. Yagodin
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using DotNetNuke.Data;
using R7.DotNetNuke.Extensions.Data;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.Components;
using R7.University.ModelExtensions;

namespace R7.University.Data
{
    public class EmployeeRepository
    {
        #region Singleton implementation

        private static readonly Lazy<EmployeeRepository> instance = new Lazy<EmployeeRepository> ();

        public static EmployeeRepository Instance
        {
            get { return instance.Value; }
        }

        #endregion

        private Dal2DataProvider dataProvider;

        public Dal2DataProvider DataProvider
        {
            get { return dataProvider ?? (dataProvider = new Dal2DataProvider ()); }
        }

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
                includeSubDivisions ? // which SP to use
                    "University_GetEmployees_ByDivisionID_Recursive" : "University_GetEmployees_ByDivisionID", 
                divisionId, sortType);
        }

        public IEnumerable<EmployeeInfo> GetTeachers ()
        {
            return DataProvider.GetObjects<EmployeeInfo> (CommandType.Text,
                @"SELECT DISTINCT E.* FROM dbo.University_Employees AS E
                    INNER JOIN dbo.vw_University_OccupiedPositions AS OP
                        ON E.EmployeeID = OP.EmployeeID
                WHERE OP.IsTeacher = 1");
        }

        public IEnumerable<EmployeeInfo> FindEmployees (string searchText, bool includeNonPublished, 
            bool teachersOnly, bool includeSubdivisions, string divisionId)
        {
            // University_FindEmployees SP could return some duplicate records - 
            // not many, so using Distinct() extension method to get rid of them 
            // is looking more sane than further SP SQL code complication.

            return DataProvider.GetObjectsFromSp<EmployeeInfo> ("University_FindEmployees", 
                    searchText, teachersOnly, includeSubdivisions, divisionId)
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

