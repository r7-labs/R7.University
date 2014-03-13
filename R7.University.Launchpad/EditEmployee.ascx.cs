using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Entities.Content.Taxonomy;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.UI.UserControls;
using DotNetNuke.Web.UI.WebControls;
using R7.University;

// TODO: ModuleAuditControl not saving label content in a ViewState - disabled in a control itself!

namespace R7.University.Launchpad
{
	public partial class EditEmployee : PortalModuleBase
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

			// setup filepicker
			pickerPhoto.FileFilter = Globals.glbImageFileTypes;
			// TODO: Get default faces folder from global / portal settings 
			pickerPhoto.FilePath = "Images/faces/";

			// add default item to user list
			// TODO: Localize Not selected value
			// listUsers.Items.Add (new ListItem (Localization.GetString("NotSelected.Text", LocalResourceFile), Null.NullInteger.ToString ()));

			/*
			comboUsers.DataTextField = "UsernameAndEmail";
			comboUsers.DataValueField = "UserID";
			comboUsers.DataSource = new List<UserView> () { new UserView (Localization.GetString("NotSelected.Text", LocalResourceFile)) };
			comboUsers.DataBind ();
*/
			comboUsers.AddItem (Localization.GetString("NotSelected.Text", LocalResourceFile), Null.NullInteger.ToString ());

			var ctrl = new LaunchpadController ();

			// if results are null or empty, lists were empty too
			var positions = new List<PositionInfo>(ctrl.GetObjects<PositionInfo> ("ORDER BY [Title] ASC"));
			var divisions = ctrl.GetObjects<DivisionInfo> ("ORDER BY [Title] ASC");

			// add default items
			positions.Insert (0, new PositionInfo () { ShortTitle = Localization.GetString("NotSelected.Text", LocalResourceFile), PositionID = Null.NullInteger });
			// divisions.Insert (0, new DivisionInfo () { ShortTitle = Localization.GetString("NotSelected.Text", LocalResourceFile), DivisionID = Null.NullInteger });

			comboPositions.DataTextField = "ShortTitle";
			comboPositions.DataValueField = "PositionID";
			comboPositions.DataSource = positions;
			comboPositions.DataBind ();

			treeDivisions.DataSource = divisions;
			treeDivisions.DataBind ();
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
				itemId = Utils.ParseToNullableInt (Request.QueryString ["employee_id"]);
      
				if (!IsPostBack)
				{
					// load the data into the control the first time we hit this page

					// check we have an item to lookup
					// ALT: if (!Null.IsNull (itemId) 
					if (itemId.HasValue)
					{
						// load the item
						var ctrl = new LaunchpadController ();
						var item = ctrl.Get<EmployeeInfo> (itemId.Value);

						if (item != null)
						{
							textLastName.Text = item.LastName;
							textFirstName.Text = item.FirstName;
							textOtherName.Text = item.OtherName;
							textNamePrefix.Text = item.NamePrefix;
							textAcademicTitle.Text = item.AcademicTitle;
							textAcademicDegree.Text = item.AcademicDegree;
							textPhone.Text = item.Phone;
							textCellPhone.Text = item.CellPhone;
							textFax.Text = item.Fax;
							textEmail.Text = item.Email;
							textSecondaryEmail.Text = item.SecondaryEmail;
							textWebSite.Text = item.WebSite;
							textMessenger.Text = item.Messenger;
							textWorkingHours.Text = item.WorkingHours;
							textWorkingPlace.Text = item.WorkingPlace;
							textBiography.Text = item.Biography;

							if (!Null.IsNull(item.ExperienceYears))
								textExperienceYears.Text = item.ExperienceYears.ToString ();

							if (!Null.IsNull(item.ExperienceYearsBySpec))
								textExperienceYearsBySpec.Text = item.ExperienceYearsBySpec.ToString ();

							checkIsPublished.Checked = item.IsPublished;
							// checkIsDeleted.Checked = item.IsDeleted;

							// set photo
							if (!Utils.IsNull (item.PhotoFileID))
								pickerPhoto.FilePath = FileManager.Instance.GetUrl (
									FileManager.Instance.GetFile (item.PhotoFileID.Value))
										.Remove (0, PortalSettings.HomeDirectory.Length);

							//	Utils.Message(this, MessageSeverity.Info, item.PhotoURL.Replace("File=",""));

							/*FilePath = FileManager.Instance.GetUrl(
								FileManager.Instance.GetFile(item.PhotoURL))
								.Remove(0, PortalSettings.HomeDirectory.Length);
*/
							/*
							// set link to a user
							// CHECK: If item.UserID is null, -1, or !HasValue
							if (!Null.IsNull(item.UserID))
								urlUser.Url = string.Format("UserID={0}", item.UserID);
							else
								// or set to "None", if Url is empty
								urlUser.UrlType = "N";
							*/

							if (!Null.IsNull (item.UserID))
							{
								var user = UserController.GetUserById (this.PortalId, item.UserID.Value);
								if (user != null)
								{
									// add previously selected user to user list...
									comboUsers.AddItem (user.Username + " / " + user.Email, user.UserID.ToString ());
									// listUsers.Items.Add (new ListItem (user.Username + " / " + user.Email, user.UserID.ToString ()));
									// and select it
									// listUsers.SelectedIndex = 1;
									comboUsers.SelectedIndex = 1;
								}
							}

							// read OccupiedPositions data
							var occupiedPositionsEx = ctrl.GetObjects<OccupiedPositionInfoEx>(
								"WHERE [EmployeeID] = @0 ORDER BY [IsPrime] DESC", itemId.Value);

							// fill view list
							var occupiedPositions = new List<OccupiedPositionView>();
							foreach (var op in occupiedPositionsEx)
								occupiedPositions.Add(new OccupiedPositionView(op));

							ViewState["occupiedPositions"] = occupiedPositions;
							gridOccupiedPositions.DataSource = OccupiedPositionsDataTable(occupiedPositions);
							gridOccupiedPositions.DataBind ();

							// setup audit control
							ctlAudit.CreatedByUser = Utils.GetUserDisplayName (item.CreatedByUserID);
							ctlAudit.CreatedDate = item.CreatedOnDate.ToLongDateString ();
							ctlAudit.LastModifiedByUser = Utils.GetUserDisplayName (item.LastModifiedByUserID);
							ctlAudit.LastModifiedDate = item.LastModifiedOnDate.ToLongDateString ();
						}
						else
							Response.Redirect (Globals.NavigateURL (), true);
					}
					else
					{
						// textExperienceYears.Text = "0";
						//	textExperienceYearsBySpec.Text = "0";

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
				var ctrl = new LaunchpadController ();
				EmployeeInfo item;

				// determine if we are adding or updating
				// ALT: if (Null.IsNull (itemId))
				if (!itemId.HasValue)
				{
					// TODO: populate new object properties with data from controls 
					// to add new record
					item = new EmployeeInfo ();

					item.LastName = textLastName.Text;
					item.FirstName = textFirstName.Text;
					item.OtherName = textOtherName.Text;
					item.NamePrefix = textNamePrefix.Text;
					item.AcademicTitle = textAcademicTitle.Text;
					item.AcademicDegree = textAcademicDegree.Text;
					item.Phone = textPhone.Text;
					item.CellPhone = textCellPhone.Text;
					item.Fax = textFax.Text;
					item.Email = textEmail.Text;
					item.SecondaryEmail = textSecondaryEmail.Text;
					item.WebSite = textWebSite.Text;
					item.Messenger = textMessenger.Text;
					item.WorkingHours = textWorkingHours.Text;
					item.WorkingPlace = textWorkingPlace.Text;
					item.Biography = textBiography.Text;

					item.ExperienceYears = Utils.ParseToNullableInt (textExperienceYears.Text);
					item.ExperienceYearsBySpec = Utils.ParseToNullableInt (textExperienceYearsBySpec.Text);

					item.IsPublished = checkIsPublished.Checked;
					// item.IsDeleted = checkIsDeleted.Checked;

					// pickerPhoto.FileID may be 0 by default
					item.PhotoFileID = (pickerPhoto.FileID > 0) ? (int?)pickerPhoto.FileID : null;

					// item.UserID = Utils.ParseToNullableInt (listUsers.SelectedValue);
					item.UserID = Utils.ParseToNullableInt (comboUsers.SelectedValue);

					// parse user url
					// item.UserID = Utils.ParseToNullableInt(urlUser.Url.ToUpperInvariant().Replace("USERID=",""));
					//Utils.Message (this, MessageSeverity.Info, urlUser.Url);

					// update audit info
					item.CreatedByUserID = item.LastModifiedByUserID = this.UserId;
					item.CreatedOnDate = item.LastModifiedOnDate = DateTime.Now;

					var occupiedPositions = ViewState["occupiedPositions"] as List<OccupiedPositionView>;
					// check if we have positions defined
					if (occupiedPositions != null)
					{
						var occupiedPositionInfos = new List<OccupiedPositionInfo>();

						foreach (var op in occupiedPositions)
							occupiedPositionInfos.Add(op.NewOccupiedPositionInfo());

						// add item
						ctrl.AddEmployee(item, occupiedPositionInfos);
					}
					else
						ctrl.Add<EmployeeInfo>(item);
				}
				else
				{
					// TODO: update properties of existing object with data from controls 
					// to update existing record
					item = ctrl.Get<EmployeeInfo> (itemId.Value);

					item.LastName = textLastName.Text;
					item.FirstName = textFirstName.Text;
					item.OtherName = textOtherName.Text;
					item.NamePrefix = textNamePrefix.Text;
					item.AcademicTitle = textAcademicTitle.Text;
					item.AcademicDegree = textAcademicDegree.Text;
					item.Phone = textPhone.Text;
					item.CellPhone = textCellPhone.Text;
					item.Fax = textFax.Text;
					item.Email = textEmail.Text;
					item.SecondaryEmail = textSecondaryEmail.Text;
					item.WebSite = textWebSite.Text;
					item.Messenger = textMessenger.Text;
					item.WorkingHours = textWorkingHours.Text;
					item.WorkingPlace = textWorkingPlace.Text;
					item.Biography = textBiography.Text;

					item.ExperienceYears = Utils.ParseToNullableInt (textExperienceYears.Text);
					item.ExperienceYearsBySpec = Utils.ParseToNullableInt (textExperienceYearsBySpec.Text);

					item.IsPublished = checkIsPublished.Checked;
					// item.IsDeleted = checkIsDeleted.Checked;

					// pickerPhoto.FileID may be 0 by default
					item.PhotoFileID = (pickerPhoto.FileID > 0) ? (int?)pickerPhoto.FileID : null;

					// item.UserID = Utils.ParseToNullableInt (listUsers.SelectedValue);
					item.UserID = Utils.ParseToNullableInt (comboUsers.SelectedValue);

					// parse user url
					// item.UserID = Utils.ParseToNullableInt(urlUser.Url.ToUpperInvariant().Replace("USERID=",""));
					//Utils.Message (this, MessageSeverity.Info, urlUser.Url);

					// update audit info
					item.LastModifiedByUserID = this.UserId;
					item.LastModifiedOnDate = DateTime.Now;

					var occupiedPositions = ViewState["occupiedPositions"] as List<OccupiedPositionView>;
					// check if we have positions defined
					if (occupiedPositions != null)
					{
						var occupiedPositionInfos = new List<OccupiedPositionInfo>();

						foreach (var op in occupiedPositions)
							occupiedPositionInfos.Add(op.NewOccupiedPositionInfo());

						// update
						ctrl.UpdateEmployee(item, occupiedPositionInfos);
					}
					else
						ctrl.Update<EmployeeInfo>(item);
				}

				Utils.SynchronizeModule(this);

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
					var ctrl = new LaunchpadController ();
					ctrl.Delete<EmployeeInfo> (itemId.Value);
					Response.Redirect (Globals.NavigateURL (), true);
				}
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		protected void buttonUserLookup_Click (object sender, EventArgs e)
		{
			try
			{
				var term = textUserLookup.Text.Trim ();
				var includeDeleted = checkIncludeDeletedUsers.Checked;

				// uncheck to minimize UX errors
				checkIncludeDeletedUsers.Checked = false;

				var usersFound = 0;
				var usersFoundTotal = 0;
			

				// TODO: Link to open admin users interface in a separate tab
				var users = UserController.GetUsersByEmail (PortalId, term, -1, -1, ref usersFound, includeDeleted, false);
				usersFoundTotal += usersFound;

				// find cross-portal users (host & others) by email
				users.AddRange (UserController.GetUsersByEmail (Null.NullInteger, term, -1, -1, ref usersFound, includeDeleted, false));
				usersFoundTotal += usersFound;

				// combine email lookup results with lookup by username
				users.AddRange (UserController.GetUsersByUserName (PortalId, term, -1, -1, ref usersFound, includeDeleted, false));
				usersFoundTotal += usersFound;

				// find cross-portal users  by username
				users.AddRange (UserController.GetUsersByUserName (Null.NullInteger, term, -1, -1, ref usersFound, includeDeleted, false));
				usersFoundTotal += usersFound;


				// clear user combox & add default item
				comboUsers.Items.Clear();
				comboUsers.AddItem (Localization.GetString("NotSelected.Text", LocalResourceFile), Null.NullInteger.ToString ());

				//listUsers.Items.Clear ();
				//listUsers.Items.Add (new ListItem (Localization.GetString("NotSelected.Text", LocalResourceFile), Null.NullInteger.ToString ()));

				if (usersFoundTotal > 0)
				{
					foreach (var userObj in users)
					{
						var user = userObj as UserInfo;
						/* labelUserNames.Text += (user as UserInfo).Username + "; ";*/
						// listUsers.Items.Add (new ListItem (user.Username + " / " + user.Email, user.UserID.ToString ())); 
						comboUsers.AddItem (user.Username + " / " + user.Email, user.UserID.ToString ());
					}

					// at least one user exists, so select first one:
					// listUsers.SelectedIndex = 1;
					comboUsers.SelectedIndex = 1;
				}
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		protected void listOccupiedPositions_ItemDataBound (object sender, DataListItemEventArgs e)
		{
		}

		protected void buttonAddOccupiedPosition_Click (object sender, EventArgs e)
		{
			try
			{
				var positionID = int.Parse(comboPositions.SelectedValue);
				var divisionID = int.Parse(treeDivisions.SelectedValue);

				if (!Null.IsNull(positionID) && !Null.IsNull(divisionID))
				{
					var occupiedPositions = ViewState["occupiedPositions"] as List<OccupiedPositionView>;
					if (occupiedPositions == null)
						occupiedPositions = new List<OccupiedPositionView>();

					occupiedPositions.Add(
						new OccupiedPositionView(positionID, comboPositions.Text, 
							divisionID, treeDivisions.SelectedNode.Text, checkIsPrime.Checked));

					ViewState["occupiedPositions"] = occupiedPositions;
					gridOccupiedPositions.DataSource = OccupiedPositionsDataTable(occupiedPositions);
					gridOccupiedPositions.DataBind ();

					// uncheck IsPrime, to minify UX errors 
					checkIsPrime.Checked = false;
				}
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		protected void gridOccupiedPositions_RowDataBound (object sender, GridViewRowEventArgs e)
		{
			// hide ItemID column, also in header
			e.Row.Cells [1].Visible = false;

			// exclude header
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				// find delete linkbutton
				var link = e.Row.Cells [0].FindControl ("linkDeleteOccupiedPosition") as LinkButton;

				// set recordId to delete
				link.CommandArgument = e.Row.Cells [1].Text;
			}
		}

		protected void linkDeleteOccupiedPosition_Command (object sender, CommandEventArgs e)
		{
			var occupiedPositions = ViewState ["occupiedPositions"] as List<OccupiedPositionView>;
			if (occupiedPositions != null)
			{
				//var itemID = (sender as LinkButton).CommandArgument;
				var itemID = e.CommandArgument.ToString ();

				// NOTE: Adding controls dynamically conflicts with ViewState!
				// Utils.Message (this, MessageSeverity.Info, itemID);

				// find position in a list
				OccupiedPositionView opFound = null;
				foreach (var op in occupiedPositions)
					if (op.ItemID.ToString () == itemID)
					{
						opFound = op;
						break;
					}

				if (opFound != null)
				{
					occupiedPositions.Remove (opFound);
					ViewState ["occupiedPositions"] = occupiedPositions;

					gridOccupiedPositions.DataSource = OccupiedPositionsDataTable(occupiedPositions);
					gridOccupiedPositions.DataBind ();
				}
			}
		}

		private DataTable OccupiedPositionsDataTable (List<OccupiedPositionView> occupiedPositions)
		{
			var dt = new DataTable ();
			DataRow dr;

			dt.Columns.Add (new DataColumn ("ItemID", typeof(int)));
			dt.Columns.Add (new DataColumn ("Division", typeof(string)));
			dt.Columns.Add (new DataColumn ("Position", typeof(string)));
			dt.Columns.Add (new DataColumn ("IsPrime", typeof(bool)));

			foreach (var op in occupiedPositions)
			{
				dr = dt.NewRow ();
				dr [0] = op.ItemID;
				dr [1] = op.DivisionShortTitle;
				dr [2] = op.PositionShortTitle;
				dr [3] = op.IsPrime;
				dt.Rows.Add (dr);
			}

			return dt;
		}

		#endregion
	}
}

