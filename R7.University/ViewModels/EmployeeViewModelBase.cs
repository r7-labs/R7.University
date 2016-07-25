//
//  EmployeeViewModelBase.cs
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
using R7.University.Data;
using R7.University.Models;

namespace R7.University.ViewModels
{
    public abstract class EmployeeViewModelBase: IEmployee
    {
        public IEmployee Model { get; protected set; }

        protected EmployeeViewModelBase (IEmployee model)
        {
            Model = model;
        }

        #region IEmployee implementation

        public int EmployeeID
        {
            get { return Model.EmployeeID; }
            set { throw new InvalidOperationException (); }
        }

        public int? UserID
        {
            get { return Model.UserID; }
            set { throw new InvalidOperationException (); }
        }

        public int? PhotoFileID
        {
            get { return Model.PhotoFileID; }
            set { throw new InvalidOperationException (); }
        }

        public string Phone
        {
            get { return Model.Phone; }
            set { throw new InvalidOperationException (); }
        }

        public string CellPhone
        {
            get { return Model.CellPhone; }
            set { throw new InvalidOperationException (); }
        }

        public string Fax
        {
            get { return Model.Fax; }
            set { throw new InvalidOperationException (); }
        }

        public string LastName
        {
            get { return Model.LastName; }
            set { throw new InvalidOperationException (); }
        }

        public string FirstName
        {
            get { return Model.FirstName; }
            set { throw new InvalidOperationException (); }
        }

        public string OtherName
        {
            get { return Model.OtherName; }
            set { throw new InvalidOperationException (); }
        }

        public string Email
        {
            get { return Model.Email; }
            set { throw new InvalidOperationException (); }
        }

        public string SecondaryEmail
        {
            get { return Model.SecondaryEmail; }
            set { throw new InvalidOperationException (); }
        }

        public string WebSite
        {
            get { return Model.WebSite; }
            set { throw new InvalidOperationException (); }
        }

        public string WebSiteLabel
        {
            get { return Model.WebSiteLabel; }
            set { throw new InvalidOperationException (); }
        }

        public string Messenger
        {
            get { return Model.Messenger; }
            set { throw new InvalidOperationException (); }
        }

        public string WorkingPlace
        {
            get { return Model.WorkingPlace; }
            set { throw new InvalidOperationException (); }
        }

        public string WorkingHours
        {
            get { return Model.WorkingHours; }
            set { throw new InvalidOperationException (); }
        }

        public string Biography
        {
            get { return Model.Biography; }
            set { throw new InvalidOperationException (); }
        }

        public int? ExperienceYears
        {
            get { return Model.ExperienceYears; }
            set { throw new InvalidOperationException (); }
        }

        public int? ExperienceYearsBySpec
        {
            get { return Model.ExperienceYearsBySpec; }
            set { throw new InvalidOperationException (); }
        }

        public DateTime? StartDate
        {
            get { return Model.StartDate; }
            set { throw new InvalidOperationException (); }
        }

        public DateTime? EndDate
        {
            get { return Model.EndDate; }
            set { throw new InvalidOperationException (); }
        }

        public int LastModifiedByUserID
        {
            get { return Model.LastModifiedByUserID; }
            set { throw new InvalidOperationException (); }
        }

        public DateTime LastModifiedOnDate
        {
            get { return Model.LastModifiedOnDate; }
            set { throw new InvalidOperationException (); }
        }

        public int CreatedByUserID
        {
            get { return Model.CreatedByUserID; }
            set { throw new InvalidOperationException (); }
        }

        public DateTime CreatedOnDate
        {
            get { return Model.CreatedOnDate; }
            set { throw new InvalidOperationException (); }
        }

        public bool ShowBarcode
        {
            get { return Model.ShowBarcode; }
            set { throw new InvalidOperationException (); }
        }

        public ICollection<EmployeeAchievementInfo> Achievements
        {
            get { return Model.Achievements; }
            set { throw new InvalidOperationException (); }
        }

        public ICollection<EmployeeDisciplineInfo> Disciplines
        {
            get { return Model.Disciplines; }
            set { throw new InvalidOperationException (); }
        }

        public ICollection<OccupiedPositionInfo> Positions
        {
            get { return Model.Positions; }
            set { throw new InvalidOperationException (); }
        }

        #endregion
    }
}

