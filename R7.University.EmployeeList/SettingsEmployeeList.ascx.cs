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
	public partial class SettingsEmployeeList : ModuleSettingsBase
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
					var settings = new EmployeeListSettings (this);
					var ctrl = new EmployeeListController ();

					// get divisions
					var divisions = ctrl.GetObjects<DivisionInfo>("ORDER BY [Title] ASC").ToList();

					// insert default item
					divisions.Insert (0, new DivisionInfo() { 
						DivisionID = Null.NullInteger, 
						ParentDivisionID = null,
						Title = Localization.GetString("NotSelected.Text", LocalResourceFile),
						ShortTitle = Localization.GetString("NotSelected.Text", LocalResourceFile)
					});

					// bind list to a tree
					treeDivisions.DataSource = divisions;
					treeDivisions.DataBind();

					// select currently stored value
					var treeNode = treeDivisions.FindNodeByValue(settings.DivisionID.ToString());
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
					checkIncludeSubdivisions.Checked = settings.IncludeSubdivisions;

					// sort type
					comboSortType.AddItem(Localization.GetString("SortTypeByMaxWeight.Text", LocalResourceFile), "0");
					comboSortType.AddItem(Localization.GetString("SortTypeByTotalWeight.Text", LocalResourceFile), "1");
					comboSortType.AddItem(Localization.GetString("SortTypeByName.Text", LocalResourceFile), "2");

					comboSortType.Select(settings.SortType.ToString(), false);

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
				var settings = new EmployeeListSettings (this);

				settings.DivisionID = int.Parse(treeDivisions.SelectedValue);
				settings.IncludeSubdivisions = checkIncludeSubdivisions.Checked;
				settings.SortType = int.Parse(comboSortType.SelectedValue);

				if (!string.IsNullOrWhiteSpace(textPhotoWidth.Text))
					settings.PhotoWidth = int.Parse(textPhotoWidth.Text);
				else
					settings.PhotoWidth = Null.NullInteger;

				Utils.SynchronizeModule (this);
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}
	}
}

