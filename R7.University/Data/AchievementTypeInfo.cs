//
// AchievementTypeInfo.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2014 
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

