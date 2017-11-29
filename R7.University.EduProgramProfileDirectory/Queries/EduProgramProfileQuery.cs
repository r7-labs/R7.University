//
//  EduProgramProfileQuery.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
//
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Collections.Generic;
using System.Linq;
using R7.University.EduProgramProfileDirectory.Models;
using R7.University.Models;
using R7.University.Queries;

namespace R7.University.EduProgramProfileDirectory.Queries
{
    internal class EduProgramProfileQuery: EduProgramProfileQueryBase
    {
        public EduProgramProfileQuery (IModelContext modelContext): base (modelContext)
        {
        }

        public IList<EduProgramProfileInfo> ListWithEduForms (IEnumerable<int> eduLevelIds, int? divisionId, DivisionLevel divisionLevel)
        {
            return OrderBy (WhereDivision (divisionId, divisionLevel, QueryEduProgramProfiles (eduLevelIds)
                .Include (epp => epp.EduProgramProfileFormYears)
                .Include (epp => epp.EduProgramProfileFormYears.Select (eppfy => eppfy.EduForm))
             )).ToList ();
        }

        public IList<EduProgramProfileInfo> ListWithDocuments (IEnumerable<int> eduLevelIds, int? divisionId, DivisionLevel divisionLevel)
        {
            return OrderBy (WhereDivision (divisionId, divisionLevel, QueryEduProgramProfiles (eduLevelIds)
                .Include (epp => epp.Documents)
                .Include (epp => epp.Documents.Select (d => d.DocumentType))
            )).ToList ();
        }

        protected IQueryable<EduProgramProfileInfo> WhereDivision (int? divisionId, DivisionLevel divisionLevel, IQueryable<EduProgramProfileInfo> eduProgramProfiles)
        { 
            if (divisionId != null) {
                if (divisionLevel == DivisionLevel.EduProgram) {
                    return eduProgramProfiles.Where (epp => epp.EduProgram.Divisions.Any (epd => epd.DivisionId == divisionId));
                } 
                if (divisionLevel == DivisionLevel.EduProgramProfile) {
                    return eduProgramProfiles.Where (epp => epp.Divisions.Any (epd => epd.DivisionId == divisionId));
                }
            }

            return eduProgramProfiles;
        }
    }
}
