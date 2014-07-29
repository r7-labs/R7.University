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
	[TableName ("University_EmployeeAchievements")]
	[PrimaryKey ("EmployeeAchievementID", AutoIncrement = true)]
	public class EmployeeAchievementInfo : IReferenceEntity
	{
		#region Fields

		#endregion

		/// <summary>
		/// Empty default cstor
		/// </summary>
		public EmployeeAchievementInfo ()
		{
		}

		#region IReferenceEntity implementation

		public string Title { get; set; }
		public string ShortTitle { get; set; }

		#endregion

		#region Properties

		public int EmployeeAchievementID { get; set; }
		public int EmployeeID  { get; set; }
		public int? AchievementID { get; set; }
		public string Description { get; set; }
		public int? YearBegin { get; set; }
		public int? YearEnd { get; set; }
		public bool IsTitle { get; set; }
		public string DocumentURL { get; set; }
		
		[ColumnName ("AchievementType")]
		public string AchievementTypeString { get; set; }
	
		#endregion
		
		[IgnoreColumn]
		public AchievementType? AchievementType
		{
			get
			{ 
				if (!string.IsNullOrWhiteSpace (AchievementTypeString))
					return (AchievementType)AchievementTypeString [0];

				return null;
			}
			set
			{ 
				if (value != null)
					AchievementTypeString = ((char)value).ToString(); 
				else
					AchievementTypeString = null;
			}
		}
		
	}
}
