//
//  EmployeeAchievementEditViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
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
using System.Xml.Serialization;
using DotNetNuke.Services.Localization;
using R7.University.Components;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Employee.ViewModels
{
    [Serializable]
    public class EmployeeAchievementEditViewModel: IEmployeeAchievement
    {
        #region IEmployeeAchievement implementation

        public int EmployeeAchievementID { get; set; }

        public int EmployeeID { get; set; }

        public int? AchievementID { get; set; }

        public string Title { get; set; }

        public string ShortTitle { get; set; }

        public string Description { get; set; }

        public int? YearBegin { get; set; }

        public int? YearEnd { get; set; }

        public bool IsTitle { get; set; }

        public string DocumentURL { get; set; }

        public string TitleSuffix { get; set; }

        public AchievementType? AchievementType { get; set; }

        [XmlIgnore]
        public AchievementInfo Achievement { get; set; }

        #endregion

        public int ItemID { get; set; }

        public string ViewYears { get; set; }

        public string ViewAchievementType { get; set; }

        public string ViewTitle
        { 
            get { return Title + " " + TitleSuffix; }
        }

        public void Localize (string resourceFile)
        {
            ViewYears = FormatHelper.FormatYears (YearBegin, YearEnd).Replace ("{ATM}", Localization.GetString ("AtTheMoment.Text", resourceFile));

            ViewAchievementType = Localization.GetString (
                AchievementTypeInfo.GetResourceKey (AchievementType), resourceFile);
        }

        public EmployeeAchievementEditViewModel ()
        {
            ItemID = ViewNumerator.GetNextItemID ();
        }

        public EmployeeAchievementEditViewModel (IEmployeeAchievement achievement, string resourceFile) : this ()
        {
            CopyCstor.Copy<IEmployeeAchievement> (achievement, this);

            // use base achievement values
            if (achievement.Achievement != null) {
                Title = achievement.Achievement.Title;
                ShortTitle = achievement.Achievement.ShortTitle;
                AchievementType = achievement.Achievement.AchievementType;
            }

            Localize (resourceFile);
        }

        public EmployeeAchievementInfo NewEmployeeAchievementInfo ()
        {
            var achievement = new EmployeeAchievementInfo ();
            CopyCstor.Copy<IEmployeeAchievement> (this, achievement);
            return achievement;
        }
    }
}
