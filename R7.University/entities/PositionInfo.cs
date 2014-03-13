using System;
using DotNetNuke.Data;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;

namespace R7.University
{
	/*
	 CREATE TABLE [dbo].[University_Positions](
	[PositionID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](100) NOT NULL,
	[ShortTitle] [nvarchar](50) NULL,
	[Weight] [int] NOT NULL,
	*/

	// More attributes for class:
	// Set caching for table: [Cacheable("R7.University_Positions", CacheItemPriority.Default, 20)]
	// Explicit mapping declaration: [DeclareColumns]
	// More attributes for class properties:
	// Custom column name: [ColumnName("PositionID")]
	// Explicit include column: [IncludeColumn]
	// Note: DAL 2 have no AutoJoin analogs from PetaPOCO at this time
	[TableName ("University_Positions")]
	[PrimaryKey ("PositionID", AutoIncrement = true)]
	public class PositionInfo : ReferenceEntityBase
	{
		#region Fields

		#endregion

		/// <summary>
		/// Empty default cstor
		/// </summary>
		public PositionInfo ()
		{
		}

		#region Properties

		public int PositionID { get; set; }
		public int Weight { get; set; }

		#endregion
	}
}

