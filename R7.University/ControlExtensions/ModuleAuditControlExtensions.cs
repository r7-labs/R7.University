//
//  ModuleAuditControlExtensions.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
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

using System;
using DotNetNuke.UI.UserControls;
using DotNetNuke.Common.Utilities;
using R7.University.Utilities;
using R7.University.Models;

namespace R7.University.ControlExtensions
{
    public static class ModuleAuditControlExtensions
    {
        public static void Bind (this ModuleAuditControl auditControl, IUniversityBaseEntity item)
        {
            auditControl.CreatedDate = item.CreatedOnDate.ToLongDateString ();
            auditControl.CreatedByUser = Utils.GetUserDisplayName (item.CreatedByUserID, Null.NullInteger.ToString ());
            auditControl.LastModifiedDate = item.LastModifiedOnDate.ToLongDateString ();
            auditControl.LastModifiedByUser = Utils.GetUserDisplayName (
                item.LastModifiedByUserID,
                Null.NullInteger.ToString ());
        }
    }
}

