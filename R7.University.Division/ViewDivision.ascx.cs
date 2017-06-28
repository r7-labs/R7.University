//
//  ViewDivision.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2017 Roman M. Yagodin
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
using System.Linq;
using System.Web;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Content.Taxonomy;
using DotNetNuke.Entities.Icons;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Security;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using R7.Dnn.Extensions.ModuleExtensions;
using R7.Dnn.Extensions.Modules;
using R7.Dnn.Extensions.Utilities;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Components;
using R7.University.Division.Components;
using R7.University.Division.Queries;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Security;
using R7.University.Utilities;

namespace R7.University.Division
{
    public partial class ViewDivision : PortalModuleBase<DivisionSettings>, IActionable
    {
        ViewModelContext viewModelContext;
        protected ViewModelContext ViewModelContext
        {
            get { return viewModelContext ?? (viewModelContext = new ViewModelContext (this)); }
        }

        ISecurityContext securityContext;
        protected ISecurityContext SecurityContext
        {
            get { return securityContext ?? (securityContext = new ModuleSecurityContext (UserInfo)); }
        }

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
        /// Handles Load event for a control
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);
			
            try {
                if (!IsPostBack || ViewState.Count == 0) { // Fix for issue #23
                    
                    var division = new DivisionQuery (ModelContext).SingleOrDefault (Settings.DivisionID);
                    var hasData = division != null;
                    var now = HttpContext.Current.Timestamp;

                    if (!hasData) {
                        // division wasn't set or not found
                        if (IsEditable) {
                            this.Message ("NothingToDisplay.Text", MessageType.Info, true);
                        }
                    }
                    else if (!division.IsPublished (now)) {
                        // division isn't published
                        if (IsEditable) {
                            this.Message ("DivisionNotPublished.Text", MessageType.Warning, true);
                        }
                    }

                    // display module only in edit mode and only if we have published data to display
                    ContainerControl.Visible = IsEditable || (hasData && division.IsPublished (now));

                    // display module content only if it exists and published (or in edit mode)
                    var displayContent = hasData && (IsEditable || division.IsPublished (now));

                    panelDivision.Visible = displayContent;

                    if (displayContent) {
                        // display division info
                        DisplayDivision (division);
                    }
                }
            }
            catch (Exception ex) {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        #endregion

        protected void DisplayDivision (DivisionInfo division)
        {
            // division title
            var divisionTitle = division.Title;

            // add division short title
            if (division.HasUniqueShortTitle) {
                divisionTitle += string.Format (" ({0})", division.ShortTitle);
            }

            // home page 
            int homeTabId;
            if (int.TryParse (division.HomePage, out homeTabId) && TabId != homeTabId) {
                // has home page, display as link 
                linkHomePage.Text = divisionTitle;
                linkHomePage.NavigateUrl = Globals.NavigateURL (homeTabId);
                labelTitle.Visible = false;
            }
            else {
                // no home page, display as label
                labelTitle.Text = divisionTitle;
                linkHomePage.Visible = false;
            }

            // link to division resources
            var displaySearchByTerm = false;
            if (division.DivisionTermID != null) {
                var termCtrl = new TermController ();
                var term = termCtrl.GetTerm (division.DivisionTermID.Value);
                if (term != null) {
                    // add raw tag to Globals.NavigateURL to allow search work independently of current friendly urls settings
                    linkSearchByTerm.NavigateUrl = Globals.NavigateURL (PortalSettings.SearchTabId) + "?tag=" + term.Name;
                    displaySearchByTerm = true;
                }
            }

            if (!displaySearchByTerm)
                linkSearchByTerm.Visible = false;

            // WebSite
            if (!string.IsNullOrWhiteSpace (division.WebSite)) {
                linkWebSite.NavigateUrl = division.FormatWebSiteUrl;
                linkWebSite.Text = division.FormatWebSiteLabel;
            }
            else
                linkWebSite.Visible = false;
				
            // email
            if (!string.IsNullOrWhiteSpace (division.Email)) {
                linkEmail.Text = division.Email;
                linkEmail.NavigateUrl = "mailto:" + division.Email;
            }
            else
                linkEmail.Visible = false;

            // secondary email
            if (!string.IsNullOrWhiteSpace (division.SecondaryEmail)) {
                linkSecondaryEmail.Text = division.SecondaryEmail;
                linkSecondaryEmail.NavigateUrl = "mailto:" + division.SecondaryEmail;
            }
            else
                linkSecondaryEmail.Visible = false;

            // phone
            if (!string.IsNullOrWhiteSpace (division.Phone))
                labelPhone.Text = division.Phone;
            else
                labelPhone.Visible = false;

            // fax
            if (!string.IsNullOrWhiteSpace (division.Fax))
                labelFax.Text = string.Format (Localization.GetString ("Fax.Format", LocalResourceFile), division.Fax);
            else
                labelFax.Visible = false;

            // location
            var location = Settings.ShowAddress
                                   ? TextUtils.FormatList (", ", division.Address, division.Location)
                                   : division.Location;

            if (!string.IsNullOrWhiteSpace (location)) {
                labelLocation.Text = location;
            }
            else {
                labelLocation.Visible = false;
            }

            // working hours
            if (!string.IsNullOrWhiteSpace (division.WorkingHours))
                labelWorkingHours.Text = division.WorkingHours;
            else
                labelWorkingHours.Visible = false;

            // document
            if (!string.IsNullOrWhiteSpace (division.DocumentUrl)) {
                linkDocumentUrl.Text = LocalizeString ("DocumentUrl.Text");
                linkDocumentUrl.NavigateUrl = Globals.LinkClick (division.DocumentUrl, TabId, ModuleId);
            }
            else {
                linkDocumentUrl.Visible = false;
            }

            // setup barcode button
            linkBarcode.Attributes.Add ("data-module-id", ModuleId.ToString ());
            linkBarcode.Attributes.Add ("data-dialog-title", division.Title);

            // barcode image
            var barcodeWidth = UniversityConfig.Instance.Barcode.DefaultWidth;
            imageBarcode.ImageUrl = UniversityUrlHelper.FullUrl (string.Format (
                    "/imagehandler.ashx?barcode=1&width={0}&height={1}&type=qrcode&encoding=UTF-8&content={2}",
                    barcodeWidth, barcodeWidth,
                    Server.UrlEncode (division.VCard.ToString ()
				    .Replace ("+", "%2b")) // fix for "+" signs in phone numbers
                ));

            imageBarcode.ToolTip = Localization.GetString ("imageBarcode.ToolTip", LocalResourceFile);
            imageBarcode.AlternateText = Localization.GetString ("imageBarcode.AlternateText", LocalResourceFile);

            var now = HttpContext.Current.Timestamp;

            // get & bind subdivisions
            var subDivisions = division.SubDivisions
                .Where (d => IsEditable || d.IsPublished (now))
                .OrderBy (d => d.Title)
                .Select (d => new SubDivisionViewModel (d, ViewModelContext)); 
			
            if (subDivisions.Any ()) {
                repeatSubDivisions.DataSource = subDivisions;
                repeatSubDivisions.DataBind ();
            }
            else {
                panelSubDivisions.Visible = false;
            }
        }

        #region IActionable implementation

        public ModuleActionCollection ModuleActions
        {
            get {
                var actions = new ModuleActionCollection ();
                var existingDivision = !Null.IsNull (Settings.DivisionID);

                actions.Add (
                    GetNextActionID (),
                    LocalizeString ("AddDivision.Action"),
                    ModuleActionType.AddContent,
                    "",
                    IconController.IconURL ("Add"),
                    EditUrl ("EditDivision"),
                    false,
                    SecurityAccessLevel.Edit,
                    !existingDivision && SecurityContext.CanAdd (typeof (DivisionInfo)),
                    false
                );

                actions.Add (
                    GetNextActionID (), 
                    LocalizeString ("EditDivision.Action"),
                    ModuleActionType.EditContent, 
                    "", 
                    IconController.IconURL ("Edit"),
                    EditUrl ("division_id", Settings.DivisionID.ToString (), "EditDivision"),
                    false, 
                    SecurityAccessLevel.Edit,
                    existingDivision, 
                    false
                );

                return actions;
            }
        }

        #endregion
    }
}
