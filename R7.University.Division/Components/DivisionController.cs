//
//  DivisionController.cs
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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Services.Search.Entities;
using R7.University.Data;

namespace R7.University.Division.Components
{
    public class DivisionController : ModuleSearchBase, IPortable
    {
        #region ModuleSearchBase implementaion

        public override IList<SearchDocument> GetModifiedSearchDocuments (ModuleInfo modInfo, DateTime beginDate)
        {
            var searchDocs = new List<SearchDocument> ();
            var settings = new DivisionSettings (modInfo);
            var division = UniversityRepository.Instance.DataProvider.Get<DivisionInfo> (settings.DivisionID);
		
            if (division != null && division.LastModifiedOnDate.ToUniversalTime () > beginDate.ToUniversalTime ()) {
                var aboutDivision = division.SearchDocumentText;
                var sd = new SearchDocument ()
                {
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
            var infos = UniversityRepository.Instance.DataProvider.GetObjects<DivisionInfo> (moduleId);

            if (infos.Any ()) {
                sb.Append ("<Divisions>");
                foreach (var info in infos) {
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
            var infos = Globals.GetContent (Content, "Divisions");
		
            foreach (XmlNode info in infos.SelectNodes("Division")) {
                var item = new DivisionInfo ();

                item.Title = info.SelectSingleNode ("content").InnerText;
                item.CreatedByUserID = UserID;

                UniversityRepository.Instance.DataProvider.Add<DivisionInfo> (item);
            }
        }

        #endregion
    }
}

