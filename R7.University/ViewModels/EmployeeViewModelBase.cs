//
//  EmployeeViewModelBase.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2018 Roman M. Yagodin
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
using System.Collections.Generic;
using R7.University.Models;

namespace R7.University.ViewModels
{
    public abstract class EmployeeViewModelBase: IEmployee
    {
        public IEmployee Employee { get; protected set; }

        protected EmployeeViewModelBase (IEmployee employee)
        {
            Employee = employee;
        }

        #region IEmployee implementation

        public int EmployeeID => Employee.EmployeeID;

        public int? UserID => Employee.UserID;

        public int? PhotoFileID => Employee.PhotoFileID;

        public int? AltPhotoFileId => Employee.AltPhotoFileId;

        public string Phone => Employee.Phone;

        public string CellPhone => Employee.CellPhone;

        public string Fax => Employee.Fax;

        public string LastName => Employee.LastName;

        public string FirstName => Employee.FirstName;

        public string OtherName => Employee.OtherName;

        public string Email => Employee.Email;

        public string SecondaryEmail => Employee.SecondaryEmail;

        public string WebSite => Employee.WebSite;

        public string WebSiteLabel => Employee.WebSiteLabel;

        public string Messenger => Employee.Messenger;

        public string WorkingPlace => Employee.WorkingPlace;

        public string WorkingHours => Employee.WorkingHours;

        public string Biography => Employee.Biography;

        public int? ExperienceYears => Employee.ExperienceYears;

        public int? ExperienceYearsBySpec => Employee.ExperienceYearsBySpec;

        public DateTime? StartDate => Employee.StartDate;

        public DateTime? EndDate => Employee.EndDate;

        public int LastModifiedByUserId => Employee.LastModifiedByUserId;

        public DateTime LastModifiedOnDate => Employee.LastModifiedOnDate;

        public int CreatedByUserId => Employee.CreatedByUserId;

        public DateTime CreatedOnDate => Employee.CreatedOnDate;

        public bool ShowBarcode => Employee.ShowBarcode;

        public int? ScienceIndexAuthorId => Employee.ScienceIndexAuthorId;

        public ICollection<EmployeeAchievementInfo> Achievements => Employee.Achievements;

        public ICollection<EmployeeDisciplineInfo> Disciplines => Employee.Disciplines;

        public ICollection<OccupiedPositionInfo> Positions => Employee.Positions;

        #endregion
    }
}

