//
//  EduProgramDirectorySettings.cs
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
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Entities.Modules;
using DotNetNuke.UI.Modules;
using R7.DotNetNuke.Extensions.Modules;

namespace R7.University.EduProgramDirectory.Components
{
    /// <summary>
    /// Provides strong typed access to settings used by module
    /// </summary>
    public partial class EduProgramDirectorySettings : SettingsWrapper
    {
        public EduProgramDirectorySettings ()
        {
        }

        public EduProgramDirectorySettings (IModuleControl module) : base (module)
        {
        }

        public EduProgramDirectorySettings (ModuleInfo module) : base (module)
        {
        }

        #region Module settings

        public int? DivisionId
        {
            get { return ReadSetting<int?> ("EduProgramDirectory_DivisionId", null); }
            set { WriteModuleSetting<int?> ("EduProgramDirectory_DivisionId", value); }
        }

        public IList<string> EduLevels
        {
            get {
                return ReadSetting<string> ("EduProgramDirectory_EduLevels", string.Empty)
                    .Split (new [] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList ();
            }
            set {
                WriteModuleSetting<string> ("EduProgramDirectory_EduLevels", string.Join (";", value));
            }
        }

        #endregion

        #region Tabmodule settings

        public IList<string> Columns
        {
            get {
                return ReadSetting<string> ("EduProgramDirectory_Columns", string.Empty)
                    .Split (new [] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList ();
            }
            set {
                WriteTabModuleSetting<string> ("EduProgramDirectory_Columns", string.Join (";", value));
            }
        }

        #endregion
    }
}

