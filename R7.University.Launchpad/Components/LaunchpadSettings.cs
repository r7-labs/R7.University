//
//  LaunchpadSettings.cs
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
using System.Collections.Generic;
using DotNetNuke.UI.Modules;
using R7.DotNetNuke.Extensions.Modules;
using R7.DotNetNuke.Extensions.Utilities;

namespace R7.University.Launchpad.Components
{
    /// <summary>
    /// Provides strong typed access to settings used by module
    /// </summary>
    public class LaunchpadSettings : SettingsWrapper
    {
        public LaunchpadSettings ()
        {
        }

        public LaunchpadSettings (IModuleControl module) : base (module)
        {
        }

        #region Properties for settings

        public int PageSize
        {
            get { return ReadSetting<int> ("Launchpad_PageSize", 20); }
            set { WriteTabModuleSetting<int> ("Launchpad_PageSize", value); }
        }

        private List<string> tables;

        public List<string> Tables
        {
            get { 
                if (tables == null) {
                    tables = new List<string> ();
                    tables.AddRange (
                        ReadSetting<string> ("Launchpad_Tables", string.Empty)
						.Split (new [] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                }
				
                return tables;
            }
            set { 
                WriteTabModuleSetting<string> ("Launchpad_Tables", TextUtils.FormatList (";", value.ToArray ())); 
            }
        }

        #endregion
	
    }
}

