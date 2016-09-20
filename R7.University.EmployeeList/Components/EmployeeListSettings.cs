//
//  EmployeeListSettings.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2016 Roman M. Yagodin
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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.UI.Modules;
using R7.DotNetNuke.Extensions.Modules;
using R7.University.Components;

namespace R7.University.EmployeeList.Components
{
    /// <summary>
    /// Provides strong typed access to settings used by module
    /// </summary>
    public class EmployeeListSettings: SettingsWrapper
    {
        public EmployeeListSettings ()
        {
        }

        public EmployeeListSettings (IModuleControl module) : base (module)
        {
        }

        public EmployeeListSettings (ModuleInfo module) : base (module)
        {
        }

        #region Properties for settings

        // cached DivisionID value for frequent use
        private int? divisionId = null;

        /// <summary>
        /// Division's ID to show employees from
        /// </summary>
        public int DivisionID
        {
            get { 
                if (divisionId == null)
                    divisionId = ReadSetting<int> ("EmployeeList_DivisionID", Null.NullInteger);

                return divisionId.Value;
            }
            set { 
                WriteModuleSetting<int> ("EmployeeList_DivisionID", value);
                divisionId = value;
            }
        }

        /// <summary>
        /// Indicates employee info extraction method
        /// </summary>
        /// <value><c>true</c> if resursively include employees from subordinate divisions; otherwise, <c>false</c>.</value>
        public bool IncludeSubdivisions
        {
            get { return ReadSetting<bool> ("EmployeeList_IncludeSubdivisions", false); }
            set { WriteTabModuleSetting<bool> ("EmployeeList_IncludeSubdivisions", value); }
        }

        public bool HideHeadEmployee
        {
            get { return ReadSetting<bool> ("EmployeeList_HideHeadEmployee", false); }
            set { WriteTabModuleSetting<bool> ("EmployeeList_HideHeadEmployee", value); }
        }

        /// <summary>
        /// Gets or sets the type of the sort.
        /// </summary>
        /// <value>The type of the sort.</value>
        public int SortType
        {
            get { return ReadSetting<int> ("EmployeeList_SortType", 0); }
            set { WriteTabModuleSetting<int> ("EmployeeList_SortType", value); }
        }

        public int PhotoWidth
        {
			get { return ReadSetting<int> ("EmployeeList_PhotoWidth", UniversityConfig.Instance.EmployeePhoto.SquareDefaultWidth); }
            set { WriteTabModuleSetting<int> ("EmployeeList_PhotoWidth", value); }
        }

        #endregion
    }
}

