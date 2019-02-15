//
//  ModuleSecurityContext.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017 Roman M. Yagodin
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
using DotNetNuke.Entities.Users;
using DotNetNuke.UI.Modules;
using R7.University.Models;

namespace R7.University.Security
{
    public class ModuleSecurityContext : IModuleSecurityContext
    {
        public int UserId { get; protected set; }

        public bool IsAdmin { get; protected set; }

        public IModuleControl Module { get; protected set; }

        public ModuleSecurityContext (UserInfo user)
        {
            UserId = user.UserID;
            IsAdmin = user.IsSuperUser || user.IsInRole ("Administrators");
        }

        public ModuleSecurityContext (UserInfo user, IModuleControl module): this (user)
        {
            Module = module;
        }

        public bool CanAdd (Type entityType)
        {
            if (typeof (ITrackableEntity).IsAssignableFrom (entityType)) {
                return IsAdmin;
            }

            return true;
        }

        public bool CanDelete<TEntity> (TEntity entity) where TEntity : class
        {
            var canDelete = true;

            if (entity is ITrackableEntity) {
                canDelete &= IsAdmin;
            }

            if (entity is ISystemEntity) {
                canDelete &= !((ISystemEntity) entity).IsSystem;
            }

            // TODO: Remove this check after splitting these entities
            if (entity is IEduVolume || entity is IContingent) {
                canDelete &= IsAdmin;
            }

            return canDelete;
        }

        public bool CanUpdate<TEntity> (TEntity entity) where TEntity : class
        {
            var canUpdate = true;

            if (entity is ISystemEntity) {
                canUpdate &= !((ISystemEntity) entity).IsSystem;
            }

            return canUpdate;
        }

        public bool CanManageModule ()
        {
            return IsAdmin;
        }
    }
}
