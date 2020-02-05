//
//  EmployeeQueryExtensions.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2018-2020 Roman M. Yagodin
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
    public static class EmployeeQueryExtensions
    {
        public static IQueryable<EmployeeInfo> IncludePositions (this IQueryable<EmployeeInfo> employees)
        {
            return employees.Include (e => e.Positions)
                            .ThenInclude (p => p.Position);
        }

        public static IQueryable<EmployeeInfo> IncludePositionsWithDivision (this IQueryable<EmployeeInfo> employees)
        {
            return employees.Include (e => e.Positions)
                            .ThenInclude (p => p.Position)
                            .Include (e => e.Positions)
                            .ThenInclude (p => p.Division);
        }

        public static IQueryable<EmployeeInfo> IncludeAchievements (this IQueryable<EmployeeInfo> employees)
        {
            return employees.Include (e => e.Achievements)
                            .ThenInclude (ea => ea.Achievement)
                            .ThenInclude (a => a.AchievementType)
                            .Include (e => e.Achievements)
                            .ThenInclude (ea => ea.AchievementType);
        }

        public static IQueryable<EmployeeInfo> IncludeDisciplines (this IQueryable<EmployeeInfo> employees)
        {
            return employees.Include (e => e.Disciplines)
                                .ThenInclude (ed => ed.EduProgramProfile)
                                    .ThenInclude (epp => epp.EduProgram)
                                        .ThenInclude (ep => ep.EduLevel)
                            .Include (e => e.Disciplines)
                                .ThenInclude (ed => ed.EduProgramProfile)
                                    .ThenInclude (epp => epp.EduLevel);
        }
    }
}
