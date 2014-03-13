using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using DotNetNuke.Data;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using R7.University;

namespace R7.University.Launchpad
{
	public partial class ViewLaunchpad : PortalModuleBase, IActionable
	{
		/*
		private int ActiveViewIndex
		{
			get 
			{
				var obj = Session ["Launchpad_ActiveViewIndex"];
				if (obj != null)
					return (int)obj;
				else
					return 0;
			}
			set { Session ["Launchpad_ActiveViewIndex"] = value; }
		}
		*/

		#region Handlers

		/// <summary>
		/// Handles Init event for a control
		/// </summary>
		/// <param name="e">Event args.</param>
		protected override void OnInit (EventArgs e)
		{
			base.OnInit (e);

			// initialize "Add" buttons
			buttonAddPosition.NavigateUrl = Utils.EditUrl (this, "EditPosition");
			buttonAddDivision.NavigateUrl = Utils.EditUrl (this, "EditDivision");
			buttonAddEmployee.NavigateUrl = Utils.EditUrl (this, "EditEmployee");

			// multiView.ActiveViewIndex = ActiveViewIndex;
		}

		/// <summary>
		/// Handles Load event for a control
		/// </summary>
		/// <param name="e">Event args.</param>
		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);

			try
			{
				if (!IsPostBack)
				{
					// multiView.ActiveViewIndex = ActiveViewIndex;

					gridDivisions.DataSource = DivisionsDataSource ();
					Session [gridDivisions.ID] = gridDivisions.DataSource;
					gridDivisions.DataBind ();

					gridEmployees.DataSource = EmployeesDataSource ();
					Session [gridEmployees.ID] = gridEmployees.DataSource;
					gridEmployees.DataBind ();

					gridPositions.DataSource = PositionsDataSource ();
					Session [gridPositions.ID] = gridPositions.DataSource;
					gridPositions.DataBind ();
				}
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
			
		}

		#endregion

		#region IActionable implementation

		public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
		{
			get
			{
				// create a new action to add an item, this will be added 
				// to the controls dropdown menu
				var actions = new ModuleActionCollection ();
				actions.Add (
					GetNextActionID (), 
					// Localization.GetString (ModuleActionType.AddContent, this.LocalResourceFile),
					"Add Position",
					ModuleActionType.AddContent, 
					"", 
					"", 
					Utils.EditUrl (this, "EditPosition"),
					false, 
					DotNetNuke.Security.SecurityAccessLevel.Edit,
					true, 
					false
				);
				actions.Add (
					GetNextActionID (), 
					"Add Division",
					ModuleActionType.AddContent, 
					"", 
					"", 
					Utils.EditUrl (this, "EditDivision"),
					false, 
					DotNetNuke.Security.SecurityAccessLevel.Edit,
					true, 
					false
				);
				actions.Add (
					GetNextActionID (), 
					"Add Employee",
					ModuleActionType.AddContent, 
					"", 
					"", 
					Utils.EditUrl (this, "EditEmployee"),
					false, 
					DotNetNuke.Security.SecurityAccessLevel.Edit,
					true, 
					false
				);

				return actions;
			}
		}

		#endregion

		protected void gridView_Sorting (object sender, GridViewSortEventArgs e)
		{
			var gv = sender as GridView;

			//Retrieve the table from the session object.
			var dt = Session [gv.ID] as DataTable;

			if (dt != null)
			{
				// Sort the data.
				dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection (gv.ID, e.SortExpression);
				gv.DataSource = dt;
				gv.DataBind ();
			}
		}

		private string GetSortDirection (string controlID, string column)
		{

			// By default, set the sort direction to ascending.
			var sortDirection = "ASC";

			// Retrieve the last column that was sorted.
			var sortExpression = ViewState [controlID + "SortExpression"] as string;

			if (sortExpression != null)
			{
				// Check if the same column is being sorted.
				// Otherwise, the default value can be returned.
				if (sortExpression == column)
				{
					var lastDirection = ViewState ["SortDirection"] as string;
					if ((lastDirection != null) && (lastDirection == "ASC"))
					{
						sortDirection = "DESC";
					}
				}
			}

			// Save new values in ViewState.

			// THINK: How this behave in case of multiple GridView's?
			ViewState ["SortDirection"] = sortDirection;
			ViewState [controlID + "SortExpression"] = column;

			return sortDirection;
		}

		private DataTable PositionsDataSource ()
		{
			var dt = new DataTable ();
			DataRow dr;

			dt.Columns.Add (new DataColumn ("PositionID", typeof(int)));
			dt.Columns.Add (new DataColumn ("Title", typeof(string)));
			dt.Columns.Add (new DataColumn ("ShortTitle", typeof(string)));
			dt.Columns.Add (new DataColumn ("Weight", typeof(int)));

			foreach (DataColumn column in dt.Columns)
				column.AllowDBNull = true;

			var ctrl = new LaunchpadController ();

			foreach (var position in ctrl.GetObjects<PositionInfo>())
			{
				dr = dt.NewRow ();
				dr [0] = position.PositionID;
				dr [1] = position.Title;
				dr [2] = position.ShortTitle;
				dr [3] = position.Weight;

				dt.Rows.Add (dr);
			}

			return dt;
		}

		private DataTable DivisionsDataSource ()
		{
			var dt = new DataTable ();
			DataRow dr;

			dt.Columns.Add (new DataColumn ("DivisionID", typeof(int)));
			dt.Columns.Add (new DataColumn ("ParentDivisionID", typeof(int)));
			dt.Columns.Add (new DataColumn ("DivisionTermID", typeof(int)));
			dt.Columns.Add (new DataColumn ("HomePage", typeof(string)));
			dt.Columns.Add (new DataColumn ("Title", typeof(string)));
			dt.Columns.Add (new DataColumn ("ShortTitle", typeof(string)));
			dt.Columns.Add (new DataColumn ("Location", typeof(string)));
			dt.Columns.Add (new DataColumn ("Phone", typeof(string)));
			dt.Columns.Add (new DataColumn ("Fax", typeof(string)));
			dt.Columns.Add (new DataColumn ("Email", typeof(string)));
			dt.Columns.Add (new DataColumn ("SecondaryEmail", typeof(string)));
			dt.Columns.Add (new DataColumn ("WebSite", typeof(string)));
			dt.Columns.Add (new DataColumn ("WorkingHours", typeof(string)));
			dt.Columns.Add (new DataColumn ("CreatedByUserID", typeof(int)));
			dt.Columns.Add (new DataColumn ("CreatedOnDate", typeof(DateTime)));
			dt.Columns.Add (new DataColumn ("LastModifiedByUserID", typeof(int)));
			dt.Columns.Add (new DataColumn ("LastModifiedOnDate", typeof(DateTime)));

			foreach (DataColumn column in dt.Columns)
				column.AllowDBNull = true;

			var ctrl = new LaunchpadController ();

			foreach (var division in ctrl.GetObjects<DivisionInfo>())
			{
				dr = dt.NewRow ();
				var i = 0;
				dr [i++] = division.DivisionID;
				dr [i++] = division.ParentDivisionID ?? Null.NullInteger;
				dr [i++] = division.DivisionTermID ?? Null.NullInteger;
				dr [i++] = division.HomePage;
				dr [i++] = division.Title;
				dr [i++] = division.ShortTitle;
				dr [i++] = division.Location;
				dr [i++] = division.Phone;
				dr [i++] = division.Fax;
				dr [i++] = division.Email;
				dr [i++] = division.SecondaryEmail;
				dr [i++] = division.WebSite;
				dr [i++] = division.WorkingHours;
				dr [i++] = division.CreatedByUserID;
				dr [i++] = division.CreatedOnDate;
				dr [i++] = division.LastModifiedByUserID;
				dr [i++] = division.LastModifiedOnDate;

				dt.Rows.Add (dr);
			}

			return dt;
		}

		private DataTable EmployeesDataSource ()
		{
			var dt = new DataTable ();
			DataRow dr;

			dt.Columns.Add (new DataColumn ("EmployeeID", typeof(int)));
			dt.Columns.Add (new DataColumn ("UserID", typeof(int)));
			dt.Columns.Add (new DataColumn ("PhotoFileID", typeof(int)));
			dt.Columns.Add (new DataColumn ("Phone", typeof(string)));
			dt.Columns.Add (new DataColumn ("CellPhone", typeof(string)));
			dt.Columns.Add (new DataColumn ("Fax", typeof(string)));
			dt.Columns.Add (new DataColumn ("LastName", typeof(string)));
			dt.Columns.Add (new DataColumn ("FirstName", typeof(string)));
			dt.Columns.Add (new DataColumn ("OtherName", typeof(string)));
			dt.Columns.Add (new DataColumn ("Email", typeof(string)));
			dt.Columns.Add (new DataColumn ("SecondaryEmail", typeof(string)));
			dt.Columns.Add (new DataColumn ("WebSite", typeof(string)));
			dt.Columns.Add (new DataColumn ("Messenger", typeof(string)));
			dt.Columns.Add (new DataColumn ("AcademicDegree", typeof(string)));
			dt.Columns.Add (new DataColumn ("AcademicTitle", typeof(string)));
			dt.Columns.Add (new DataColumn ("NamePrefix", typeof(string)));
			dt.Columns.Add (new DataColumn ("WorkingPlace", typeof(string)));
			dt.Columns.Add (new DataColumn ("WorkingHours", typeof(string)));
			dt.Columns.Add (new DataColumn ("Biography", typeof(string)));
			dt.Columns.Add (new DataColumn ("ExperienceYears", typeof(int)));
			dt.Columns.Add (new DataColumn ("ExperienceYearsBySpec", typeof(int)));
			dt.Columns.Add (new DataColumn ("IsPublished", typeof(bool)));
			//dt.Columns.Add (new DataColumn ("IsDeleted", typeof(bool)));
			dt.Columns.Add (new DataColumn ("CreatedByUserID", typeof(int)));
			dt.Columns.Add (new DataColumn ("CreatedOnDate", typeof(DateTime)));
			dt.Columns.Add (new DataColumn ("LastModifiedByUserID", typeof(int)));
			dt.Columns.Add (new DataColumn ("LastModifiedOnDate", typeof(DateTime)));

			foreach (DataColumn column in dt.Columns)
				column.AllowDBNull = true;

			var ctrl = new LaunchpadController ();

			foreach (var employee in ctrl.GetObjects<EmployeeInfo>())
			{
				dr = dt.NewRow ();
				var i = 0;
				dr [i++] = employee.EmployeeID;
				dr [i++] = employee.UserID ?? Null.NullInteger;
				dr [i++] = employee.PhotoFileID ?? Null.NullInteger;
				dr [i++] = employee.Phone;
				dr [i++] = employee.CellPhone;
				dr [i++] = employee.Fax;
				dr [i++] = employee.LastName;
				dr [i++] = employee.FirstName;
				dr [i++] = employee.OtherName;
				dr [i++] = employee.Email;
				dr [i++] = employee.SecondaryEmail;
				dr [i++] = employee.WebSite;
				dr [i++] = employee.Messenger;
				dr [i++] = employee.AcademicDegree;
				dr [i++] = employee.AcademicTitle;
				dr [i++] = employee.NamePrefix;
				dr [i++] = employee.WorkingPlace;
				dr [i++] = employee.WorkingHours;
				dr [i++] = employee.Biography;
				dr [i++] = employee.ExperienceYears ?? Null.NullInteger;
				dr [i++] = employee.ExperienceYearsBySpec ?? Null.NullInteger;
				dr [i++] = employee.IsPublished;
				//dr [i++] = employee.IsDeleted;
				dr [i++] = employee.CreatedByUserID;
				dr [i++] = employee.CreatedOnDate;
				dr [i++] = employee.LastModifiedByUserID;
				dr [i++] = employee.LastModifiedOnDate;

				dt.Rows.Add (dr);
			}

			return dt;
		}

		protected void gridView_PageIndexChanging (object sender, GridViewPageEventArgs e)
		{
			var gv = sender as GridView;
			var dt = Session [gv.ID] as DataTable;

			if (dt != null)
			{
				gv.PageIndex = e.NewPageIndex;
				gv.DataSource = dt;
				gv.DataBind ();
			}

			// restore pager visibility, may also TopPagerRow
			gv.BottomPagerRow.Visible = true;
		}

		protected void gridView_RowDataBound (object sender, GridViewRowEventArgs e)
		{
			// exclude header?
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				// find edit hyperlink
				var link = e.Row.Cells [0].FindControl ("linkEdit") as HyperLink;

				if (sender == gridPositions)
					// assuming e.Row.Cells[1] contains ID
					link.NavigateUrl = Utils.EditUrl (this, "EditPosition", "position_id", e.Row.Cells [1].Text);
				else if (sender == gridDivisions)
					link.NavigateUrl = Utils.EditUrl (this, "EditDivision", "division_id", e.Row.Cells [1].Text);
				else if (sender == gridEmployees)
					link.NavigateUrl = Utils.EditUrl (this, "EditEmployee", "employee_id", e.Row.Cells [1].Text);
			}
		}

		/// <summary>
		/// Handles the items being bound to the datalist control. In this method we merge the data with the
		/// template defined for this control to produce the result to display to the user
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void lstContent_ItemDataBound (object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
		{
			// use e.Item.DataItem as object of LaunchpadInfo class,
			// as we really know it is:
			var item = e.Item.DataItem as LaunchpadInfo;
			
			// find controls in DataList item template
			var lblUserName = e.Item.FindControl ("lblUserName") as Label;
			var lblCreatedOnDate = e.Item.FindControl ("lblCreatedOnDate") as Label;
			var lblContent = e.Item.FindControl ("lblContent") as Label;
			var linkEdit = e.Item.FindControl ("linkEdit") as HyperLink;
			var iconEdit = e.Item.FindControl ("imageEdit") as Image;
			
			// read module settings (may be useful in a future)
			// var settings = new LaunchpadSettings (this);            
            
			// edit link
			if (IsEditable)
			{
				linkEdit.NavigateUrl = Utils.EditUrl (this, "Edit", "launchpad_id", item.LaunchpadID.ToString ());
				// WTF: iconEdit.NavigateUrl = Utils.FormatURL (this, image.Url, false);
			}

			// make edit link visible in edit mode
			linkEdit.Visible = IsEditable;
			iconEdit.Visible = IsEditable;
            
			// fill the controls
			lblUserName.Text = item.CreatedByUserName;
			lblCreatedOnDate.Text = item.CreatedOnDate.ToShortDateString ();
			lblContent.Text = Server.HtmlDecode (item.Content);
		}

		protected void linkTab_Clicked (object sender, EventArgs e)
		{
			liPositions.Attributes ["class"] = "";
			liDivisions.Attributes ["class"] = "";
			liEmployees.Attributes ["class"] = "";

			if (sender == linkPositions)
			{
				liPositions.Attributes ["class"] = "ui-tabs-active";
				// ActiveViewIndex = 0;
				multiView.ActiveViewIndex = 0;
			}
			else if (sender == linkDivisions)
			{
				liDivisions.Attributes ["class"] = "ui-tabs-active";
				// ActiveViewIndex = 1;
				multiView.ActiveViewIndex = 1;
			}
			else if (sender == linkEmployees)
			{
				liEmployees.Attributes ["class"] = "ui-tabs-active";
				// ActiveViewIndex = 2;
				multiView.ActiveViewIndex = 2;
			}
			else
			{
				liPositions.Attributes ["class"] = "ui-tabs-active";
				// ActiveViewIndex = 0;
				multiView.ActiveViewIndex = 0;
			}
		}
	}
	// class
}
 // namespace

