//
//  EduProgramExtensions.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2018 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
//
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Common;
using DotNetNuke.Entities.Modules;
using DotNetNuke.Entities.Portals;
using R7.Dnn.Extensions.Text;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.ModelExtensions
{
    public static class EduProgramExtensions
    {
        // TODO: Extend IDocument instead, rename to WhereDocumentType
        public static IEnumerable<IDocument> GetDocumentsOfType (this IEduProgram eduProgram, SystemDocumentType documentType)
        {
            return eduProgram.Documents.Where (d => d.GetSystemDocumentType () == documentType);
        }

        public static string FormatTitle (this IEduProgram ep)
        {
            return UniversityFormatHelper.FormatEduProgramTitle (ep.Code, ep.Title);
        }

        public static string GetSearchUrl (this IEduProgram eduProgram, ModuleInfo module, PortalSettings portalSettings)
        {
            if (!string.IsNullOrEmpty (eduProgram.HomePage)) {
                return Globals.NavigateURL (int.Parse (eduProgram.HomePage), false, portalSettings, "",
                    portalSettings.PortalAlias.CultureCode);
            }

            return Globals.NavigateURL (module.TabID, false, portalSettings, "",
                portalSettings.PortalAlias.CultureCode, "", "mid", module.ModuleID.ToString ());
        }

        public static string SearchText (this IEduProgram eduProgram)
        {
            return eduProgram.EduProgramProfiles.Select (epp => epp.FormatTitle ()).JoinNotNullOrEmpty ("; ");
        }
    }
}
