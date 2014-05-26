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

			linkReturn.Attributes.Add ("onclick", "javascript:return " +
			UrlUtils.ClosePopUp (refresh: false, url: "", onClickEvent: true));

			linkVCard.NavigateUrl = Utils.EditUrl (this, "VCard", "employee_id", EmployeeID.ToString ()); 
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
				
				} // if (!IsPostBack)
			}
			catch (Exception ex)
			{
				Exceptions.ProcessModuleLoadException (this, ex);
			}
		}

		#endregion

	} // class
} // namespace

