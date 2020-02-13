//
//  EmployeeSettings.cs
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

namespace R7.University.Employees.Models
{
    /// <summary>
    /// Provides strong typed access to settings used by module
    /// </summary>
    [Serializable]
    public class EmployeeSettings
    {
        public EmployeeSettings ()
        {
            if (HttpContext.Current != null) {
                PhotoWidth = UniversityConfig.Instance.EmployeePhoto.DefaultWidth;
            }
        }
 
        /// <summary>
        /// Gets or sets the EmployeeID setting value. 
        /// Use <see cref="EmployeePortalModuleBase.GetEmployee()"/> 
        /// and <see cref="EmployeePortalModuleBase.GetEmployeeId()"/>
        /// to get employee info in the view contols.
        /// </summary>
        /// <value>The employee Id.</value>
        // TODO: Convert to Nullable<int>
        [ModuleSetting (Prefix = "Employee_")]
        public int EmployeeID { get; set; } = Null.NullInteger;

        [ModuleSetting (Prefix = "Employee_")]
        public bool ShowCurrentUser { get; set; } = false;

        [TabModuleSetting (Prefix = "Employee_")]
        public bool AutoTitle { get; set; } = true;

        // TODO: Make PhotoWidth settings nullable
        [TabModuleSetting (Prefix = "Employee_")]
        public int PhotoWidth { get; set; }
    }
}
