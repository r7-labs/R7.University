//
// UniversityRepository.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015-2016 Roman M. Yagodin
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
using R7.University;

namespace R7.University.Data
{
    public class UniversityRepository
    {
        #region Singleton implementation

        private static readonly Lazy<UniversityRepository> instance = new Lazy<UniversityRepository> ();

        public static UniversityRepository Instance
        {
            get { return instance.Value; }
        }

        #endregion

        private Dal2DataProvider dataProvider;

        public Dal2DataProvider DataProvider
        {
            get { return dataProvider ?? (dataProvider = new Dal2DataProvider ()); }
        }

        public EmployeeInfo GetEmployeeByUserId (int userId)
        {
            EmployeeInfo employee;

            using (var ctx = DataContext.Instance ()) {
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

            return DataProvider.GetObjects<EmployeeInfo> (
                CommandType.StoredProcedure, 
                "University_FindEmployees",
                searchText,
                includeNonPublished,
                teachersOnly,
                includeSubdivisions,
                divisionId)
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
                                employee.EmployeeID, Utils.FormatList (", ", occupiedPositonIDs))); 
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
                                employee.EmployeeID, Utils.FormatList (", ", employeeAchievementIDs))); 
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
                                employee.EmployeeID, Utils.FormatList (", ", employeeDisciplineIDs))); 
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
                }
                catch {
                    ctx.RollbackTransaction ();
                    throw;
                }
            }
        }

        public IEnumerable<DivisionInfo> FindDivisions (string searchText, bool includeSubdivisions, string divisionId)
        {
            return DataProvider.GetObjects<DivisionInfo> (CommandType.StoredProcedure, 
                "University_FindDivisions", searchText, includeSubdivisions, divisionId);
        }

        public EmployeeInfo GetHeadEmployee (int divisionId, int? headPositionId)
        {
            if (headPositionId != null) {
                return DataProvider.GetObjects<EmployeeInfo> (CommandType.StoredProcedure, 
                    "University_GetHeadEmployee", divisionId, headPositionId.Value).FirstOrDefault ();
            }
        
            return null;
        }

        public IEnumerable<EmployeeInfo> GetTeachersByEduProgramProfile (int eduProfileId)
        {
            // TODO: Convert to stored procedure or Linq query
            
            return DataProvider.GetObjects<EmployeeInfo> (CommandType.Text,
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
            return DataProvider.GetObjects<EmployeeInfo> (CommandType.Text,
                @"SELECT DISTINCT E.* FROM dbo.University_Employees AS E
                    INNER JOIN dbo.vw_University_OccupiedPositions AS OP
                        ON E.EmployeeID = OP.EmployeeID
                    WHERE OP.IsTeacher = 1 AND E.IsPublished = 1 AND E.EmployeeID NOT IN 
                        (SELECT DISTINCT EmployeeID FROM dbo.University_EmployeeDisciplines)");
        }

        public IEnumerable<DivisionInfo> GetSubDivisions (int divisionId)
        {
            return DataProvider.GetObjects<DivisionInfo> (CommandType.Text,
                @"SELECT DISTINCT D.*, DH.[Level], DH.[Path] FROM dbo.University_Divisions AS D 
                    INNER JOIN dbo.University_DivisionsHierarchy (@0) AS DH
                        ON D.DivisionID = DH.DivisionID
                    ORDER BY DH.[Path], D.Title", divisionId);
        }

        public IEnumerable<DivisionInfo> GetRootDivisions ()
        {
            return DataProvider.GetObjects<DivisionInfo> ("WHERE [ParentDivisionID] IS NULL");
        }

        public IEnumerable<EduProgramInfo> GetEduPrograms (IEnumerable<string> eduLevelIds, bool getAll)
        {
            if (eduLevelIds.Any ()) {
                if (getAll) {
                    return DataProvider.GetObjects<EduProgramInfo> (string.Format ("WHERE EduLevelID IN ({0})",
                            Utils.FormatList (",", eduLevelIds))
                    );
                }

                return DataProvider.GetObjects<EduProgramInfo> (string.Format ("WHERE (StartDate IS NULL OR @0 >= StartDate) " +
                        "AND (EndDate IS NULL OR @0 < EndDate) AND EduLevelID IN ({0})",
                        Utils.FormatList (",", eduLevelIds)), DateTime.Now
                );
            }

            return Enumerable.Empty<EduProgramInfo> ();
        }

        public void AddEduProgram (EduProgramInfo eduProgram, IList<DocumentInfo> documents)
        {
            using (var ctx = DataContext.Instance ()) {
                ctx.BeginTransaction ();

                try {
                    // add edu program
                    DataProvider.Add<EduProgramInfo> (eduProgram);

                    // add new documents
                    foreach (var document in documents) {
                        document.ItemID = "EduProgramID=" + eduProgram.EduProgramID;
                        DataProvider.Add<DocumentInfo> (document);
                    }

                    ctx.Commit ();
                }
                catch {
                    ctx.RollbackTransaction ();
                    throw;
                }
            }
        }

        public void UpdateEduProgram (EduProgramInfo eduProgram, IList<DocumentInfo> documents)
        {
            using (var ctx = DataContext.Instance ()) {
                ctx.BeginTransaction ();

                try {
                    // update edu program
                    DataProvider.Update<EduProgramInfo> (eduProgram);

                    var documentIds = documents.Select (d => d.DocumentID.ToString ());
                    if (documentIds.Any ()) {
                        // delete specific documents
                        DataProvider.Delete<DocumentInfo> (string.Format ("WHERE [ItemID] = N'{0}' AND [DocumentID] NOT IN ({1})", 
                                "EduProgramID=" + eduProgram.EduProgramID,
                                Utils.FormatList (", ", documentIds))); 
                    }
                    else {
                        // delete all edu program documents
                        DataProvider.Delete<DocumentInfo> (string.Format (
                                "WHERE [ItemID] = N'EduProgramID={0}'",
                                eduProgram.EduProgramID)); 
                    }

                    // add new documents
                    foreach (var document in documents) {
                        document.ItemID = "EduProgramID=" + eduProgram.EduProgramID;
                        if (document.DocumentID <= 0)
                            DataProvider.Add<DocumentInfo> (document);
                        else
                            DataProvider.Update<DocumentInfo> (document);
                    }

                    ctx.Commit ();
                }
                catch {
                    ctx.RollbackTransaction ();
                    throw;
                }
            }
        }

        public void DeleteEduProgram (int eduProgramId)
        {
            using (var ctx = DataContext.Instance ()) {
                ctx.BeginTransaction ();

                try {
                    // delete documents
                    DataProvider.Delete<DocumentInfo> (string.Format (
                            "WHERE [ItemID] = N'EduProgramID={0}'",
                            eduProgramId));

                    // delete edu program
                    DataProvider.Delete<EduProgramInfo> (eduProgramId);
                  
                    ctx.Commit ();
                }
                catch {
                    ctx.RollbackTransaction ();
                    throw;
                }
            }
        }

        public void UpdateDocuments (IList<DocumentInfo> documents, string itemKey, int itemId)
        {
            using (var ctx = DataContext.Instance ()) {
                ctx.BeginTransaction ();

                try {
                    var originalDocuments = DataProvider.GetObjects<DocumentInfo> (string.Format (
                                                    "WHERE ItemID = N'{0}={1}'", itemKey, itemId)).ToList ();
                    
                    foreach (var document in documents) {
                        if (document.DocumentID <= 0) {
                            document.ItemID = itemKey + "=" + itemId;
                            DataProvider.Add<DocumentInfo> (document);
                        }
                        else {
                            DataProvider.Update<DocumentInfo> (document);

                            // documents with same ID could be different objects!
                            var updatedDocument = originalDocuments.FirstOrDefault (d => d.DocumentID == document.DocumentID);
                            if (updatedDocument != null) {
                                // do not delete this document later
                                originalDocuments.Remove (updatedDocument);
                            }
                        }
                    }

                    // delete remaining documents
                    foreach (var document in originalDocuments) {
                        DataProvider.Delete<DocumentInfo> (document);
                    }

                    ctx.Commit ();
                }
                catch {
                    ctx.RollbackTransaction ();
                    throw;
                }
            }
        }

        public void UpdateEduProgramProfileForms (IList<EduProgramProfileFormInfo> eduForms, int eduProgramProfileId)
        {
            using (var ctx = DataContext.Instance ()) {
                ctx.BeginTransaction ();

                try {
                    var originalEduForms = DataProvider.GetObjects<EduProgramProfileFormInfo> (
                                               "WHERE EduProgramProfileID = @0", eduProgramProfileId).ToList ();
                    
                    foreach (var eduForm in eduForms) {
                        if (eduForm.EduProgramProfileFormID <= 0) {
                            eduForm.EduProgramProfileID = eduProgramProfileId;
                            DataProvider.Add<EduProgramProfileFormInfo> (eduForm);
                        }
                        else {
                            DataProvider.Update<EduProgramProfileFormInfo> (eduForm);

                            // objects with same ID could be different!
                            var updatedEduForm = originalEduForms.FirstOrDefault (ef => 
                                ef.EduProgramProfileFormID == eduForm.EduProgramProfileFormID);
                            
                            if (updatedEduForm != null) {
                                // do not delete this object later
                                originalEduForms.Remove (updatedEduForm);
                            }
                        }
                    }

                    // delete remaining items
                    foreach (var eduForm in originalEduForms) {
                        DataProvider.Delete<EduProgramProfileFormInfo> (eduForm);
                    }

                    ctx.Commit ();
                }
                catch {
                    ctx.RollbackTransaction ();
                    throw;
                }
            }
        }
    }
}
