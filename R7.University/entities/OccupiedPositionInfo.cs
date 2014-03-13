using System;
using DotNetNuke.Data;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;

namespace R7.University
{
	// TODO: Add Unique constraint to OccupiedPositions table FK's

	// More attributes for class:
	// Set caching for table: [Cacheable("R7.University_OccupiedPositions", CacheItemPriority.Default, 20)]
	// Explicit mapping declaration: [DeclareColumns]
	// More attributes for class properties:
	// Custom column name: [ColumnName("OccupiedPositionID")]
	// Explicit include column: [IncludeColumn]
	// Note: DAL 2 have no AutoJoin analogs from PetaPOCO at this time
	[TableName ("University_OccupiedPositions")]
	[PrimaryKey ("OccupiedPositionID", AutoIncrement = true)]
	[Scope ("DivisionID")]
	public class OccupiedPositionInfo  
	{
		#region Fields

		#endregion

		/// <summary>
		/// Empty default cstor
		/// </summary>
		public OccupiedPositionInfo ()
		{
		}

	
		#region Properties

		public int OccupiedPositionID { get; set; }
		public int PositionID { get; set; }
		public int DivisionID { get; set; }
		public int EmployeeID { get; set; }
		public bool IsPrime { get; set; }

		#endregion
	}
}

