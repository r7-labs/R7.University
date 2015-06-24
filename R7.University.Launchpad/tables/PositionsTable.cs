//
// PositionsTable.cs
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

namespace R7.University.Launchpad
{
    public class PositionsTable: LaunchpadTableBase
	{
        public PositionsTable (): base ("positions")
        {

        }

        public override DataTable GetDataTable (LaunchpadPortalModuleBase module, string search)
        {
            var dt = new DataTable ();
            DataRow dr;

            dt.Columns.Add (new DataColumn ("PositionID", typeof(int)));
            dt.Columns.Add (new DataColumn ("Title", typeof(string)));
            dt.Columns.Add (new DataColumn ("ShortTitle", typeof(string)));
            dt.Columns.Add (new DataColumn ("Weight", typeof(int)));
            dt.Columns.Add (new DataColumn ("IsTeacher", typeof(bool)));

            foreach (DataColumn column in dt.Columns)
                column.AllowDBNull = true;

            var positions = module.LaunchpadController.FindObjects<PositionInfo> (false,
                @"WHERE CONCAT([Title], ' ', [ShortTitle]) LIKE N'%{0}%'", search);

            foreach (var position in positions)
            {
                dr = dt.NewRow ();
                dr [0] = position.PositionID;
                dr [1] = position.Title;
                dr [2] = position.ShortTitle;
                dr [3] = position.Weight;
                dr [4] = position.IsTeacher;

                dt.Rows.Add (dr);
            }

            return dt;
        }
	}
}

