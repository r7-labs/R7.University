using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Linq;
using DotNetNuke.Data;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using R7.University;

namespace R7.University.Launchpad
{
	public partial class ViewLaunchpad : LaunchpadPortalModuleBase, IActionable
	{
		#region Properties

		protected string EditIconUrl
		{
			get { return IconController.IconURL ("Edit"); }
		}

        protected LaunchpadTables Tables = new LaunchpadTables ();

		#endregion

		#region Handlers

		/// <summary>
		/// Get DataTable stored in Session by GridView ID
		/// </summary>
		/// <returns>The data table.</returns>
		/// <param name="gridviewId">Gridview identifier.</param>
		private DataTable GetDataTable (string gridviewId)
		{
			var session = Session [gridviewId];
			if (session == null)
			{
                session = Tables.GridsDictionary [gridviewId].GetDataTable (this, (string) Session ["EmployeeSearch"]);
                Session [gridviewId] = session;
			}
            return (DataTable) session;
		}

		/// <summary>
		/// Handles Init event for a control
		/// </summary>
		/// <param name="e">Event args.</param>
		protected override void OnInit (EventArgs e)
		{
			base.OnInit (e);
		
			// read tab names
			var tables = LaunchpadSettings.Tables;
			if (tables == null || tables.Count == 0)
			{
				Utils.Message (this, "NotConfigured.Text", MessageType.Info, true);
				return;
			}
			else if (tables.Count > 1)
			{
				// bind tabs
				repeatTabs.DataSource = tables;
				repeatTabs.DataBind ();
			}

			// wireup LoadComplete handler 
			Page.LoadComplete += new EventHandler (OnLoadComplete);

            // bind employee search handlers
            buttonEmployeeSearch.Click += buttonEmployeeSearch_Click;

			// show first view if no session info available
			if (Session ["Launchpad_ActiveView_" + TabModuleId] == null)
			{
				// if no tabs set in settings, don't set active view
				if (tables != null && tables.Count > 0)
				{
					multiView.SetActiveView (FindView (tables [0]));
					SelectTab (tables [0]);
				}
			}

            // restore employee search text from session
            if (Session ["EmployeeSearch"] != null)
                textEmployeeSearch.Text = (string) Session ["EmployeeSearch"];
           
            // initialize Launchpad tables
            InitTables ();
        }

        void InitTables ()
        {
            var pageSize = LaunchpadSettings.PageSize;
            foreach (var table in Tables.Tables)
            {
                switch (table.Name)
                {
                    case "achievements":
                        table.Init (this, gridAchievements, buttonAddAchievement, pageSize);
                        break;
                    case "positions":
                        table.Init (this, gridPositions, buttonAddPosition, pageSize);
                        break;
                    case "divisions":
                        table.Init (this, gridDivisions, buttonAddDivision, pageSize);
                        break;
                    case "employees":
                        table.Init (this, gridEmployees, buttonAddEmployee, pageSize);
                        break;
                    case "edulevels":
                        table.Init (this, gridEduLevels, buttonAddEduLevel, pageSize);
                        break;
                    case "eduprograms":
                        table.Init (this, gridEduPrograms, buttonAddEduProgram, pageSize);
                        break;
                }
            }

            // can bind grid.ID => table now
            Tables.InitGridsDictionary ();
        }

		/// <summary>
		/// Fires when module load is complete (after all postback handlers)
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event agruments.</param>
		protected void OnLoadComplete (object sender, EventArgs e)
		{
			try
			{
				if (!IsPostBack)
				{
					// restore multiview state from session on first load
					if (Session ["Launchpad_ActiveView_" + TabModuleId] != null)
					{
						multiView.SetActiveView (FindView ((string)Session ["Launchpad_ActiveView_" + TabModuleId]));
						SelectTab ((string)Session ["Launchpad_ActiveView_" + TabModuleId]);
					}

                    // bind data to selected tables
                    foreach (var table in Tables.Tables)
                        if (LaunchpadSettings.Tables.Contains (table.Name))
                            table.DataBind (this);
				}
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		/// <summary>
		/// Mirrors multiview ActiveViewIndex changes in session variable
		/// </summary>
		/// <param name="sender">Sender (a MultiView)</param>
		/// <param name="e">Event arguments</param>
		protected void multiView_ActiveViewChanged (object sender, EventArgs e)
		{
			// set session variable to active view name without "view" prefix
			Session ["Launchpad_ActiveView_" + TabModuleId] = 
				multiView.GetActiveView ().ID.Substring (4).ToLowerInvariant ();
		}

		/// <summary>
		/// Handles ItemDataBound event for tabs repeater 
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event arguments.</param>
		protected void repeatTabs_ItemDataBound (object sender, RepeaterItemEventArgs e)
		{
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
                var link = (LinkButton) e.Item.FindControl ("linkTab");
                var tabName = (string) e.Item.DataItem;
				link.Text = Utils.FirstCharToUpperInvariant (tabName);
				link.CommandArgument = tabName;
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

                foreach (var table in Tables.Tables)
                    actions.Add (table.GetAction (this));

				return actions;
			}
		}

		#endregion

		protected void gridView_Sorting (object sender, GridViewSortEventArgs e)
		{
			var gv = sender as GridView;

			// retrieve the table from the session object
			var dt = GetDataTable (gv.ID);

			if (dt != null)
			{
				// sort the data 
                dt.DefaultView.Sort = e.SortExpression + " " + GetSortDirection (gv.ID, e.SortExpression);
				gv.DataSource = dt;
				gv.DataBind ();
			}
		}

        private string GetSortDirection (string controlID, string column)
		{
			// by default, set the sort direction to ascending
            var sortDirection = "ASC";

			// retrieve the last column that was sorted
			var sortExpression = ViewState [controlID + "SortExpression"] as string;

			if (sortExpression != null)
			{
				// check if the same column is being sorted
				// otherwise, the default value can be returned
                if (sortExpression == column)
                {
                    if (ViewState ["SortDirection"] != null)
                    {
                        var lastDirection = (string) ViewState ["SortDirection"];
                        if (lastDirection == "ASC")
                            sortDirection = "DESC";
                    }
				}
			}

			// Save new values in ViewState.
			// REVIEW: How this behave in case of multiple GridView's?
			ViewState ["SortDirection"] = sortDirection;
			ViewState [controlID + "SortExpression"] = column;

			return sortDirection;
		}

		protected void gridView_PageIndexChanging (object sender, GridViewPageEventArgs e)
		{
			var gv = sender as GridView;
			var dt = GetDataTable (gv.ID);

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

                // assuming e.Row.Cells[1] contains ID
                Tables.GridsDictionary [((GridView) sender).ID].SetEditLink (this, link, e.Row.Cells [1].Text);
			}
		}

		/// <summary>
		/// Makes tab selected by its name.
		/// </summary>
		/// <param name="tabName">Tab name.</param>
		protected void SelectTab (string tabName)
		{
			// enumerate all repeater items
            foreach (RepeaterItem item in repeatTabs.Items)
            {
                // enumerate all child controls in a item 
                foreach (var control in item.Controls)
                    if (control is HtmlControl)
                    {
                        // this means <li>
                        var li = control as HtmlControl;

                        // set CSS class attribute to <li>,
                        // depending on linkbutton's (first child of <li>) commandname
                        li.Attributes ["class"] = 
							((li.Controls [0] as LinkButton).CommandArgument == tabName) ? "ui-tabs-active" : "";
                    }
            }
		}

		/// <summary>
		/// Finds the view in a multiview by its name.
		/// </summary>
		/// <returns>The view.</returns>
		/// <param name="viewName">View name without prefix.</param>
		/// <param name="prefix">View name prefix.</param>
		protected View FindView (string viewName, string prefix = "view")
		{
			foreach (View view in multiView.Views)
				if (view.ID.ToLowerInvariant () == prefix + viewName)
					return view;

			return null;
		}

		/// <summary>
		/// Handles click on tab linkbutton
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">Event arguments.</param>
		protected void linkTab_Clicked (object sender, EventArgs e)
		{
			var tabName = (sender as LinkButton).CommandArgument;
			multiView.SetActiveView (FindView (tabName));
			SelectTab (tabName);
		}

        protected void buttonEmployeeSearch_Click (object sender, EventArgs e)
        {
            try
            {
                if (sender == buttonEmployeeResetSearch)
                {
                    textEmployeeSearch.Text = string.Empty;
                    Session ["EmployeeSearch"] = null;
                }
                else
                    Session ["EmployeeSearch"] = textEmployeeSearch.Text.Trim ();

                Tables.NamesDictionary ["employees"].DataBind (this, textEmployeeSearch.Text.Trim ());
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException(this, ex);
            }
        }
	}
}
