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

            if (Request.QueryString ["popup"] != null)
            {
                linkReturn.Attributes.Add ("onclick", "javascript:return " +
                    UrlUtils.ClosePopUp (refresh: false, url: "", onClickEvent: true));
            }
            else
                linkReturn.NavigateUrl = Globals.NavigateURL ();
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
			
					EmployeeInfo employee = null;

					if (employeeId != null)
					{
						// get employee by querystring param
						employee = EmployeeController.Get<EmployeeInfo> (employeeId.Value);
					}
					else if (ModuleConfiguration.ModuleDefinition.DefinitionName == "R7.University.Employee")
					{
						// if not, try to use Employee module settings
						employee = GetEmployee ();
					}

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

                            if (IsEditable || UserInfo.IsSuperUser) 
                            {
                                linkEdit.Visible = true;
                                linkEdit.NavigateUrl = Utils.EditUrl (this, "EditEmployee", "employee_id", employeeId.Value.ToString ());
							}
						}
						else
							// can show only published
							Response.Redirect (Globals.NavigateURL (), true);
					}
					else 
						// nothing to show
						Response.Redirect (Globals.NavigateURL (), true);

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
			
            if (Request.QueryString ["popup"] != null)
            {
                // set popup title to employee name
                ((DotNetNuke.Framework.CDefault) this.Page).Title = fullname;
            }
            else
            {
                // display employee name in label
                literalFullName.Text = "<h2>" + fullname + "</h2>";
            }

			// occupied positions
			var occupiedPositions = EmployeeController.GetObjects<OccupiedPositionInfoEx> ("WHERE [EmployeeID] = @0 ORDER BY [IsPrime] DESC, [PositionWeight] DESC", employee.EmployeeID);
			if (occupiedPositions != null && occupiedPositions.Any ())
			{
				repeaterPositions.DataSource = OccupiedPositionInfoEx.GroupByDivision (occupiedPositions);
				repeaterPositions.DataBind ();
			}
			else
				repeaterPositions.Visible = false;
			
            EmployeePhotoLogic.Bind (employee, imagePhoto, EmployeeSettings.PhotoWidth);
			
			Barcode (employee);
					
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
				linkWebSite.NavigateUrl = employee.FormatWebSiteUrl;
				linkWebSite.Text = employee.FormatWebSiteLabel;
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

			// disciplines
			if (!string.IsNullOrWhiteSpace (employee.Disciplines))
				litDisciplines.Text = Server.HtmlDecode (employee.Disciplines);
			else
			{
				// hide entire Disciplines tab
				linkDisciplines.Visible = false;
			}

			Experience (employee);
			
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
			var noExpYears = false;
			
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
				// hide label for experience years
				labelExperienceYears.Visible = false;
				
				// about to hide Experience tab
				noExpYears = true;
			}

			// get all empoyee achievements
			var achievements = EmployeeController.GetObjects<EmployeeAchievementInfo> (
				                   CommandType.Text, "SELECT * FROM dbo.vw_University_EmployeeAchievements WHERE [EmployeeID] = @0",
				                   employee.EmployeeID);
	
			// employee titles
			var titles = achievements.Where (ach => ach.IsTitle).Select (ach => Utils.FirstCharToLower (ach.DisplayShortTitle)).ToList ();

			// add academic degree and title for backward compatibility
			titles.Add (employee.AcademicDegree);
			titles.Add (employee.AcademicTitle);

			var strTitles = Utils.FormatList (", ", titles);
			if (!string.IsNullOrWhiteSpace (strTitles))
				labelAcademicDegreeAndTitle.Text = Utils.FirstCharToUpper (strTitles);
			else
				labelAcademicDegreeAndTitle.Visible = false;

			// get only experience-related achievements
			var experiences = achievements.Where (ach => ach.AchievementType == AchievementType.Education ||
			                  ach.AchievementType == AchievementType.Training ||
			                  ach.AchievementType == AchievementType.Work);

			if (experiences != null && experiences.Any ())
			{
				gridExperience.DataSource = AchievementsDataTable (experiences.OrderByDescending (exp => exp.YearBegin));
				gridExperience.DataBind ();
			}
			else if (noExpYears)
			{
				// hide experience tab
				linkExperience.Visible = false;
			}
		
			// get all other achievements
			achievements = achievements.Where (ach => ach.AchievementType == AchievementType.Achievement);
			if (achievements != null && achievements.Any ())
			{
				gridAchievements.DataSource = AchievementsDataTable (achievements.OrderByDescending (ach => ach.YearBegin));
				gridAchievements.DataBind ();
			}
			else
			{	
				// hide achievements tab
				linkAchievements.Visible = false;
			}
		}

		private DataTable AchievementsDataTable (IEnumerable<EmployeeAchievementInfo> achievements)
		{
			var dt = new DataTable ();
			DataRow dr;
			
			dt.Columns.Add (new DataColumn (LocalizeString ("Years.Column"), typeof(string)));
			dt.Columns.Add (new DataColumn (LocalizeString ("Title.Column"), typeof(string)));
			dt.Columns.Add (new DataColumn (LocalizeString ("AchievementType.Column"), typeof(string)));
			dt.Columns.Add (new DataColumn (LocalizeString ("DocumentUrl.Column"), typeof(string)));
		
			// add description column (no need to localize as it's hidden)
			dt.Columns.Add (new DataColumn ("Description.Column", typeof(string)));
					
			foreach (DataColumn column in dt.Columns)
				column.AllowDBNull = true;

			var atTheMoment = LocalizeString ("AtTheMoment.Text");

			foreach (var achievement in achievements)
			{
				var col = 0;
				dr = dt.NewRow ();
				dr [col++] = achievement.FormatYears.Replace ("{ATM}", atTheMoment);
				dr [col++] = achievement.Title + " " + achievement.TitleSuffix;
				dr [col++] = LocalizeString (AchievementTypeInfo.GetResourceKey (achievement.AchievementType));
				dr [col++] = achievement.DocumentURL; 
				dr [col++] = achievement.Description;
					
				dt.Rows.Add (dr);
			}

			return dt;
		}

		protected void gridExperience_RowDataBound (object sender, GridViewRowEventArgs e)
		{
			// description
			e.Row.Cells [4].Visible = false;
			e.Row.ToolTip = Server.HtmlDecode (e.Row.Cells [4].Text);

			// exclude header
			if (e.Row.RowType == DataControlRowType.DataRow)
			{
                // make link to the document
				// WTF: empty DocumentURL's cells contains non-breakable spaces?
				var documentUrl = e.Row.Cells [3].Text.Replace ("&nbsp;", "");
				if (!string.IsNullOrWhiteSpace (documentUrl))
					e.Row.Cells [3].Text = string.Format ("<a href=\"{0}\" target=\"_blank\">{1}</a>", 
						Globals.LinkClick (documentUrl, TabId, ModuleId), LocalizeString ("DocumentUrl.Text"));

				// e.Row.Cells [4].Text = "...";
				// e.Row.Cells [4].Attributes.Add("onclick", "javascript:confirm('" + description + "')");
			}
		}

		protected void repeaterPositions_ItemDataBound (object sender, RepeaterItemEventArgs e)
		{
            RepeaterPositionsLogic.ItemDataBound (this, sender, e);
		}
	}
	// class
}
// namespace

