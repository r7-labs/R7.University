//
//  ViewLaunchpad.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2016 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
//
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Data;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using R7.DotNetNuke.Extensions.ModuleExtensions;
using R7.DotNetNuke.Extensions.Modules;
using R7.University.Launchpad.Components;
using R7.University.Models;

namespace R7.University.Launchpad
{
    public partial class ViewLaunchpad : PortalModuleBase<LaunchpadSettings>, IActionable
    {
        #region Properties

        protected string EditIconUrl
        {
            get { return IconController.IconURL ("Edit"); }
        }

        protected LaunchpadTables Tables = new LaunchpadTables ();

        #endregion

        #region Model context

        private UniversityModelContext modelContext;
        protected UniversityModelContext ModelContext
        {
            get { return modelContext ?? (modelContext = new UniversityModelContext ()); }
        }

        public override void Dispose ()
        {
            if (modelContext != null) {
                modelContext.Dispose ();
            }

            base.Dispose ();
        }

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
            if (session == null) {
                var tabName = GetActiveTabName ();
                session = Tables.GetByGridId (gridviewId).GetDataTable (this, ModelContext, (string) Session [tabName + "_Search"]);
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
            var tables = Settings.Tables;
            if (tables == null || tables.Count == 0) {
                this.Message ("NotConfigured.Text", MessageType.Info, true);
                return;
            }
			
            if (tables.Count > 1) {
                // bind tabs
                repeatTabs.DataSource = tables;
                repeatTabs.DataBind ();
            }

            // wireup LoadComplete handler 
            Page.LoadComplete += OnLoadComplete;

            // show first view if no session info available
            if (Session ["Launchpad_ActiveView_" + TabModuleId] == null) {
                // if no tabs set in settings, don't set active view
                if (tables != null && tables.Count > 0) {
                    multiView.SetActiveView (FindView (tables [0]));
                    SelectTab (tables [0]);
                }
            }

            // initialize Launchpad tables
            InitTables ();
        }

        void InitTables ()
        {
            var pageSize = Settings.PageSize;
            foreach (var table in Tables.Tables) {
                switch (table.Name) {
                    case "achievements":
                        table.Init (this, gridAchievements, pageSize);
                        break;
                    case "positions":
                        table.Init (this, gridPositions, pageSize);
                        break;
                    case "divisions":
                        table.Init (this, gridDivisions, pageSize);
                        break;
                    case "employees":
                        table.Init (this, gridEmployees, pageSize);
                        break;
                    case "edulevels":
                        table.Init (this, gridEduLevels, pageSize);
                        break;
                    case "eduprograms":
                        table.Init (this, gridEduPrograms, pageSize);
                        break;
                    case "eduprogramprofiles":
                        table.Init (this, gridEduProgramProfiles, pageSize);
                        break;
                    case "documenttypes":
                        table.Init (this, gridDocumentTypes, pageSize);
                        break;
                    case "documents":
                        table.Init (this, gridDocuments, pageSize);
                        break;
                    case "eduforms":
                        table.Init (this, gridEduForms, pageSize);
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
            try {
                if (!IsPostBack) {
                    // restore multiview state from session on first load
                    if (Session ["Launchpad_ActiveView_" + TabModuleId] != null) {
                        multiView.SetActiveView (FindView ((string) Session ["Launchpad_ActiveView_" + TabModuleId]));
                        SelectTab ((string) Session ["Launchpad_ActiveView_" + TabModuleId]);
                    }

                    var tabName = GetActiveTabName ();

                    // restore search phrase from session
                    if (Session [tabName + "_Search"] != null) {
                        textSearch.Text = (string) Session [tabName + "_Search"];
                    }

                    BindTab (tabName);
                }
                else {
                    // get postback initiator
                    var eventTarget = Request.Form ["__EVENTTARGET"];

                    // check if tab was switched
                    if (!string.IsNullOrEmpty (eventTarget) && eventTarget.Contains ("$linkTab")) {
                        BindTab (GetActiveTabName ());
                    }
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void BindTab (string tabName)
        {
            // bind active table
            var table = Tables.GetByName (tabName);

            var searchText = textSearch.Text;
            if (!string.IsNullOrWhiteSpace (searchText)) {
                table.DataBind (this, ModelContext, searchText);
            }
            else {
                table.DataBind (this, ModelContext);
            }

            // set URL to add new item
            linkAddItem.NavigateUrl = table.GetAddUrl (this);
            linkAddItem.Enabled = table.IsEditable;
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
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) {
                var link = (LinkButton) e.Item.FindControl ("linkTab");
                var tabName = (string) e.Item.DataItem;
                link.Text = LocalizeString (Tables.GetByName (tabName).ResourceKey);
                link.CommandArgument = tabName;
            }
        }

        #endregion

        #region IActionable implementation

        public ModuleActionCollection ModuleActions
        {
            get {
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

            if (dt != null) {
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

            if (sortExpression != null) {
                // check if the same column is being sorted
                // otherwise, the default value can be returned
                if (sortExpression == column) {
                    if (ViewState ["SortDirection"] != null) {
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

            if (dt != null) {
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
            if (e.Row.RowType == DataControlRowType.DataRow) {
                // find edit hyperlink
                var linkEdit = (HyperLink) e.Row.Cells [0].FindControl ("linkEdit");
                var table = Tables.GetByGridId (((GridView) sender).ID);

                // assuming what e.Row.Cells[1] contains item ID
                linkEdit.NavigateUrl = table.GetEditUrl (this, e.Row.Cells [1].Text);
                linkEdit.Enabled = table.IsEditable;
            }
        }

        /// <summary>
        /// Makes tab selected by its name.
        /// </summary>
        /// <param name="tabName">Tab name.</param>
        protected void SelectTab (string tabName)
        {
            // enumerate all repeater items
            foreach (RepeaterItem item in repeatTabs.Items) {
                // enumerate all child controls in a item 
                foreach (var control in item.Controls)
                    if (control is HtmlControl) {
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

        protected string GetActiveTabName ()
        {
            return multiView.GetActiveView ().ID.ToLowerInvariant ().Replace ("view", "");
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

            // restore search phrase
            textSearch.Text = Session [tabName + "_Search"] != null ? 
                (string) Session [tabName + "_Search"] : string.Empty;
        }

        protected void buttonSearch_Click (object sender, EventArgs e)
        {
            try {
                var tabName = GetActiveTabName ();

                Session [tabName + "_Search"] = textSearch.Text.Trim ();

                Tables.GetByName (tabName).DataBind (this, ModelContext, textSearch.Text.Trim ());
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void buttonResetSearch_Click (object sender, EventArgs e)
        {
            try {
                var tabName = GetActiveTabName ();

                textSearch.Text = string.Empty;
                Session [tabName + "_Search"] = null;

                Tables.GetByName (tabName).DataBind (this, ModelContext);
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }
    }
}
