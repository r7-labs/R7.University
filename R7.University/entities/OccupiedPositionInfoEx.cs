using System;
using System.Linq;
using System.Collections.Generic;
using DotNetNuke.Data;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;

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

		#endregion

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
				for (var j = i + 1; j < opList.Count; )
				{
					if (opList [i].DivisionID == opList [j].DivisionID)
					{
						opList [i].PositionShortTitle += ", " + opList [j].PositionShortTitle;
						opList.RemoveAt (j);
					}
					else j++;
				}
			}

			return opList;
		}
	}
}

