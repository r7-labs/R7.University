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
		/// <summary>
		/// Handles the loading of the module setting for this control
		/// </summary>
		public override void LoadSettings ()
		{
			try
			{
				if (!IsPostBack)
				{
					// get divisions
					var divisions = EmployeeListController.GetObjects<DivisionInfo> ("ORDER BY [Title] ASC").ToList ();

					// insert default item
					divisions.Insert (0, new DivisionInfo () { 
						DivisionID = Null.NullInteger, 
						ParentDivisionID = null,
						Title = Localization.GetString ("NotSelected.Text", LocalResourceFile),
						ShortTitle = Localization.GetString ("NotSelected.Text", LocalResourceFile)
					});

					// bind list to a tree
					treeDivisions.DataSource = divisions;
					treeDivisions.DataBind ();

					// select currently stored value
					var treeNode = treeDivisions.FindNodeByValue (EmployeeListSettings.DivisionID.ToString ());
					if (treeNode != null)
					{
						treeNode.Selected = true;

						// expand all parent nodes
						treeNode = treeNode.ParentNode;
						while (treeNode != null)
						{
							treeNode.Expanded = true;
							treeNode = treeNode.ParentNode;
						} 
					}

					// check / uncheck IncludeSubdivisions
					checkIncludeSubdivisions.Checked = EmployeeListSettings.IncludeSubdivisions;

					// sort type
					comboSortType.AddItem (Localization.GetString ("SortTypeByMaxWeight.Text", LocalResourceFile), "0");
					comboSortType.AddItem (Localization.GetString ("SortTypeByTotalWeight.Text", LocalResourceFile), "1");
					comboSortType.AddItem (Localization.GetString ("SortTypeByName.Text", LocalResourceFile), "2");

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

