//
// DivisionController.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2014 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Linq;
using DotNetNuke.Collections;
using DotNetNuke.Data;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search;
using DotNetNuke.Services.Search.Entities;
using R7.University;

namespace R7.University.Division
{
	public partial class DivisionController : UniversityControllerBase, IPortable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="R7.University.Division.DivisionController"/> class.
		/// </summary>
		public DivisionController () : base ()
		{ 

		}

		#region ModuleSearchBase implementaion

		public override IList<SearchDocument> GetModifiedSearchDocuments (ModuleInfo modInfo, DateTime beginDate)
		{
			var searchDocs = new List<SearchDocument> ();
			var settings = new DivisionSettings (modInfo);
			var division = Get<DivisionInfo> (settings.DivisionID);
		
			if (division != null && division.LastModifiedOnDate.ToUniversalTime () > beginDate.ToUniversalTime ())
			{
				var aboutDivision = division.SearchDocumentText;
				var sd = new SearchDocument () {
					PortalId = modInfo.PortalID,
					AuthorUserId = division.LastModifiedByUserID,
					Title = division.Title,
					// Description = HtmlUtils.Shorten (aboutDivision, 255, "..."),
					Body = aboutDivision,
					ModifiedTimeUtc = division.LastModifiedOnDate.ToUniversalTime (),
					UniqueKey = string.Format ("University_Division_{0}", division.DivisionID),
                    Url = string.Format ("/Default.aspx?tabid={0}#{1}", modInfo.TabID, modInfo.ModuleID),
					IsActive = true // division.IsPublished
				};
	
				searchDocs.Add (sd);
			}
			
			return searchDocs;
		}

		#endregion

		#region IPortable members

		/// <summary>
		/// Exports a module to XML
		/// </summary>
		/// <param name="ModuleID">a module ID</param>
		/// <returns>XML string with module representation</returns>
		public string ExportModule (int moduleId)
		{
			var sb = new StringBuilder ();
			var infos = GetObjects<DivisionInfo> (moduleId);

            if (infos.Any ())
			{
				sb.Append ("<Divisions>");
				foreach (var info in infos)
				{
					sb.Append ("<Division>");
					sb.Append ("<content>");
					sb.Append (XmlUtils.XMLEncode (info.Title));
					sb.Append ("</content>");
					sb.Append ("</Division>");
				}
				sb.Append ("</Divisions>");
			}
			
			return sb.ToString ();
		}

		/// <summary>
		/// Imports a module from an XML
		/// </summary>
		/// <param name="ModuleID"></param>
		/// <param name="Content"></param>
		/// <param name="Version"></param>
		/// <param name="UserID"></param>
		public void ImportModule (int ModuleID, string Content, string Version, int UserID)
		{
			var infos = DotNetNuke.Common.Globals.GetContent (Content, "Divisions");
		
			foreach (XmlNode info in infos.SelectNodes("Division"))
			{
				var item = new DivisionInfo ();

				item.Title = info.SelectSingleNode ("content").InnerText;
				item.CreatedByUserID = UserID;

				Add<DivisionInfo> (item);
			}
		}

		#endregion
	}
}

