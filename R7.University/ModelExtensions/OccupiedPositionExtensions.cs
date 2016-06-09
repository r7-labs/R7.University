//
//  OccupiedPositionExtensions.cs
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
using System.Collections.Generic;
using System.Linq;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.Data;
using R7.University.ViewModels;

namespace R7.University.ModelExtensions
{
    public static class OccupiedPositionExtensions
    {
        /// <summary>
        /// Groups the occupied positions in same division
        /// </summary>
        /// <returns>The occupied positions grouped by division.</returns>
        /// <param name="occupiedPositions">The occupied positions to group by division.</param>
        public static IEnumerable<OccupiedPositionInfoEx> GroupByDivision (this IEnumerable<OccupiedPositionInfoEx> occupiedPositions)
        {
            var opList = occupiedPositions.ToList ();

            for (var i = 0; i < opList.Count; i++) {
                var op = opList [i];
                // first combine position short title with it's suffix
                op.PositionShortTitle = TextUtils.FormatList (" ", 
                    FormatHelper.FormatShortTitle (op.PositionShortTitle, op.PositionTitle), 
                    op.TitleSuffix);

                for (var j = i + 1; j < opList.Count;) {
                    if (op.DivisionID == opList [j].DivisionID) {
                        op.PositionShortTitle += ", " + TextUtils.FormatList (" ", 
                            FormatHelper.FormatShortTitle (opList [j].PositionShortTitle, opList [j].PositionTitle), 
                            opList [j].TitleSuffix);

                        // remove groupped item
                        opList.RemoveAt (j);
                        continue;
                    }
                    j++;
                }
            }

            return opList;
        }
    }
}

