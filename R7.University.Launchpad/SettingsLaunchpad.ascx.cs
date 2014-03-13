using System;
using System.Web.UI.WebControls;
using System.Linq;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.UI.UserControls;
using R7.University;

namespace R7.University.Launchpad
{
	public partial class SettingsLaunchpad : ModuleSettingsBase
	{
		/// <summary>
		/// Handles the loading of the module setting for this control
		/// </summary>
		public override void LoadSettings ()
		{
			try {
				if (!IsPostBack) {
					var settings = new LaunchpadSettings (this);
										
					if (!string.IsNullOrWhiteSpace (settings.Template)) {
						txtTemplate.Text = settings.Template;
					}
				}
			} catch (Exception ex) {
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		/// <summary>
		/// handles updating the module settings for this control
		/// </summary>
		public override void UpdateSettings ()
		{
			try {
				var settings = new LaunchpadSettings (this);
				
				settings.Template = txtTemplate.Text;

				// NOTE: update module cache (temporary fix before 7.2.0)?
				// more info: https://github.com/dnnsoftware/Dnn.Platform/pull/21
				var moduleController = new ModuleController();
				moduleController.ClearCache(TabId);

				Utils.SynchronizeModule(this);

			} catch (Exception ex) {
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}
	}
}

