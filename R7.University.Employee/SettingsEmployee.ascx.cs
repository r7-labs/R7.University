using System;
using System.Web.UI.WebControls;
using System.Linq;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Common.Utilities;
using DotNetNuke.UI.UserControls;
using R7.University;

namespace R7.University.Employee
{
	public partial class SettingsEmployee : EmployeeModuleSettingsBase
	{
        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // bind employees to the combobox
            comboEmployees.DataSource = EmployeeController.GetObjects<EmployeeInfo> ("ORDER BY [LastName]");
            comboEmployees.DataBind ();

            // add default item
            comboEmployees.Items.Insert (0, new ListItem (LocalizeString ("NotSelected.Text"), Null.NullInteger.ToString ()));
        }

		/// <summary>
		/// Handles the loading of the module setting for this control
		/// </summary>
		public override void LoadSettings ()
		{
			try
			{
                if (DotNetNuke.Framework.AJAX.IsInstalled ())
                    DotNetNuke.Framework.AJAX.RegisterScriptManager ();

				if (!IsPostBack)
				{
					if (!Null.IsNull (EmployeeSettings.EmployeeID))
                        Utils.SelectByValue (comboEmployees, EmployeeSettings.EmployeeID);
                    else
                        comboEmployees.SelectedIndex = 0;

					checkAutoTitle.Checked = EmployeeSettings.AutoTitle;
					checkShowCurrentUser.Checked = EmployeeSettings.ShowCurrentUser;
					
					if (!Null.IsNull (EmployeeSettings.PhotoWidth))
						textPhotoWidth.Text = EmployeeSettings.PhotoWidth.ToString ();
					
					if (!Null.IsNull (EmployeeSettings.DataCacheTime))
						textDataCacheTime.Text = EmployeeSettings.DataCacheTime.ToString ();
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
				EmployeeSettings.ShowCurrentUser = checkShowCurrentUser.Checked;

				EmployeeSettings.EmployeeID = int.Parse (comboEmployees.SelectedValue);

				EmployeeSettings.AutoTitle = checkAutoTitle.Checked;

				if (!string.IsNullOrWhiteSpace (textPhotoWidth.Text))
					EmployeeSettings.PhotoWidth = int.Parse (textPhotoWidth.Text);
				else
					EmployeeSettings.PhotoWidth = Null.NullInteger;
				
				if (!string.IsNullOrWhiteSpace (textDataCacheTime.Text))
					EmployeeSettings.DataCacheTime = int.Parse (textDataCacheTime.Text);
				else
					EmployeeSettings.DataCacheTime = Null.NullInteger;
				
				Utils.SynchronizeModule (this);

			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}
	}
}

