//
//  EmployeeAchievementViewModel.cs
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
using R7.University.Models;

namespace R7.University.Employee.ViewModels
{
    public class EmployeeAchievementViewModel: IEmployeeAchievement
    {
        public IEmployeeAchievement Model { get; protected set; }

        public EmployeeAchievementViewModel (IEmployeeAchievement model)
        {
            Model = model;
        }

        #region IEmployeeAchievement implementation

        public int EmployeeAchievementID
        {
            get { return Model.EmployeeAchievementID; }
            set { throw new NotImplementedException (); }
        }

        public int EmployeeID
        {
            get { return Model.EmployeeID; } 
            set { throw new NotImplementedException (); }
        }

        public int? AchievementID
        {
            get { return Model.AchievementID; }
            set { throw new NotImplementedException (); }
        }

        public string Title
        {
            get { return (AchievementID != null) ? Model.Achievement.Title : Model.Title; }
            set { throw new NotImplementedException (); }
        }

        public string ShortTitle
        {
            get { return (AchievementID != null) ? Model.Achievement.ShortTitle : Model.ShortTitle; }
            set { throw new NotImplementedException (); }
        }

        public string Description
        {
            get { return Model.Description; }
            set { throw new NotImplementedException (); }
        }

        public int? YearBegin
        {
            get { return Model.YearBegin; }
            set { throw new NotImplementedException (); }
        }

        public int? YearEnd
        {
            get { return Model.YearEnd; }
            set { throw new NotImplementedException (); }
        }

        public bool IsTitle
        {
            get { return Model.IsTitle; }
            set { throw new NotImplementedException (); }
        }

        public string DocumentURL
        {
            get { return Model.DocumentURL; }
            set { throw new NotImplementedException (); }
        }

        public string TitleSuffix
        {
            get { return Model.TitleSuffix; }
            set { throw new NotImplementedException (); }
        }

        public AchievementType? AchievementType
        {
            get { return (AchievementID != null) ? Model.Achievement.AchievementType : Model.AchievementType; }
            set { throw new NotImplementedException (); }
        }

        public AchievementInfo Achievement
        {
            get { return Model.Achievement; }
            set { throw new NotImplementedException (); }
        }

        #endregion
    }
}

