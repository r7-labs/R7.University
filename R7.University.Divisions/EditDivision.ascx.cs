//
//  EditDivision.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2020 Roman M. Yagodin
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
using DotNetNuke.Services.Localization;
using R7.Dnn.Extensions.Controls;
using R7.Dnn.Extensions.Text;
using R7.Dnn.Extensions.Utilities;
using R7.University.Commands;
using R7.University.ControlExtensions;
using R7.University.Divisions.Models;
using R7.University.Divisions.Queries;
using R7.University.Models;
using R7.University.Modules;
using R7.University.Queries;
using R7.University.SharedLogic;
using R7.University.ViewModels;

namespace R7.University.Divisions
{
    public partial class EditDivision: UniversityEditPortalModuleBase<DivisionInfo>
    {
        #region Types

        public enum EditDivisionTab
        {
            Common,
            Contacts,
            Documents,
            Bindings,
            Audit
        }

        #endregion

        #region Properties

        protected EditDivisionTab SelectedTab {
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
            var itemId = ParseHelper.ParseToNullable<int> (Request.QueryString ["division_id"]);

            // FIXME: Possible circular dependency as list can still contain childrens of current division
            parentDivisionSelector.DataSource = new DivisionQuery (ModelContext).ListExcept (itemId).OrderBy (d => d.Title);
            parentDivisionSelector.DataBind ();

            // init working hours
            WorkingHoursLogic.Init (this, comboWorkingHours);

            // bind positions
            var positions = new FlatQuery<PositionInfo> (ModelContext).ListOrderBy (p => p.Title);
            comboHeadPosition.DataSource = positions.Select (p => new {
                p.PositionID,
                Title = UniversityFormatHelper.FormatTitleWithShortTitle (p.Title, p.ShortTitle)
            });

            comboHeadPosition.DataBind ();
            comboHeadPosition.InsertDefaultItem (LocalizeString ("NotSelected.Text"));
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override string GetItemTitle (DivisionInfo item)
        {
            return item.Title;
        }

        protected override void LoadItem (DivisionInfo item)
        {
            base.LoadItem (item);

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
            checkIsSingleEntity.Checked = item.IsSingleEntity;
            checkIsInformal.Checked = item.IsInformal;
            checkIsGoverning.Checked = item.IsGoverning;
            comboHeadPosition.SelectByValue (item.HeadPositionID);

            // load working hours
            WorkingHoursLogic.Load (comboWorkingHours, textWorkingHours, item.WorkingHours);

            // select parent division
            parentDivisionSelector.DivisionId = item.ParentDivisionID;

            // set HomePage url
            if (!string.IsNullOrWhiteSpace (item.HomePage))
                urlHomePage.Url = item.HomePage;
            else {
                urlHomePage.Url = string.Empty;
            }

            // set Document url
            if (!string.IsNullOrWhiteSpace (item.DocumentUrl))
                urlDocumentUrl.Url = item.DocumentUrl;
            else {
                urlDocumentUrl.Url = string.Empty;
            }

            ctlAudit.Bind (item, PortalId, LocalizeString ("Unknown"));
        }

        protected override void BeforeUpdateItem (DivisionInfo item, bool isNew)
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
            item.HomePage = urlHomePage.Url;
            item.DocumentUrl = urlDocumentUrl.Url;
            item.StartDate = datetimeStartDate.SelectedDate;
            item.EndDate = datetimeEndDate.SelectedDate;
            item.IsSingleEntity = checkIsSingleEntity.Checked;
            item.IsInformal = checkIsInformal.Checked;
            item.IsGoverning = checkIsGoverning.Checked;
            item.HeadPositionID = ParseHelper.ParseToNullable<int> (comboHeadPosition.SelectedValue, true);
        }

        #region Implemented abstract members of UniverisityEditPortalModuleBase

        protected override int GetItemId (DivisionInfo item) => item.DivisionID;

        protected override void AddItem (DivisionInfo item)
        {
            if (SecurityContext.CanAdd (typeof (DivisionInfo))) {

                // update working hours
                item.WorkingHours = WorkingHoursLogic.Update (
                    comboWorkingHours,
                    textWorkingHours.Text,
                    checkAddToVocabulary.Checked
                );

                new AddDivisionCommand (ModelContext, SecurityContext).Add (item, DateTime.Now);
                ModelContext.SaveChanges ();

                // then adding new division from Division module, 
                // set calling module to display new division info
                if (ModuleConfiguration.ModuleDefinition.DefinitionName == "R7_University_Division") {
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

            new UpdateDivisionCommand (ModelContext, SecurityContext).Update (item, DateTime.Now);
            ModelContext.SaveChanges ();
        }

        protected override void DeleteItem (DivisionInfo item)
        {
            new DeleteDivisionCommand (ModelContext, SecurityContext).Delete (item);
            ModelContext.SaveChanges ();
        }

        #endregion
    }
}

