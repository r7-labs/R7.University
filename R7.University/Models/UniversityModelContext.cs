//
//  UniversityModelContext.cs
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
using R7.University.Components;
using R7.University.Data;

namespace R7.University.Models
{
    public class UniversityModelContext: ModelContextBase
    {
        public UniversityModelContext (IDataContext dataContext): base (dataContext)
        {
        }

        public UniversityModelContext ()
        {
        }

        #region ModelContextBase implementation

        public override IDataContext CreateDataContext ()
        {
            return UniversityDataContextFactory.Instance.Create ();
        }

        public override bool SaveChanges (bool dispose = true)
        {
            var result = base.SaveChanges (dispose);

            // drop cache on final call
            if (dispose) {
                CacheHelper.RemoveCacheByPrefix ("//r7_University");
            }

            return result;
        }

        #endregion

        #region Custom methods

        // TODO: Convert to queries

        public IQueryable<DocumentInfo> QueryDocuments_ForEduProgram (int eduProgramId)
        {
            return Query<DocumentInfo> ().Include (d => d.DocumentType).Where (d => d.EduProgramId == eduProgramId);
        }

        public IQueryable<EduProgramInfo> QueryEduPrograms ()
        {
            return Query<EduProgramInfo> ()
                .Include (ep => ep.EduLevel)
                .Include (ep => ep.Documents)
                .Include (ep => ep.Documents.Select (d => d.DocumentType));
        }

        public IQueryable<EduProgramInfo> QueryEduPrograms_ByEduLevels (IList<int> eduLevelIds)
        {
            if (eduLevelIds.Count > 0) {
                return QueryEduPrograms ()
                   .Where (ep => eduLevelIds.Contains (ep.EduLevelID));
            }

            return QueryEduPrograms ();
        }

        public IQueryable<EduProgramInfo> QueryEduPrograms_ByDivisionAndEduLevels (int divisionId, IList<int> eduLevelIds)
        {
            if (eduLevelIds.Count > 0) {
                return QueryEduPrograms ()
                    .Where (ep => ep.DivisionId == divisionId && eduLevelIds.Contains (ep.EduLevelID));
            }

            return QueryEduPrograms ()
                .Where (ep => ep.DivisionId == divisionId);
        }

        public IQueryable<EduProgramProfileInfo> QueryEduProgramProfile (int eduProgramProfileId)
        {
            return QueryOne<EduProgramProfileInfo> (epp => epp.EduProgramProfileID == eduProgramProfileId)
                .Include (epp => epp.EduProgram)
                .Include (epp => epp.EduProgram.EduLevel)
                .Include (epp => epp.EduLevel);
        }

        #endregion
    }
}

