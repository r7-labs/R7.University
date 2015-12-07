//
// ViewDivision.ascx.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2014 
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
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Entities.Content.Taxonomy;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.FileSystem;
using R7.University;

namespace R7.University.Division
{
	public partial class ViewDivision : DivisionPortalModuleBase, IActionable
	{
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

		/*
		/// <summary>
		/// Handles Init event for a control
		/// </summary>
		/// <param name="e">Event args.</param>
		protected override void OnInit (EventArgs e)
		{
			base.OnInit (e);
		}*/

		/// <summary>
		/// Handles Load event for a control
		/// </summary>
		/// <param name="e">Event args.</param>
		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);
			
			try
			{
				if (!IsPostBack || ViewState.Count == 0) // Fix for issue #23
				{
					var display = false;
					if (!Null.IsNull (DivisionSettings.DivisionID))
					{
						var item = DivisionController.Get<DivisionInfo> (DivisionSettings.DivisionID);
						if (item != null)
						{	
							display = true;
							DisplayDivision (item);
						}
					}

					if (!display)
					{
						if (IsEditable)
						{
							Utils.Message (this, "NothingToDisplay.Text", MessageType.Info, true);
							// hide only module content
							panelDivision.Visible = false;
						}
						else
							// hide entire module from regular users
							ContainerControl.Visible = false;
					}
				}
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		#endregion

		protected void DisplayDivision (DivisionInfo division)
		{
			// division title
			var divisionTitle = division.Title;

			// add division short title
			if (division.HasUniqueShortTitle)
			{
				divisionTitle += string.Format (" ({0})", division.ShortTitle);
			}

			// home page 
			int homeTabId;
			if (int.TryParse (division.HomePage, out homeTabId) && TabId != homeTabId)
			{
				// has home page, display as link 
				linkHomePage.Text = divisionTitle;
				linkHomePage.NavigateUrl = Globals.NavigateURL (homeTabId);
				labelTitle.Visible = false;
			}
			else
			{
				// no home page, display as label
				labelTitle.Text = divisionTitle;
				linkHomePage.Visible = false;
			}

			// link to division resources
			var displaySearchByTerm = false;
			if (division.DivisionTermID != null)
			{
				var termCtrl = new TermController ();
				var term = termCtrl.GetTerm (division.DivisionTermID.Value);
				if (term != null)
				{
					// NOTE: Add raw tag to Globals.NavigateURL to allow search work 
					// independently of current friendly urls settings
					// linkSearchByTerm.NavigateUrl = "/Default.aspx?tabid=" + PortalSettings.SearchTabId + "&tag=" + term.Name;
					linkSearchByTerm.NavigateUrl = Globals.NavigateURL (PortalSettings.SearchTabId) + "?tag=" + term.Name;
					displaySearchByTerm = true;
				}
			}

			if (!displaySearchByTerm)
				linkSearchByTerm.Visible = false;

			// WebSite
			if (!string.IsNullOrWhiteSpace (division.WebSite))
			{
				linkWebSite.NavigateUrl = division.FormatWebSiteUrl;
				linkWebSite.Text = division.FormatWebSiteLabel;
			}
			else
				linkWebSite.Visible = false;
				
			// email
			if (!string.IsNullOrWhiteSpace (division.Email))
			{
				linkEmail.Text = division.Email;
				linkEmail.NavigateUrl = "mailto:" + division.Email;
			}
			else
				linkEmail.Visible = false;

			// secondary email
			if (!string.IsNullOrWhiteSpace (division.SecondaryEmail))
			{
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
			if (!string.IsNullOrWhiteSpace (division.Location))
				labelLocation.Text = division.Location;
			else
				labelLocation.Visible = false;

			// working hours
			if (!string.IsNullOrWhiteSpace (division.WorkingHours))
				labelWorkingHours.Text = division.WorkingHours;
			else
				labelWorkingHours.Visible = false;

			// document
			if (!string.IsNullOrWhiteSpace (division.DocumentUrl))
			{
				// apply CSS class according to url type or file extension
				var urlType = Globals.GetURLType (division.DocumentUrl);
				if (urlType == TabType.File)
				{
					var file = FileManager.Instance.GetFile (int.Parse (division.DocumentUrl.Remove (0, "FileID=".Length)));
					linkDocumentUrl.CssClass += " " + file.Extension.ToLowerInvariant ();
				}
				else if (urlType == TabType.Tab)
					linkDocumentUrl.CssClass += " page";
				else 
					linkDocumentUrl.CssClass += " url";

				linkDocumentUrl.Text = LocalizeString ("DocumentUrl.Text");
				linkDocumentUrl.NavigateUrl = Globals.LinkClick (division.DocumentUrl, TabId, ModuleId);
				linkDocumentUrl.Target = "_blank";
			}
			else
				linkDocumentUrl.Visible = false;

			// barcode image test
			var barcodeWidth = DivisionSettings.BarcodeWidth;
            imageBarcode.ImageUrl = R7.University.Utilities.UrlUtils.FullUrl (string.Format (
                "/imagehandler.ashx?barcode=1&width={0}&height={1}&type=qrcode&encoding=UTF-8&content={2}",
				barcodeWidth, barcodeWidth, 
				Server.UrlEncode (division.VCard.ToString ()
						.Replace ("+", "%2b")) // fix for "+" signs in phone numbers
            ));

			imageBarcode.ToolTip = Localization.GetString ("imageBarcode.ToolTip", LocalResourceFile);
			imageBarcode.AlternateText = Localization.GetString ("imageBarcode.AlternateText", LocalResourceFile);

			// get & bind subdivisions
			var subDivisions = DivisionController.GetObjects<DivisionInfo> (
                "WHERE [ParentDivisionID] = @0", division.DivisionID)
                .Where (d => IsEditable || d.IsPublished)
                .OrderBy (d => d.Title)
                .Select (d => new SubDivisionViewModel (d, ViewModelContext)); 
			
            if (subDivisions.Any ())
			{
				repeatSubDivisions.DataSource = subDivisions;
				repeatSubDivisions.DataBind ();
			}
		}

		#region IActionable implementation

		public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
		{
			get
			{
				// create a new action to add an item, this will be added 
				// to the controls dropdown menu
				var actions = new ModuleActionCollection ();
				var existingDivision = !Null.IsNull (DivisionSettings.DivisionID);

				actions.Add (
					GetNextActionID (), 
					Localization.GetString ("AddDivision.Action", LocalResourceFile),
					ModuleActionType.AddContent, 
					"", 
					"",
					Utils.EditUrl (this, "EditDivision"),
					false, 
					DotNetNuke.Security.SecurityAccessLevel.Edit,
					!existingDivision,
					false
				);

				actions.Add (
					GetNextActionID (), 
					Localization.GetString ("EditDivision.Action", LocalResourceFile),
					ModuleActionType.EditContent, 
					"", 
					"", 
					Utils.EditUrl (this, "EditDivision", "division_id", DivisionSettings.DivisionID.ToString ()),
					false, 
					DotNetNuke.Security.SecurityAccessLevel.Edit,
					existingDivision, 
					false
				);

				actions.Add (
					GetNextActionID (), 
					Localization.GetString ("VCard.Action", LocalResourceFile),
					ModuleActionType.ContentOptions, 
					"", 
					"", 
					Utils.EditUrl (this, "VCard", "division_id", DivisionSettings.DivisionID.ToString ()),
					false, 
					DotNetNuke.Security.SecurityAccessLevel.View,
					existingDivision, 
					true // open in new window
				);

				return actions;
			}
		}

		#endregion
    }
}
