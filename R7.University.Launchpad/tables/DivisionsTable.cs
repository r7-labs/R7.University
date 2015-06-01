//
// DivisionsTable.cs
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
using System.Web.UI;
using DotNetNuke.Common.Utilities;

namespace R7.University.Launchpad
{
    public class DivisionsTable: LaunchpadTableBase
	{
        public DivisionsTable (): base ("divisions")
        {

        }

        public override DataTable GetDataTable (LaunchpadPortalModuleBase module, string filter)
        {
            var dt = new DataTable ();
            DataRow dr;

            dt.Columns.Add (new DataColumn ("DivisionID", typeof(int)));
            dt.Columns.Add (new DataColumn ("ParentDivisionID", typeof(int)));
            dt.Columns.Add (new DataColumn ("DivisionTermID", typeof(int)));
            dt.Columns.Add (new DataColumn ("Title", typeof(string)));
            dt.Columns.Add (new DataColumn ("ShortTitle", typeof(string)));
            dt.Columns.Add (new DataColumn ("HomePage", typeof(string)));
            dt.Columns.Add (new DataColumn ("DocumentUrl", typeof(string)));
            dt.Columns.Add (new DataColumn ("Location", typeof(string)));
            dt.Columns.Add (new DataColumn ("Phone", typeof(string)));
            dt.Columns.Add (new DataColumn ("Fax", typeof(string)));
            dt.Columns.Add (new DataColumn ("Email", typeof(string)));
            dt.Columns.Add (new DataColumn ("SecondaryEmail", typeof(string)));
            dt.Columns.Add (new DataColumn ("WebSite", typeof(string)));
            dt.Columns.Add (new DataColumn ("WorkingHours", typeof(string)));
            dt.Columns.Add (new DataColumn ("CreatedByUserID", typeof(int)));
            dt.Columns.Add (new DataColumn ("CreatedOnDate", typeof(DateTime)));
            dt.Columns.Add (new DataColumn ("LastModifiedByUserID", typeof(int)));
            dt.Columns.Add (new DataColumn ("LastModifiedOnDate", typeof(DateTime)));

            foreach (DataColumn column in dt.Columns)
                column.AllowDBNull = true;

            foreach (var division in module.LaunchpadController.GetObjects<DivisionInfo>())
            {
                dr = dt.NewRow ();
                var i = 0;
                dr [i++] = division.DivisionID;
                dr [i++] = division.ParentDivisionID ?? Null.NullInteger;
                dr [i++] = division.DivisionTermID ?? Null.NullInteger;
                dr [i++] = division.Title;
                dr [i++] = division.ShortTitle;
                dr [i++] = division.HomePage;
                dr [i++] = division.DocumentUrl;
                dr [i++] = division.Location;
                dr [i++] = division.Phone;
                dr [i++] = division.Fax;
                dr [i++] = division.Email;
                dr [i++] = division.SecondaryEmail;
                dr [i++] = division.WebSite;
                dr [i++] = division.WorkingHours;
                dr [i++] = division.CreatedByUserID;
                dr [i++] = division.CreatedOnDate;
                dr [i++] = division.LastModifiedByUserID;
                dr [i++] = division.LastModifiedOnDate;

                dt.Rows.Add (dr);
            }

            return dt;
        }
	}
}

