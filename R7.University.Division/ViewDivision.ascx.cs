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
using DotNetNuke.Entities.Content.Taxonomy;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;

using R7.University;

namespace R7.University.Division
{
	public partial class ViewDivision : PortalModuleBase, IActionable
	{
		private bool divisionIDLoaded = false;
		private int divisionID = Null.NullInteger;

		protected int DivisionID
		{
			get 
			{
				if (divisionIDLoaded)
					return divisionID;
				else
				{
					var settings = new DivisionSettings (this);
					divisionID = settings.DivisionID;
					divisionIDLoaded = true;
					return divisionID;
				}
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
					var ctrl = new DivisionController ();

					var display = false;

					if (!Null.IsNull(DivisionID))
					{
						var item = ctrl.Get<DivisionInfo> (DivisionID);
						if (item != null )
						{	
							display = true;
							DisplayDivision(item);
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
			if (division.ShortTitle.Length < division.Title.Length)
				divisionTitle += string.Format (" ({0})", division.ShortTitle);

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
					linkSearchByTerm.NavigateUrl = Globals.NavigateURL (PortalSettings.SearchTabId, "", "Tag", term.Name);
					displaySearchByTerm = true;
				}
			}

			if (!displaySearchByTerm)
				linkSearchByTerm.Visible = false;

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
				labelFax.Text = string.Format (Localization.GetString("Fax.Format", LocalResourceFile), division.Fax);
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

			// barcode image test
			var settings = new DivisionSettings (this);
			var barcodeWidth = settings.BarcodeWidth;
			imageBarcode.ImageUrl = 
				string.Format ("/imagehandler.ashx?barcode=1&width={0}&height={1}&type=qrcode&encoding=UTF-8&content={2}",
					barcodeWidth, barcodeWidth, 
					Server.UrlEncode(division.VCard.ToString()
						.Replace("+","%2b")) // fix for "+" signs in phone numbers
			);

			imageBarcode.ToolTip = Localization.GetString ("imageBarcode.ToolTip", LocalResourceFile);
			imageBarcode.AlternateText = Localization.GetString ("imageBarcode.AlternateText", LocalResourceFile);

			// get & bind subdivisions
			var ctrl = new DivisionController ();
			var subDivisions = ctrl.GetObjects<DivisionInfo> (
				"WHERE [ParentDivisionID] = @0 ORDER BY [Title]", division.DivisionID); 
			if (subDivisions != null && subDivisions.Any ())
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
				var existingDivision = !Null.IsNull (DivisionID);

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
					Utils.EditUrl (this, "Edit", "division_id", DivisionID.ToString ()),
					false, 
					DotNetNuke.Security.SecurityAccessLevel.Edit,
					existingDivision, 
					false
				);

				actions.Add (
					GetNextActionID (), 
					Localization.GetString("VCard.Action", LocalResourceFile),
					ModuleActionType.ContentOptions, 
					"", 
					"", 
					Utils.EditUrl (this, "VCard", "division_id", DivisionID.ToString ()),
					false, 
					DotNetNuke.Security.SecurityAccessLevel.View,
					existingDivision, 
					true // open in new window
				);

				return actions;
			}
		}

		#endregion

		protected void repeaterSubDivisions_ItemDataBound (object sender, RepeaterItemEventArgs e)
		{
			// exclude header & footer
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				var division = e.Item.DataItem as DivisionInfo;

				var labelSubDivision = e.Item.FindControl ("labelSubDivision") as Label;
				var linkSubDivision = e.Item.FindControl ("linkSubDivision") as HyperLink;

				// home page 
				int homeTabId;
				if (int.TryParse (division.HomePage, out homeTabId) && TabId != homeTabId)
				{
					// has home page, display as link 
					linkSubDivision.NavigateUrl = Globals.NavigateURL (homeTabId);
					linkSubDivision.Text = division.Title;
					labelSubDivision.Visible = false;
				}
				else
				{
					// no home page, display as label
					labelSubDivision.Text = division.Title;
					linkSubDivision.Visible = false;
				}	
			}
		}

	}
}

