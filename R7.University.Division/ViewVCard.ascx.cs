//
//  ViewVCard.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2016 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Text;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Exceptions;
using R7.University;
using R7.University.Data;

namespace R7.University.Division
{
	public class ViewVCard : PortalModuleBase
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
					var division_id = Request.QueryString ["division_id"];
					if (!string.IsNullOrWhiteSpace (division_id))
					{
                        var division = UniversityRepository.Instance.DataProvider.Get<DivisionInfo> (int.Parse (division_id));
						if (division != null)
						{
							var vcard = division.VCard;

							Response.Clear ();
							Response.ContentType = "text/x-vcard";
							Response.AddHeader ("content-disposition", string.Format ("attachment; filename=\"{0}.vcf\"", division.FileName));

							if (Request.Browser.Platform.ToUpperInvariant ().StartsWith ("WIN"))
							{
								// HACK: Windows russian version hack
								// TODO: Need a way to determine language / locale for division description
								Response.ContentEncoding = Encoding.GetEncoding (1251);
								vcard.Encoding = Response.ContentEncoding;
							}
							else
								Response.ContentEncoding = Encoding.UTF8;

							Response.Write (vcard.ToString ());
							Response.Flush ();
							Response.Close ();
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

	}
	// class
}
 // namespace

