using System;
using System.Linq;
using System.Collections.Generic;
using DotNetNuke.Data;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;
using DotNetNuke.UI.Modules;

namespace R7.University
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
	[Scope ("DivisionID")]
	public class OccupiedPositionInfoEx : OccupiedPositionInfo
	{
		/// <summary>
		/// Empty default cstor
		/// </summary>
		public OccupiedPositionInfoEx ()
		{
		}

		#region Extended (external) properties

		// NOTE: [ReadOnlyColumn] attribute prevents data from loading?
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
            if (ParentDivisionID != null)
            {
                var strDivision = DivisionInfo.FormatShortTitle (DivisionTitle, DivisionShortTitle);
                if (!string.IsNullOrWhiteSpace (HomePage))
                    strDivision = string.Format ("<a href=\"{0}\">{1}</a>", 
                        Utils.FormatURL (module, HomePage, false), strDivision);

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

			for (var i = 0; i < opList.Count; i++)
			{
                var op = opList [i];
				// first combine position short title with it's suffix
				op.PositionShortTitle = Utils.FormatList (" ", 
                    PositionInfo.FormatShortTitle (op.PositionTitle, op.PositionShortTitle), 
                    op.TitleSuffix);

				for (var j = i + 1; j < opList.Count;)
				{
					if (op.DivisionID == opList [j].DivisionID)
					{
						op.PositionShortTitle += ", " + Utils.FormatList (" ", 
                            PositionInfo.FormatShortTitle (opList [j].PositionTitle, opList [j].PositionShortTitle), 
                            opList [j].TitleSuffix);
					
						// remove groupped item
						opList.RemoveAt (j);
					}
					else j++;
				}
			}

			return opList;
		}
	}
}

