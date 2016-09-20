//
//  EduProgramProfileDirectorySettings.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
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

namespace R7.University.EduProgramProfileDirectory.Components
{
    /// <summary>
    /// Provides strong typed access to settings used by module
    /// </summary>
    public class EduProgramProfileDirectorySettings : SettingsWrapper
    {
        public EduProgramProfileDirectorySettings ()
        {
        }

        public EduProgramProfileDirectorySettings (IModuleControl module) : base (module)
        {
        }

        public EduProgramProfileDirectorySettings (ModuleInfo module) : base (module)
        {
        }

        #region Properties for settings

        public IList<int> EduLevels
        {
            get {
                return ReadSetting<string> ("EduProgramProfileDirectory_EduLevels", string.Empty)
                    .Split (new [] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList ()
                    .ConvertAll (s => int.Parse (s));
            }
            set {
                WriteModuleSetting<string> ("EduProgramProfileDirectory_EduLevels", string.Join (";", value));
            }
        }

        public EduProgramProfileDirectoryMode? Mode
        {
            get { return ReadSetting<EduProgramProfileDirectoryMode?> ("EduProgramProfileDirectory_Mode", null); }
            set { WriteModuleSetting<EduProgramProfileDirectoryMode?> ("EduProgramProfileDirectory_Mode", value); }
        }

        public int? DivisionId {
            get { return ReadSetting<int?> ("EduProgramProfileDirectory_DivisionId", null); }
            set { WriteModuleSetting<int?> ("EduProgramProfileDirectory_DivisionId", value); }
        }

        public DivisionLevel DivisionLevel {
            get { return ReadSetting<DivisionLevel> ("EduProgramProfileDirectory_DivisionLevel", DivisionLevel.EduProgram); }
            set { WriteModuleSetting<DivisionLevel> ("EduProgramProfileDirectory_DivisionLevel", value); }
        }

        #endregion
    }
}

