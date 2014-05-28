using System;
using System.IO;
using System.Data;
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
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.Localization;
using DotNetNuke.Services.FileSystem;
using R7.University;

namespace R7.University.EmployeeList
{
	public partial class ViewEmployeeList : PortalModuleBase, IActionable
	{
		private EmployeeListController __ctrl;
		protected EmployeeListController Ctrl
		{
			get { return __ctrl ?? (__ctrl = new EmployeeListController ()); }
		}

		private EmployeeListSettings __customSettings;
		protected EmployeeListSettings CustomSettings
		{
			get { return __customSettings ?? (__customSettings = new EmployeeListSettings (this)); }
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
					if (Cache_OnLoad()) return;
					
					// REVIEW: Add Employees.LastYearRating field and sorting by it!
					
					// get employees by DivisionID, in edit mode show also non-published employees
					var	items = Ctrl.GetObjects<EmployeeInfo> (CommandType.StoredProcedure, 
						(CustomSettings.IncludeSubdivisions)? // which SP to use
							"University_GetRecursiveEmployeesByDivisionID" : "University_GetEmployeesByDivisionID", 
						CustomSettings.DivisionID, CustomSettings.SortType, IsEditable
					);

					// check if we have some content to display, 
					// otherwise display a message for module editors or hide module from regular users
					if (items == null || !items.Any())
					{
						// set container control visibility to common users
						 Cache_SetContainerVisible(false);
						
						if (IsEditable)
							Utils.Message (this, "NothingToDisplay.Text", MessageType.Info, true);
						else
							// hide entire module
							ContainerControl.Visible = false;
					}
					else
					{
						// set container control visibility to common users
						Cache_SetContainerVisible(true);
						
						// bind the data
						listEmployees.DataSource = items;
						listEmployees.DataBind ();
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
					Null.IsNull(CustomSettings.DivisionID)?
					Utils.EditUrl (this, "Edit") : Utils.EditUrl (this, "Edit", "division_id", CustomSettings.DivisionID.ToString()),
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
		protected void listEmployees_ItemDataBound (object sender, System.Web.UI.WebControls.DataListItemEventArgs e)
		{
			// use e.Item.DataItem as object of EmployeeListInfo class,
			// as we really know it is:
			var employee = e.Item.DataItem as EmployeeInfo;
			
			// find controls in DataList item template
			var linkEdit = e.Item.FindControl ("linkEdit") as HyperLink;
			var imageEdit = e.Item.FindControl ("imageEdit") as Image;
			var imagePhoto = e.Item.FindControl ("imagePhoto") as Image;
			var linkDetails = e.Item.FindControl ("linkDetails") as HyperLink; 
			var labelFullName = e.Item.FindControl ("labelFullName") as Label;
			var labelAcademicDegreeAndTitle = e.Item.FindControl ("labelAcademicDegreeAndTitle") as Label;
			var labelPositions = e.Item.FindControl ("labelPositions") as Label;
			var labelPhones = e.Item.FindControl ("labelPhones") as Label;
			var linkEmail = e.Item.FindControl ("linkEmail") as HyperLink;
			var linkSecondaryEmail = e.Item.FindControl ("linkSecondaryEmail") as HyperLink;
			var linkWebSite = e.Item.FindControl ("linkWebSite") as HyperLink;
			var linkUserProfile = e.Item.FindControl ("linkUserProfile") as HyperLink;

			// read module settings (may be useful in a future)
			// var settings = new EmployeeListSettings (this);            
            
			// edit link
			if (IsEditable)
			{
				if (Null.IsNull (CustomSettings.DivisionID))
					linkEdit.NavigateUrl = Utils.EditUrl (this, "Edit", "employee_id", employee.EmployeeID.ToString ());
				else
					linkEdit.NavigateUrl = Utils.EditUrl (this, "Edit", "employee_id", employee.EmployeeID.ToString (), 
						"division_id", CustomSettings.DivisionID.ToString ());
				// WTF: iconEdit.NavigateUrl = Utils.FormatURL (this, image.Url, false);
			}

			// make edit link visible in edit mode
			linkEdit.Visible = IsEditable;
			imageEdit.Visible = IsEditable;
            
			// mark non-published employees, as they visible only to editors
			if (!employee.IsPublished)
			if (e.Item.ItemType == ListItemType.Item)
				e.Item.CssClass = listEmployees.ItemStyle.CssClass + " NonPublished";
			else
				e.Item.CssClass = listEmployees.AlternatingItemStyle.CssClass + " NonPublished";

			// fill the controls

			// photo
			if (!Utils.IsNull (employee.PhotoFileID))
			{
				var photo = FileManager.Instance.GetFile (employee.PhotoFileID.Value);
				if (photo != null)
				{
					/*
					var miniPhoto = FileManager.Instance.GetFile (
						// FIXME: Remove hard-coded photo filename replace options
						FolderManager.Instance.GetFolder (photo.FolderId), photo.FileName.Replace ("_prev.", "_square_prev."));

					if (miniPhoto != null && miniPhoto.FileId != photo.FileId)
					{
						imagePhoto.ImageUrl = Utils.FormatURL (this, "FileID=" + miniPhoto.FileId, false);
						imagePhoto.Width = miniPhoto.Width;
						imagePhoto.Height = miniPhoto.Height;
					}*/
				
					var squarePhoto = FileManager.Instance.GetFile (
						// FIXME: Remove hard-coded photo filename options
						                  FolderManager.Instance.GetFolder (photo.FolderId), 
						                  Path.GetFileNameWithoutExtension (photo.FileName) + "_square" + Path.GetExtension (photo.FileName));

					var photoWidth = CustomSettings.PhotoWidth;

					if (squarePhoto != null)
					{
						if (!Null.IsNull (photoWidth))
						{
							imagePhoto.ImageUrl = string.Format (
								"/imagehandler.ashx?fileid={0}&width={1}", squarePhoto.FileId, photoWidth);
							imagePhoto.Width = photoWidth;
							imagePhoto.Height = photoWidth;
						}
						else
						{
							imagePhoto.ImageUrl = FileManager.Instance.GetUrl (squarePhoto);
							imagePhoto.Width = squarePhoto.Width;
							imagePhoto.Height = squarePhoto.Height;
						}
					}
					else if (squarePhoto == null)
					{
						imagePhoto.ImageUrl = FileManager.Instance.GetUrl (photo);
						imagePhoto.Width = photo.Width;
						imagePhoto.Height = photo.Height;
					}
				}
			}

			// photo fallback
			if (string.IsNullOrWhiteSpace (imagePhoto.ImageUrl))
			{
				// TODO: Add fallback mini photo here
				// imagePhoto.Visible = false;
				linkDetails.Visible = false;
			}
			else
			{
				// link to employee details
				linkDetails.NavigateUrl = Utils.EditUrl(this, "Details", "employee_id", employee.EmployeeID.ToString()).Replace("550,950", "450,950");
			}

			// employee fullname
			labelFullName.Text = employee.FullName;

			// academic degree and title
			var degreeAndTitle = Utils.FormatList (", ", employee.AcademicDegree, employee.AcademicTitle);
			if (!string.IsNullOrWhiteSpace (degreeAndTitle))
				labelAcademicDegreeAndTitle.Text = "&nbsp;&ndash; " + degreeAndTitle;
			else
				labelAcademicDegreeAndTitle.Visible = false;

			// phones
			var phones = Utils.FormatList (", ", employee.Phone, employee.CellPhone);
			if (!string.IsNullOrWhiteSpace (phones))
				labelPhones.Text = phones;
			else
				labelPhones.Visible = false;

			// email
			if (!string.IsNullOrWhiteSpace (employee.Email))
			{
				linkEmail.NavigateUrl = "mailto:" + employee.Email;
				linkEmail.Text = employee.Email; 
			}
			else
				linkEmail.Visible = false;

			// secondary email
			if (!string.IsNullOrWhiteSpace (employee.SecondaryEmail))
			{
				linkSecondaryEmail.NavigateUrl = "mailto:" + employee.SecondaryEmail;
				linkSecondaryEmail.Text = employee.SecondaryEmail; 
			}
			else
				linkSecondaryEmail.Visible = false;

			// website
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

			// Profile link
			if (!Utils.IsNull<int> (employee.UserID))
			{
				linkUserProfile.NavigateUrl = Globals.UserProfileURL (employee.UserID.Value);
				// TODO: Replace profile text with something more sane
				linkUserProfile.Text = Localization.GetString ("VisitProfile.Text", LocalResourceFile);
			}
			else
				linkUserProfile.Visible = false;

			// occupied positions
			// NOTE: Current division positions go first, then checks IsPrime, then PositionWeight
			// TODO: Need to retrieve occupied positions more effectively, e.g. preload them
			// THINK: add "AND [DivisionID] = @1" to display employee positions only from current division
			var ops = Ctrl.GetObjects<OccupiedPositionInfoEx> (
				"WHERE [EmployeeID] = @0 ORDER BY (CASE WHEN [DivisionID]=@1 THEN 0 ELSE 1 END), [IsPrime] DESC, [PositionWeight] DESC", 
				employee.EmployeeID, CustomSettings.DivisionID
			);

			// build positions value
			var positionsVisible = false;
			if (ops != null && ops.Any())
			{
				var strOps = string.Empty;
				foreach (var op in OccupiedPositionInfoEx.GroupByDivision (ops))
				{
					// do not display division title for high-level divisions AND current division
					if (op.DivisionID == CustomSettings.DivisionID || op.ParentDivisionID == null)
						strOps = Utils.FormatList (", ", strOps, op.PositionShortTitle);
					else
					{
						// division name or link
						var strDivision = "";
						if (!string.IsNullOrWhiteSpace (op.HomePage))
							strDivision = string.Format ("<a href=\"{0}\">{1}</a>", 
								Utils.FormatURL(this, op.HomePage, false), op.DivisionShortTitle);
						else
							strDivision = op.DivisionShortTitle;

						strOps = Utils.FormatList (", ", strOps, 
							Utils.FormatList (": ", op.PositionShortTitle, strDivision));
					}
				}

				if (!string.IsNullOrWhiteSpace (strOps))
				{
					labelPositions.Text = strOps;
					positionsVisible = true;
				}
			}
			labelPositions.Visible = positionsVisible;
		}
	}
}

