//
//  EduProgramProfileQueryableExtensions.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2018 Roman M. Yagodin
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
using R7.University.ModelExtensions;
using R7.University.Models;

namespace R7.University.Queries
{
    public static class EduProgramProfileQueryableExtensions
    {
        public static IQueryable<EduProgramProfileInfo> IncludeEduProgramAndDivisions (this IQueryable<EduProgramProfileInfo> eduProgramProfiles)
        {
            return eduProgramProfiles.Include (epp => epp.EduLevel)
                                     .Include (epp => epp.EduProgram)
                                     .Include (epp => epp.EduProgram.EduLevel)
                                     .Include (epp => epp.Divisions)
                                     .Include (epp => epp.EduProgram.Divisions);
        }

        public static IQueryable<EduProgramProfileInfo> WhereEduLevelsOrAll (this IQueryable<EduProgramProfileInfo> eduProgramProfiles, IEnumerable<int> eduLevelIds)
        {
            if (!eduLevelIds.IsNullOrEmpty ()) {
                return eduProgramProfiles.Where (epp => eduLevelIds.Contains (epp.EduLevelId));
            }

            return eduProgramProfiles;
        }

        public static IQueryable<EduProgramProfileInfo> WhereDivisionOrAll (this IQueryable<EduProgramProfileInfo> eduProgramProfiles, int? divisionId, DivisionLevel divisionLevel)
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

        public static IQueryable<EduProgramProfileInfo> Order (this IQueryable<EduProgramProfileInfo> source)
        {
            return source.OrderBy (epp => epp.EduProgram.EduLevel.SortIndex)
                         .ThenBy (epp => epp.EduProgram.Code)
                         .ThenBy (epp => epp.EduProgram.Title)
                         .ThenBy (epp => epp.ProfileCode)
                         .ThenBy (epp => epp.ProfileTitle)
                         .ThenBy (epp => epp.EduLevel.SortIndex);
        }
    }
}
