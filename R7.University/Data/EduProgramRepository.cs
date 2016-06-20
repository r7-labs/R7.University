//
//  EduProgramRepository.cs
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
using System.Linq;
using DotNetNuke.Data;
using R7.DotNetNuke.Extensions.Data;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.Components;

namespace R7.University.Data
{
    public class EduProgramRepository
    {
        #region Singleton implementation

        private static readonly Lazy<EduProgramRepository> instance = new Lazy<EduProgramRepository> (
            () => new EduProgramRepository (UniversityDataProvider.Instance)
        );

        public static EduProgramRepository Instance
        {
            get { return instance.Value; }
        }

        #endregion

        protected Dal2DataProvider DataProvider;

        public EduProgramRepository (Dal2DataProvider dataProvider)
        {
            DataProvider = dataProvider;
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
                    CacheHelper.RemoveCacheByPrefix ("//r7_University");
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
                    CacheHelper.RemoveCacheByPrefix ("//r7_University");
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
                    CacheHelper.RemoveCacheByPrefix ("//r7_University");
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

        public IEnumerable<EduProgramInfo> GetEduPrograms_ByEduLevels (IEnumerable<string> eduLevelIds)
        {
            if (eduLevelIds.Any ()) {
                return DataProvider.GetObjects<EduProgramInfo> (string.Format ("WHERE EduLevelID IN ({0})",
                    TextUtils.FormatList (",", eduLevelIds))
                );
            }

            return Enumerable.Empty<EduProgramInfo> ();
        }

        public IEnumerable<EduProgramInfo> GetEduPrograms_ByDivisionAndEduLevels (int divisionId, IEnumerable<string> eduLevelIds)
        {
            if (eduLevelIds.Any ()) {
                return DataProvider.GetObjects<EduProgramInfo> (string.Format ("WHERE DivisionID = {0} AND EduLevelID IN ({1})",
                    divisionId,
                    TextUtils.FormatList (",", eduLevelIds))
                );
            }

            return Enumerable.Empty<EduProgramInfo> ();
        }
    }
}

