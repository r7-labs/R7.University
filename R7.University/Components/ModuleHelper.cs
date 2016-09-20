//
//  ModuleHelper.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
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
using DotNetNuke.Entities.Modules;

namespace R7.University.Components
{
    public static class ModuleHelper
    {
        public static void UpdateModuleTitle (int tabModileId, string title)
        {
            var module = ModuleController.Instance.GetTabModule (tabModileId);
            if (module.ModuleTitle != title) {
                module.ModuleTitle = title;
                ModuleController.Instance.UpdateModule (module);
            }
        }
    }
}

