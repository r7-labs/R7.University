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
using R7.DotNetNuke.Extensions.Utilities;
using R7.University;
using R7.University.Models;

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
