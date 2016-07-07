//
//  EduProgramProfileQuery.cs
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
using R7.University.Models;
using System.Collections.Generic;
using System.Linq;

namespace R7.University.Queries
{
    public class EduProgramProfileQuery
    {
        private readonly IModelContext modelContext;

        public EduProgramProfileQuery (IModelContext modelContext)
        {
            this.modelContext = modelContext;
        }

        protected IQueryable<EduProgramProfileInfo> QueryEduProgramProfiles (IEnumerable<int> eduLevelIds)
        {
            return modelContext.Query<EduProgramProfileInfo> ()
                .Where (epp => eduLevelIds.Contains (epp.EduLevelId))
                .Include (epp => epp.EduLevel)
                .Include (epp => epp.EduProgram);
        }

        protected IQueryable<EduProgramProfileInfo> OrderBy (IQueryable<EduProgramProfileInfo> source)
        {
            return source.OrderBy (epp => epp.EduProgram.EduLevel.SortIndex)
                .ThenBy (epp => epp.EduProgram.Code)
                .ThenBy (epp => epp.EduProgram.Title)
                .ThenBy (epp => epp.ProfileCode)
                .ThenBy (epp => epp.ProfileTitle)
                .ThenBy (epp => epp.EduLevel.SortIndex);
        }

        public IEnumerable<EduProgramProfileInfo> Execute (IEnumerable<int> eduLevelIds)
        {
            return OrderBy (QueryEduProgramProfiles (eduLevelIds)).ToList ();
        }

        public IEnumerable<EduProgramProfileInfo> WithEduForms (IEnumerable<int> eduLevelIds)
        {
            return OrderBy (QueryEduProgramProfiles (eduLevelIds)
                .Include (epp => epp.EduProgramProfileForms)
                .Include (epp => epp.EduProgramProfileForms.Select (eppf => eppf.EduForm))
            ).ToList ();
        }

        public IEnumerable<EduProgramProfileInfo> WithDocuments (IEnumerable<int> eduLevelIds)
        {
            return OrderBy (QueryEduProgramProfiles (eduLevelIds)
                .Include (epp => epp.Documents)
                .Include (epp => epp.Documents.Select (d => d.DocumentType))
            ).ToList ();
        }
    }
}
