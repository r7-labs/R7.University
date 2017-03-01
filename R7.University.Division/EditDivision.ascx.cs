//
//  EditDivision.ascx.cs
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
using DotNetNuke.Entities.Content.Taxonomy;
using DotNetNuke.Services.Localization;
using R7.DotNetNuke.Extensions.ControlExtensions;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.Commands;
using R7.University.ControlExtensions;
using R7.University.Division.Components;
using R7.University.Division.Queries;
using R7.University.Models;
using R7.University.Modules;
using R7.University.Queries;
using R7.University.SharedLogic;

namespace R7.University.Division
{
    public partial class EditDivision: UniversityEditPortalModuleBase<DivisionInfo>
    {
        #region Types

        public enum EditDivisionTab
        {
            Common,
            Contacts,
            Documents,
            Bindings
        }

        #endregion

        #region Properties

        protected EditDivisionTab SelectedTab
        {
            get {
                // get postback initiator
                var eventTarget = Request.Form ["__EVENTTARGET"];

                if (!string.IsNullOrEmpty (eventTarget) && eventTarget.Contains ("$" + urlHomePage.ID + "$")) {
                    ViewState ["SelectedTab"] = EditDivisionTab.Bindings;
                    return EditDivisionTab.Bindings;
                }

                if (!string.IsNullOrEmpty (eventTarget) && eventTarget.Contains ("$" + urlDocumentUrl.ID + "$")) {
                    ViewState ["SelectedTab"] = EditDivisionTab.Documents;
                    return EditDivisionTab.Documents;
                }

                // otherwise, get current tab from viewstate
                var obj = ViewState ["SelectedTab"];
                return (obj != null) ? (EditDivisionTab) obj : EditDivisionTab.Common;
            }
            set { ViewState ["SelectedTab"] = value; }
        }

        #endregion

        protected EditDivision () : base ("division_id")
        {
        }

        /// <summary>
        /// Handles Init event for a control.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // parse QueryString
            var itemId = TypeUtils.ParseToNullable<int> (Request.QueryString ["division_id"]);

            // FIXME: Possible circular dependency as list can still contain childrens of current division
            parentDivisionSelector.DataSource = new DivisionQuery (ModelContext).ListExcept (itemId).OrderBy (d => d.Title);
            parentDivisionSelector.DataBind ();

            // init working hours
            WorkingHoursLogic.Init (this, comboWorkingHours);

            // Fill terms list
            // TODO: Org. structure vocabulary name must be set in settings?
            var termCtrl = new TermController ();
            var terms = termCtrl.GetTermsByVocabulary ("University_Structure").ToList (); 

            // add default term, 
            // TermId = Null.NullInteger is set in cstor
            terms.Insert (0, new Term (Localization.GetString ("NotSelected.Text", LocalResourceFile)));

            // bind terms to the tree
            treeDivisionTerms.DataSource = terms;
            treeDivisionTerms.DataBind ();

            // bind positions
            comboHeadPosition.DataSource = new FlatQuery<PositionInfo> (ModelContext).ListOrderBy (p => p.Title);
            comboHeadPosition.DataBind ();
            comboHeadPosition.InsertDefaultItem (LocalizeString ("NotSelected.Text"));
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override void LoadItem (DivisionInfo item)
        {
            txtTitle.Text = item.Title;
            txtShortTitle.Text = item.ShortTitle;
            txtWebSite.Text = item.WebSite;
            textWebSiteLabel.Text = item.WebSiteLabel;
            txtEmail.Text = item.Email;
            txtSecondaryEmail.Text = item.SecondaryEmail;
            textAddress.Text = item.Address;
            txtLocation.Text = item.Location;
            txtPhone.Text = item.Phone;
            txtFax.Text = item.Fax;
            datetimeStartDate.SelectedDate = item.StartDate;
            datetimeEndDate.SelectedDate = item.EndDate;
            checkIsVirtual.Checked = item.IsVirtual;
            checkIsInformal.Checked = item.IsInformal;
            comboHeadPosition.SelectByValue (item.HeadPositionID);

            // load working hours
            WorkingHoursLogic.Load (comboWorkingHours, textWorkingHours, item.WorkingHours);

            // select parent division
            parentDivisionSelector.DivisionId = item.ParentDivisionID;

            // select taxonomy term
            var treeNode = treeDivisionTerms.FindNodeByValue (item.DivisionTermID.ToString ());
            if (treeNode != null) {
                treeNode.Selected = true;

                // expand all parent nodes
                treeNode = treeNode.ParentNode;
                while (treeNode != null) {
                    treeNode.Expanded = true;
                    treeNode = treeNode.ParentNode;
                } 
            }
            else
                treeDivisionTerms.Nodes [0].Selected = true;

            // set HomePage url
            if (!string.IsNullOrWhiteSpace (item.HomePage))
                urlHomePage.Url = item.HomePage;
            else
                // or set to "None", if Url is empty
                urlHomePage.UrlType = "N";

            // set Document url
            if (!string.IsNullOrWhiteSpace (item.DocumentUrl))
                urlDocumentUrl.Url = item.DocumentUrl;
            else
                // or set to "None", if url is empty
                urlDocumentUrl.UrlType = "N";

            ctlAudit.Bind (item);
        }

        protected override void BeforeUpdateItem (DivisionInfo item)
        {
            // fill the object
            item.Title = txtTitle.Text.Trim ();
            item.ShortTitle = txtShortTitle.Text.Trim ();
            item.Email = txtEmail.Text.Trim ().ToLowerInvariant ();
            item.SecondaryEmail = txtSecondaryEmail.Text.Trim ().ToLowerInvariant ();
            item.Phone = txtPhone.Text.Trim ();
            item.Fax = txtFax.Text.Trim ();
            item.Address = textAddress.Text.Trim ();
            item.Location = txtLocation.Text.Trim ();
            item.WebSite = txtWebSite.Text.Trim ();
            item.WebSiteLabel = textWebSiteLabel.Text.Trim ();
            item.ParentDivisionID = parentDivisionSelector.DivisionId;
            item.DivisionTermID = TypeUtils.ParseToNullable<int> (treeDivisionTerms.SelectedValue);
            item.HomePage = urlHomePage.Url;
            item.DocumentUrl = urlDocumentUrl.Url;
            item.StartDate = datetimeStartDate.SelectedDate;
            item.EndDate = datetimeEndDate.SelectedDate;
            item.IsVirtual = checkIsVirtual.Checked;
            item.IsInformal = checkIsInformal.Checked;
            item.HeadPositionID = TypeUtils.ParseToNullable<int> (comboHeadPosition.SelectedValue);
        }

        #region Implemented abstract members of UniverisityEditPortalModuleBase

        protected override void AddItem (DivisionInfo item)
        {
            if (SecurityContext.CanAdd (typeof (DivisionInfo))) {

                // update working hours
                item.WorkingHours = WorkingHoursLogic.Update (
                    comboWorkingHours,
                    textWorkingHours.Text,
                    checkAddToVocabulary.Checked
                );

                new AddCommand<DivisionInfo> (ModelContext, SecurityContext).Add (item);
                ModelContext.SaveChanges ();

                // then adding new division from Division module, 
                // set calling module to display new division info
                if (ModuleConfiguration.ModuleDefinition.DefinitionName == "R7.University.Division") {
                    var settingsRepository = new DivisionSettingsRepository ();
                    var settings = settingsRepository.GetSettings (ModuleConfiguration);
                    settings.DivisionID = item.DivisionID;
                    settingsRepository.SaveSettings (ModuleConfiguration, settings);
                }
            }
        }

        protected override void UpdateItem (DivisionInfo item)
        {
            // update working hours
            item.WorkingHours = WorkingHoursLogic.Update (
                comboWorkingHours,
                textWorkingHours.Text,
                checkAddToVocabulary.Checked
            );

            // update audit info
            item.LastModifiedByUserID = UserId;
            item.LastModifiedOnDate = DateTime.Now;

            ModelContext.Update<DivisionInfo> (item);
            ModelContext.SaveChanges ();
        }

        protected override void DeleteItem (DivisionInfo item)
        {
            new DeleteCommand<DivisionInfo> (ModelContext, SecurityContext).Delete (item);
            ModelContext.SaveChanges ();
        }

        #endregion
    }
}

