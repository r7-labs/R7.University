//
//  ContingentDirectorySettings.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2017 Roman M. Yagodin
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
using DotNetNuke.Entities.Modules.Settings;

namespace R7.University.EduProgramProfiles.Models
{
    [Serializable]
    public class ContingentDirectorySettings: DirectorySettingsBase
    {
        [ModuleSetting (Prefix = "ContingentDirectory_")]
        public override string EduLevels { get; set; } = string.Empty;

        [ModuleSetting (Prefix = "ContingentDirectory_")]
        public override int? DivisionId { get; set; }

        [ModuleSetting (Prefix = "ContingentDirectory_")]
        public override DivisionLevel DivisionLevel { get; set; } = DivisionLevel.EduProgram;

        [ModuleSetting (Prefix = "ContingentDirectory_")]
        // TODO: Default value should be null
        public ContingentDirectoryMode? Mode { get; set; } = ContingentDirectoryMode.Actual;
    }

    public class ContingentDirectorySettingsRepository: SettingsRepository<ContingentDirectorySettings>
    {
    }
}
