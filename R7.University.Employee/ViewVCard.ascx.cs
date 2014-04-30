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
	public partial class ViewVCard : PortalModuleBase
	{
		#region Handlers

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

					var employee_id = Request.QueryString["employee_id"];
					if (!string.IsNullOrWhiteSpace(employee_id))
					{
						var employee = ctrl.Get<EmployeeInfo> (int.Parse(employee_id));
						if (employee != null)
						{

							Response.Clear();
							Response.ContentType = "text/x-vcard";
							Response.Write(employee.VCard.ToString());
							Response.Flush();
							Response.Close();

							// TODO: Add filename to vCard
							/* Attachment filename example
							var attachmentFilename = strOriginalFileName;

							if (Request.Browser.Browser.Contains("MSIE") || Request.Browser.Browser.StartsWith("IE"))
							{
								attachmentFilename = Server.UrlEncode(attachmentFilename);
								if (!string.IsNullOrEmpty(attachmentFilename)) 
									attachmentFilename = attachmentFilename.Replace("+", "%20");
							}   
							Response.AddHeader("content-disposition", string.Format("attachment; filename=\"{0}\"", attachmentFilename));
		                    Response.ContentEncoding = System.Text.Encoding.UTF8;
		                    Response.ContentType = Utils.GetMimeType(Path.GetExtension(strPath).ToLower());*/
						}
						else
							throw new Exception ("No employee found with EmployeeID=" + employee_id);
					}
				} 
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		#endregion

	} // class
} // namespace

