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

// TODO: ModuleAuditControl not saving label content in a ViewState!

namespace R7.University.Employee
{
	public partial class EditEmployee : EmployeePortalModuleBase
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
			comboUsers.AddItem (Localization.GetString("NotSelected.Text", LocalResourceFile), Null.NullInteger.ToString ());

			// init working hours
			SharedLogic.WorkingHours.Init (this, comboWorkingHours);

			// if results are null or empty, lists were empty too
			var positions = new List<PositionInfo> (EmployeeController.GetObjects<PositionInfo> ("ORDER BY [Title] ASC"));
			var divisions = new List<DivisionInfo> (EmployeeController.GetObjects<DivisionInfo> ("ORDER BY [Title] ASC"));

			// add default items
			positions.Insert (0, new PositionInfo () { ShortTitle = Localization.GetString("NotSelected.Text", LocalResourceFile), PositionID = Null.NullInteger });
			divisions.Insert (0, new DivisionInfo () { ShortTitle = Localization.GetString("NotSelected.Text", LocalResourceFile), DivisionID = Null.NullInteger });

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
						var item = EmployeeController.Get<EmployeeInfo> (itemId.Value);

						if (item != null)
						{
							textLastName.Text = item.LastName;
							textFirstName.Text = item.FirstName;
							textOtherName.Text = item.OtherName;
							//textNamePrefix.Text = item.NamePrefix;
							textAcademicTitle.Text = item.AcademicTitle;
							textAcademicDegree.Text = item.AcademicDegree;
							textPhone.Text = item.Phone;
							textCellPhone.Text = item.CellPhone;
							textFax.Text = item.Fax;
							textEmail.Text = item.Email;
							textSecondaryEmail.Text = item.SecondaryEmail;
							textWebSite.Text = item.WebSite;
							textMessenger.Text = item.Messenger;
							textWorkingPlace.Text = item.WorkingPlace;
							textBiography.Text = item.Biography;
							
							// load working hours
							SharedLogic.WorkingHours.Load (comboWorkingHours, textWorkingHours, item.WorkingHours);

							if (!Null.IsNull(item.ExperienceYears))
								textExperienceYears.Text = item.ExperienceYears.ToString ();

							if (!Null.IsNull(item.ExperienceYearsBySpec))
								textExperienceYearsBySpec.Text = item.ExperienceYearsBySpec.ToString ();

							checkIsPublished.Checked = item.IsPublished;

							// set photo
							if (!Utils.IsNull (item.PhotoFileID))
							{
								var photo = FileManager.Instance.GetFile (item.PhotoFileID.Value);
								if (photo != null)
								{
									pickerPhoto.FilePath = FileManager.Instance.GetUrl (photo)
										.Remove (0, PortalSettings.HomeDirectory.Length);
								}
							}

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
							var occupiedPositionsEx = EmployeeController.GetObjects<OccupiedPositionInfoEx>(
								"WHERE [EmployeeID] = @0 ORDER BY [IsPrime] DESC", itemId.Value);

							// fill view list
							var occupiedPositions = new List<OccupiedPositionView>();
							foreach (var op in occupiedPositionsEx)
								occupiedPositions.Add(new OccupiedPositionView(op));

							ViewState["occupiedPositions"] = occupiedPositions;
							gridOccupiedPositions.DataSource = OccupiedPositionsDataTable(occupiedPositions);
							gridOccupiedPositions.DataBind ();

							// setup audit control
							ctlAudit.CreatedByUser = Utils.GetUserDisplayName (item.CreatedByUserID, LocalizeString("System.Text"));
							ctlAudit.CreatedDate = item.CreatedOnDate.ToLongDateString ();
							ctlAudit.LastModifiedByUser = Utils.GetUserDisplayName (item.LastModifiedByUserID, LocalizeString("System.Text"));
							ctlAudit.LastModifiedDate = item.LastModifiedOnDate.ToLongDateString ();
						}
						else
							Response.Redirect (Globals.NavigateURL (), true);
					}
					else
					{
						buttonDelete.Visible = false;
						ctlAudit.Visible = false;
					}

					// then edit / add from EmployeeList, divisionId query param
					// can be set to current division ID
					var divisionId = Request.QueryString["division_id"];
					if (!string.IsNullOrWhiteSpace(divisionId))
					{
						var treeNode = treeDivisions.FindNodeByValue(divisionId);
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
					}

					// select first (default) node, if none selected - 
					// fix for issue #8
					if (treeDivisions.SelectedNode == null)
						treeDivisions.Nodes[0].Selected = true;
				}
				else 
				{
					// NOTE: Fix for issue #1 - just update FilePath every postback
					if (pickerPhoto.FileID > 0)
						pickerPhoto.FilePath = FileManager.Instance.GetUrl (
							FileManager.Instance.GetFile (pickerPhoto.FileID))
								.Remove (0, PortalSettings.HomeDirectory.Length);
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
				EmployeeInfo item;

				// determine if we are adding or updating
				// ALT: if (Null.IsNull (itemId))
				if (!itemId.HasValue)
				{
					// to add new record
					item = new EmployeeInfo ();
				}
				else
				{
					// update existing record
					item = EmployeeController.Get<EmployeeInfo> (itemId.Value);
				}

				// fill the object
				item.LastName = textLastName.Text.Trim();
				item.FirstName = textFirstName.Text.Trim();
				item.OtherName = textOtherName.Text.Trim();
				//item.NamePrefix = textNamePrefix.Text.Trim();
				item.AcademicTitle = textAcademicTitle.Text.Trim();
				item.AcademicDegree = textAcademicDegree.Text.Trim();
				item.Phone = textPhone.Text.Trim();
				item.CellPhone = textCellPhone.Text.Trim();
				item.Fax = textFax.Text.Trim();
				item.Email = textEmail.Text.Trim().ToLowerInvariant();
				item.SecondaryEmail = textSecondaryEmail.Text.Trim().ToLowerInvariant();
				item.WebSite = textWebSite.Text.Trim();
				item.Messenger = textMessenger.Text.Trim();
				item.WorkingPlace = textWorkingPlace.Text.Trim();
				item.Biography = textBiography.Text.Trim();

				// update working hours
				item.WorkingHours = SharedLogic.WorkingHours.Update (comboWorkingHours, textWorkingHours.Text, checkAddToVocabulary.Checked);

				item.ExperienceYears = Utils.ParseToNullableInt (textExperienceYears.Text);
				item.ExperienceYearsBySpec = Utils.ParseToNullableInt (textExperienceYearsBySpec.Text);

				item.IsPublished = checkIsPublished.Checked;

				// pickerPhoto.FileID may be 0 by default
				item.PhotoFileID = (pickerPhoto.FileID > 0) ? (int?)pickerPhoto.FileID : null;
				item.UserID = Utils.ParseToNullableInt (comboUsers.SelectedValue);

				if (!itemId.HasValue)
				{		
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
						EmployeeController.AddEmployee(item, occupiedPositionInfos);
					}
					else
						EmployeeController.Add<EmployeeInfo>(item);

					// then adding new employee from Employee module, 
					// set calling module to display new employee
					if (ModuleConfiguration.ModuleDefinition.DefinitionName == "R7.University.Employee")
					{
						//var mctrl = new ModuleController();
						//mctrl.UpdateModuleSetting (ModuleId, "Employee_EmployeeID", item.EmployeeID.ToString());
						EmployeeSettings.EmployeeID = item.EmployeeID;
					}
				}
				else
				{
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
						EmployeeController.UpdateEmployee(item, occupiedPositionInfos);
					}
					else
						EmployeeController.Update<EmployeeInfo>(item);
				}

				Utils.SynchronizeModule(this);
				DataCache.RemoveCache("Employee_" + TabModuleId + "_RenderedContent");

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
					EmployeeController.Delete<EmployeeInfo> (itemId.Value);
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

				if (usersFoundTotal > 0)
				{
					foreach (var userObj in users)
					{
						var user = userObj as UserInfo;
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

		protected void buttonAddPosition_Click (object sender, EventArgs e)
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

					// determine if we should add prime position
					var isPrime = sender == buttonAddPrimePosition;

					occupiedPositions.Add(
						new OccupiedPositionView(positionID, comboPositions.Text, 
							divisionID, treeDivisions.SelectedNode.Text, isPrime));

					ViewState["occupiedPositions"] = occupiedPositions;
					gridOccupiedPositions.DataSource = OccupiedPositionsDataTable(occupiedPositions);
					gridOccupiedPositions.DataBind ();
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

