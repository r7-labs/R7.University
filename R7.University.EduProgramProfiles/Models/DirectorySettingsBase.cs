//
//  DirectorySettingsBase.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2018 Roman M. Yagodin
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
using R7.University.Models;

namespace R7.University.EduProgramProfiles.Models
{
    [Serializable]
    public abstract class DirectorySettingsBase
    {
        public virtual string EduLevels { get; set; } = string.Empty;

        public IEnumerable<int> EduLevelIds
        {
            get {
                return EduLevels
                    .Split (new [] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select (s => int.Parse (s));
            }
            set {
                EduLevels = string.Join (";", value);
            }
        }

        public virtual int? DivisionId { get; set; }

        public virtual DivisionLevel DivisionLevel { get; set; } = DivisionLevel.EduProgram;
    }
}

