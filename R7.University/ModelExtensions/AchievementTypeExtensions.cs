//
//  AchievementTypeExtensions.cs
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

using System;
using R7.University.Models;

namespace R7.University.ModelExtensions
{
    public static class AchievementTypeExtensions
    {
        public static SystemAchievementType GetSystemType (this IAchievementType achievementType)
        {
            if (achievementType != null) {
                SystemAchievementType result;
                if (Enum.TryParse (achievementType.Type, out result)) {
                    return result;
                }
            }

            return SystemAchievementType.Custom;
        }

        public static bool IsOneOf (this IAchievementType achievementType, params SystemAchievementType [] systemAchievementTypes)
        {
            var achievementTypeParsed = GetSystemType (achievementType);
            foreach (var systemAchievementType in systemAchievementTypes) {
                if (systemAchievementType == achievementTypeParsed) {
                    return true;
                }
            }
        
            return false;
        }
    }
}
