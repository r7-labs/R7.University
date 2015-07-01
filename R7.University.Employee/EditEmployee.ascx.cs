using System;
using System.IO;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Entities.Icons;
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
		#region Types

		public enum EditEmployeeTab { Common, Positions, Achievements, EduPrograms, Disciplines, About };

		#endregion

		private int? itemId = null;
	
		#region Properties

		protected EditEmployeeTab SelectedTab
		{
			get 
			{
                // get postback initiator
                var eventTarget = Request.Form ["__EVENTTARGET"];

                // urlDocumentURL control is on Achievements tab
                if (!string.IsNullOrEmpty (eventTarget) && eventTarget.Contains ("$urlDocumentURL$"))
                {
                    ViewState ["SelectedTab"] = EditEmployeeTab.Achievements;
                    return EditEmployeeTab.Achievements;
                }

                // otherwise, get current tab from viewstate
				var obj = ViewState ["SelectedTab"];
				return (obj != null) ? (EditEmployeeTab)obj : EditEmployeeTab.Common;
			}
			set { ViewState ["SelectedTab"] = value; }
		}

		private List<AchievementInfo> CommonAchievements
		{
			get
			{ 
				var commonAchievements = ViewState ["commonAchievements"] as List<AchievementInfo>;
				if (commonAchievements == null)
				{
					commonAchievements = EmployeeController.GetObjects<AchievementInfo> ().ToList ();
					ViewState ["commonAchievements"] = commonAchievements;
				}
				
				return commonAchievements;
			}
		}

		protected string EditIconUrl
		{
			get { return IconController.IconURL ("Edit"); }
		}

		protected string DeleteIconUrl
		{
			get { return IconController.IconURL ("Delete"); }
		}

		#endregion

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

			// setup filepicker
            pickerPhoto.FileFilter = Globals.glbImageFileTypes;
            // TODO: Get default faces folder from global / portal settings 
            pickerPhoto.FilePath = "Images/faces/";
           
			// add default item to user list
            comboUsers.AddItem (LocalizeString ("NotSelected.Text"), Null.NullInteger.ToString ());

			// init working hours
			WorkingHoursLogic.Init (this, comboWorkingHours);

			// if results are null or empty, lists were empty too
			var positions = new List<PositionInfo> (EmployeeController.GetObjects<PositionInfo> ("ORDER BY [Title] ASC"));
			var divisions = new List<DivisionInfo> (EmployeeController.GetObjects<DivisionInfo> ("ORDER BY [Title] ASC"));
			var commonAchievements = new List<AchievementInfo> (EmployeeController.GetObjects<AchievementInfo> ("ORDER BY [Title] ASC"));

            ViewState ["commonAchievements"] = commonAchievements;

			// add default items
			positions.Insert (0, new PositionInfo () {
				ShortTitle = LocalizeString ("NotSelected.Text"), PositionID = Null.NullInteger
			});

			commonAchievements.Insert (0, new AchievementInfo () {
                ShortTitle = LocalizeString ("NotSelected.Text"), AchievementID = Null.NullInteger
			});

            divisions.Insert (0, DivisionInfo.DefaultItem (LocalizeString ("NotSelected.Text")));

            // bind positions
			comboPositions.DataSource = positions;
			comboPositions.DataBind ();
            comboPositions.SelectedIndex = 0;

            // bind achievements
			comboAchievements.SelectedIndexChanged += comboAchievements_SelectedIndexChanged;
			comboAchievements.DataSource = commonAchievements;
			comboAchievements.DataBind ();

            // bind divisions
			treeDivisions.DataSource = divisions;
			treeDivisions.DataBind ();

			// bind achievement types
			comboAchievementTypes.DataSource = AchievementTypeInfo.GetLocalizedAchievementTypes (LocalizeString);
			comboAchievementTypes.DataBind ();

            // get edu programs
            var eduPrograms = EmployeeController.GetObjects<EduProgramInfo> ("ORDER BY [EduLevelID], [Code]").ToList ();

            // add default value
            eduPrograms.Insert (0, new EduProgramInfo { 
                Title = LocalizeString ("NotSelected.Text"), EduProgramID = Null.NullInteger 
            });

            // bind edu programs
            comboEduProgram.DataSource = eduPrograms;
            comboEduProgram.DataBind ();
            comboEduProgram.SelectedIndex = 0;

            // localize bounded gridviews
            gridAchievements.LocalizeColumns (LocalResourceFile);
            gridOccupiedPositions.LocalizeColumns (LocalResourceFile);
            gridEduPrograms.LocalizeColumns (LocalResourceFile);
		}

		/// <summary>
		/// Handles Load event for a control.
		/// </summary>
		/// <param name="e">Event args.</param>
		protected override void OnLoad (EventArgs e)
		{
			base.OnLoad (e);

            if (DotNetNuke.Framework.AJAX.IsInstalled ())
                DotNetNuke.Framework.AJAX.RegisterScriptManager ();
            
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
							textWebSiteLabel.Text = item.WebSiteLabel;
							textMessenger.Text = item.Messenger;
							textWorkingPlace.Text = item.WorkingPlace;
							textBiography.Text = item.Biography;
							textDisciplines.Text = item.Disciplines;
					
							// load working hours
							WorkingHoursLogic.Load (comboWorkingHours, textWorkingHours, item.WorkingHours);

							if (!Null.IsNull (item.ExperienceYears))
								textExperienceYears.Text = item.ExperienceYears.ToString ();

							if (!Null.IsNull (item.ExperienceYearsBySpec))
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
							var occupiedPositionInfoExs = EmployeeController.GetObjects<OccupiedPositionInfoEx> (
								                              "WHERE [EmployeeID] = @0 ORDER BY [IsPrime] DESC", itemId.Value);

							// fill view list
							var occupiedPositions = new List<OccupiedPositionView> ();
							foreach (var op in occupiedPositionInfoExs)
								occupiedPositions.Add (new OccupiedPositionView (op));

							// bind occupied positions
							ViewState ["occupiedPositions"] = occupiedPositions;
							gridOccupiedPositions.DataSource = OccupiedPositionsDataTable (occupiedPositions);
							gridOccupiedPositions.DataBind ();

							// read employee achievements
							var achievementInfos = EmployeeController.GetObjects<EmployeeAchievementInfo> (
								                       CommandType.Text, "SELECT * FROM dbo.vw_University_EmployeeAchievements WHERE [EmployeeID] = @0", itemId.Value);

							// fill achievements list
							var achievements = new List<EmployeeAchievementView> ();
							foreach (var achievement in achievementInfos)
                            {
                                var achView = new EmployeeAchievementView (achievement);
                                achView.Localize (LocalResourceFile);
								achievements.Add (achView);
                            }

							// bind achievements
							ViewState ["achievements"] = achievements;
							gridAchievements.DataSource = AchievementsDataTable (achievements);
							gridAchievements.DataBind ();

                            // read employee educational programs 
                            var eduprogramInfos = EmployeeController.GetObjects<EmployeeEduProgramInfoEx> ("WHERE [EmployeeID] = @0", itemId.Value);

                            // fill edu programs list
                            var eduprograms = new List<EmployeeEduProgramView> ();
                            foreach (var eduprogram in eduprogramInfos)
                                eduprograms.Add (new EmployeeEduProgramView (eduprogram));

                            // bind edu programs
                            ViewState ["eduprograms"] = eduprograms;
                            gridEduPrograms.DataSource = EduProgramsDataTable (eduprograms);
                            gridEduPrograms.DataBind ();

                            // setup audit control
							ctlAudit.CreatedByUser = Utils.GetUserDisplayName (item.CreatedByUserID, LocalizeString ("System.Text"));
							ctlAudit.CreatedDate = item.CreatedOnDate.ToLongDateString ();
							ctlAudit.LastModifiedByUser = Utils.GetUserDisplayName (item.LastModifiedByUserID, LocalizeString ("System.Text"));
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
					var divisionId = Request.QueryString ["division_id"];
					Utils.SelectAndExpandByValue (treeDivisions, divisionId);
				
					// select first (default) node, if none selected - 
					// fix for issue #8
					if (treeDivisions.SelectedNode == null)
						treeDivisions.Nodes [0].Selected = true;
				}
                else // if (IsPostBack)
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
				item.LastName = textLastName.Text.Trim ();
				item.FirstName = textFirstName.Text.Trim ();
				item.OtherName = textOtherName.Text.Trim ();
				//item.NamePrefix = textNamePrefix.Text.Trim();
				item.AcademicTitle = textAcademicTitle.Text.Trim ();
				item.AcademicDegree = textAcademicDegree.Text.Trim ();
				item.Phone = textPhone.Text.Trim ();
				item.CellPhone = textCellPhone.Text.Trim ();
				item.Fax = textFax.Text.Trim ();
				item.Email = textEmail.Text.Trim ().ToLowerInvariant ();
				item.SecondaryEmail = textSecondaryEmail.Text.Trim ().ToLowerInvariant ();
				item.WebSite = textWebSite.Text.Trim ();
				item.WebSiteLabel = textWebSiteLabel.Text.Trim ();
				item.Messenger = textMessenger.Text.Trim ();
				item.WorkingPlace = textWorkingPlace.Text.Trim ();
				item.Biography = textBiography.Text.Trim ();
				item.Disciplines = textDisciplines.Text.Trim ();

				// update working hours
				item.WorkingHours = WorkingHoursLogic.Update (comboWorkingHours, textWorkingHours.Text, checkAddToVocabulary.Checked);

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
	
					// add employee
                    EmployeeController.AddEmployee (item, GetOccupiedPositions (), 
                        GetEmployeeAchievements (), GetEmployeeEduPrograms());

					// then adding new employee from Employee or EmployeeDetails modules, 
					// set calling module to display new employee
					if (ModuleConfiguration.ModuleDefinition.DefinitionName == "R7.University.Employee" || 
                        ModuleConfiguration.ModuleDefinition.DefinitionName == "R7.University.EmployeeDetails")
					{
						EmployeeSettings.EmployeeID = item.EmployeeID;

						// we adding new employee, so he/she should be displayed in the module
						EmployeeSettings.ShowCurrentUser = false;
					}
				}
				else
				{
					// update audit info
					item.LastModifiedByUserID = this.UserId;
					item.LastModifiedOnDate = DateTime.Now;

					// update employee
                    EmployeeController.UpdateEmployee (item, GetOccupiedPositions (), 
                        GetEmployeeAchievements (), GetEmployeeEduPrograms());
				}

				Utils.SynchronizeModule (this);
				DataCache.RemoveCache ("Employee_" + TabModuleId + "_RenderedContent");

				Response.Redirect (Globals.NavigateURL (), true);
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		private List<OccupiedPositionInfo> GetOccupiedPositions ()
		{
			var occupiedPositions = ViewState ["occupiedPositions"] as List<OccupiedPositionView>;
					
			var occupiedPositionInfos = new List<OccupiedPositionInfo> ();
			if (occupiedPositions != null)
				foreach (var op in occupiedPositions)
					occupiedPositionInfos.Add (op.NewOccupiedPositionInfo ());

			return occupiedPositionInfos;
		}

		private List<EmployeeAchievementInfo> GetEmployeeAchievements ()
		{
			var achievements = ViewState ["achievements"] as List<EmployeeAchievementView>;
				
			var achievementInfos = new List<EmployeeAchievementInfo> ();
			if (achievements != null)
				foreach (var ach in achievements)
					achievementInfos.Add (ach.NewEmployeeAchievementInfo ());

			return achievementInfos;
		}

        private List<EmployeeEduProgramInfo> GetEmployeeEduPrograms ()
        {
            var eduPrograms = ViewState ["eduprograms"] as List<EmployeeEduProgramView>;

            var eduProgramInfos = new List<EmployeeEduProgramInfo> ();
            if (eduPrograms != null)
                foreach (var ep in eduPrograms)
                    eduProgramInfos.Add (ep.NewEmployeeEduProgramInfo ());

            return eduProgramInfos;
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
				SelectedTab = EditEmployeeTab.Common;

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
				comboUsers.Items.Clear ();
				comboUsers.AddItem (LocalizeString ("NotSelected.Text"), Null.NullInteger.ToString ());

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

        protected void buttonPhotoLookup_Click (object sender, EventArgs e)
        {
            try
            {
                SelectedTab = EditEmployeeTab.Common;

                var folderPath = "Images/faces/";
                var folder = FolderManager.Instance.GetFolder (PortalId, folderPath);

                if (folder != null)
                {
                    var employeeName = EmployeeInfo.GetFileName (textFirstName.Text, textLastName.Text, textOtherName.Text);

                    // TODO: EmployeeInfo should contain culture data?
                    var employeeNameTL = TextUtils.Transliterate (employeeName, TextUtils.RuTranslitTable).ToLowerInvariant ();

                    // get files from default folder recursively
                    foreach (var file in FolderManager.Instance.GetFiles (folder, true))
                    {
                        var fileName = Path.GetFileNameWithoutExtension (file.FileName).ToLowerInvariant ();
                        if (fileName == employeeName || fileName == employeeNameTL)
                        {
                            // setting FileID for picker is sufficent, 
                            // as we update FilePath on each postback anyway
                            pickerPhoto.FileID = file.FileId;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void buttonCancelEditEduProgram_Click (object sender, EventArgs e)
        {
            try
            {
                SelectedTab = EditEmployeeTab.EduPrograms;

                ResetEditEduProgramForm ();
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

		protected void buttonCancelEditPosition_Click (object sender, EventArgs e)
		{
			try
			{
				SelectedTab = EditEmployeeTab.Positions;

				ResetEditPositionForm ();
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

        private void ResetEditEduProgramForm ()
        {
            // restore default buttons visibility
            buttonAddEduProgram.Visible = true;
            buttonUpdateEduProgram.Visible = false;

            comboEduProgram.SelectedIndex = 0;
            textProgramDisciplines.Text = string.Empty;
        }

		private void ResetEditPositionForm ()
		{
			// restore default buttons visibility
			buttonAddPosition.Visible = true;
			buttonUpdatePosition.Visible = false;

			// reset divisions treeview
			var divisionId = Request.QueryString ["division_id"];
			Utils.SelectAndExpandByValue (treeDivisions, 
				!string.IsNullOrWhiteSpace (divisionId)? divisionId : Null.NullInteger.ToString ());
		
			// reset other controls
			comboPositions.SelectedIndex = 0;
			textPositionTitleSuffix.Text = "";
			checkIsPrime.Checked = false;
			hiddenOccupiedPositionItemID.Value = "";
		}

		protected void buttonAddPosition_Command (object sender, CommandEventArgs e)
		{
			try
			{
				SelectedTab = EditEmployeeTab.Positions;

				var positionID = int.Parse (comboPositions.SelectedValue);
				var divisionID = int.Parse (treeDivisions.SelectedValue);

				if (!Null.IsNull (positionID) && !Null.IsNull (divisionID))
				{
					OccupiedPositionView occupiedPosition;

					var occupiedPositions = ViewState ["occupiedPositions"] as List<OccupiedPositionView>;

					// creating new list, if none
					if (occupiedPositions == null)
						occupiedPositions = new List<OccupiedPositionView>();

					var command = e.CommandArgument.ToString ();
					if (command == "Add")
					{
						occupiedPosition = new OccupiedPositionView ();
					}
					else // update 
					{
						// restore ItemID from hidden field
						var hiddenItemID = int.Parse (hiddenOccupiedPositionItemID.Value);
						occupiedPosition = occupiedPositions.Find (op => op.ItemID == hiddenItemID);
					}
					
					// fill the object
					occupiedPosition.PositionID = positionID;
					occupiedPosition.DivisionID = divisionID;
                    occupiedPosition.PositionShortTitle = comboPositions.SelectedItem.Text;
					occupiedPosition.DivisionShortTitle = treeDivisions.SelectedNode.Text;
					occupiedPosition.IsPrime = checkIsPrime.Checked;
					occupiedPosition.TitleSuffix = textPositionTitleSuffix.Text.Trim();
					
					if (command == "Add")
					{
						occupiedPositions.Add (occupiedPosition);
					}

					ResetEditPositionForm ();

					ViewState ["occupiedPositions"] = occupiedPositions;
					gridOccupiedPositions.DataSource = OccupiedPositionsDataTable (occupiedPositions);
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
			grids_RowDataBound (sender, e);
		}

		protected void gridAchievements_RowDataBound (object sender, GridViewRowEventArgs e)
		{
			grids_RowDataBound (sender, e);
		}

        protected void gridEduPrograms_RowDataBound (object sender, GridViewRowEventArgs e)
        {
            grids_RowDataBound (sender, e);
        }
		
		private void grids_RowDataBound (object sender, GridViewRowEventArgs e)
		{
			// hide ItemID column, also in header
			e.Row.Cells [1].Visible = false;

            // TODO: Move to gridAchievements_RowDataBound()
            if (sender == gridAchievements)
            {
                // hide description
                e.Row.Cells [6].Visible = false;

                // exclude header
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    // add description as row tooltip
                    e.Row.ToolTip = Server.HtmlDecode (e.Row.Cells [6].Text);

                    // make link to the document
                    // WTF: empty DocumentURL's cells contains non-breakable spaces?
                    var documentUrl = e.Row.Cells [7].Text.Replace ("&nbsp;", "");
                    if (!string.IsNullOrWhiteSpace (documentUrl))
                        e.Row.Cells [7].Text = string.Format ("<a href=\"{0}\" target=\"_blank\">{1}</a>", 
                            Globals.LinkClick (documentUrl, TabId, ModuleId), LocalizeString ("DocumentUrl.Text"));
                }
            }

			// exclude header
			if (e.Row.RowType == DataControlRowType.DataRow)
			{

				// find edit and delete linkbuttons
				var linkDelete = e.Row.Cells [0].FindControl ("linkDelete") as LinkButton;
				var linkEdit = e.Row.Cells [0].FindControl ("linkEdit") as LinkButton;

				// set recordId to delete
				linkEdit.CommandArgument = e.Row.Cells [1].Text;
				linkDelete.CommandArgument = e.Row.Cells [1].Text;

				// add confirmation dialog to delete link
				linkDelete.Attributes.Add ("onClick", "javascript:return confirm('" + Localization.GetString ("DeleteItem") + "');");
			}
		}

		protected void linkEditOccupiedPosition_Command (object sender, CommandEventArgs e)
		{
			try
			{
				SelectedTab = EditEmployeeTab.Positions;

				var occupiedPositions = ViewState ["occupiedPositions"] as List<OccupiedPositionView>;
				if (occupiedPositions != null)
				{
					var itemID = e.CommandArgument.ToString ();
	
					// find position in a list
					var occupiedPosition = occupiedPositions.Find (op => op.ItemID.ToString () == itemID);
	
					if (occupiedPosition != null)
					{
						// fill the form
						treeDivisions.CollapseAllNodes ();
						Utils.SelectAndExpandByValue (treeDivisions, occupiedPosition.DivisionID.ToString ());
						Utils.SelectByValue (comboPositions, occupiedPosition.PositionID);
                        checkIsPrime.Checked = occupiedPosition.IsPrime;
						textPositionTitleSuffix.Text = occupiedPosition.TitleSuffix;
						
						// set hidden field value to ItemID of edited item
						hiddenOccupiedPositionItemID.Value = occupiedPosition.ItemID.ToString ();

						// show / hide buttonss
						buttonAddPosition.Visible = false;
						buttonUpdatePosition.Visible = true;
					}
				}
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		protected void linkDeleteOccupiedPosition_Command (object sender, CommandEventArgs e)
		{
			try
			{
				SelectedTab = EditEmployeeTab.Positions;

				var occupiedPositions = ViewState ["occupiedPositions"] as List<OccupiedPositionView>;
				if (occupiedPositions != null)
				{
					//var itemID = (sender as LinkButton).CommandArgument;
					var itemID = e.CommandArgument.ToString ();
	
					// NOTE: Adding controls dynamically conflicts with ViewState!
					// Utils.Message (this, MessageSeverity.Info, itemID);
	
					// find position in a list
					var opFound = occupiedPositions.Find (op => op.ItemID.ToString () == itemID);
				
					if (opFound != null)
					{
						occupiedPositions.Remove (opFound);
						ViewState ["occupiedPositions"] = occupiedPositions;
	
						gridOccupiedPositions.DataSource = OccupiedPositionsDataTable (occupiedPositions);
						gridOccupiedPositions.DataBind ();

						// reset form if we deleting currently edited position
						if (buttonUpdatePosition.Visible && hiddenOccupiedPositionItemID.Value == itemID)
							ResetEditPositionForm ();
					}
				}
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		private DataTable OccupiedPositionsDataTable (List<OccupiedPositionView> occupiedPositions)
		{
            return DataTableConstructor.FromIEnumerable (occupiedPositions);
		}

		private DataTable AchievementsDataTable (List<EmployeeAchievementView> achievements)
		{
            return DataTableConstructor.FromIEnumerable (achievements);
		}

        private DataTable EduProgramsDataTable (List<EmployeeEduProgramView> eduPrograms)
        {
            return DataTableConstructor.FromIEnumerable (eduPrograms);
        }

		protected void linkDeleteAchievement_Command (object sender, CommandEventArgs e)
		{
			try
			{
				SelectedTab = EditEmployeeTab.Achievements;

				var achievements = ViewState ["achievements"] as List<EmployeeAchievementView>;
				if (achievements != null)
				{
					var itemID = e.CommandArgument.ToString ();
	
					// find position in a list
					var achievement = achievements.Find (ach => ach.ItemID.ToString () == itemID);
	
					if (achievement != null)
					{
						// remove achievement
						achievements.Remove (achievement);
						
						// refresh viewstate
						ViewState ["achievements"] = achievements;
	
						// bind achievements to the gridview
						gridAchievements.DataSource = AchievementsDataTable (achievements);
						gridAchievements.DataBind ();

						// reset form if we deleting currently edited achievement
						if (buttonUpdateAchievement.Visible && hiddenAchievementItemID.Value == itemID)
							ResetEditAchievementForm ();
					}
				}
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		protected void linkEditAchievement_Command (object sender, CommandEventArgs e)
		{
			try
			{
				SelectedTab = EditEmployeeTab.Achievements;

				var achievements = ViewState ["achievements"] as List<EmployeeAchievementView>;
				if (achievements != null)
				{
					var itemID = e.CommandArgument.ToString ();
	
					// find position in a list
					var achievement = achievements.Find (ach => ach.ItemID.ToString () == itemID);
	
					if (achievement != null)
					{
						// fill achievements form
						
						if (achievement.AchievementID != null)
						{
							comboAchievements.Select (achievement.AchievementID.ToString (), false);
	
							panelAchievementTitle.Visible = false;
							panelAchievementShortTitle.Visible = false;
							panelAchievementTypes.Visible = false;
						}
						else
						{
							comboAchievements.Select (Null.NullInteger.ToString (), false);
	
							textAchievementTitle.Text = achievement.Title;
							textAchievementShortTitle.Text = achievement.ShortTitle;
							comboAchievementTypes.Select (achievement.AchievementType.ToString (), false);
							
							panelAchievementTitle.Visible = true;
							panelAchievementShortTitle.Visible = true;
							panelAchievementTypes.Visible = true;
						}
	
						textAchievementTitleSuffix.Text = achievement.TitleSuffix;
						textAchievementDescription.Text = achievement.Description;
						textYearBegin.Text = achievement.YearBegin.ToString ();
						textYearEnd.Text = achievement.YearEnd.ToString ();
						checkIsTitle.Checked = achievement.IsTitle;

						if (!string.IsNullOrWhiteSpace (achievement.DocumentURL))
							urlDocumentURL.Url = achievement.DocumentURL;
						else
							urlDocumentURL.UrlType = "N";

						// show update and cancel buttons (enter edit mode)
						buttonAddAchievement.Visible = false;
						buttonUpdateAchievement.Visible = true;

						// store ItemID in the hidden field
						hiddenAchievementItemID.Value = achievement.ItemID.ToString ();
					}
				}
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		protected void buttonCancelEditAchievement_Click (object sender, EventArgs e)
		{
			try
			{
				SelectedTab = EditEmployeeTab.Achievements;

				ResetEditAchievementForm ();
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		private void ResetEditAchievementForm ()
		{
			// restore default buttons visibility
			buttonAddAchievement.Visible = true;
			buttonUpdateAchievement.Visible = false;

			// restore default panels visibility
			panelAchievementTitle.Visible = true;
			panelAchievementShortTitle.Visible = true;
			panelAchievementTypes.Visible = true;

			// reset controls
			comboAchievements.SelectedIndex = 0;
			comboAchievementTypes.SelectedIndex = 0;
			textAchievementTitle.Text = "";
			textAchievementShortTitle.Text = "";
			textAchievementTitleSuffix.Text = "";
			textAchievementDescription.Text = "";
			textYearBegin.Text = "";
			textYearEnd.Text = "";
			checkIsTitle.Checked = false;
			urlDocumentURL.UrlType = "N";
			hiddenAchievementItemID.Value = "";
		}

		protected void buttonAddAchievement_Command (object sender, CommandEventArgs e)
		{
			try
			{
				SelectedTab = EditEmployeeTab.Achievements;

				EmployeeAchievementView achievement;

				// get achievements list from viewstate
				var achievements = ViewState ["achievements"] as List<EmployeeAchievementView>;
				
				// creating new list, if none
				if (achievements == null)
					achievements = new List<EmployeeAchievementView>();

				var command = e.CommandArgument.ToString ();
				if (command == "Add")
				{
					achievement = new EmployeeAchievementView ();
				}
				else
				{
					// restore ItemID from hidden field
					var hiddenItemID = int.Parse (hiddenAchievementItemID.Value);
					achievement = achievements.Find (ach => ach.ItemID == hiddenItemID);
				}
	
				achievement.AchievementID = Utils.ParseToNullableInt (comboAchievements.SelectedValue);
				if (achievement.AchievementID == null)
				{
					achievement.Title = textAchievementTitle.Text.Trim();
					achievement.ShortTitle = textAchievementShortTitle.Text.Trim();
					achievement.AchievementType = (AchievementType)Enum.Parse (typeof(AchievementType), comboAchievementTypes.SelectedValue);
				}
				else
				{
					var ach = CommonAchievements.Find (a => a.AchievementID.ToString () == comboAchievements.SelectedValue);

					achievement.Title = ach.Title;
					achievement.ShortTitle = ach.ShortTitle;
					achievement.AchievementType = ach.AchievementType;
				}

				achievement.TitleSuffix = textAchievementTitleSuffix.Text.Trim();
				achievement.Description = textAchievementDescription.Text.Trim();
				achievement.IsTitle = checkIsTitle.Checked;
				achievement.YearBegin = Utils.ParseToNullableInt (textYearBegin.Text);
				achievement.YearEnd = Utils.ParseToNullableInt (textYearEnd.Text);
				achievement.DocumentURL = urlDocumentURL.Url;

                achievement.Localize (LocalResourceFile);

				if (command == "Add")
				{
					achievements.Add (achievement);
				}

				ResetEditAchievementForm ();

				// refresh viewstate
				ViewState ["achievements"] = achievements;

				// bind achievements to the gridview
				gridAchievements.DataSource = AchievementsDataTable (achievements);
				gridAchievements.DataBind ();
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

        protected void buttonAddEduProgram_Command (object sender, CommandEventArgs e)
        {
            try
            {
                SelectedTab = EditEmployeeTab.EduPrograms;

                if (!Null.IsNull (int.Parse (comboEduProgram.SelectedValue)))
                {
                    EmployeeEduProgramView eduprogram;

                    // get achievements list from viewstate
                    var eduPrograms = ViewState ["eduprograms"] as List<EmployeeEduProgramView>;

                    // creating new list, if none
                    if (eduPrograms == null)
                        eduPrograms = new List<EmployeeEduProgramView>();

                    var command = e.CommandArgument.ToString ();
                    if (command == "Add")
                    {
                        eduprogram = new EmployeeEduProgramView ();
                    }
                    else
                    {
                        // restore ItemID from hidden field
                        var hiddenItemID = int.Parse (hiddenEduProgramItemID.Value);
                        eduprogram = eduPrograms.Find (ep1 => ep1.ItemID == hiddenItemID);
                    }

                    eduprogram.EduProgramID = int.Parse (comboEduProgram.SelectedValue);
                    eduprogram.Disciplines = textProgramDisciplines.Text.Trim ();

                    var ep = EmployeeController.Get<EduProgramInfo> (eduprogram.EduProgramID);
                    eduprogram.Code = ep.Code;
                    eduprogram.Title = ep.Title;
                    eduprogram.ProfileCode = ep.ProfileCode;
                    eduprogram.ProfileTitle = ep.ProfileTitle;

                    if (command == "Add")
                    {
                        eduPrograms.Add (eduprogram);
                    }

                    ResetEditEduProgramForm ();

                    // refresh viewstate
                    ViewState ["eduprograms"] = eduPrograms;

                    // bind items to the gridview
                    gridEduPrograms.DataSource = EduProgramsDataTable (eduPrograms);
                    gridEduPrograms.DataBind ();
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void linkEditEduProgram_Command (object sender, CommandEventArgs e)
        {
            try
            {
                SelectedTab = EditEmployeeTab.EduPrograms;

                var eduprograms = ViewState ["eduprograms"] as List<EmployeeEduProgramView>;
                if (eduprograms != null)
                {
                    var itemID = e.CommandArgument.ToString ();

                    // find position in a list
                    var eduprogram = eduprograms.Find (ach => ach.ItemID.ToString () == itemID);

                    if (eduprogram != null)
                    {
                        // fill achievements form
                        Utils.SelectByValue (comboEduProgram, eduprogram.EduProgramID.ToString ());
                        textProgramDisciplines.Text = eduprogram.Disciplines;

                        // store ItemID in the hidden field
                        hiddenEduProgramItemID.Value = eduprogram.ItemID.ToString ();

                        // show / hide buttons
                        buttonAddEduProgram.Visible = false;
                        buttonUpdateEduProgram.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

        protected void linkDeleteEduProgram_Command (object sender, CommandEventArgs e)
        {
            try
            {
                SelectedTab = EditEmployeeTab.EduPrograms;

                var eduprograms = ViewState ["eduprograms"] as List<EmployeeEduProgramView>;
                if (eduprograms != null)
                {
                    var itemID = e.CommandArgument.ToString ();

                    // find position in a list
                    var eduprogramIndex = eduprograms.FindIndex (ep => ep.ItemID.ToString () == itemID);

                    if (eduprogramIndex >= 0)
                    {
                        // remove edu program
                        eduprograms.RemoveAt (eduprogramIndex);

                        // refresh viewstate
                        ViewState ["eduprograms"] = eduprograms;

                        // bind edu programs to the gridview
                        gridEduPrograms.DataSource = EduProgramsDataTable (eduprograms);
                        gridEduPrograms.DataBind ();

                        // reset form if we deleting currently edited edu program
                        if (buttonUpdateEduProgram.Visible && hiddenEduProgramItemID.Value == itemID)
                            ResetEditEduProgramForm ();
                    }
                }
            }
            catch (Exception ex)
            {
                Exceptions.ProcessModuleLoadException (this, ex);
            }
        }

		protected void comboAchievements_SelectedIndexChanged (object sender, Telerik.Web.UI.RadComboBoxSelectedIndexChangedEventArgs e)
		{
			try
			{
				SelectedTab = EditEmployeeTab.Achievements;

				if (e.Value == Null.NullInteger.ToString())
				{
					panelAchievementTitle.Visible = true;
					panelAchievementShortTitle.Visible = true;
					panelAchievementTypes.Visible = true;
				}
				else
				{
					panelAchievementTitle.Visible = false;
					panelAchievementShortTitle.Visible = false;
					panelAchievementTypes.Visible = false;
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

