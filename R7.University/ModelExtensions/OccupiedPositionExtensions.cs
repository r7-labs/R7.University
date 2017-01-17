//
//  OccupiedPositionExtensions.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2017 Roman M. Yagodin
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

using System.Collections.Generic;
using System.Linq;
using DotNetNuke.UI.Modules;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.Models;
using R7.University.Utilities;
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
        public static IEnumerable<GroupedOccupiedPosition> GroupByDivision (this IEnumerable<OccupiedPositionInfo> occupiedPositions)
        {
            var gops = occupiedPositions.Select (op => new GroupedOccupiedPosition (op)).ToList ();

            for (var i = 0; i < gops.Count; i++) {
                var gop = gops [i];
                var gopp = gop.OccupiedPosition;

                // first combine position short title with it's suffix
                gop.Title = TextUtils.FormatList (" ",
                    FormatHelper.FormatShortTitle (gopp.Position.ShortTitle, gopp.Position.Title), gopp.TitleSuffix);

                for (var j = i + 1; j < gops.Count;) {
                    if (gopp.DivisionID == gops [j].OccupiedPosition.DivisionID) {
                        gop.Title += ", " + TextUtils.FormatList (" ",
                            FormatHelper.FormatShortTitle (gops [j].OccupiedPosition.Position.ShortTitle, gops [j].OccupiedPosition.Position.Title), gops [j].OccupiedPosition.TitleSuffix);

                        // remove groupped item
                        gops.RemoveAt (j);
                        continue;
                    }
                    j++;
                }
            }

            return gops;
        }
        public static string FormatDivisionLink (this OccupiedPositionInfo op, IModuleControl module)
        {
            // do not display division title for high-level divisions
            if (op.Division.ParentDivisionID != null) {
                var strDivision = FormatHelper.FormatShortTitle (op.Division.ShortTitle, op.Division.Title);
                if (!string.IsNullOrWhiteSpace (op.Division.HomePage))
                    strDivision = string.Format ("<a href=\"{0}\" target=\"_blank\">{1}</a>", 
                        UniversityUrlHelper.FormatURL (module, op.Division.HomePage, false), strDivision);

                return strDivision;
            }

            return string.Empty;
        }
    }
}

