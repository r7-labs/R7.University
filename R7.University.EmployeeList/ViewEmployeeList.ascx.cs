//
// ViewEmployeeList.ascx.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2014-2016 Roman M. Yagodin
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
using System.Collections.Generic;
using System.Linq;
using System.Web.Caching;
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using R7.DotNetNuke.Extensions.ModuleExtensions;
using R7.DotNetNuke.Extensions.Modules;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University;
using R7.University.Components;
using R7.University.Data;
using R7.University.EmployeeList.Components;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.SharedLogic;
using R7.University.ViewModels;

namespace R7.University.EmployeeList
{
    public partial class ViewEmployeeList: PortalModuleBase<EmployeeListSettings>, IActionable
    {
        #region Properties

        protected string EditIconUrl
        {
            get { return IconController.IconURL ("Edit"); }
        }

        #endregion

        #region Handlers

        // REVIEW: Move to repository class?
        protected IEnumerable<IEmployee> GetEmployees ()
        {
            var cacheKey = "//r7_University/EmployeeList?TabModuleId=" + TabModuleId;
            return DataCache.GetCachedData<IEnumerable<IEmployee>> (
                new CacheItemArgs (cacheKey, UniversityConfig.Instance.DataCacheTime, CacheItemPriority.Normal),
                c => GetEmployees_Internal ()
            );
        }

        protected IEnumerable<IEmployee> GetEmployees_Internal ()
        {
            // get employees by DivisionID
            return EmployeeRepository.Instance.GetEmployees_ByDivisionId (Settings.DivisionID,
                Settings.IncludeSubdivisions, Settings.SortType)
                    .WithAchievements ()
                    .WithOccupiedPositions (Settings.DivisionID);
        }

        /// <summary>
        /// Handles Load event for a control
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);
			
            try {
                if (!IsPostBack || ViewState.Count == 0) { // Fix for issue #23
                    var items = GetEmployees ();
            
                    // check if we have some content to display, 
                    // otherwise display a message for module editors or hide module from regular users
                    if (items.IsNullOrEmpty ()) {
                        if (IsEditable) {
                            this.Message ("NothingToDisplay.Text", MessageType.Info, true);
                        }
                        else {
							// hide entire module
							ContainerControl.Visible = false;
                        }
                    }
                    else {
                        // bind the data
                        listEmployees.DataSource = items.Where (empl => IsEditable || empl.IsPublished ());
                        listEmployees.DataBind ();
                    }
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
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
                actions.Add (
                    GetNextActionID (), 
                    Localization.GetString ("AddEmployee.Action", this.LocalResourceFile),
                    ModuleActionType.AddContent, 
                    "", 
                    "", 
                    Null.IsNull (Settings.DivisionID) ?
                        EditUrl ("EditEmployee")
                    // pass division_id to select division in which to add employee
                        : EditUrl ("division_id", Settings.DivisionID.ToString (), "EditEmployee"),
                    false, 
                    SecurityAccessLevel.Edit,
                    true, 
                    false
                );
			
                return actions;
            }
        }

        #endregion

        /// <summary>
        /// Handles the items being bound to the datalist control. In this method we merge the data with the
        /// template defined for this control to produce the result to display to the user
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void listEmployees_ItemDataBound (object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
        {
            // e.Item.DataItem is of EmployeeListInfo class
            var employee = (EmployeeInfo) e.Item.DataItem;
			
            // find controls in DataList item template
            var linkEdit = (HyperLink) e.Item.FindControl ("linkEdit");
            var imageEdit = (Image) e.Item.FindControl ("imageEdit");
            var imagePhoto = (Image) e.Item.FindControl ("imagePhoto");
            var linkDetails = (HyperLink) e.Item.FindControl ("linkDetails"); 
            var linkFullName = (HyperLink) e.Item.FindControl ("linkFullName");
            var labelAcademicDegreeAndTitle = (Label) e.Item.FindControl ("labelAcademicDegreeAndTitle");
            var labelPositions = (Label) e.Item.FindControl ("labelPositions");
            var labelPhones = (Label) e.Item.FindControl ("labelPhones");
            var linkEmail = (HyperLink) e.Item.FindControl ("linkEmail");
            var linkSecondaryEmail = (HyperLink) e.Item.FindControl ("linkSecondaryEmail");
            var linkWebSite = (HyperLink) e.Item.FindControl ("linkWebSite");
            var linkUserProfile = (HyperLink) e.Item.FindControl ("linkUserProfile");

            // edit link
            if (IsEditable) {
                if (Null.IsNull (Settings.DivisionID)) {
                    linkEdit.NavigateUrl = EditUrl ("employee_id", employee.EmployeeID.ToString (), "EditEmployee");
                }
                else {
                    linkEdit.NavigateUrl = EditUrl ("employee_id", employee.EmployeeID.ToString (),
                        "EditEmployee", "division_id", Settings.DivisionID.ToString ());
                }
            }

            // make edit link visible in edit mode
            linkEdit.Visible = IsEditable;
            imageEdit.Visible = IsEditable;
            
            // mark non-published employees, as they visible only to editors
            if (!employee.IsPublished ()) {
                if (e.Item.ItemType == ListItemType.Item) {
                    e.Item.CssClass = listEmployees.ItemStyle.CssClass + " _nonpublished";
                }
                else {
                    e.Item.CssClass = listEmployees.AlternatingItemStyle.CssClass + " _nonpublished";
                }
            }

            // fill the controls

            // employee photo
            EmployeePhotoLogic.Bind (employee, imagePhoto, Settings.PhotoWidth, true);

            var employeeDetailsUrl = EditUrl ("employee_id", employee.EmployeeID.ToString (), "EmployeeDetails");
                
            // photo fallback
            if (string.IsNullOrWhiteSpace (imagePhoto.ImageUrl)) {
                linkDetails.Visible = false;
            }
            else {
                // link to employee details
                linkDetails.NavigateUrl = employeeDetailsUrl;
            }

            // employee fullname
            linkFullName.Text = employee.FullName;
            linkFullName.NavigateUrl = employeeDetailsUrl;

            // get current employee title achievements
            var achievements = employee.Achievements.Where (ach => ach.IsTitle);
            var titles = achievements.Select (ach => R7.University.Utilities.Utils.FirstCharToLower (ach.DisplayShortTitle));
			
            // employee title achievements
            var strTitles = TextUtils.FormatList (", ", titles);
            if (!string.IsNullOrWhiteSpace (strTitles))
                labelAcademicDegreeAndTitle.Text = "&nbsp;&ndash; " + strTitles;
            else
                labelAcademicDegreeAndTitle.Visible = false;
			
            // phones
            var phones = TextUtils.FormatList (", ", employee.Phone, employee.CellPhone);
            if (!string.IsNullOrWhiteSpace (phones))
                labelPhones.Text = phones;
            else
                labelPhones.Visible = false;

            // email
            if (!string.IsNullOrWhiteSpace (employee.Email)) {
                linkEmail.NavigateUrl = "mailto:" + employee.Email;
                linkEmail.Text = employee.Email; 
            }
            else
                linkEmail.Visible = false;

            // secondary email
            if (!string.IsNullOrWhiteSpace (employee.SecondaryEmail)) {
                linkSecondaryEmail.NavigateUrl = "mailto:" + employee.SecondaryEmail;
                linkSecondaryEmail.Text = employee.SecondaryEmail; 
            }
            else
                linkSecondaryEmail.Visible = false;

            // webSite
            if (!string.IsNullOrWhiteSpace (employee.WebSite)) {
                linkWebSite.NavigateUrl = FormatHelper.FormatWebSiteUrl (employee.WebSite);
                linkWebSite.Text = FormatHelper.FormatWebSiteLabel (employee.WebSite, employee.WebSiteLabel);
            }
            else {
                linkWebSite.Visible = false;
            }

            // profile link
            if (!TypeUtils.IsNull<int> (employee.UserID)) {
                linkUserProfile.NavigateUrl = Globals.UserProfileURL (employee.UserID.Value);
                // TODO: Replace profile text with something more sane
                linkUserProfile.Text = Localization.GetString ("VisitProfile.Text", LocalResourceFile);
            }
            else
                linkUserProfile.Visible = false;

            // get current employee occupied positions
            var ops = employee.OccupiedPositions;

            // build positions value
            var positionsVisible = false;
            if (!ops.IsNullOrEmpty ()) {
                var strOps = string.Empty;
                foreach (var op in OccupiedPositionInfoEx.GroupByDivision (ops)) {
                    var strOp = PositionInfo.FormatShortTitle (op.PositionTitle, op.PositionShortTitle);

                    // op.PositionShortTitle is a comma-separated list of positions, including TitleSuffix
                    strOps = TextUtils.FormatList ("; ", strOps, TextUtils.FormatList (": ", strOp, 
                        // do not display division title also for current division
                            (op.DivisionID != Settings.DivisionID) ? op.FormatDivisionLink (this) : string.Empty));
                }

                if (!string.IsNullOrWhiteSpace (strOps)) {
                    labelPositions.Text = strOps;
                    positionsVisible = true;
                }
            }
            labelPositions.Visible = positionsVisible;
        }
    }
}

