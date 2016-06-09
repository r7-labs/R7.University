//
//  AchievementTypeInfo.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2016 Roman M. Yagodin
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
using R7.University.Models;

namespace R7.University.Data
{
    public class AchievementTypeInfo
    {
        public AchievementType AchievementType { get; set; }

        public string LocalizedAchivementType
        {
            get { return OnLocalize != null ? OnLocalize (ResourceKey) : ResourceKey; }
        }

        public AchievementTypeInfo (AchievementType achievementType)
        {
            AchievementType = achievementType;
        }

        #region Private members

        private event LocalizeHandler OnLocalize;

        private string ResourceKey
        {
            get { return GetResourceKey (AchievementType); } 
        }

        #endregion

        #region Static members

        public static List<AchievementTypeInfo> GetLocalizedAchievementTypes (LocalizeHandler localizeHandler)
        {
            var achievementTypes = new List<AchievementTypeInfo> ();
            foreach (AchievementType achievementType in Enum.GetValues(typeof(AchievementType))) {   
                var achievement = new AchievementTypeInfo (achievementType);
                achievement.OnLocalize += new LocalizeHandler (localizeHandler);
                achievementTypes.Add (achievement);
            }

            return achievementTypes;
        }

        public static string GetResourceKey (AchievementType? achievementType)
        {
            if (achievementType != null)
                return "AchievementType" + (char) achievementType.Value + ".Text";
		
            return "AchievementTypeN.Text";
        }

        #endregion
    }
}

