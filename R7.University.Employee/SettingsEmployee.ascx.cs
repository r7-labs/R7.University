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
		/// <summary>
		/// Handles the loading of the module setting for this control
		/// </summary>
		public override void LoadSettings ()
		{
			try
			{
				if (!IsPostBack)
				{
					comboEmployees.AddItem (Localization.GetString ("NotSelected.Text", LocalResourceFile), Null.NullInteger.ToString ());
					foreach (var employee in EmployeeController.GetObjects<EmployeeInfo>("ORDER BY [LastName]"))
						comboEmployees.AddItem (employee.AbbrName, employee.EmployeeID.ToString ());
				
					if (!Null.IsNull (EmployeeSettings.EmployeeID))
						comboEmployees.Select (EmployeeSettings.EmployeeID.ToString (), false);

					checkAutoTitle.Checked = EmployeeSettings.AutoTitle;

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

