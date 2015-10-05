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
using R7.University.Extensions;

namespace R7.University.Division
{
	public partial class EditDivision : DivisionPortalModuleBase
	{
		// ALT: private int itemId = Null.NullInteger;
		private int? itemId = null;

		#region Handlers

		/// <summary>
		/// Handles Init event for a control.
		/// </summary>
		/// <param name="e">Event args.</param>
		protected override void OnInit (EventArgs e)
		{
			base.OnInit (e);

			// set url for Cancel link
			linkCancel.NavigateUrl = Globals.NavigateURL ();

			// add confirmation dialog to delete button
			buttonDelete.Attributes.Add ("onClick", "javascript:return confirm('" + Localization.GetString ("DeleteItem") + "');");

			// parse QueryString
			itemId = Utils.ParseToNullableInt (Request.QueryString ["division_id"]);
		
			// fill divisions dropdown
            var divisions = DivisionController.GetObjects<DivisionInfo> ()
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
			// TODO: Org. structure vocabulary name must be set in settings
			var termCtrl = new TermController ();
			var terms = termCtrl.GetTermsByVocabulary ("University_Structure").ToList (); 

			// add default term, 
			// TermId = Null.NullInteger is set in cstor
			terms.Insert (0, new Term (Localization.GetString ("NotSelected.Text", LocalResourceFile)));

            // bind terms to the tree
            treeDivisionTerms.DataSource = terms;
            treeDivisionTerms.DataBind ();
		}

		/// <summary>
		/// Handles Load event for a control.
		/// </summary>
		/// <param name="e">Event args.</param>
		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);
			
			try
			{
				// parse querystring parameters
				itemId = Utils.ParseToNullableInt (Request.QueryString ["division_id"]);
      
				if (!IsPostBack)
				{
					// load the data into the control the first time we hit this page

					// check we have an item to lookup
					// ALT: if (!Null.IsNull (itemId) 
					if (itemId != null)
					{

						// load the item
						var item = DivisionController.Get<DivisionInfo> (itemId.Value);

						if (item != null)
						{

							txtTitle.Text = item.Title;
							txtShortTitle.Text = item.ShortTitle;
							txtWebSite.Text = item.WebSite;
							textWebSiteLabel.Text = item.WebSiteLabel;
							txtEmail.Text = item.Email;
							txtSecondaryEmail.Text = item.SecondaryEmail;
							txtLocation.Text = item.Location;
							txtPhone.Text = item.Phone;
							txtFax.Text = item.Fax;
							
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
						else
							Response.Redirect (Globals.NavigateURL (), true);
					}
					else
					{
						buttonDelete.Visible = false;
						ctlAudit.Visible = false;
					}
				}
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		/// <summary>
		/// Handles Click event for Update button
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// Event args.
		/// </param>
		protected void buttonUpdate_Click (object sender, EventArgs e)
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
					item = DivisionController.Get<DivisionInfo> (itemId.Value);
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

				// update working hours
				item.WorkingHours = WorkingHoursLogic.Update (comboWorkingHours, textWorkingHours.Text, checkAddToVocabulary.Checked);
				
				if (!itemId.HasValue)
				{
					// update audit info
					item.CreatedByUserID = item.LastModifiedByUserID = this.UserId;
					item.CreatedOnDate = item.LastModifiedOnDate = DateTime.Now;

					DivisionController.Add<DivisionInfo> (item);

					// then adding new division from Division module, 
					// set calling module to display new division info
					if (ModuleConfiguration.ModuleDefinition.DefinitionName == "R7.University.Division")
					{
						var mctrl = new ModuleController ();
						DivisionSettings.DivisionID = item.DivisionID;
					}
				}
				else
				{
					// update audit info
					item.LastModifiedByUserID = this.UserId;
					item.LastModifiedOnDate = DateTime.Now;

					DivisionController.Update<DivisionInfo> (item);
				}

				Utils.SynchronizeModule (this);

				Response.Redirect (Globals.NavigateURL (), true);
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		/// <summary>
		/// Handles Click event for Delete button
		/// </summary>
		/// <param name='sender'>
		/// Sender.
		/// </param>
		/// <param name='e'>
		/// Event args.
		/// </param>
		protected void buttonDelete_Click (object sender, EventArgs e)
		{
			try
			{
				// ALT: if (!Null.IsNull (itemId))
				if (itemId.HasValue)
				{
					DivisionController.Delete<DivisionInfo> (itemId.Value);
					Response.Redirect (Globals.NavigateURL (), true);
				}
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		#endregion
	}
}

