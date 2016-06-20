//
//  R7.University.EduProgramSettings.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
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
using DotNetNuke.Entities.Modules;
using DotNetNuke.UI.Modules;
using R7.DotNetNuke.Extensions.Modules;

namespace R7.University.EduProgram.Components
{
    /// <summary>
    /// Provides strong typed access to settings used by module
    /// </summary>
    public class EduProgramSettings: SettingsWrapper
    {
        public EduProgramSettings ()
        {
        }

        public EduProgramSettings (IModuleControl module): base (module)
        {
        }

        public EduProgramSettings (ModuleInfo module): base (module)
        {
        }

        #region Properties for settings

        #endregion
    }
}

