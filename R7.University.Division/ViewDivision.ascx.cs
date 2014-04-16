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
					var settings = new DivisionSettings (this);

					var display = false;

					if (!Null.IsNull(settings.DivisionID))
					{
						var item = ctrl.Get<DivisionInfo> (settings.DivisionID);
						if (item != null )
						{	
							display = true;
							DisplayDivision(item);
						}
					}

					if (!display && IsEditable)
					{
						Utils.Message (this, MessageSeverity.Info, Localization.GetString ("NothingToDisplay.Text", LocalResourceFile));
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
			labelTitle.Text = division.Title;

			// division short title
			if (division.ShortTitle.Length < division.Title.Length)
				labelShortTitle.Text = string.Format ("({0})", division.ShortTitle);
			else
				labelShortTitle.Visible = false;

			// link to division resources
			if (division.DivisionTermID != null)
			{
				var termCtrl = new TermController ();
				var term = termCtrl.GetTerm (division.DivisionTermID.Value);
				if (term != null)
				{
					linkTerm.Text = term.Name;
					linkTerm.NavigateUrl = Globals.NavigateURL (PortalSettings.SearchTabId, "", "Tag", term.Name);
				}
			}

			// home page 
			int homeTabId;
			if (int.TryParse (division.HomePage, out homeTabId) && TabId != homeTabId)
			{
				// REVIEW: Display tab name instead?
				linkHomePage.Text = "Home page";
				linkHomePage.NavigateUrl = Globals.NavigateURL (homeTabId);
			}
			else
				linkHomePage.Visible = false;

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
		}

		#region IActionable implementation

		public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
		{
			get
			{
				// create a new action to add an item, this will be added 
				// to the controls dropdown menu
				var actions = new ModuleActionCollection ();
				actions.Add (
					GetNextActionID (), 
					Localization.GetString (ModuleActionType.AddContent, this.LocalResourceFile),
					ModuleActionType.AddContent, 
					"", 
					"", 
					Utils.EditUrl (this, "EditDivision"),
					false, 
					DotNetNuke.Security.SecurityAccessLevel.Edit,
					true, 
					false
				);

				return actions;
			}
		}

		#endregion

	}
}

