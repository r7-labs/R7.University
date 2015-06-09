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
	public partial class EditEduProgram : LaunchpadPortalModuleBase
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

			// bind education levels
            comboEduLevel.DataSource = LaunchpadController.GetObjects<EduLevelInfo> ();
            comboEduLevel.DataBind ();
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
				itemId = Utils.ParseToNullableInt (Request.QueryString ["eduprogram_id"]);
      
				if (!IsPostBack)
				{
					// load the data into the control the first time we hit this page

					// check we have an item to lookup
					// ALT: if (!Null.IsNull (itemId) 
					if (itemId.HasValue)
					{
						// load the item
						var item = LaunchpadController.Get<EduProgramInfo> (itemId.Value);

						if (item != null)
						{
							textCode.Text = item.Code;
							textTitle.Text = item.Title;
                            textProfileCode.Text = item.ProfileCode;
                            textProfileTitle.Text = item.ProfileTitle;
                               
                            Utils.SelectByValue (comboEduLevel, item.EduLevelID.ToString ());
						}
						else
							Response.Redirect (Globals.NavigateURL (), true);
					}
					else
					{
						buttonDelete.Visible = false;
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
				EduProgramInfo item;

				// determine if we are adding or updating
				// ALT: if (Null.IsNull (itemId))
				if (!itemId.HasValue)
				{
					// add new record
					item = new EduProgramInfo ();
				}
				else
				{
					// update existing record
					item = LaunchpadController.Get<EduProgramInfo> (itemId.Value);
				}

				// fill the object
				item.Code = textCode.Text.Trim ();
				item.Title = textTitle.Text.Trim ();
                item.ProfileCode = textProfileCode.Text.Trim ();
                item.ProfileTitle = textProfileTitle.Text.Trim ();

                item.EduLevelID = int.Parse (comboEduLevel.SelectedValue);

				if (!itemId.HasValue)
                    LaunchpadController.Add<EduProgramInfo> (item);
				else
                    LaunchpadController.Update<EduProgramInfo> (item);

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
                    LaunchpadController.Delete<EduProgramInfo> (itemId.Value);
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

