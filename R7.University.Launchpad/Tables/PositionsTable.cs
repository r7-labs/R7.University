//
//  PositionsTable.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2015 Roman M. Yagodin
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
using System.Data;
using DotNetNuke.Entities.Modules;
using R7.University.Components;
using R7.University.Models;
using R7.University.Queries;

namespace R7.University.Launchpad
{
    public class PositionsTable: LaunchpadTableBase
    {
        public PositionsTable () : base ("Positions")
        {
        }

        public override DataTable GetDataTable (PortalModuleBase module, UniversityModelContext modelContext, string search)
        {
            // REVIEW: Cannot set comparison options
            var positions = (search == null)
                ? new FlatQuery<PositionInfo> (modelContext).List ()
                : new FlatQuery<PositionInfo> (modelContext)
                    .ListWhere (p => p.Title.Contains (search) || p.ShortTitle.Contains (search));

            return DataTableConstructor.FromIEnumerable (positions);
        }
    }
}

