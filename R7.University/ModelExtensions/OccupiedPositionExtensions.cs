//
// OccupiedPositionExtensions.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2016 Roman M. Yagodin
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

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

