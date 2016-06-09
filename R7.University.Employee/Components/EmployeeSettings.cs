//
//  EmployeeSettings.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2016 Roman M. Yagodin
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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.UI.Modules;
using R7.DotNetNuke.Extensions.Modules;
using R7.University.Components;

namespace R7.University.Employee.Components
{
    /// <summary>
    /// Provides strong typed access to settings used by module
    /// </summary>
    public class EmployeeSettings : SettingsWrapper
    {
        public EmployeeSettings ()
        {
        }

        public EmployeeSettings (IModuleControl module) : base (module)
        {
        }

        public EmployeeSettings (ModuleInfo module) : base (module)
        {
        }

        #region Properties for settings

        private int? employeeId;

        /// <summary>
        /// Gets or sets the EmployeeID setting value. 
        /// Use <see cref="EmployeePortalModuleBase.GetEmployee()"/> 
        /// and <see cref="EmployeePortalModuleBase.GetEmployeeId()"/>
        /// to get employee info in the view contols.
        /// </summary>
        /// <value>The employee Id.</value>
        public int EmployeeID
        {
            get { 
                if (employeeId == null)
                    employeeId = ReadSetting<int> ("Employee_EmployeeID", Null.NullInteger); 
				
                return employeeId.Value;
            }
            set { 
                WriteModuleSetting<int> ("Employee_EmployeeID", value); 
                employeeId = value;
            }
        }

        public bool ShowCurrentUser
        {
            get { return ReadSetting<bool> ("Employee_ShowCurrentUser", false); }
            set { WriteModuleSetting<bool> ("Employee_ShowCurrentUser", value); }
        }

        public bool AutoTitle
        {
            get { return ReadSetting<bool> ("Employee_AutoTitle", true); }
            set { WriteModuleSetting<bool> ("Employee_AutoTitle", value); }
        }

        public int PhotoWidth
        {
			get { return ReadSetting<int> ("Employee_PhotoWidth", UniversityConfig.Instance.EmployeePhoto.DefaultWidth); }
            set { WriteTabModuleSetting<int> ("Employee_PhotoWidth", value); }
        }

        #endregion
    }
}

