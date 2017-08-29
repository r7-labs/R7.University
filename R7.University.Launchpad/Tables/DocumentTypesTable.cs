//
//  DocumentTypesTable.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2017 Roman M. Yagodin
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

using System.Data;
using DotNetNuke.Entities.Modules;
using R7.University.Launchpad.Components;
using R7.University.Models;
using R7.University.Queries;

namespace R7.University.Launchpad
{
    public class DocumentTypesTable: LaunchpadTableBase
    {
        public DocumentTypesTable () : base (typeof (DocumentTypeInfo))
        {
        }

        public override DataTable GetDataTable (PortalModuleBase module, UniversityModelContext modelContext, string search)
        {
            // FIXME: Cannot set comparison options
            var documentTypes = (search == null)
                ? new FlatQuery<DocumentTypeInfo> (modelContext).List ()
                : new FlatQuery<DocumentTypeInfo> (modelContext)
                    .ListWhere (dt => dt.Type.Contains (search) || dt.Description.Contains (search));

            return DataTableConstructor.FromIEnumerable (documentTypes);
        }
    }
}
