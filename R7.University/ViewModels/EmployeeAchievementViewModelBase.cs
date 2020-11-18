//
//  EmployeeAchievementViewModelBase.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2019 Roman M. Yagodin
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

using R7.University.Models;

namespace R7.University.ViewModels
{
    public abstract class EmployeeAchievementViewModelBase: IEmployeeAchievement
    {
        public IEmployeeAchievement EmployeeAchievement { get; protected set; }

        protected EmployeeAchievementViewModelBase (IEmployeeAchievement model)
        {
            EmployeeAchievement = model;
        }

        #region IEmployeeAchievement implementation

        public int EmployeeAchievementID => EmployeeAchievement.EmployeeAchievementID;

        public int EmployeeID => EmployeeAchievement.EmployeeID;

        public int? AchievementID => EmployeeAchievement.AchievementID;

        public int? AchievementTypeId => EmployeeAchievement.AchievementTypeId;

        public string Title => (Achievement != null) ? EmployeeAchievement.Achievement.Title : EmployeeAchievement.Title;

        public string ShortTitle => (Achievement != null) ? EmployeeAchievement.Achievement.ShortTitle : EmployeeAchievement.ShortTitle;

        public string Description => EmployeeAchievement.Description;

        public int? YearBegin => EmployeeAchievement.YearBegin;

        public int? YearEnd => EmployeeAchievement.YearEnd;

        public bool IsTitle => EmployeeAchievement.IsTitle;

        public string DocumentURL => EmployeeAchievement.DocumentURL;

        public string TitleSuffix => EmployeeAchievement.TitleSuffix;

        public int? Hours => EmployeeAchievement.Hours;

        public IAchievement Achievement => EmployeeAchievement.Achievement;

        public IAchievementType AchievementType => (EmployeeAchievement.Achievement != null) ? EmployeeAchievement.Achievement.AchievementType : EmployeeAchievement.AchievementType;

        #endregion
    }
}
