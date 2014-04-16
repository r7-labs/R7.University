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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Modules.Actions;
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
					var items = ctrl.GetObjects<DivisionInfo> (this.ModuleId);

					// check if we have some content to display, 
					// otherwise display a message for module editors.
					if (items == null && IsEditable)
					{
						Utils.Message (this, MessageSeverity.Info, Localization.GetString ("NothingToDisplay.Text", LocalResourceFile));
					}
					else
					{
						// bind the data
						lstContent.DataSource = items;
						lstContent.DataBind ();
					}
				}
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		#endregion

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
					Utils.EditUrl (this, "Edit"),
					false, 
					DotNetNuke.Security.SecurityAccessLevel.Edit,
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
		protected void lstContent_ItemDataBound (object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
		{
			// use e.Item.DataItem as object of DivisionInfo class,
			// as we really know it is:
			var item = e.Item.DataItem as DivisionInfo;
			
			// find controls in DataList item template
			var lblUserName = e.Item.FindControl ("lblUserName") as Label;
			var lblCreatedOnDate = e.Item.FindControl ("lblCreatedOnDate") as Label;
			var lblContent = e.Item.FindControl ("lblContent") as Label;
			var linkEdit = e.Item.FindControl ("linkEdit") as HyperLink;
			var iconEdit = e.Item.FindControl ("imageEdit") as Image;
			
			// read module settings (may be useful in a future)
			// var settings = new DivisionSettings (this);            
            
			// edit link
			if (IsEditable)
			{
				linkEdit.NavigateUrl = Utils.EditUrl (this, "Edit", "DivisionID", item.DivisionID.ToString ());
				// WTF: iconEdit.NavigateUrl = Utils.FormatURL (this, image.Url, false);
			}

			// make edit link visible in edit mode
			linkEdit.Visible = IsEditable;
			iconEdit.Visible = IsEditable;
            
			// fill the controls

			lblCreatedOnDate.Text = item.CreatedOnDate.ToShortDateString ();
			lblContent.Text = Server.HtmlDecode (item.Title);
		}
	}
}

