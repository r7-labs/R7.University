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

		/// <summary>
		/// Template used to render the module content
		/// </summary>
		public string Template {
			get { 
				return ReadSetting<string> ("template", 
					"<i>[CREATEDONDATE]<i> <b>[CREATEDBYUSERNAME]</b>:<br />[CONTENT]", 
					true); 
			}
			set { WriteSetting<string> ("template", value, true); }
		}

		#endregion
	}
}

