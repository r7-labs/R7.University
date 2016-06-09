//
//  DivisionSettings.cs
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

namespace R7.University.Division.Components
{
    /// <summary>
    /// Provides strong typed access to settings used by module
    /// </summary>
    public class DivisionSettings : SettingsWrapper
    {
        public DivisionSettings ()
        {
        }

        public DivisionSettings (IModuleControl module) : base (module)
        {
        }

        public DivisionSettings (ModuleInfo module) : base (module)
        {
        }

        #region Properties for settings

        private int? divisionId;

        /// <summary>
        /// Division ID
        /// </summary>
        public int DivisionID
        {
            get {
                if (divisionId == null)
                    divisionId = ReadSetting<int> ("Division_DivisionID", Null.NullInteger); 
			
                return divisionId.Value;
            }
            set { 
                WriteModuleSetting<int> ("Division_DivisionID", value); 
                divisionId = value;
            }
        }

        #endregion
    }
}

