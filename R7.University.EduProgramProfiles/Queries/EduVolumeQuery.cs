//
//  EduVolumeQuery.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017 Roman M. Yagodin
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
using R7.University.EduProgramProfiles.Models;
using R7.University.Models;

namespace R7.University.EduProgramProfiles.Queries
{
    public class EduVolumeQuery
    {
        protected readonly IModelContext ModelContext;

        public EduVolumeQuery (IModelContext modelContext)
        {
            ModelContext = modelContext;
        }

        public IEnumerable<EduProgramProfileFormYearInfo> ListByDivisionAndEduLevels (IEnumerable<int> eduLevelIds,
                                                                                      int? divisionId,
                                                                                      DivisionLevel divisionLevel)
        {
            return ModelContext.Query<EduProgramProfileFormYearInfo> ()
                               .Include (eppfy => eppfy.EduProgramProfile)
                               .Include (eppfy => eppfy.EduProgramProfile.EduLevel)
                               .Include (eppfy => eppfy.EduProgramProfile.EduProgram)
                               .Include (eppfy => eppfy.EduForm)
                               .Include (eppfy => eppfy.EduVolume)
                               .Include (eppfy => eppfy.Year)
                               .Where (eppfy => eduLevelIds.Contains (eppfy.EduProgramProfile.EduLevelId))
                               .Where (eppfy => !eppfy.Year.AdmissionIsOpen)
                               .WhereDivision (divisionId, divisionLevel)
                               .Order ()
                               .ToList ();
        }
    }

    public static class EduProgramProfileFormYearQueryableExtensions
    {
        public static IQueryable<EduProgramProfileFormYearInfo> WhereDivision (this IQueryable<EduProgramProfileFormYearInfo> eduProgramProfileFormYears, int? divisionId, DivisionLevel divisionLevel)
        { 
            if (divisionId != null) {
                if (divisionLevel == DivisionLevel.EduProgram) {
                    return eduProgramProfileFormYears.Where (eppfy => eppfy.EduProgramProfile.EduProgram.Divisions.Any (epd => epd.DivisionId == divisionId));
                } 
                if (divisionLevel == DivisionLevel.EduProgramProfile) {
                    return eduProgramProfileFormYears.Where (eppfy => eppfy.EduProgramProfile.Divisions.Any (epd => epd.DivisionId == divisionId));
                }
            }

            return eduProgramProfileFormYears;
        }

        public static IQueryable<EduProgramProfileFormYearInfo> Order (this IQueryable<EduProgramProfileFormYearInfo> source)
        {
            return source.OrderBy (ev => ev.EduProgramProfile.EduProgram.EduLevel.SortIndex)
                         .ThenBy (ev => ev.EduProgramProfile.EduProgram.Code)
                         .ThenBy (ev => ev.EduProgramProfile.EduProgram.Title)
                         .ThenBy (ev => ev.EduProgramProfile.EduLevel.SortIndex)
                         .ThenBy (ev => ev.EduProgramProfile.ProfileCode)
                         .ThenBy (ev => ev.EduProgramProfile.ProfileTitle)
                         .ThenByDescending (ev => ev.Year.Year)
                         .ThenBy (ev => ev.EduForm.Title);
        }
    }
}
