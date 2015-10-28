//
// ViewDivisionDirectory.ascx.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2014-2015
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Data;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.FileSystem;
using R7.University;
using R7.University.Extensions;

namespace R7.University.DivisionDirectory
{
    // TODO: Make module instances co-exist on same page

    public partial class ViewDivisionDirectory : DivisionDirectoryPortalModuleBase
    {
        #region Properties

        protected string SearchText
        {
            get
            { 
                var objSearchText = Session ["DivisionDirectory.SearchText." + TabModuleId];
                return (string) objSearchText ?? string.Empty;
            }
            set { Session ["DivisionDirectory.SearchText." + TabModuleId] = value; }
        }

        protected string SearchDivision
        {
            get
            { 
                var objSearchDivision = Session ["DivisionDirectory.SearchDivision." + TabModuleId];
                return (string) objSearchDivision ?? Null.NullInteger.ToString ();

            }
            set { Session ["DivisionDirectory.SearchDivision." + TabModuleId] = value; }
        }

        protected bool SearchIncludeSubdivisions
        {
            get
            { 
                var objSearchIncludeSubdivisions = Session ["DivisionDirectory.SearchIncludeSubdivisions." + TabModuleId];
                return objSearchIncludeSubdivisions != null ? (bool) objSearchIncludeSubdivisions : true;

            }
            set { Session ["DivisionDirectory.SearchIncludeSubdivisions." + TabModuleId] = value; }
        }

        #endregion

        private ViewModelContext viewModelContext;
        protected ViewModelContext ViewModelContext
        {
            get
            { 
                if (viewModelContext == null)
                    viewModelContext = new ViewModelContext (this);

                return viewModelContext;
            }
        }

        #region Handlers

        /// <summary>
        /// Handles Init event for a control
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            mviewDivisionDirectory.ActiveViewIndex = Utils.GetViewIndexByID (mviewDivisionDirectory, "view" + DivisionDirectorySettings.Mode.ToString ());

            if (DivisionDirectorySettings.Mode == DivisionDirectoryMode.Search)
            {
                // display search hint
                Utils.Message (this, "SearchHint.Info", MessageType.Info, true); 

                var divisions = DivisionDirectoryController.GetObjects <DivisionInfo> ().OrderBy (d => d.Title).ToList ();
                
                divisions.Insert (0, new DivisionInfo
                    {
                        DivisionID = Null.NullInteger, 
                        Title = LocalizeString ("AllDivisions.Text") 
                    });
           
                treeDivisions.DataSource = divisions;
                treeDivisions.DataBind ();

                // REVIEW: Level should be set in settings?
                Utils.ExpandToLevel (treeDivisions, 2);

                gridDivisions.LocalizeColumns (LocalResourceFile);
            }
            else if (DivisionDirectorySettings.Mode == DivisionDirectoryMode.ObrnadzorDivisions)
            {
                gridObrnadzorDivisions.LocalizeColumns (LocalResourceFile);
            }
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
                    if (DivisionDirectorySettings.Mode == DivisionDirectoryMode.Search)
                    {
                        if (!string.IsNullOrWhiteSpace (SearchText) || !string.IsNullOrWhiteSpace (SearchDivision))
                        {
                            // restore current search
                            textSearch.Text = SearchText;
                            Utils.SelectAndExpandByValue (treeDivisions, SearchDivision);
                            checkIncludeSubdivisions.Checked = SearchIncludeSubdivisions;

                            // perform search
                            if (SearchParamsOK (SearchText, SearchDivision, SearchIncludeSubdivisions, false))
                                DoSearch (SearchText, SearchDivision, SearchIncludeSubdivisions);
                        }
                    }
                    else if (DivisionDirectorySettings.Mode == DivisionDirectoryMode.ObrnadzorDivisions)
                    {
                        // getting all root divisions
                        var rootDivisions = DivisionDirectoryController.GetRootDivisions ().OrderBy (d => d.Title);

                        if (rootDivisions.Any ())
                        {
                            var divisions = new List<DivisionInfo> ();

                            foreach (var rootDivision in rootDivisions)
                            {
                                divisions.AddRange (DivisionDirectoryController.GetSubDivisions (rootDivision.DivisionID));
                            }

                            // bind divisions to the grid
                            var divisionViewModels = DivisionObrnadzorViewModel.Create (divisions, ViewModelContext);
                            gridObrnadzorDivisions.DataSource = divisionViewModels;
                            gridObrnadzorDivisions.DataBind ();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        #endregion

        protected bool SearchParamsOK (string searchText, string searchDivision, bool includeSubdivisions, bool showMessages = true)
        {
            var divisionIsSpecified = Utils.ParseToNullableInt (searchDivision) != null;
            var searchTextIsEmpty = string.IsNullOrWhiteSpace (searchText);

            // no search params - shouldn't perform search
            if (searchTextIsEmpty && !divisionIsSpecified)
            {
                if (showMessages)
                    Utils.Message (this, "SearchParams.Warning", MessageType.Warning, true);

                gridDivisions.Visible = false;
                return false;
            }
                
            // There are not much divisions as employees, so it's OK to don't check search phrase length
            /*
            if ((!divisionIsSpecified || // no division specified
                (divisionIsSpecified && includeSubdivisions)) && // division specified, but subdivisions flag is set
                (searchTextIsEmpty || // search phrase is empty
                (!searchTextIsEmpty && searchText.Length < 3))) // search phrase is too short
            {
                if (showMessages)
                    Utils.Message (this, "SearchPhrase.Warning", MessageType.Warning, true);

                gridDivisions.Visible = false;
                return false;
            }*/
           
            return true;
        }

        protected void DoSearch (string searchText, string searchDivision, bool includeSubdivisions)
        {
            var divisions = DivisionDirectoryController.FindDivisions (searchText,
                includeSubdivisions, searchDivision); 

            if (!divisions.Any ())
            {
                Utils.Message (this, "NoDivisionsFound.Warning", MessageType.Warning, true);
            }

            gridDivisions.DataSource = divisions;
            gridDivisions.DataBind ();

            // make divisions grid visible anyway
            gridDivisions.Visible = true;
        }

        protected void linkSearch_Click (object sender, EventArgs e)
        {
            var searchText = textSearch.Text.Trim ();
            var searchDivision = (treeDivisions.SelectedNode != null) ? 
                treeDivisions.SelectedNode.Value : Null.NullInteger.ToString ();
            var includeSubdivisions = checkIncludeSubdivisions.Checked;

            if (SearchParamsOK (searchText, searchDivision, includeSubdivisions))
            {
                // save current search
                SearchText = searchText;
                SearchDivision = searchDivision;
                SearchIncludeSubdivisions = includeSubdivisions;

                // perform search
                DoSearch (SearchText, SearchDivision, SearchIncludeSubdivisions);
            }
        }

        protected void gridDivisions_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            // show / hide edit column
            e.Row.Cells [0].Visible = IsEditable;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var division = (DivisionInfo) e.Row.DataItem;

                if (IsEditable)
                {
                    // get edit link controls
                    var linkEdit = (HyperLink) e.Row.FindControl ("linkEdit");
                    var iconEdit = (Image) e.Row.FindControl ("iconEdit");

                    // fill edit link controls
                    linkEdit.NavigateUrl = Utils.EditUrl (this, "EditDivision", "division_id", division.DivisionID.ToString ());
                    iconEdit.ImageUrl = IconController.IconURL ("Edit");
                }

                var labelTitle = (Label) e.Row.FindControl ("labelTitle");
                var linkTitle = (HyperLink) e.Row.FindControl ("linkTitle");
                var literalPhone = (Literal) e.Row.FindControl ("literalPhone");
                var linkEmail = (HyperLink) e.Row.FindControl ("linkEmail");
                var literalLocation = (Literal) e.Row.FindControl ("literalLocation");
                var linkDocument =  (HyperLink) e.Row.FindControl ("linkDocument");
                var linkContactPerson =  (HyperLink) e.Row.FindControl ("linkContactPerson");

                // division label / link
                var divisionTitle = division.Title + ((division.HasUniqueShortTitle)? string.Format (" ({0})", division.ShortTitle) : string.Empty);
                if (!string.IsNullOrWhiteSpace (division.HomePage))
                {
                    linkTitle.NavigateUrl = Utils.FormatURL (this, division.HomePage, false);
                    linkTitle.Text = divisionTitle;
                    labelTitle.Visible = false;
                }
                else
                {
                    labelTitle.Text = divisionTitle;
                    linkTitle.Visible = false;
                }

                literalPhone.Text = division.Phone;
                literalLocation.Text = division.Location;

                // email
                if (!string.IsNullOrWhiteSpace (division.Email))
                {
                    linkEmail.Text = division.Email;
                    linkEmail.NavigateUrl = division.FormatEmailUrl;
                }
                else
                    linkEmail.Visible = false;

                // (main) document
                if (!string.IsNullOrWhiteSpace (division.DocumentUrl))
                {
                    linkDocument.Text = LocalizeString ("Regulations.Text");
                    linkDocument.NavigateUrl = Globals.LinkClick (division.DocumentUrl, TabId, ModuleId);

                    // REVIEW: Add GetUrlCssClass() method to the utils
                    // set link CSS class according to file extension
                    if (Globals.GetURLType (division.DocumentUrl) == TabType.File)
                    {
                        var fileId = int.Parse (division.DocumentUrl.Remove (0, "FileId=".Length));
                        var file = FileManager.Instance.GetFile (fileId);
                        if (file != null)
                            linkDocument.CssClass = file.Extension.ToLowerInvariant ();
                    }
                }
                else
                    linkDocument.Visible = false;

                // contact person (head employee)
                var contactPerson = DivisionDirectoryController.GetHeadEmployee (division.DivisionID, division.HeadPositionID);
                if (contactPerson != null)
                {
                    linkContactPerson.Text = contactPerson.AbbrName;
                    linkContactPerson.ToolTip = contactPerson.FullName;
                    linkContactPerson.NavigateUrl = Utils.EditUrl (this, "EmployeeDetails", "employee_id", contactPerson.EmployeeID.ToString ()).Replace ("550,950", "450,950");
                }
            }
        }

        protected void gridObrnadzorDivisions_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            // show / hide edit column
            e.Row.Cells [0].Visible = IsEditable;

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var division = (DivisionObrnadzorViewModel) e.Row.DataItem;

                if (IsEditable)
                {
                    // get edit link controls
                    var linkEdit = (HyperLink) e.Row.FindControl ("linkEdit");
                    var iconEdit = (Image) e.Row.FindControl ("iconEdit");

                    // fill edit link controls
                    linkEdit.NavigateUrl = Utils.EditUrl (this, "EditDivision", "division_id", division.DivisionID.ToString ());
                    iconEdit.ImageUrl = IconController.IconURL ("Edit");
                }

                #region Contact person

                // REVIEW: Should not access database here, maybe in the model? 
                // TODO: Add link to open employee details

                var literalContactPerson = (Literal) e.Row.FindControl ("literalContactPerson");

                // contact person (head employee)
                var contactPerson = DivisionDirectoryController.GetHeadEmployee (division.DivisionID, division.HeadPositionID);
                if (contactPerson != null)
                {
                    var headPosition = DivisionDirectoryController.GetObjects<OccupiedPositionInfoEx> (
                        "WHERE [EmployeeID] = @0 AND [PositionID] = @1", 
                        contactPerson.EmployeeID, division.HeadPositionID).FirstOrDefault ();
                    
                    literalContactPerson.Text = "<strong itemprop=\"Fio\">" + contactPerson.FullName + "</strong><br />"
                        + headPosition.PositionShortTitle + " " + headPosition.TitleSuffix;
                }

                #endregion
            }
        }
    }
}

