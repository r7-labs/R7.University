//
//  EduProgramProfileFormYearQueryableExtensions.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017-2019 Roman M. Yagodin
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
using R7.Dnn.Extensions.Collections;
using R7.University.Models;

namespace R7.University.EduProgramProfiles.Queries
{
    public static class EduProgramProfileFormYearQueryableExtensions
    {
        public static IQueryable<EduProgramProfileFormYearInfo> WhereEduLevelsOrAll (this IQueryable<EduProgramProfileFormYearInfo> eduProgramProfileFormYears, IEnumerable<int> eduLevelIds)
        {
            if (!eduLevelIds.IsNullOrEmpty ()) {
                return eduProgramProfileFormYears.Where (eppfy => eduLevelIds.Contains (eppfy.EduProfile.EduLevelId));
            }

            return eduProgramProfileFormYears;
        }

        public static IQueryable<EduProgramProfileFormYearInfo> WhereDivisionOrAll (this IQueryable<EduProgramProfileFormYearInfo> eduProgramProfileFormYears, int? divisionId, DivisionLevel divisionLevel)
        {
            if (divisionId != null) {
                if (divisionLevel == DivisionLevel.EduProgram) {
                    return eduProgramProfileFormYears.Where (eppfy => eppfy.EduProfile.EduProgram.Divisions.Any (epd => epd.DivisionId == divisionId));
                }
                if (divisionLevel == DivisionLevel.EduProgramProfile) {
                    return eduProgramProfileFormYears.Where (eppfy => eppfy.EduProfile.Divisions.Any (epd => epd.DivisionId == divisionId));
                }
            }

            return eduProgramProfileFormYears;
        }

        public static IOrderedQueryable<EduProgramProfileFormYearInfo> DefaultOrder (this IQueryable<EduProgramProfileFormYearInfo> source)
        {
            return source.OrderBy (ev => ev.EduProfile.EduProgram.EduLevel.SortIndex)
                         .ThenBy (ev => ev.EduProfile.EduProgram.Code)
                         .ThenBy (ev => ev.EduProfile.EduProgram.Title)
                         .ThenBy (ev => ev.EduProfile.ProfileCode)
                         .ThenBy (ev => ev.EduProfile.ProfileTitle
                                  // SQL-compatible PadLeft (10, '0') equivalent
                                  + ("0000000000" + ev.EduProfile.EduLevel.SortIndex.ToString ())
                                    .Substring (ev.EduProfile.EduLevel.SortIndex.ToString ().Length))
                         .ThenBy (ev => ev.EduForm.SortIndex);
        }
    }
}
