//
// EduProgramRepository.cs
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
using System.Linq;
using DotNetNuke.Data;
using R7.DotNetNuke.Extensions.Data;
using R7.DotNetNuke.Extensions.Utilities;

namespace R7.University.Data
{
    public class EduProgramRepository
    {
        #region Singleton implementation

        private static readonly Lazy<EduProgramRepository> instance = new Lazy<EduProgramRepository> ();

        public static EduProgramRepository Instance
        {
            get { return instance.Value; }
        }

        #endregion

        /*
        public EduProgramProfileRepository (Dal2DataProvider dataProvider)
        {
            this.dataProvider = dataProvider;
        }
        */

        private Dal2DataProvider dataProvider;

        public Dal2DataProvider DataProvider
        {
            get { return dataProvider ?? (dataProvider = new Dal2DataProvider ()); }
        }

        #region CUD Methods

        public void AddEduProgram (EduProgramInfo eduProgram, IList<DocumentInfo> documents)
        {
            using (var ctx = DataContext.Instance ()) {
                var eduProgramRepo = ctx.GetRepository<EduProgramInfo> ();
                var documentRepo = ctx.GetRepository<DocumentInfo> ();

                try {
                    ctx.BeginTransaction ();

                    // add edu program
                    eduProgramRepo.Insert (eduProgram);

                    // add new documents
                    foreach (var document in documents) {
                        document.ItemID = "EduProgramID=" + eduProgram.EduProgramID;
                        documentRepo.Insert (document);
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
                var eduProgramRepo = ctx.GetRepository<EduProgramInfo> ();
                var documentRepo = ctx.GetRepository<DocumentInfo> ();

                try {
                    ctx.BeginTransaction ();

                    // update edu program
                    eduProgramRepo.Update (eduProgram);

                    var documentIds = documents.Select (d => d.DocumentID.ToString ());
                    if (documentIds.Any ()) {
                        // delete specific documents
                        documentRepo.Delete (string.Format ("WHERE [ItemID] = N'{0}' AND [DocumentID] NOT IN ({1})", 
                            "EduProgramID=" + eduProgram.EduProgramID,
                            TextUtils.FormatList (", ", documentIds))); 
                    }
                    else {
                        // delete all edu program documents
                        documentRepo.Delete (string.Format (
                            "WHERE [ItemID] = N'EduProgramID={0}'",
                            eduProgram.EduProgramID)); 
                    }

                    // add new documents
                    foreach (var document in documents) {
                        document.ItemID = "EduProgramID=" + eduProgram.EduProgramID;
                        if (document.DocumentID <= 0) {
                            documentRepo.Insert (document);
                        }
                        else {
                            documentRepo.Update (document);
                        }
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
                var eduProgramRepo = ctx.GetRepository<EduProgramInfo> ();
                var documentRepo = ctx.GetRepository<DocumentInfo> ();

                try {
                    ctx.BeginTransaction ();

                    // delete documents
                    documentRepo.Delete (string.Format (
                        "WHERE [ItemID] = N'EduProgramID={0}'",
                        eduProgramId));

                    // delete edu program
                    eduProgramRepo.Delete ("WHERE [EduProgramID]=@0", eduProgramId);

                    ctx.Commit ();
                }
                catch {
                    ctx.RollbackTransaction ();
                    throw;
                }
            }
        }

        #endregion

        public IEnumerable<EduProgramInfo> GetEduPrograms_ByEduLevel (int eduLevelId)
        {
            return DataProvider.GetObjects<EduProgramInfo> ("WHERE EduLevelID = @0", eduLevelId)
                .OrderBy (ep => ep.Code)
                .ThenBy (ep => ep.Title);
        }

        public IEnumerable<EduProgramInfo> GetEduPrograms_ByEduLevels (IEnumerable<string> eduLevelIds, bool getAll)
        {
            if (eduLevelIds.Any ()) {
                if (getAll) {
                    return DataProvider.GetObjects<EduProgramInfo> (string.Format ("WHERE EduLevelID IN ({0})",
                        TextUtils.FormatList (",", eduLevelIds))
                    );
                }

                return DataProvider.GetObjects<EduProgramInfo> (string.Format ("WHERE (StartDate IS NULL OR @0 >= StartDate) " +
                    "AND (EndDate IS NULL OR @0 < EndDate) AND EduLevelID IN ({0})",
                    TextUtils.FormatList (",", eduLevelIds)), DateTime.Now
                );
            }

            return Enumerable.Empty<EduProgramInfo> ();
        }
    }
}

