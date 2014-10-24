using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Linq;
using System.Data;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Host;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Services.Localization;
using R7.University;

namespace R7.University.Employee
{
	public partial class ViewEmployee : EmployeePortalModuleBase, IActionable
	{
		#region Properties

		#endregion

		#region Handlers

		/*
		/// <summary>
		/// Handles Init event for a control
		/// </summary>
		/// <param name="e">Event args.</param>
		protected override void OnInit (EventArgs e)
		{
			base.OnInit (e);

			//#if (DATACACHE)
			//AddActionHandler (ClearDataCache_Action);
			//#endif
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
					if (Cache_OnLoad ()) return;
					
					IEnumerable<EmployeeAchievementInfo> achievements = null;

					var employee = GetEmployee ();
					if (employee == null)
					{
						// employee isn't set or not found
						if (IsEditable)
							Utils.Message (this, "NothingToDisplay.Text", MessageType.Info, true);
					}
					else if (!employee.IsPublished)
					{
						// employee isn't published
						if (IsEditable)
							Utils.Message (this, "EmployeeNotPublished.Text", MessageType.Warning, true);
					}

					var hasData = employee != null;

					// if we have something published to display
					// then display module to common users
					Cache_SetContainerVisible (hasData && employee.IsPublished);
											
					// display module only in edit mode
					// only if we have published data to display
					ContainerControl.Visible = IsEditable || (hasData && employee.IsPublished);
											
					// display module content only if it exists
					// and if publshed or in edit mode
					var displayContent = hasData && (IsEditable || employee.IsPublished);

					panelEmployee.Visible = displayContent;
					
					if (displayContent)
					{
						if (EmployeeSettings.AutoTitle)
							AutoTitle (employee);
						
						// get employee achievements (titles) only then it about to display
						achievements = EmployeeController.GetObjects<EmployeeAchievementInfo> (
							CommandType.Text, "SELECT * FROM dbo.vw_University_EmployeeAchievements WHERE [EmployeeID] = @0 AND [IsTitle] = 1", employee.EmployeeID);

						// display employee info
						Display (employee, achievements);
					}

				} // if (!IsPostBack)
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		#endregion

		

		protected void AutoTitle (EmployeeInfo employee)
		{
			// replace module title
			var mctrl = new ModuleController ();
			var module = mctrl.GetModule (ModuleId);
			if (module.ModuleTitle != employee.AbbrName)
			{
				module.ModuleTitle = employee.AbbrName;
				mctrl.UpdateModule (module);
			}
		}

		/// <summary>
		/// Displays the specified employee.
		/// </summary>
		/// <param name="employee">Employee.</param>
		protected void Display (EmployeeInfo employee, IEnumerable<EmployeeAchievementInfo> achievements)
		{
			// occupied positions
			var occupiedPositions = EmployeeController.GetObjects<OccupiedPositionInfoEx> ("WHERE [EmployeeID] = @0 ORDER BY [IsPrime] DESC, [PositionWeight] DESC", employee.EmployeeID);
			if (occupiedPositions != null && occupiedPositions.Any ())
			{
				repeaterPositions.DataSource = OccupiedPositionInfoEx.GroupByDivision (occupiedPositions);
				repeaterPositions.DataBind ();
			}
			else
				repeaterPositions.Visible = false;

			// Full name
			var fullName = employee.FullName;
			labelFullName.Text = fullName;

			var imageVisible = false;
			// Photo
			if (!Utils.IsNull<int> (employee.PhotoFileID))
			{
				// imagePhoto.ImageUrl = Utils.FormatURL (this, "FileID=" + employee.PhotoFileID, false);
				// var image = FileManager.Instance.GetFile (employee.PhotoFileID.Value);
				// imagePhoto.Width = image.Width;
				// imagePhoto.Height = image.Height;

				// REVIEW: Need add ON DELETE rule to FK, linking PhotoFileID & Files.FileID 

				var image = FileManager.Instance.GetFile (employee.PhotoFileID.Value);
				if (image != null)
				{
					var photoWidth = EmployeeSettings.PhotoWidth;

					if (!Null.IsNull (photoWidth))
					{
						imagePhoto.Width = photoWidth;
						imagePhoto.Height = (int)(image.Height * (float)photoWidth / image.Width);

						imagePhoto.ImageUrl = string.Format (
							"/imagehandler.ashx?fileid={0}&width={1}", employee.PhotoFileID, photoWidth);
					}
					else
					{
						// use original image
						imagePhoto.Width = image.Width;
						imagePhoto.Height = image.Height;
						imagePhoto.ImageUrl = FileManager.Instance.GetUrl (image);
					}

					// set alt & title for photo
					imagePhoto.AlternateText = fullName;
					imagePhoto.ToolTip = fullName;

					// make image visible
					imageVisible = true;
				}
			}

			if (imageVisible)
			{
				// imagePhoto.Attributes.Add("onclick", Utils.EditUrl (this, "Details", "employee_id", EmployeeID.ToString ()));
				var popupUrl = Utils.EditUrl (this, "Details", "employee_id", employee.EmployeeID.ToString ());
				
				// alter popup window height
				linkPhoto.NavigateUrl = popupUrl.Replace ("550,950", "450,950");
			}

			// REVIEW: Need to add fallback image?
			linkPhoto.Visible = imageVisible;
			// imagePhoto.Visible = imageVisible;

			/* // Old academic degree & title
			var degreeAndTitle = Utils.FormatList (", ", employee.AcademicDegree, employee.AcademicTitle);
			if (!string.IsNullOrWhiteSpace (degreeAndTitle))
				labelAcademicDegreeAndTitle.Text = "&nbsp;&ndash; " + degreeAndTitle;
			else
				labelAcademicDegreeAndTitle.Visible = false;
			*/
			
			// Employee titles
			var titles = achievements.Select (ach => Utils.FirstCharToLower (ach.DisplayShortTitle)).ToList ();
			
			// add academic degree and title for backward compatibility
			titles.Add (employee.AcademicDegree);
			titles.Add (employee.AcademicTitle);
	
			var strTitles = Utils.FormatList (", ", titles);
			if (!string.IsNullOrWhiteSpace (strTitles))
				labelAcademicDegreeAndTitle.Text = "&nbsp;&ndash; " + strTitles;
			else
				labelAcademicDegreeAndTitle.Visible = false;
	
			// Phone
			if (!string.IsNullOrWhiteSpace (employee.Phone))
				labelPhone.Text = employee.Phone;
			else
				labelPhone.Visible = false;

			// CellPhome
			if (!string.IsNullOrWhiteSpace (employee.CellPhone))
				labelCellPhone.Text = employee.CellPhone;
			else
				labelCellPhone.Visible = false;

			// Fax
			if (!string.IsNullOrWhiteSpace (employee.Fax))
				labelFax.Text = string.Format (Localization.GetString ("Fax.Format", LocalResourceFile), employee.Fax);
			else
				labelFax.Visible = false;

			// Messenger
			if (!string.IsNullOrWhiteSpace (employee.Messenger))
				labelMessenger.Text = employee.Messenger;
			else
				labelMessenger.Visible = false;

			// Working place and Hours
			var workingPlaceAndHours = Utils.FormatList (", ", employee.WorkingPlace, employee.WorkingHours);
			if (!string.IsNullOrWhiteSpace (workingPlaceAndHours))
				labelWorkingPlaceAndHours.Text = workingPlaceAndHours;
			else
				labelWorkingPlaceAndHours.Visible = false;

			/*
			// Working place
			if (!string.IsNullOrWhiteSpace (employee.WorkingPlace))
				labelWorkingPlace.Text = employee.WorkingPlace;
			else
				labelWorkingPlace.Visible = false;

			// Working hours
			if (!string.IsNullOrWhiteSpace (employee.WorkingHours))
				labelWorkingHours.Text = employee.WorkingHours;
			else
				labelWorkingHours.Visible = false;
			*/

			/*
			// WebSite
			if (!string.IsNullOrWhiteSpace (employee.WebSite))
			{
				// THINK: Do we have to check if WebSite starting with http:// or https://?
				linkWebSite.NavigateUrl = "http://" + employee.WebSite;
				linkWebSite.Text = employee.WebSite;
			}
			else
				linkWebSite.Visible = false;
			*/

			// WebSite
			if (!string.IsNullOrWhiteSpace (employee.WebSite))
			{
				// REVIEW: Less optimistic protocol detection?
				var lowerWebSite = employee.WebSite.ToLowerInvariant ();
				if (lowerWebSite.StartsWith ("http://") || lowerWebSite.StartsWith ("https://"))
				{
					linkWebSite.NavigateUrl = employee.WebSite;
					// 01234567890
					// http://www.volgau.com
					// https://www.volgau.com
					linkWebSite.Text = employee.WebSite.Remove (0, employee.WebSite.IndexOf ("://") + 3); 
				}
				else
				{
					// add http by default
					linkWebSite.NavigateUrl = "http://" + employee.WebSite;
					linkWebSite.Text = employee.WebSite; 
				}
			}
			else
				linkWebSite.Visible = false;


			// Email
			if (!string.IsNullOrWhiteSpace (employee.Email))
			{
				linkEmail.NavigateUrl = "mailto:" + employee.Email;
				linkEmail.Text = employee.Email;
			}
			else
				linkEmail.Visible = false;

			// Secondary email
			if (!string.IsNullOrWhiteSpace (employee.SecondaryEmail))
			{
				linkSecondaryEmail.NavigateUrl = "mailto:" + employee.SecondaryEmail;
				linkSecondaryEmail.Text = employee.SecondaryEmail;
			}
			else
				linkSecondaryEmail.Visible = false;

			// Profile link
			if (!Utils.IsNull<int> (employee.UserID))
			{
				linkUserProfile.NavigateUrl = Globals.UserProfileURL (employee.UserID.Value);
				// TODO: Replace profile text with something more sane
				linkUserProfile.Text = Localization.GetString ("VisitProfile.Text", LocalResourceFile);
			}
			else
				linkUserProfile.Visible = false;
		}

		#region IActionable implementation

		public DotNetNuke.Entities.Modules.Actions.ModuleActionCollection ModuleActions
		{
			get
			{
				// create a new action to add an item, this will be added 
				// to the controls dropdown menu
				var actions = new ModuleActionCollection ();

				var employeeId = GetEmployeeId ();

				actions.Add (
					GetNextActionID (), 
					Localization.GetString ("AddEmployee.Action", this.LocalResourceFile),
					ModuleActionType.AddContent, 
					"", 
					"", 
					Utils.EditUrl (this, "Edit"),
					false, 
					DotNetNuke.Security.SecurityAccessLevel.Edit,
					employeeId == null, 
					false
				);

				// otherwise, add "edit" action
				actions.Add (
					GetNextActionID (), 
					Localization.GetString ("EditEmployee.Action", this.LocalResourceFile),
					ModuleActionType.EditContent, 
					"", 
					"", 
					Utils.EditUrl (this, "Edit", "employee_id", employeeId.ToString ()),
					false, 
					DotNetNuke.Security.SecurityAccessLevel.Edit,
					employeeId != null, 
					false
				);

				actions.Add (
					GetNextActionID (), 
					Localization.GetString ("Details.Action", this.LocalResourceFile),
					ModuleActionType.ContentOptions, 
					"", 
					"", 
					Utils.EditUrl (this, "Details", "employee_id", employeeId.ToString ()).Replace ("550,950", "450,950"),
					false, 
					DotNetNuke.Security.SecurityAccessLevel.View,
					employeeId != null, 
					false
				);

				actions.Add (
					GetNextActionID (), 
					Localization.GetString ("VCard.Action", this.LocalResourceFile),
					ModuleActionType.ContentOptions, 
					"", 
					"", 
					Utils.EditUrl (this, "VCard", "employee_id", employeeId.ToString ()),
					false,
					DotNetNuke.Security.SecurityAccessLevel.View,
					employeeId != null,
					true
				);

				/*
				#if (DATACACHE)
				actions.Add (
					GetNextActionID (), 
					"Clear Data Cache", // Localization.GetString("ClearDataCache.Action", this.LocalResourceFile),
					"ClearDataCache.Action", "", 
					"/images/action_refresh.gif",
					"", //Utils.EditUrl (this, "VCard", "employee_id", EmployeeID.ToString ()),
					true,  // use action event
					DotNetNuke.Security.SecurityAccessLevel.Edit,
					true, 
					false // open in new window
				);
				#endif*/

				return actions;
			}
		}

		#endregion

		protected void repeaterPositions_ItemDataBound (object sender, RepeaterItemEventArgs e)
		{
			// exclude header & footer
			if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
			{
				var opex = e.Item.DataItem as OccupiedPositionInfoEx;

				var labelPosition = e.Item.FindControl ("labelPosition") as Label;
				var labelDivision = e.Item.FindControl ("labelDivision") as Label;
				var linkDivision = e.Item.FindControl ("linkDivision") as HyperLink;

				// opex.PositionShortTitle is a comma-separated 
				// list of positions, including TitleSuffix
				labelPosition.Text = opex.PositionShortTitle;

				// don't display division title for highest level divisions
				if (Utils.IsNull (opex.ParentDivisionID))
				{
					labelDivision.Visible = false;
					linkDivision.Visible = false;
				}
				else
				{
					if (!string.IsNullOrWhiteSpace (opex.HomePage))
					{
						// link to division's homepage
						labelDivision.Visible = false;
						linkDivision.NavigateUrl = Utils.FormatURL (this, opex.HomePage, false);
					}
					else
					{	
						// only division title
						linkDivision.Visible = false;
					}

					labelPosition.Text += ": "; // to prev label!
					linkDivision.Text = opex.DivisionShortTitle;
				}
			}
		}

	}
	// class
}
// namespace

