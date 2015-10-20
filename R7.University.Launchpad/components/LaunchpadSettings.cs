using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using DotNetNuke.Entities.Modules;
using DotNetNuke.UI.Modules;
using DotNetNuke.R7;
using R7.University;

namespace R7.University.Launchpad
{
	/// <summary>
	/// Provides strong typed access to settings used by module
	/// </summary>
	public class LaunchpadSettings : SettingsWrapper
	{
        public LaunchpadSettings ()
        {
        }

		public LaunchpadSettings (IModuleControl module) : base (module)
		{
		}

		#region Properties for settings

		public int PageSize
		{
			get { return ReadSetting<int> ("Launchpad_PageSize", 20); }
			set { WriteTabModuleSetting<int> ("Launchpad_PageSize", value); }
		}

		private List<string> tables;

		public List<string> Tables
		{
			get
			{ 
				if (tables == null)
				{
					tables = new List<string> ();
					tables.AddRange (
                        ReadSetting<string> ("Launchpad_Tables", string.Empty)
						.Split (new [] { ';' }, StringSplitOptions.RemoveEmptyEntries));
				}
				
				return tables;
			}
			set
			{ 
				WriteTabModuleSetting<string> ("Launchpad_Tables", Utils.FormatList (";", value.ToArray ())); 
			}
		}

		#endregion
	
	}
}

