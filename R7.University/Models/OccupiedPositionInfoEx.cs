//
//  OccupiedPositionInfoEx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
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
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.UI.Modules;
using R7.University.ViewModels;

namespace R7.University.Models
{
    [TableName ("vw_University_OccupiedPositions")]
    [PrimaryKey ("OccupiedPositionID", AutoIncrement = false)]
    public class OccupiedPositionInfoEx //: OccupiedPositionInfo
    {
        public int OccupiedPositionID { get; set; }

        public int PositionID { get; set; }

        public int DivisionID { get; set; }

        public int EmployeeID { get; set; }

        public bool IsPrime { get; set; }

        public string TitleSuffix { get; set; }

        #region Extended (Position and Division) properties

        public string PositionShortTitle { get; set; }

        public string PositionTitle { get; set; }

        public string DivisionShortTitle { get; set; }

        public string DivisionTitle { get; set; }

        public int PositionWeight { get; set; }

        public string HomePage { get; set; }

        public int? ParentDivisionID { get; set; }

        public bool IsTeacher { get; set; }

        #endregion

        public string FormatDivisionLink (IModuleControl module)
        {
            // do not display division title for high-level divisions
            if (ParentDivisionID != null) {
                var strDivision = FormatHelper.FormatShortTitle (DivisionShortTitle, DivisionTitle);
                if (!string.IsNullOrWhiteSpace (HomePage))
                    strDivision = string.Format ("<a href=\"{0}\">{1}</a>", 
                        R7.University.Utilities.Utils.FormatURL (module, HomePage, false), strDivision);

                return strDivision;
            }
              
            return string.Empty;
        }
    }
}

