//
//  ModuleAuditControlExtensions.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2019 Roman M. Yagodin
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

using DotNetNuke.UI.UserControls;
using R7.University.Models;
using R7.Dnn.Extensions.Users;

namespace R7.University.ControlExtensions
{
    // TODO: Move to the base library
    public static class ModuleAuditControlExtensions
    {
        public static void Bind (this ModuleAuditControl auditControl, ITrackableEntity item, int portalId, string unknownUserName)
        {
            auditControl.CreatedDate = item.CreatedOnDate.ToLongDateString ();
            auditControl.CreatedByUser = UserHelper.GetUserDisplayName (portalId, item.CreatedByUserId) ?? unknownUserName;
            auditControl.LastModifiedDate = item.LastModifiedOnDate.ToLongDateString ();
            auditControl.LastModifiedByUser = UserHelper.GetUserDisplayName (portalId, item.LastModifiedByUserId) ?? unknownUserName;
        }
    }
}

