//
//  EmployeeExtensions.cs
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
using System.Collections.Generic;
using System.Linq;
using R7.University.Data;
using R7.University.Models;

namespace R7.University.ModelExtensions
{
    public static class EmployeeExtensions
    {
        public static bool IsPublished (this IEmployee employee)
        {
            return ModelHelper.IsPublished (employee.StartDate, employee.EndDate);
        }

        public static IEmployee WithAchievements (this IEmployee employee)
        {
            if (employee != null) {
                employee.Achievements = EmployeeAchievementRepository.Instance
                .GetEmployeeAchievements (employee.EmployeeID)
                .Cast<IEmployeeAchievement> ()
                .ToList ();
            }

            return employee;
        }

        public static IEnumerable<IEmployee> WithDisciplines (
            this IEnumerable<IEmployee> employees, 
            IEnumerable<IEmployeeDiscipline> disciplines)
        {
            if (!employees.IsNullOrEmpty () && !disciplines.IsNullOrEmpty ()) {
                foreach (var employee in employees) {
                    employee.Disciplines = disciplines.Where (d => d.EmployeeID == employee.EmployeeID).ToList ();
                }
            }

            return employees;
        }

        public static IEnumerable<IEmployee> WithAchievements (
            this IEnumerable<IEmployee> employees,
            IEnumerable<IEmployeeAchievement> achievements)
        {
            if (!employees.IsNullOrEmpty () && !achievements.IsNullOrEmpty ()) {
                foreach (var employee in employees) {
                    employee.Achievements = achievements.Where (ach => ach.EmployeeID == employee.EmployeeID)
                    .ToList ();
                }
            }

            return employees;
        }

        [Obsolete]
        public static IEnumerable<IEmployee> WithAchievements (this IEnumerable<IEmployee> employees)
        {
            if (!employees.IsNullOrEmpty ()) {
                var commonAchievements = EmployeeAchievementRepository.Instance.GetAchievements_ForEmployees (
                    employees.Select (e => e.EmployeeID));

                foreach (var employee in employees) {
                    employee.Achievements = commonAchievements.Where (ca => ca.EmployeeID == employee.EmployeeID)
                        .Cast<IEmployeeAchievement> ()
                        .ToList ();
                }
            }

            return employees;
        }

        [Obsolete]
        public static IEnumerable<IEmployee> WithOccupiedPositions (this IEnumerable<IEmployee> employees, int divisionId)
        {
            if (!employees.IsNullOrEmpty ()) {
                var commonOps = OccupiedPositionRepository.Instance.GetOccupiedPositions_ForEmployees (
                                employees.Select (e => e.EmployeeID), divisionId);

                foreach (var employee in employees) {
                    employee.OccupiedPositions = commonOps.Where (op => op.EmployeeID == employee.EmployeeID)
                    .GroupByDivision ()
                    .ToList ();
                }
            }

            return employees;
        }

        public static IEnumerable<IEmployee> WithOccupiedPositions (this IEnumerable<IEmployee> employees, 
            IEnumerable<OccupiedPositionInfoEx> occupiedPositions)
        {
            if (!employees.IsNullOrEmpty () && !occupiedPositions.IsNullOrEmpty ()) {
                foreach (var employee in employees) {
                    employee.OccupiedPositions = occupiedPositions.Where (op => op.EmployeeID == employee.EmployeeID)
                        .GroupByDivision ()
                        .ToList ();
                }
            }

            return employees;
        }

        public static IEmployee WithOccupiedPositions (this IEmployee employee)
        {
            if (employee != null) {
                employee.OccupiedPositions = OccupiedPositionRepository.Instance
                .GetOccupiedPositions (employee.EmployeeID)
                .GroupByDivision ()
                .ToList ();
            }

            return employee;
        }
    }
}
