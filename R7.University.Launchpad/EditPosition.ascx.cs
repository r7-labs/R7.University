using System;
using System.Web.UI.WebControls;
using System.Linq;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.UI.UserControls;
using R7.University;

namespace R7.University.Launchpad
{
	public partial class EditPosition : PortalModuleBase
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

			// wireup event handlers
			buttonUpdate.Click += buttonUpdate_Click;
			buttonDelete.Click += buttonDelete_Click;

			// add confirmation dialog to delete button
			buttonDelete.Attributes.Add ("onClick", "javascript:return confirm('" + Localization.GetString ("DeleteItem") + "');");
		}

		/// <summary>
		/// Handles Load event for a control.
		/// </summary>
		/// <param name="e">Event args.</param>
		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);
			
			try {
				// parse querystring parameters
				itemId = Utils.ParseToNullableInt (Request.QueryString ["position_id"]);
      
				if (!IsPostBack) {
					// load the data into the control the first time we hit this page

					// check we have an item to lookup
					// ALT: if (!Null.IsNull (itemId) 
					if (itemId.HasValue) {
						// load the item
						var ctrl = new LaunchpadController ();
						var item = ctrl.Get<PositionInfo> (itemId.Value);

						if (item != null) {
											
							txtTitle.Text = item.Title;
							txtShortTitle.Text = item.ShortTitle;
							txtWeight.Text = item.Weight.ToString();
														
						} else
							Response.Redirect (Globals.NavigateURL (), true);
					} else {
						buttonDelete.Visible = false;
					}
				}
			} catch (Exception ex) {
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
			try {
				var ctrl = new LaunchpadController ();
				PositionInfo item;

				// determine if we are adding or updating
				// ALT: if (Null.IsNull (itemId))
				if (!itemId.HasValue) {

					// to add new record
					item = new PositionInfo ();
					item.Title = txtTitle.Text;
					item.ShortTitle = txtShortTitle.Text;
					item.Weight = int.Parse(txtWeight.Text);				

					ctrl.Add<PositionInfo> (item);
				} else {
					 
					// to update existing record
					item = ctrl.Get<PositionInfo> (itemId.Value);
					item.Title = txtTitle.Text;
					item.ShortTitle = txtShortTitle.Text;
					item.Weight = int.Parse(txtWeight.Text);	

					ctrl.Update<PositionInfo> (item);
				}

				Utils.SynchronizeModule(this);

				Response.Redirect (Globals.NavigateURL (), true);
			} catch (Exception ex) {
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
			try {
				// ALT: if (!Null.IsNull (itemId))
				if (itemId.HasValue) {
					var ctrl = new LaunchpadController ();
					ctrl.Delete<PositionInfo> (itemId.Value);
					Response.Redirect (Globals.NavigateURL (), true);
				}
			} catch (Exception ex) {
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		#endregion
	}
}

