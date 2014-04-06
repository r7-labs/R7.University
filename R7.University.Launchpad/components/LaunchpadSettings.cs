using System;
using DotNetNuke.Entities.Modules;
using DotNetNuke.UI.Modules;
using R7.University;

namespace R7.University.Launchpad
{
	/// <summary>
	/// Provides strong typed access to settings used by module
	/// </summary>
	public class LaunchpadSettings : SettingsWrapper
	{
		public LaunchpadSettings (IModuleControl module) : base (module)
		{
		}

		#region Properties for settings

		public int PageSize
		{
			get { return ReadSetting<int> ("Launchpad_PageSize", 20, true); }
			set { WriteSetting<int> ("Launchpad_PageSize", value, true); }
		}

		public string Tables
		{
			get { return ReadSetting<string> ("Launchpad_Tables", "positions", true); }
			set { WriteSetting<string> ("Launchpad_Tables", value, true); }
		}

		#endregion
	}
}

