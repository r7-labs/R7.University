using System;
using System.Text;
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

namespace R7.University.Division
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
					var ctrl = new DivisionController ();

					var division_id = Request.QueryString["division_id"];
					if (!string.IsNullOrWhiteSpace(division_id))
					{
						var division = ctrl.Get<DivisionInfo> (int.Parse(division_id));
						if (division != null)
						{
							var vcard = division.VCard;

							Response.Clear();
							Response.ContentType = "text/x-vcard";
							Response.AddHeader("content-disposition", string.Format("attachment; filename=\"{0}.vcf\"", division.FileName));

							if (Request.Browser.Platform.ToUpperInvariant().StartsWith("WIN"))
							{
								// HACK: Windows russian version hack
								// TODO: Need a way to determine language / locale for division description
								Response.ContentEncoding = Encoding.GetEncoding(1251);
								vcard.Encoding = Response.ContentEncoding;
							}
							else 
								Response.ContentEncoding = Encoding.UTF8;

							Response.Write(vcard.ToString());
							Response.Flush();
							Response.Close();
						}
						else
							throw new Exception ("No division found with DivisionID=" + division_id);
					}
					else
						throw new Exception ("\"division_id\" query parameter should not be empty");
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

