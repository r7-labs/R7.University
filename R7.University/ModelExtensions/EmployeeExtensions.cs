//
// EmployeeExtensions.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2016 Roman M. Yagodin
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

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
            employee.Achievements = EmployeeAchievementRepository.Instance
                .GetEmployeeAchievements (employee.EmployeeID).ToList ();

            return employee;
        }

        public static IEnumerable<IEmployee> WithAchievements (this IEnumerable<IEmployee> employees)
        {
            var commonAchievements = EmployeeAchievementRepository.Instance.GetAchievements_ForEmployees (
                employees.Select (e => e.EmployeeID));
            
            foreach (var employee in employees) {
                employee.Achievements = commonAchievements.Where (ca => ca.EmployeeID == employee.EmployeeID).ToList ();
            }

            return employees;
        }

        public static IEnumerable<IEmployee> WithOccupiedPositions (this IEnumerable<IEmployee> employees, int divisionId)
        {
            var commonOps = OccupiedPositionRepository.Instance.GetOccupiedPositions_ForEmployees (
                employees.Select (e => e.EmployeeID), divisionId);

            foreach (var employee in employees) {
                employee.OccupiedPositions = commonOps.Where (op => op.EmployeeID == employee.EmployeeID)
                    .GroupByDivision ()
                    .ToList ();
            }

            return employees;
        }

        public static IEmployee WithOccupiedPositions (this IEmployee employee)
        {
            employee.OccupiedPositions = OccupiedPositionRepository.Instance
                .GetOccupiedPositions (employee.EmployeeID)
                .GroupByDivision ()
                .ToList ();
            
            return employee;
        }
    }
}
