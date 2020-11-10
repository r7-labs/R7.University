﻿//
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
using Microsoft.EntityFrameworkCore;
using R7.Dnn.Extensions.Collections;
using R7.University.Models;

namespace R7.University.Queries
{
    public static class EduProgramProfileQueryableExtensions
    {
        public static IQueryable<EduProfileInfo> IncludeEduProgramAndDivisions (this IQueryable<EduProfileInfo> eduProgramProfiles)
        {
            return eduProgramProfiles.Include (epp => epp.EduLevel)
                                     .Include (epp => epp.EduProgram)
                                        .ThenInclude (ep => ep.EduLevel)
                                     .Include (epp => epp.Divisions)
                                     .Include (epp => epp.EduProgram)
                                        .ThenInclude (epd => epd.Divisions);
        }

        public static IQueryable<EduProfileInfo> IncludeEduProgramProfileFormYears (this IQueryable<EduProfileInfo> eduProgramProfiles)
        {
            return eduProgramProfiles.Include (epp => epp.EduProgramProfileFormYears)
                                        .ThenInclude (eppfy => eppfy.Year)
                                     .Include (epp => epp.EduProgramProfileFormYears)
                                        .ThenInclude (eppfy => eppfy.EduForm)
                                     .Include (epp => epp.EduProgramProfileFormYears)
                                        .ThenInclude (eppfy => eppfy.EduVolume);
        }

        public static IQueryable<EduProfileInfo> IncludeContingent (this IQueryable<EduProfileInfo> eduProgramProfiles)
        {
            return eduProgramProfiles.Include (epp => epp.EduProgramProfileFormYears)
                                        .ThenInclude (eppfy => eppfy.Contingent);
        }

        public static IQueryable<EduProfileInfo> IncludeEduProgramProfileFormYearsAndForms (this IQueryable<EduProfileInfo> eduProgramProfiles)
        {
            return eduProgramProfiles.Include (epp => epp.EduProgramProfileFormYears)
                                        .ThenInclude (eppfy => eppfy.Year)
                                     .Include (epp => epp.EduProgramProfileFormYears)
                                        .ThenInclude (eppfy => eppfy.EduForm);
        }

        public static IQueryable<EduProfileInfo> IncludeDivisions (this IQueryable<EduProfileInfo> eduProgramProfiles)
        {
            return eduProgramProfiles.Include (epp => epp.Divisions)
                                        .ThenInclude (d => d.Division);
        }

        public static IQueryable<EduProfileInfo> IncludeDocuments (this IQueryable<EduProfileInfo> eduProgramProfiles)
        {
            return eduProgramProfiles.Include (epp => epp.Documents)
                                        .ThenInclude (d => d.DocumentType);
        }

        public static IQueryable<EduProfileInfo> WhereEduLevelsOrAll (this IQueryable<EduProfileInfo> eduProgramProfiles, IEnumerable<int> eduLevelIds)
        {
            if (!eduLevelIds.IsNullOrEmpty ()) {
                return eduProgramProfiles.Where (epp => eduLevelIds.Contains (epp.EduLevelId));
            }

            return eduProgramProfiles;
        }

        public static IQueryable<EduProfileInfo> WhereDivisionOrAll (this IQueryable<EduProfileInfo> eduProgramProfiles, int? divisionId, DivisionLevel divisionLevel)
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

        public static IQueryable<EduProfileInfo> DefaultOrder (this IQueryable<EduProfileInfo> source)
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
