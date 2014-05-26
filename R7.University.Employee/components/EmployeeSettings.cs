using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.UI.Modules;
using DotNetNuke.Common.Utilities;
using R7.University;

namespace R7.University.Employee
{
	/// <summary>
	/// Provides strong typed access to settings used by module
	/// </summary>
	public partial class EmployeeSettings : SettingsWrapper
	{
		public EmployeeSettings (IModuleControl module) : base (module)
		{
		}

		public EmployeeSettings (ModuleInfo module) : base (module)
		{
		}

		#region Properties for settings

		public int EmployeeID
		{
			get { return ReadSetting<int> ("Employee_EmployeeID", Null.NullInteger, false); }
			set { WriteSetting<int> ("Employee_EmployeeID", value, false); }
		}

		public bool AutoTitle
		{
			get { return ReadSetting<bool> ("Employee_AutoTitle", true, false); }
			set { WriteSetting<bool> ("Employee_AutoTitle", value, false); }
		}

		public int PhotoWidth
		{
			// REVIEW: Need a way to customize default settings like PhotoWidth
			get { return ReadSetting<int> ("Employee_PhotoWidth", 192, true); }
			set { WriteSetting<int> ("Employee_PhotoWidth", value, true); }
		}
		
		private int? dataCacheTime;
		
		public int DataCacheTime
		{
			get 
			{ 
				if (dataCacheTime == null)
					dataCacheTime = ReadSetting<int> ("Employee_DataCacheTime", 1200, true);
				
				return dataCacheTime.Value;
			}
			set 
			{ 
				WriteSetting<int> ("Employee_DataCacheTime", value, true); 
				dataCacheTime = value;
			}
		}
		
		#endregion
	}
}

