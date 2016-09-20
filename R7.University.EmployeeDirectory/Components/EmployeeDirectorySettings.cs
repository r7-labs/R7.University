//
//  EmployeeDirectorySettings.cs
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
using System.Linq;
using DotNetNuke.Entities.Modules;
using DotNetNuke.UI.Modules;
using R7.DotNetNuke.Extensions.Modules;

namespace R7.University.EmployeeDirectory.Components
{
    /// <summary>
    /// Provides strong typed access to settings used by module
    /// </summary>
    public class EmployeeDirectorySettings: SettingsWrapper
    {
        public EmployeeDirectorySettings ()
        {
        }

        public EmployeeDirectorySettings (IModuleControl module) : base (module)
        {
        }

        public EmployeeDirectorySettings (ModuleInfo module) : base (module)
        {
        }

        #region Module settings

        public EmployeeDirectoryMode Mode
        {
            get { return ReadSetting<EmployeeDirectoryMode> ("EmployeeDirectory_Mode", EmployeeDirectoryMode.Search); }
            set { WriteModuleSetting<EmployeeDirectoryMode> ("EmployeeDirectory_Mode", value); }
        }

        public IList<int> EduLevels
        {
            get {
                return ReadSetting<string> ("EmployeeDirectory_EduLevels", string.Empty)
                    .Split (new [] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList ()
                    .ConvertAll (s => int.Parse (s));
            }
            set {
                WriteModuleSetting<string> ("EmployeeDirectory_EduLevels", string.Join (";", value));
            }
        }

        public bool ShowAllTeachers
        {
            get { return ReadSetting<bool> ("EmployeeDirectory_ShowAllTeachers", false); }
            set { WriteModuleSetting<bool> ("EmployeeDirectory_ShowAllTeachers", value); }
        }

        #endregion
    }
}

