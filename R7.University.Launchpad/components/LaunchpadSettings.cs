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
		#region Properties for settings

		public LaunchpadSettings (IModuleControl module) : base (module)
		{
			// ctrl = new ModuleController (); 
			// this.module = module;
		}

		public int PageSize
		{
			get { return ReadSetting<int> ("Launchpad_PageSize", 15, true); }
			set { WriteSetting<int> ("Launchpad_PageSize", value, true); }
		}

		#endregion
	}
}

