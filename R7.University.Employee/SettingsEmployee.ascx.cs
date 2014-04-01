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
	public partial class SettingsEmployee : ModuleSettingsBase
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
					var ctrl = new EmployeeController ();
					var settings = new EmployeeSettings (this);

					comboEmployees.AddItem (Localization.GetString("NotSelected.Text", LocalResourceFile), Null.NullInteger.ToString());
					foreach (var employee in  ctrl.GetObjects<EmployeeInfo>("ORDER BY [LastName]"))
						comboEmployees.AddItem (employee.AbbrName, employee.EmployeeID.ToString());
				
					if (!Null.IsNull(settings.EmployeeID))
						comboEmployees.Select(settings.EmployeeID.ToString(), false);

					checkAutoTitle.Checked = settings.AutoTitle;

					if (!Null.IsNull(settings.PhotoWidth))
						textPhotoWidth.Text = settings.PhotoWidth.ToString();
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
				var settings = new EmployeeSettings (this);
				
				settings.EmployeeID = int.Parse(comboEmployees.SelectedValue);
				settings.AutoTitle = checkAutoTitle.Checked;

				if (!string.IsNullOrWhiteSpace(textPhotoWidth.Text))
					settings.PhotoWidth = int.Parse(textPhotoWidth.Text);
				else
					settings.PhotoWidth = Null.NullInteger;

				Utils.SynchronizeModule(this);

			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}
	}
}

