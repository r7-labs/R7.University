using System;
using System.Text;
using DotNetNuke.Data;
using DotNetNuke.Common.Utilities;
using DotNetNuke.ComponentModel.DataAnnotations;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Users;

namespace R7.University
{
	// More attributes for class:
	// Set caching for table: [Cacheable("University_Employees", CacheItemPriority.Default, 20)]
	// Explicit mapping declaration: [DeclareColumns]
	// More attributes for class properties:
	// Custom column name: [ColumnName("EmployeeID")]
	// Explicit include column: [IncludeColumn]
	// Note: DAL 2 have no AutoJoin analogs from PetaPOCO at this time
	[TableName ("University_Employees")]
	[PrimaryKey ("EmployeeID", AutoIncrement = true)]
	public class EmployeeInfo : EntityBase
	{
		#region Fields

		#endregion

		/// <summary>
		/// Empty default cstor
		/// </summary>
		public EmployeeInfo ()
		{
		}

		#region Properties

		public int EmployeeID { get; set; }
		public int? UserID { get; set; }  
		public int? PhotoFileID { get; set; }
		public string Phone { get; set; }
		public string CellPhone { get; set; }
		public string Fax { get; set; }
		public string LastName { get; set; }
		public string FirstName { get; set; }
		public string OtherName { get; set; }
		public string Email { get; set; }
		public string SecondaryEmail { get; set; }
		public string WebSite { get; set; }
		public string Messenger { get; set; }
		public string AcademicDegree { get; set; }
		public string AcademicTitle { get; set; }
		public string NamePrefix { get; set; } // THINK: Use Academic Degree & Title, of just NamePrefix?
		public string WorkingPlace { get; set; }
		public string WorkingHours { get; set; }
		public string Biography { get; set; }

		// NOTE: Employee stage may be not continuous, so using starting date is not possible
		public int? ExperienceYears { get; set; } 
		// NOTE: Employee ExpYearsBySpec even more unbinded to dates
		public int? ExperienceYearsBySpec { get; set; }

		public bool IsPublished { get; set; }
		//public bool IsDeleted { get; set; }

		#endregion

		#region Calculated properties

		[IgnoreColumn]
		public string AbbrName
		{
			get { return string.Format ("{0} {1}.{2}.", LastName, FirstName.Substring(0,1), OtherName.Substring(0,1)); }
		}

		[IgnoreColumn]
		public string FullName
		{
			get { return Utils.FormatList(" ", LastName, FirstName, OtherName); }
		}

		[IgnoreColumn]
		public string SearchDocumentText 
		{
			get 
			{
				var text = Utils.FormatList (", ",
					           FullName,
					           AcademicDegree,
					           AcademicTitle,
					           Phone,
					           CellPhone,
					           Fax,
					           Email,
					           SecondaryEmail,
					           WebSite,
					           Messenger,
					           WorkingPlace,
					           WorkingHours,
					           HtmlUtils.ConvertToText (Biography)
				);

				// TODO: Add positions to the index

				return text;
			}
		}

		#endregion

	}
}

