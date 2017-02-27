//
//  EmployeeAchievementViewModelBase.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2017 Roman M. Yagodin
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

namespace R7.University.ViewModels
{
    public abstract class EmployeeAchievementViewModelBase: IEmployeeAchievement
    {
        public IEmployeeAchievement Model { get; protected set; }

        protected EmployeeAchievementViewModelBase (IEmployeeAchievement model)
        {
            Model = model;
        }

        #region IEmployeeAchievement implementation

        public int EmployeeAchievementID
        {
            get { return Model.EmployeeAchievementID; }
            set { throw new InvalidOperationException (); }
        }

        public int EmployeeID
        {
            get { return Model.EmployeeID; } 
            set { throw new InvalidOperationException (); }
        }

        public int? AchievementID
        {
            get { return Model.AchievementID; }
            set { throw new InvalidOperationException (); }
        }

        public int? AchievementTypeId
        {
            get { return Model.AchievementTypeId; }
            set { throw new InvalidOperationException (); }
        }

        public string Title
        {
            get { return (Achievement != null) ? Model.Achievement.Title : Model.Title; }
            set { throw new InvalidOperationException (); }
        }

        public string ShortTitle
        {
            get { return (Achievement != null) ? Model.Achievement.ShortTitle : Model.ShortTitle; }
            set { throw new InvalidOperationException (); }
        }

        public string Description
        {
            get { return Model.Description; }
            set { throw new InvalidOperationException (); }
        }

        public int? YearBegin
        {
            get { return Model.YearBegin; }
            set { throw new InvalidOperationException (); }
        }

        public int? YearEnd
        {
            get { return Model.YearEnd; }
            set { throw new InvalidOperationException (); }
        }

        public bool IsTitle
        {
            get { return Model.IsTitle; }
            set { throw new InvalidOperationException (); }
        }

        public string DocumentURL
        {
            get { return Model.DocumentURL; }
            set { throw new InvalidOperationException (); }
        }

        public string TitleSuffix
        {
            get { return Model.TitleSuffix; }
            set { throw new InvalidOperationException (); }
        }

        public AchievementTypeInfo AchievementType
        {
            get { return (Model.Achievement != null) ? Model.Achievement.AchievementType : Model.AchievementType; }
            set { throw new InvalidOperationException (); }
        }

        public AchievementInfo Achievement
        {
            get { return Model.Achievement; }
            set { throw new InvalidOperationException (); }
        }

        #endregion
    }
}
