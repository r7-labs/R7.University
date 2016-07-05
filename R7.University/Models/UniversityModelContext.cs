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
using System.Linq;
using System.Collections.Generic;
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

        #region DataRepositoryBase implementation

        public override IDataContext CreateDataContext ()
        {
            return UniversityDataContextFactory.Instance.Create ();
        }

        #endregion

        #region Custom methods

        public IQueryable<EduLevelInfo> QueryEduLevels ()
        {
            return Query<EduLevelInfo> ().OrderBy (el => el.EduLevelID);
        }

        public IQueryable<DivisionInfo> QueryDivisions ()
        {
            return Query<DivisionInfo> ().OrderBy (d => d.Title);
        }

        public IQueryable<DivisionInfo> QueryRootDivisions ()
        {
            return Query<DivisionInfo> ().Where (d => d.ParentDivisionID == null);
        }

        public IQueryable<DocumentTypeInfo> QueryDocumentTypes ()
        {
            return Query<DocumentTypeInfo> ();
        }

        public IQueryable<DocumentInfo> QueryDocuments_ForEduProgram (int eduProgramId)
        {
            return Query<DocumentInfo> ().Include (d => d.DocumentType).Where (d => d.EduProgramId == eduProgramId);
        }

        public IQueryable<DocumentInfo> QueryDocuments_ForEduPrograms ()
        {
            return Query<DocumentInfo> ().Include (d => d.DocumentType).Where (d => d.EduProgramId != null);
        }

        public IQueryable<DocumentInfo> QueryDocuments_ForEduProgramProfile (int eduProgramProfileId)
        {
            return Query<DocumentInfo> ().Include (d => d.DocumentType).Where (d => d.EduProgramProfileId == eduProgramProfileId);
        }

        public IQueryable<DocumentInfo> QueryDocuments_ForEduProgramProfiles ()
        {
            return Query<DocumentInfo> ().Include (d => d.DocumentType).Where (d => d.EduProgramProfileId != null);
        }

        public IQueryable<EduProgramInfo> QueryEduProgram (int eduProgramId)
        {
            return QueryOne<EduProgramInfo> (ep => ep.EduProgramID == eduProgramId)
                .Include (ep => ep.EduLevel)
                .Include (ep => ep.Documents)
                .Include (ep => ep.Documents.Select (d => d.DocumentType));
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

        public IQueryable<EduFormInfo> QueryEduForms ()
        {
            return Query<EduFormInfo> ();
        }

        public IQueryable<EduProgramProfileInfo> QueryEduProgramProfile (int eduProgramProfileId)
        {
            return QueryOne<EduProgramProfileInfo> (epp => epp.EduProgramProfileID == eduProgramProfileId)
                .Include (epp => epp.EduProgram)
                .Include (epp => epp.EduProgram.EduLevel)
                .Include (epp => epp.EduLevel);
        }

        public IQueryable<EduProgramProfileInfo> QueryEduProgramProfiles ()
        {
            return Query<EduProgramProfileInfo> ()
                .Include (epp => epp.EduProgram)
                .Include (epp => epp.EduProgram.EduLevel)
                .Include (epp => epp.EduLevel);
        }

        public IQueryable<EduProgramProfileInfo> QueryEduProgramProfiles_ByEduProgram (int eduProgramId)
        {
            return  QueryEduProgramProfiles ().Where (epp => epp.EduProgramID == eduProgramId);
        }

        #endregion
    }
}

