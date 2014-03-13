using System;
using DotNetNuke.Data;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;

namespace R7.University
{
	// More attributes for class:
	// Set caching for table: [Cacheable("R7.University_Divisions", CacheItemPriority.Default, 20)]
	// Explicit mapping declaration: [DeclareColumns]
	// More attributes for class properties:
	// Custom column name: [ColumnName("DivisionID")]
	// Explicit include column: [IncludeColumn]
	// Note: DAL 2 have no AutoJoin analogs from PetaPOCO at this time
	[TableName ("University_Divisions")]
	[PrimaryKey ("DivisionID", AutoIncrement = true)]
	public class DivisionInfo : EntityBase, IReferenceEntity
	{
		#region Fields

		#endregion

		/// <summary>
		/// Empty default cstor
		/// </summary>
		public DivisionInfo ()
		{
		}

		#region IReferenceEntity implementation

		public string Title { get; set; }
		public string ShortTitle { get; set; }

		#endregion

		#region Properties

		public int DivisionID { get; set; }
		public int? ParentDivisionID  { get; set; }
		public int? DivisionTermID  { get; set; }
		public string HomePage { get; set; }
		public string WebSite { get; set; }
		public string Phone { get; set; }
		public string Fax { get; set; }
		public string Email { get; set; }
		public string SecondaryEmail { get; set; }
		public string Location { get; set; }
		public string WorkingHours { get; set; }

		#endregion


	}
}

