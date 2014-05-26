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

			linkVCard.NavigateUrl = Utils.EditUrl (this, "VCard", "employee_id", EmployeeSettings.EmployeeID.ToString ()); 
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
					if (!Null.IsNull(EmployeeSettings.EmployeeID))
					{
						var employee = EmployeeController.Get<EmployeeInfo>(EmployeeSettings.EmployeeID);
						
						if (employee != null)
							Display(employee);
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
			
			#region Photo 
			
			var imageVisible = false;
			
			// Photo
			if (!Utils.IsNull<int> (employee.PhotoFileID))
			{
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
					imagePhoto.AlternateText = fullname;
					imagePhoto.ToolTip = fullname;

					// make image visible
					imageVisible = true;
				}
			}

			imagePhoto.Visible = imageVisible;
			
			#endregion
		}

	} // class
} // namespace

