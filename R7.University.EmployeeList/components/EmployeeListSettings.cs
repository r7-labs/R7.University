using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.UI.Modules;
using DotNetNuke.Common.Utilities;
using DotNetNuke.R7;
using R7.University;

namespace R7.University.EmployeeList
{
	/// <summary>
	/// Provides strong typed access to settings used by module
	/// </summary>
	public partial class EmployeeListSettings : SettingsWrapper
	{
		public EmployeeListSettings (IModuleControl module) : base (module)
		{
		}

		public EmployeeListSettings (ModuleInfo module) : base (module)
		{
		}

		#region Properties for settings

		// REVIEW: Use Attributes to describe settings (tabspecific, data type, name, default value)

		// cached DivisionID value for frequent use
		private int? divisionId = null;

		/// <summary>
		/// Division's ID to show employees from
		/// </summary>
		public int DivisionID
		{
			get
			{ 
				if (divisionId == null)
					divisionId = ReadSetting<int> ("EmployeeList_DivisionID", Null.NullInteger);

				return divisionId.Value;
			}
			set
			{ 
				WriteModuleSetting<int> ("EmployeeList_DivisionID", value);
				divisionId = value;
			}
		}

		/// <summary>
		/// Indicates employee info extraction method
		/// </summary>
		/// <value><c>true</c> if resursively include employees from subordinate divisions; otherwise, <c>false</c>.</value>
		public bool IncludeSubdivisions
		{
			get { return ReadSetting<bool> ("EmployeeList_IncludeSubdivisions", false); }
			set { WriteModuleSetting<bool> ("EmployeeList_IncludeSubdivisions", value); }
		}

		/// <summary>
		/// Gets or sets the type of the sort.
		/// </summary>
		/// <value>The type of the sort.</value>
		public int SortType
		{
			get { return ReadSetting<int> ("EmployeeList_SortType", 0); }
			set { WriteTabModuleSetting<int> ("EmployeeList_SortType", value); }
		}

		public int PhotoWidth
		{
			// REVIEW: Need a way to customize default settings like PhotoWidth
			get { return ReadSetting<int> ("EmployeeList_PhotoWidth", 120); }
			set { WriteTabModuleSetting<int> ("EmployeeList_PhotoWidth", value); }
		}

		private int? dataCacheTime;

		public int DataCacheTime
		{
			get
			{ 
				if (dataCacheTime == null)
					dataCacheTime = ReadSetting<int> ("EmployeeList_DataCacheTime", 1200);
				
				return dataCacheTime.Value;
			}
			set
			{ 
				WriteTabModuleSetting<int> ("EmployeeList_DataCacheTime", value); 
				dataCacheTime = value;
			}
		}

		#endregion
	}
}

