//
// EmployeeViewModel.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2016 Roman M. Yagodin
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

