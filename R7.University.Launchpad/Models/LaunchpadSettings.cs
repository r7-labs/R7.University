//
//  LaunchpadSettings.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2017 Roman M. Yagodin
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
using DotNetNuke.Entities.Modules.Settings;
using R7.Dnn.Extensions.Utilities;

namespace R7.University.Launchpad.Models
{
    /// <summary>
    /// Provides strong typed access to settings used by module
    /// </summary>
    [Serializable]
    public class LaunchpadSettings
    {
        [TabModuleSetting (Prefix = "Launchpad_")]
        public int PageSize { get; set; } = 20;

        [ModuleSetting (Prefix = "Launchpad_", ParameterName = "Tables")]
        public string TablesInternal { get; set; } = string.Empty;

        private List<string> tables;
        public List<string> Tables
        {
            get {
                return tables ?? (tables = TablesInternal.Split (new [] { ';' }, StringSplitOptions.RemoveEmptyEntries).ToList ());
            }
            set {
                tables = value;
                TablesInternal = TextUtils.FormatList (";", value.ToArray ());
            }
        }
    }
}

