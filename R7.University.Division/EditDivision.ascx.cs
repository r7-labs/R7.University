using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Content.Taxonomy;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.UserControls;
using DotNetNuke.Web.UI.WebControls;
using R7.University;
using R7.University.ControlExtensions;
using DotNetNuke.R7;

namespace R7.University.Division
{
    public partial class EditDivision: EditModuleBase<DivisionController, DivisionSettings, DivisionInfo>
	{
        private int? itemId;

        protected EditDivision (): base ("division_id")
        {}

        /// <summary>
        /// Handles Init event for a control.
        /// </summary>
        /// <param name="e">Event args.</param>
        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            // parse QueryString
            itemId = Utils.ParseToNullableInt (Request.QueryString ["division_id"]);

            // fill divisions dropdown
            var divisions = Controller.GetObjects<DivisionInfo> ()
                // exclude current division
                .Where (d => (itemId == null || itemId != d.DivisionID)).OrderBy (dd => dd.Title).ToList ();

            // insert default item
            divisions.Insert (0, DivisionInfo.DefaultItem (LocalizeString ("NotSelected.Text")));

            // bind divisions to the tree
            treeParentDivisions.DataSource = divisions;
            treeParentDivisions.DataBind ();

            // init working hours
            WorkingHoursLogic.Init (this, comboWorkingHours);

            // Fill terms list
            // REVIEW: Org. structure vocabulary name must be set in settings?
            var termCtrl = new TermController ();
            var terms = termCtrl.GetTermsByVocabulary ("University_Structure").ToList (); 

            // add default term, 
            // TermId = Null.NullInteger is set in cstor
            terms.Insert (0, new Term (Localization.GetString ("NotSelected.Text", LocalResourceFile)));

            // bind terms to the tree
            treeDivisionTerms.DataSource = terms;
            treeDivisionTerms.DataBind ();

            // bind positions
            var positions = Controller.GetObjects<PositionInfo> ().OrderBy (p => p.Title).ToList ();
            positions.Insert (0, new PositionInfo { ShortTitle = LocalizeString ("NotSelected.Text"), PositionID = Null.NullInteger });
            comboHeadPosition.DataSource = positions;
            comboHeadPosition.DataBind ();
            comboHeadPosition.SelectedIndex = 0;
        }

        protected override void OnInitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override void OnLoadItem (DivisionInfo item)
        {
            // FIXME: Need support in EditModuleBase to drop this on top of OnLoad method
            // if (DotNetNuke.Framework.AJAX.IsInstalled ())
            //    DotNetNuke.Framework.AJAX.RegisterScriptManager ();
            
            txtTitle.Text = item.Title;
            txtShortTitle.Text = item.ShortTitle;
            txtWebSite.Text = item.WebSite;
            textWebSiteLabel.Text = item.WebSiteLabel;
            txtEmail.Text = item.Email;
            txtSecondaryEmail.Text = item.SecondaryEmail;
            txtLocation.Text = item.Location;
            txtPhone.Text = item.Phone;
            txtFax.Text = item.Fax;
            datetimeStartDate.SelectedDate = item.StartDate;
            datetimeEndDate.SelectedDate = item.EndDate;
            checkIsVirtual.Checked = item.IsVirtual;
            Utils.SelectByValue (comboHeadPosition, item.HeadPositionID);

            // load working hours
            WorkingHoursLogic.Load (comboWorkingHours, textWorkingHours, item.WorkingHours);

            // select parent division
            Utils.SelectAndExpandByValue (treeParentDivisions, item.ParentDivisionID.ToString ());

            // select taxonomy term
            var treeNode = treeDivisionTerms.FindNodeByValue (item.DivisionTermID.ToString ());
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

        protected override void OnUpdateItem (DivisionInfo item)
        {
            // nothing here, entire OnButtonUpdateClick is overriden
        }

		#region Handlers

		/// <summary>
		/// Handles Click event for Update button
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// Event args.
		/// </param>
        protected override void OnButtonUpdateClick (object sender, EventArgs e)
		{
			try
			{
				DivisionInfo item;
		
				// determine if we are adding or updating
				// ALT: if (Null.IsNull (itemId))
				if (!itemId.HasValue)
				{
					// add new record
					item = new DivisionInfo ();
				}
				else
				{	
					// update existing record
					item = Controller.Get<DivisionInfo> (itemId.Value);
				}

				// fill the object
				item.Title = txtTitle.Text.Trim ();
				item.ShortTitle = txtShortTitle.Text.Trim ();
				item.Email = txtEmail.Text.Trim ().ToLowerInvariant ();
				item.SecondaryEmail = txtSecondaryEmail.Text.Trim ().ToLowerInvariant ();
				item.Phone = txtPhone.Text.Trim ();
				item.Fax = txtFax.Text.Trim ();
				item.Location = txtLocation.Text.Trim ();
				item.WebSite = txtWebSite.Text.Trim ();
				item.WebSiteLabel = textWebSiteLabel.Text.Trim ();
				item.ParentDivisionID = Utils.ParseToNullableInt (treeParentDivisions.SelectedValue);
				item.DivisionTermID = Utils.ParseToNullableInt (treeDivisionTerms.SelectedValue);
				item.HomePage = urlHomePage.Url;
				item.DocumentUrl = urlDocumentUrl.Url;
                item.StartDate = datetimeStartDate.SelectedDate;
                item.EndDate = datetimeEndDate.SelectedDate;
                item.IsVirtual = checkIsVirtual.Checked;
                item.HeadPositionID = Utils.ParseToNullableInt (comboHeadPosition.SelectedValue);

				// update working hours
				item.WorkingHours = WorkingHoursLogic.Update (comboWorkingHours, textWorkingHours.Text, checkAddToVocabulary.Checked);
				
				if (!itemId.HasValue)
				{
					// update audit info
					item.CreatedByUserID = item.LastModifiedByUserID = this.UserId;
					item.CreatedOnDate = item.LastModifiedOnDate = DateTime.Now;

					Controller.Add<DivisionInfo> (item);

					// then adding new division from Division module, 
					// set calling module to display new division info
					if (ModuleConfiguration.ModuleDefinition.DefinitionName == "R7.University.Division")
					{
						Settings.DivisionID = item.DivisionID;
					}
				}
				else
				{
					// update audit info
					item.LastModifiedByUserID = this.UserId;
					item.LastModifiedOnDate = DateTime.Now;

					Controller.Update<DivisionInfo> (item);
				}

				Utils.SynchronizeModule (this);

				Response.Redirect (Globals.NavigateURL (), true);
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		#endregion
	}
}

