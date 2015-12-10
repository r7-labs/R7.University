//
// EmployeesTable.cs
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
using System.Web.UI.WebControls;
using System.Data;
using System.Linq;
using DotNetNuke.Entities.Modules;
using System.Threading;
using R7.University;
using DotNetNuke.Common.Utilities;

namespace R7.University.Launchpad
{
    public class EmployeesTable: LaunchpadTableBase
    {
        public EmployeesTable (): base ("Employees")
        {
        }

        public override DataTable GetDataTable (LaunchpadPortalModuleBase module, string search)
        {
            var dt = new DataTable ();
            DataRow dr;

            dt.Columns.Add (new DataColumn ("EmployeeID", typeof(int)));
            dt.Columns.Add (new DataColumn ("UserID", typeof(int)));
            dt.Columns.Add (new DataColumn ("PhotoFileID", typeof(int)));
            dt.Columns.Add (new DataColumn ("Phone", typeof(string)));
            dt.Columns.Add (new DataColumn ("CellPhone", typeof(string)));
            dt.Columns.Add (new DataColumn ("Fax", typeof(string)));
            dt.Columns.Add (new DataColumn ("LastName", typeof(string)));
            dt.Columns.Add (new DataColumn ("FirstName", typeof(string)));
            dt.Columns.Add (new DataColumn ("OtherName", typeof(string)));
            dt.Columns.Add (new DataColumn ("Email", typeof(string)));
            dt.Columns.Add (new DataColumn ("SecondaryEmail", typeof(string)));
            dt.Columns.Add (new DataColumn ("WebSite", typeof(string)));
            dt.Columns.Add (new DataColumn ("Messenger", typeof(string)));
            dt.Columns.Add (new DataColumn ("WorkingPlace", typeof(string)));
            dt.Columns.Add (new DataColumn ("WorkingHours", typeof(string)));
            dt.Columns.Add (new DataColumn ("Biography", typeof(string)));
            dt.Columns.Add (new DataColumn ("ExperienceYears", typeof(int)));
            dt.Columns.Add (new DataColumn ("ExperienceYearsBySpec", typeof(int)));
            dt.Columns.Add (new DataColumn ("IsPublished", typeof(bool)));
            //dt.Columns.Add (new DataColumn ("IsDeleted", typeof(bool)));

            // TODO: Remove audit fields
            dt.Columns.Add (new DataColumn ("CreatedByUserID", typeof(int)));
            dt.Columns.Add (new DataColumn ("CreatedOnDate", typeof(DateTime)));
            dt.Columns.Add (new DataColumn ("LastModifiedByUserID", typeof(int)));
            dt.Columns.Add (new DataColumn ("LastModifiedOnDate", typeof(DateTime)));

            foreach (DataColumn column in dt.Columns)
                column.AllowDBNull = true;

            var employees = module.LaunchpadController.FindObjects<EmployeeInfo> (
                @"WHERE CONCAT([LastName], ' ', [FirstName], ' ', [OtherName], ' ',
                [Phone], ' ', [CellPhone], ' ', [Fax], ' ', 
                [Email], ' ', [SecondaryEmail], ' ', [WebSite], ' ',
                [WorkingHours]) LIKE N'%{0}%'", search, false);

            foreach (var employee in employees)
            {
                dr = dt.NewRow ();
                var i = 0;
                dr [i++] = employee.EmployeeID;
                dr [i++] = employee.UserID ?? Null.NullInteger;
                dr [i++] = employee.PhotoFileID ?? Null.NullInteger;
                dr [i++] = employee.Phone;
                dr [i++] = employee.CellPhone;
                dr [i++] = employee.Fax;
                dr [i++] = employee.LastName;
                dr [i++] = employee.FirstName;
                dr [i++] = employee.OtherName;
                dr [i++] = employee.Email;
                dr [i++] = employee.SecondaryEmail;
                dr [i++] = Utils.FormatList (": ", employee.WebSiteLabel, employee.WebSite);
                dr [i++] = employee.Messenger;
                dr [i++] = employee.WorkingPlace;
                dr [i++] = employee.WorkingHours;

                dr [i++] = !string.IsNullOrWhiteSpace (employee.Biography) ? 
                    HtmlUtils.Shorten (employee.Biography, 16, "...") : string.Empty;
                
                dr [i++] = employee.ExperienceYears ?? Null.NullInteger;
                dr [i++] = employee.ExperienceYearsBySpec ?? Null.NullInteger;
                dr [i++] = employee.IsPublished;
                //dr [i++] = employee.IsDeleted;

                // TODO: Remove audit fields
                dr [i++] = employee.CreatedByUserID;
                dr [i++] = employee.CreatedOnDate;
                dr [i++] = employee.LastModifiedByUserID;
                dr [i++] = employee.LastModifiedOnDate;

                dt.Rows.Add (dr);
            }

            return dt;

        }
    }
}
