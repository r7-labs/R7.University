//
//  EduProgramQueryableExtensions.cs
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
    public static class EduProgramQueryableExtensions
    {
        public static IQueryable<EduProgramInfo> IncludeDivisions (this IQueryable<EduProgramInfo> eduPrograms)
        {
            return eduPrograms.Include (ep => ep.Divisions)
                                .ThenInclude (d => d.Division);
        }

        public static IQueryable<EduProgramInfo> IncludeDocuments (this IQueryable<EduProgramInfo> eduPrograms)
        {
            return eduPrograms.Include (ep => ep.Documents)
                                .ThenInclude (d => d.DocumentType);
        }

        public static IQueryable<EduProgramInfo> IncludeEduLevelDivisionsAndDocuments (this IQueryable<EduProgramInfo> eduPrograms)
        {
            return eduPrograms.Include (ep => ep.EduLevel)
                              .IncludeDivisions ()
                              .IncludeDocuments ();
        }

        public static IQueryable<EduProgramInfo> IncludeEduProgramProfiles (this IQueryable<EduProgramInfo> eduPrograms)
        {
            return eduPrograms.Include (ep => ep.EduProgramProfiles)
                                .ThenInclude (epp => epp.EduLevel)
                              .Include (ep => ep.EduProgramProfiles)
                                .ThenInclude (eppd => eppd.Divisions)
                                    .ThenInclude (d => d.Division)
                              .Include (ep => ep.EduProgramProfiles)
                                .ThenInclude (epp => epp.EduProgramProfileFormYears)
                                    .ThenInclude (eppfy => eppfy.EduForm)
                              .Include (ep => ep.EduProgramProfiles)
                                .ThenInclude (epp => epp.EduProgramProfileFormYears)
                                    .ThenInclude (eppfy => eppfy.EduVolume)
                              .Include (ep => ep.EduProgramProfiles)
                                .ThenInclude (epp => epp.EduProgramProfileFormYears)
                                    .ThenInclude (eppfy => eppfy.Year);
        }
    }
}
