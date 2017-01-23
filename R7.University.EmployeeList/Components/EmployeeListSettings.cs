//
//  EmployeeListSettings.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2017 Roman M. Yagodin
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
using System.Web;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules.Settings;
using R7.University.Components;

namespace R7.University.EmployeeList.Components
{
    /// <summary>
    /// Provides strong typed access to settings used by module
    /// </summary>
    [Serializable]
    public class EmployeeListSettings
    {
        public EmployeeListSettings ()
        {
            if (HttpContext.Current != null) {
                PhotoWidth = UniversityConfig.Instance.EmployeePhoto.SquareDefaultWidth;
            }
        }

        /// <summary>
        /// Division's ID to show employees from
        /// </summary>
        // TODO: Convert to Nullable<int>
        [ModuleSetting (Prefix = "EmployeeList_")]
        public int DivisionID { get; set; } = Null.NullInteger;

        /// <summary>
        /// Indicates employee info extraction method
        /// </summary>
        /// <value><c>true</c> if resursively include employees from subordinate divisions; otherwise, <c>false</c>.</value>
        [TabModuleSetting (Prefix = "EmployeeList_")]
        public bool IncludeSubdivisions { get; set; } = false;

        [TabModuleSetting (Prefix = "EmployeeList_")]
        public bool HideHeadEmployee { get; set; } = false;

        /// <summary>
        /// Gets or sets the type of the sort.
        /// </summary>
        /// <value>The type of the sort.</value>
        [TabModuleSetting (Prefix = "EmployeeList_")]
        public int SortType { get; set; } = 0;

        [TabModuleSetting (Prefix = "EmployeeList_")]
        public int PhotoWidth { get; set; }
    }
}
