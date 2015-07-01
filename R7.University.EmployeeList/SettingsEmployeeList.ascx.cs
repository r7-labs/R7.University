using System;
using System.Web.UI.WebControls;
using System.Linq;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.UserControls;
using DotNetNuke.Common.Utilities;
using R7.University;

namespace R7.University.EmployeeList
{
	public partial class SettingsEmployeeList : EmployeeListModuleSettingsBase
	{
        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // get divisions
            var divisions = EmployeeListController.GetObjects<DivisionInfo> ().OrderBy (d => d.Title).ToList ();

            // insert default item
            divisions.Insert (0, DivisionInfo.DefaultItem (LocalizeString ("NotSelected.Text")));

            // bind divisions to the tree
            treeDivisions.DataSource = divisions;
            treeDivisions.DataBind ();

            // sort type
            comboSortType.AddItem (LocalizeString ("SortTypeByMaxWeight.Text"), "0");
            comboSortType.AddItem (LocalizeString ("SortTypeByTotalWeight.Text"), "1");
            comboSortType.AddItem (LocalizeString ("SortTypeByName.Text"), "2");
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
			        // select node and expand tree to it
                    Utils.SelectAndExpandByValue (treeDivisions, EmployeeListSettings.DivisionID.ToString ());

					// check / uncheck IncludeSubdivisions
					checkIncludeSubdivisions.Checked = EmployeeListSettings.IncludeSubdivisions;

					comboSortType.Select (EmployeeListSettings.SortType.ToString (), false);

					if (!Null.IsNull (EmployeeListSettings.PhotoWidth))
						textPhotoWidth.Text = EmployeeListSettings.PhotoWidth.ToString ();
					
					if (!Null.IsNull (EmployeeListSettings.DataCacheTime))
						textDataCacheTime.Text = EmployeeListSettings.DataCacheTime.ToString ();
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
				EmployeeListSettings.DivisionID = int.Parse (treeDivisions.SelectedValue);
				EmployeeListSettings.IncludeSubdivisions = checkIncludeSubdivisions.Checked;
				EmployeeListSettings.SortType = int.Parse (comboSortType.SelectedValue);

				if (!string.IsNullOrWhiteSpace (textPhotoWidth.Text))
					EmployeeListSettings.PhotoWidth = int.Parse (textPhotoWidth.Text);
				else
					EmployeeListSettings.PhotoWidth = Null.NullInteger;
				
				if (!string.IsNullOrWhiteSpace (textDataCacheTime.Text))
					EmployeeListSettings.DataCacheTime = int.Parse (textDataCacheTime.Text);
				else
					EmployeeListSettings.DataCacheTime = Null.NullInteger;

				Utils.SynchronizeModule (this);
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}
	}
}

