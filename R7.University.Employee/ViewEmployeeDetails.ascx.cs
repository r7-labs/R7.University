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
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Users;
using DotNetNuke.Entities.Modules.Actions;
using DotNetNuke.Services.Exceptions;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Services.Localization;
using R7.University;

namespace R7.University.Employee
{
	public partial class ViewEmployeeDetails : EmployeePortalModuleBase
	{
		#region Properties

		#endregion

		#region Handlers

		/// <summary>
		/// Handles Init event for a control
		/// </summary>
		/// <param name="e">Event args.</param>
		protected override void OnInit (EventArgs e)
		{
			base.OnInit (e);

			linkReturn.Attributes.Add ("onclick", "javascript:return " +
			UrlUtils.ClosePopUp (refresh: false, url: "", onClickEvent: true));
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
					// try get EmployeeID from querysting
					var employeeId = Utils.ParseToNullableInt (Request.QueryString ["employee_id"]);
			
					// if not, use module settings
					if (employeeId == null && !Null.IsNull (EmployeeSettings.EmployeeID))
						employeeId = EmployeeSettings.EmployeeID;
					
					if (employeeId != null)
					{
						var employee = EmployeeController.Get<EmployeeInfo> (employeeId.Value);
					
						if (employee != null)
						{
							if (IsEditable || employee.IsPublished)
							{
								Display (employee);
								
								if (IsEditable)
								{
									linkVCard.Visible = true;
									linkVCard.NavigateUrl = Utils.EditUrl (this, "VCard", "employee_id", employeeId.Value.ToString ());
								}
							}
							else
								// can show only published
								Response.Redirect (Globals.NavigateURL (), true);
						}
						else 
							// nothing to show
							Response.Redirect (Globals.NavigateURL (), true);
					}
				} // if (!IsPostBack)
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		#endregion

		protected void Display (EmployeeInfo employee)
		{
			var fullname = employee.FullName;
			
			// set popup title
			((DotNetNuke.Framework.CDefault)this.Page).Title = fullname;
			
			
			// occupied positions
			var occupiedPositions = EmployeeController.GetObjects<OccupiedPositionInfoEx> ("WHERE [EmployeeID] = @0 ORDER BY [IsPrime] DESC, [PositionWeight] DESC", employee.EmployeeID);
			if (occupiedPositions != null && occupiedPositions.Any ())
			{
				repeaterPositions.DataSource = OccupiedPositionInfoEx.GroupByDivision (occupiedPositions);
				repeaterPositions.DataBind ();
			}
			else
				repeaterPositions.Visible = false;
			
			Photo (employee, fullname);
			
			Barcode (employee);
			
			// Academic degree & title
			var degreeAndTitle = Utils.FormatList (", ", employee.AcademicDegree, employee.AcademicTitle);
			if (!string.IsNullOrWhiteSpace (degreeAndTitle))
				labelAcademicDegreeAndTitle.Text = Utils.FirstCharToUpper (degreeAndTitle);
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
			
			
			// about
			if (!string.IsNullOrWhiteSpace (employee.Biography))
				litAbout.Text = Server.HtmlDecode (employee.Biography);
			else
			{
				// hide entire About tab
				linkAbout.Visible = false;
			}
			
			Experience (employee);
			
		}

		void Photo (EmployeeInfo employee, string fullname)
		{
			var imageVisible = false;
			
			// Photo
			if (!Utils.IsNull<int> (employee.PhotoFileID))
			{
				// REVIEW: Need add ON DELETE rule to FK, linking PhotoFileID & Files.FileID 

				var image = FileManager.Instance.GetFile (employee.PhotoFileID.Value);
				if (image != null)
				{
					// TODO: Then opening from EmployeeList module, default PhotoWidth value is used
					// as no Employee_PhotoWidth setting exists for this module
					
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
					imagePhoto.AlternateText = fullname;
					imagePhoto.ToolTip = fullname;

					// make image visible
					imageVisible = true;
				}
			}

			imagePhoto.Visible = imageVisible;
		}

		void Barcode (EmployeeInfo employee)
		{
			// barcode image test
			var barcodeWidth = 150;
			imageBarcode.ImageUrl = 
				string.Format ("/imagehandler.ashx?barcode=1&width={0}&height={1}&type=qrcode&encoding=UTF-8&content={2}",
				barcodeWidth, barcodeWidth, 
				Server.UrlEncode (employee.VCard.ToString ()
						.Replace ("+", "%2b")) // fix for "+" signs in phone numbers
			);

			imageBarcode.ToolTip = LocalizeString ("imageBarcode.ToolTip");
			imageBarcode.AlternateText = LocalizeString ("imageBarcode.AlternateText");
		}

		void Experience (EmployeeInfo employee)
		{
			// experience years
			var exp1 = false;
			var exp2 = false;
			
			// Общий стаж работы (лет): {0}
			// Общий стаж работы по специальности (лет): {0}
			// Общий стаж работы (лет): {0}, из них по специальности: {1}
			
			if (employee.ExperienceYears != null && employee.ExperienceYears.Value > 0)
				exp1 = true;
			
			if (employee.ExperienceYearsBySpec != null && employee.ExperienceYearsBySpec.Value > 0)
				exp2 = true;
			
			if (exp1 && !exp2)
			{
				labelExperienceYears.Text = string.Format (
					LocalizeString ("ExperienceYears.Format1"), employee.ExperienceYears.Value);
			}
			else if (!exp1 && exp2)
			{
				labelExperienceYears.Text = string.Format (
					LocalizeString ("ExperienceYears.Format2"), employee.ExperienceYearsBySpec);
			}
			else if (exp1 && exp2)
			{
				labelExperienceYears.Text = string.Format (
					LocalizeString ("ExperienceYears.Format3"), 
					employee.ExperienceYears.Value, employee.ExperienceYearsBySpec);
			}
			else
			{
				// hide Experience tab
				linkExperience.Visible = false;
			}

			// get all empoyee achievements
			var achievements = EmployeeController.GetObjects<EmployeeAchievementInfo> (
				                   CommandType.Text, "SELECT * FROM dbo.vw_University_EmployeeAchievements WHERE [EmployeeID] = @0",
				                   employee.EmployeeID);
	
			// get only experience-related achievements
			gridExperience.DataSource = AchievementsDataTable (
				achievements.Where (ach => ach.AchievementType == AchievementType.Education ||
				ach.AchievementType == AchievementType.Training ||
				ach.AchievementType == AchievementType.Work)
				.OrderByDescending (ach => ach.YearBegin));
			gridExperience.DataBind ();

			// get all other achievements
			gridAchievements.DataSource = AchievementsDataTable (
				achievements.Where (ach => ach.AchievementType == AchievementType.Achievement)
				.OrderByDescending (ach => ach.YearBegin));
			gridAchievements.DataBind ();
		}

		private DataTable AchievementsDataTable (IEnumerable<EmployeeAchievementInfo> achievements)
		{
			var dt = new DataTable ();
			DataRow dr;
			
			dt.Columns.Add (new DataColumn (LocalizeString ("Years.Column"), typeof(string)));
			dt.Columns.Add (new DataColumn (LocalizeString ("Title.Column"), typeof(string)));
			dt.Columns.Add (new DataColumn (LocalizeString ("AchievementType.Column"), typeof(string)));
			dt.Columns.Add (new DataColumn (LocalizeString ("DocumentUrl.Column"), typeof(string)));
		
			foreach (DataColumn column in dt.Columns)
				column.AllowDBNull = true;

			foreach (var achievement in achievements)
			{
				var col = 0;
				dr = dt.NewRow ();
				dr [col++] = achievement.FormatYears;
				dr [col++] = achievement.Title + " " + achievement.TitleSuffix;
				dr [col++] = LocalizeString (AchievementTypeInfo.GetResourceKey (achievement.AchievementType));
				dr [col++] = achievement.DocumentURL; 
					
				dt.Rows.Add (dr);
			}

			return dt;
		}

		protected void gridExperience_RowDataBound (object sender, GridViewRowEventArgs e)
		{
			// exclude header
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
				// WTF: empty DocumentURL's cells contains non-breakable spaces?
				var documentUrl = e.Row.Cells [3].Text.Replace ("&nbsp;", "");
				if (!string.IsNullOrWhiteSpace (documentUrl))
					e.Row.Cells [3].Text = string.Format ("<a href=\"{0}\" target=\"_blank\">{1}</a>", 
						Globals.LinkClick (documentUrl, TabId, ModuleId), LocalizeString ("DocumentUrl.Text"));
			}
		}

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
	}
	// class
}
 // namespace

