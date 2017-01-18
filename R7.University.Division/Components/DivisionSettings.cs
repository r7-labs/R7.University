//
//  DivisionSettings.cs
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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules.Settings;

namespace R7.University.Division.Components
{
    /// <summary>
    /// Provides strong typed access to settings used by module
    /// </summary>
    [Serializable]
    public class DivisionSettings
    {
        /// <summary>
        /// Division ID
        /// </summary>
        // TODO: Convert to Nullable<int>
        [ModuleSetting (Prefix = "Division_")]
        public int DivisionID { get; set; } = Null.NullInteger;

        /// <value><c>true</c> if show address; otherwise, <c>false</c>.</value>
        [TabModuleSetting (Prefix = "Division_")]
        public bool ShowAddress { get; set; } = false;
    }
}

