//
//  EmployeeViewModel.cs
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
using DotNetNuke.Common.Utilities;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.Data;
using R7.University.Models;

namespace R7.University.Launchpad.ViewModels
{
    public class EmployeeViewModel: IEmployee
    {
        public IEmployee Model { get; protected set; }

        public EmployeeViewModel (IEmployee model)
        {
            Model = model;
        }

        #region IEmployee implementation

        public int EmployeeID
        {
            get { return Model.EmployeeID; }
            set { throw new NotImplementedException (); }
        }

        public int? UserID
        {
            get { return Model.UserID; }
            set { throw new NotImplementedException (); }
        }

        public int? PhotoFileID
        {
            get { return Model.PhotoFileID; }
            set { throw new NotImplementedException (); }
        }

        public string Phone
        {
            get { return Model.Phone; }
            set { throw new NotImplementedException (); }
        }

        public string CellPhone
        {
            get { return Model.CellPhone; }
            set { throw new NotImplementedException (); }
        }

        public string Fax
        {
            get { return Model.Fax; }
            set { throw new NotImplementedException (); }
        }

        public string LastName
        {
            get { return Model.LastName; }
            set { throw new NotImplementedException (); }
        }

        public string FirstName
        {
            get { return Model.FirstName; }
            set { throw new NotImplementedException (); }
        }

        public string OtherName
        {
            get { return Model.OtherName; }
            set { throw new NotImplementedException (); }
        }

        public string Email
        {
            get { return Model.Email; }
            set { throw new NotImplementedException (); }
        }

        public string SecondaryEmail
        {
            get { return Model.SecondaryEmail; }
            set { throw new NotImplementedException (); }
        }

        public string WebSite
        {
            get { return Model.WebSite; }
            set { throw new NotImplementedException (); }
        }

        public string WebSiteLabel
        {
            get { return Model.WebSiteLabel; }
            set { throw new NotImplementedException (); }
        }

        public string Messenger
        {
            get { return Model.Messenger; }
            set { throw new NotImplementedException (); }
        }

        public string WorkingPlace
        {
            get { return Model.WorkingPlace; }
            set { throw new NotImplementedException (); }
        }

        public string WorkingHours
        {
            get { return Model.WorkingHours; }
            set { throw new NotImplementedException (); }
        }

        public string Biography
        {
            get { return Model.Biography; }
            set { throw new NotImplementedException (); }
        }

        public int? ExperienceYears
        {
            get { return Model.ExperienceYears; }
            set { throw new NotImplementedException (); }
        }

        public int? ExperienceYearsBySpec
        {
            get { return Model.ExperienceYearsBySpec; }
            set { throw new NotImplementedException (); }
        }

        public DateTime? StartDate
        {
            get { return Model.StartDate; }
            set { throw new NotImplementedException (); }
        }

        public DateTime? EndDate
        {
            get { return Model.EndDate; }
            set { throw new NotImplementedException (); }
        }

        public int LastModifiedByUserID
        {
            get { return Model.LastModifiedByUserID; }
            set { throw new NotImplementedException (); }
        }

        public DateTime LastModifiedOnDate
        {
            get { return Model.LastModifiedOnDate; }
            set { throw new NotImplementedException (); }
        }

        public int CreatedByUserID
        {
            get { return Model.CreatedByUserID; }
            set { throw new NotImplementedException (); }
        }

        public DateTime CreatedOnDate
        {
            get { return Model.CreatedOnDate; }
            set { throw new NotImplementedException (); }
        }

        public IList<IEmployeeAchievement> Achievements
        {
            get { return Model.Achievements; }
            set { throw new NotImplementedException (); }
        }

        public IList<IEmployeeDiscipline> Disciplines
        {
            get { return Model.Disciplines; }
            set { throw new NotImplementedException (); }
        }

        public IList<OccupiedPositionInfoEx> OccupiedPositions
        {
            get { return Model.OccupiedPositions; }
            set { throw new NotImplementedException (); }
        }

        #endregion

        #region Bindable properties

        public string WebSite_String
        {
            get { return TextUtils.FormatList (": ", Model.WebSiteLabel, Model.WebSite); }
        }

        public string Biography_String
        {
            get { 
                return !string.IsNullOrWhiteSpace (Model.Biography) ? 
                    HtmlUtils.Shorten (Model.Biography, 16, "...") : string.Empty;
            }
        }

        #endregion
    }
}

