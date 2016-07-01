//
//  EmployeeAchievementView.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015 Roman M. Yagodin
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
using DotNetNuke.Services.Localization;
using R7.University.Components;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Employee
{
    [Serializable]
    public class EmployeeAchievementView: EmployeeAchievementInfo
    {
        public int ItemID { get; set; }

        public string ViewYears { get; protected set; }

        public string ViewAchievementType { get; protected set; }

        public string ViewTitle
        { 
            get { return Title + " " + TitleSuffix; }
        }

        public void Localize (string resourceFile)
        {
            ViewYears = FormatYears.Replace ("{ATM}", Localization.GetString ("AtTheMoment.Text", resourceFile));
            ViewAchievementType = Localization.GetString (
                AchievementTypeInfo.GetResourceKey (AchievementType), resourceFile);
        }

        public EmployeeAchievementView ()
        {
            ItemID = ViewNumerator.GetNextItemID ();
        }

        public EmployeeAchievementView (EmployeeAchievementInfo achievement) : this ()
        {
            CopyCstor.Copy<EmployeeAchievementInfo> (achievement, this);
        }

        public EmployeeAchievementInfo NewEmployeeAchievementInfo ()
        {
            var achievement = new EmployeeAchievementInfo ();
            CopyCstor.Copy<EmployeeAchievementInfo> (this, achievement);
            return achievement;
        }
    }
}
