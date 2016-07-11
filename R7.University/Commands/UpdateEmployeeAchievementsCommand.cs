//
//  UpdateOccupiedPositionsCommand.cs
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
using R7.University.Components;
using R7.University.Models;

namespace R7.University.Commands
{
    public class UpdateEmployeeAchievementsCommand
    {
        protected readonly IModelContext ModelContext;

        public UpdateEmployeeAchievementsCommand (IModelContext modelContext)
        {
            ModelContext = modelContext;
        }

        public void UpdateEmployeeAchievements (IList<EmployeeAchievementInfo> achievements, int employeeId)
        {
            var originalAchievements = ModelContext.Query<EmployeeAchievementInfo> ()
                .Where (op => op.EmployeeID == employeeId)
                .ToList ();
            
            foreach (var achievement in achievements) {
                var originalAchievement = originalAchievements.SingleOrDefault (ea => ea.EmployeeAchievementID == achievement.EmployeeAchievementID);
                if (originalAchievement == null) {
                    achievement.EmployeeID = employeeId;
                    ModelContext.Add<EmployeeAchievementInfo> (achievement);
                }
                else {
                    CopyCstor.Copy<EmployeeAchievementInfo> (achievement, originalAchievement);
                    ModelContext.Update<EmployeeAchievementInfo> (originalAchievement);

                    // do not delete this document later
                    originalAchievements.Remove (originalAchievement);
                }
            }

            // should delete all remaining edu. forms
            foreach (var originalAchievement in originalAchievements) {
                ModelContext.Remove<EmployeeAchievementInfo> (originalAchievement);
            }
        }
    }
}

