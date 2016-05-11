//
// OccupiedPositionInfoEx.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015-2016 Roman M. Yagodin
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
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.UI.Modules;
using R7.DotNetNuke.Extensions.Utilities;

namespace R7.University.Data
{
    // More attributes for class:
    // Set caching for table: [Cacheable("R7.University_OccupiedPositions", CacheItemPriority.Default, 20)]
    // Explicit mapping declaration: [DeclareColumns]
    // More attributes for class properties:
    // Custom column name: [ColumnName("OccupiedPositionID")]
    // Explicit include column: [IncludeColumn]
    // Note: DAL 2 have no AutoJoin analogs from PetaPOCO at this time
    [TableName ("vw_University_OccupiedPositions")]
    [PrimaryKey ("OccupiedPositionID", AutoIncrement = false)]
    public class OccupiedPositionInfoEx : OccupiedPositionInfo
    {
        /// <summary>
        /// Empty default cstor
        /// </summary>
        public OccupiedPositionInfoEx ()
        {
        }

        #region Extended (external) properties

        // WTF: [ReadOnlyColumn] attribute prevents data from loading?
        public string PositionShortTitle { get; set; }

        public string PositionTitle { get; set; }

        public string DivisionShortTitle { get; set; }

        public string DivisionTitle { get; set; }

        public int PositionWeight { get; set; }

        public string HomePage { get; set; }

        public int? ParentDivisionID { get; set; }

        public bool IsTeacher { get; set; }

        #endregion

        public string FormatDivisionLink (IModuleControl module)
        {
            // do not display division title for high-level divisions
            if (ParentDivisionID != null) {
                var strDivision = DivisionInfo.FormatShortTitle (DivisionTitle, DivisionShortTitle);
                if (!string.IsNullOrWhiteSpace (HomePage))
                    strDivision = string.Format ("<a href=\"{0}\">{1}</a>", 
                        R7.University.Utilities.Utils.FormatURL (module, HomePage, false), strDivision);

                return strDivision;
            }
              
            return string.Empty;
        }

        /// <summary>
        /// Groups the occupied positions in same division
        /// </summary>
        /// <returns>The occupied positions.</returns>
        /// <param name="occupiedPositions">The occupied positions groupped by division.</param>
        public static IEnumerable<OccupiedPositionInfoEx> GroupByDivision (IEnumerable<OccupiedPositionInfoEx> occupiedPositions)
        {
            var opList = occupiedPositions.ToList ();

            for (var i = 0; i < opList.Count; i++) {
                var op = opList [i];
                // first combine position short title with it's suffix
                op.PositionShortTitle = TextUtils.FormatList (" ", 
                    PositionInfo.FormatShortTitle (op.PositionTitle, op.PositionShortTitle), 
                    op.TitleSuffix);

                for (var j = i + 1; j < opList.Count;) {
                    if (op.DivisionID == opList [j].DivisionID) {
                        op.PositionShortTitle += ", " + TextUtils.FormatList (" ", 
                            PositionInfo.FormatShortTitle (opList [j].PositionTitle, opList [j].PositionShortTitle), 
                            opList [j].TitleSuffix);
					
                        // remove groupped item
                        opList.RemoveAt (j);
                    }
                    else
                        j++;
                }
            }

            return opList;
        }
    }
}

