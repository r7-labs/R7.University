using System;
using System.Web.UI.WebControls;
using System.Linq;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.UI.UserControls;
using R7.University;

namespace R7.University.Launchpad
{
	public partial class SettingsLaunchpad : LaunchpadModuleSettingsBase
	{
		public void Page_Init ()
		{
			// fill PageSize combobox
			comboPageSize.AddItem ("10", "10");
			comboPageSize.AddItem ("25", "25");
			comboPageSize.AddItem ("50", "50");
			comboPageSize.AddItem ("100", "100");

			// fill tables list
			foreach (var table in LaunchpadTableInfo.AvailableTables)
				listTables.Items.Add (new Telerik.Web.UI.RadListBoxItem (Utils.FirstCharToUpperInvariant (table), table));
		}

		/// <summary>
		/// Handles the loading of the module setting for this control
		/// </summary>
		public override void LoadSettings ()
		{ 
			try
			{
				if (!IsPostBack)
				{
					// TODO: Allow select nearest pagesize value
					comboPageSize.Select (LaunchpadSettings.PageSize.ToString (), false);

					// check table list items
					foreach (var table in LaunchpadSettings.Tables)
					{
						var item = listTables.FindItemByValue (table);
						if (item != null) item.Checked = true;
					}

				}
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		/// <summary>
		/// handles updating the module settings for this control
		/// </summary>
		public override void UpdateSettings ()
		{
			try
			{
				LaunchpadSettings.PageSize = int.Parse (comboPageSize.SelectedValue);
				LaunchpadSettings.Tables = listTables.CheckedItems.Select (i => i.Value).ToList ();

				// remove session variable for active view,
				// since view set may be changed
				Session.Remove ("Launchpad_ActiveView_" + TabModuleId);

				Utils.SynchronizeModule (this);

			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}
	}
}

