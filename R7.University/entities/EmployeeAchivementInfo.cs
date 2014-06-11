using System;
using System.Text.RegularExpressions;
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
	[TableName ("University_EmployeeAchivements")]
	[PrimaryKey ("EmployeeAchivementID", AutoIncrement = true)]
	public class EmployeeAchivementInfo : IReferenceEntity
	{
		#region Fields

		#endregion

		/// <summary>
		/// Empty default cstor
		/// </summary>
		public EmployeeAchivementInfo ()
		{
		}

		#region IReferenceEntity implementation

		public string Title { get; set; }
		public string ShortTitle { get; set; }

		#endregion

		#region Properties

		public int EmployeeAchivementID { get; set; }
		public int EmployeeID  { get; set; }
		public string Description { get; set; }
		public int? YearBegin { get; set; }
		public int? YearEnd { get; set; }
		public bool IsTitle { get; set; }
		public string DocumentURL { get; set; }
		
		[ColumnName ("AchivementType")]
		public string AchivementTypeString { get; set; }
	
		#endregion
		
		[IgnoreColumn]
		public char AchivementType
		{
			get { return AchivementTypeString [0]; }
		}
		
	}
}
