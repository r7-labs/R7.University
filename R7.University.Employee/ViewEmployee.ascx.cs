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
using DotNetNuke.Entities.Users;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Services.Localization;
using R7.University;

namespace R7.University.Employee
{
	public partial class ViewEmployee : PortalModuleBase, IActionable
	{
		#region Properties

		private bool employeeIDLoaded = false;
		private int employeeID = Null.NullInteger;

		protected int EmployeeID
		{
			get 
			{
				if (employeeIDLoaded)
					return employeeID;
				else
				{
					var settings = new EmployeeSettings (this);
					employeeID = settings.EmployeeID;
					employeeIDLoaded = true;
					return employeeID;
				}
			} 
		}


		#endregion 

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
					var ctrl = new EmployeeController ();
					var settings = new EmployeeSettings (this);

					var display = true;
					EmployeeInfo employee = null;

					// check if we have something to display
					if (Null.IsNull (EmployeeID))
					{
						if (IsEditable)
							Utils.Message (this, MessageSeverity.Info, "NothingToDisplay.Text", true);

						display = false;
					}
					else 
					{
						employee = ctrl.Get<EmployeeInfo>(EmployeeID);

						if (employee == null)
						{
							// employee is hard-deleted!
							if (IsEditable)
								Utils.Message (this, MessageSeverity.Error, "EmployeeHardDeleted.Text", true);

							// were is nothing to display
							display = false;
						}
						else 
						{
							/*
							if (employee.IsDeleted)
							{
								// employee is soft-deleted
								if (IsEditable)
									Utils.Message (this, MessageSeverity.Warning, "EmployeeSoftDeleted.Text", true);

								// display only in edit mode
								display = IsEditable;
							}*/

							if (!employee.IsPublished)
							{
								// employee isn't published
								if (IsEditable)
									Utils.Message (this, MessageSeverity.Warning, "EmployeeNotPublished.Text", true);

								// display only in edit mode
								display = IsEditable;
							}
						}
					}

					// display or hide entire module
					ContainerControl.Visible = display || IsEditable;

					// display or hide module content
					panelEmployee.Visible = display;

					if (display)
					{
						if (settings.AutoTitle)
							AutoTitle (employee);

						// display employee info
						Display (employee);
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
		protected void Display (EmployeeInfo employee)
		{
			var ctrl = new EmployeeController ();
			var setting = new EmployeeSettings (this);

			// occupied positions
			var occupiedPositions = ctrl.GetObjects<OccupiedPositionInfoEx> ("WHERE [EmployeeID] = @0 ORDER BY [IsPrime] DESC, [PositionWeight] DESC", employee.EmployeeID);
			if (occupiedPositions != null && occupiedPositions.Any())
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
					var photoWidth = setting.PhotoWidth;

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

			// REVIEW: Need to add fallback image?
			imagePhoto.Visible = imageVisible;

			// Academic degree & title
			var degreeAndTitle = Utils.FormatList (", ", employee.AcademicDegree, employee.AcademicTitle);
			if (!string.IsNullOrWhiteSpace (degreeAndTitle))
				labelAcademicDegreeAndTitle.Text = "&nbsp;&ndash; " + degreeAndTitle;
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
				labelFax.Text = employee.Fax + " " + Localization.GetString("Fax.Text", LocalResourceFile);
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
				// THINK: Less optimistic protocol detection?
				var lowerWebSite = employee.WebSite.ToLowerInvariant ();
				if (lowerWebSite.StartsWith ("http://") ||  lowerWebSite.StartsWith ("https://"))
				{
					linkWebSite.NavigateUrl = employee.WebSite;
					// 01234567890
					// http://www.volgau.com
					// https://www.volgau.com
					linkWebSite.Text = employee.WebSite.Remove(0, employee.WebSite.IndexOf("://")+3); 
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

				// add "add" action only if we have no employee to display
				if (Null.IsNull (EmployeeID))
				{
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
				}
				else
				{
					// otherwise, add "edit" action
					actions.Add (
						GetNextActionID (), 
						Localization.GetString (ModuleActionType.EditContent, this.LocalResourceFile),
						ModuleActionType.EditContent, 
						"", 
						"", 
						Utils.EditUrl (this, "Edit", "employee_id", EmployeeID.ToString ()),
						false, 
						DotNetNuke.Security.SecurityAccessLevel.Edit,
						true, 
						false
					);
				}

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

				labelPosition.Text = opex.PositionShortTitle;

				// don't display division title for highest level divisions
				if (Utils.IsNull(opex.ParentDivisionID))
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

						labelPosition.Text += ": "; // to prev label!
						linkDivision.Text = opex.DivisionShortTitle;
					}
					else
					{	
						// only division title
						linkDivision.Visible = false;

						labelPosition.Text += ": "; // to prev label!
						labelDivision.Text = opex.DivisionShortTitle;
					}
				}
			}
		}

	} // class
} // namespace

