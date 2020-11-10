//
//  EduProgramProfileFormYearQueryableExtensions.cs
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

using System.Linq;
using Microsoft.EntityFrameworkCore;
using R7.University.Models;

namespace R7.University.Queries
{
    public static class EduProgramProfileFormYearQueryableExtensions
    {
        public static IQueryable<EduProgramProfileFormYearInfo> IncludeEduProgramProfileWithEduProgramAndDivisions (this IQueryable<EduProgramProfileFormYearInfo> eduProgramProfileFormYears)
        {
            return eduProgramProfileFormYears.Include (eppfy => eppfy.EduProfile)
                                                .ThenInclude (epp => epp.EduLevel)
                                             .Include (eppfy => eppfy.EduProfile)
                                                .ThenInclude (eppd => eppd.Divisions)
                                             .Include (eppfy => eppfy.EduProfile)
                                                .ThenInclude (epp => epp.EduProgram)
                                                    .ThenInclude (ep => ep.EduLevel)
                                             .Include (eppfy => eppfy.EduProfile)
                                                .ThenInclude (epp => epp.EduProgram)
                                                    .ThenInclude (ep => ep.Divisions);
        }

        public static IQueryable<EduProgramProfileFormYearInfo> IncludeEduProgramProfileWithEduProgram (this IQueryable<EduProgramProfileFormYearInfo> eduProgramProfileFormYears)
        {
            return eduProgramProfileFormYears.Include (eppfy => eppfy.EduProfile)
                                                .ThenInclude (epp => epp.EduLevel)
                                             .Include (eppfy => eppfy.EduProfile)
                                                .ThenInclude (epp => epp.EduProgram);
        }
    }
}
