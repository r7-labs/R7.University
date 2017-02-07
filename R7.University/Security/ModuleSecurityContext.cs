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

using DotNetNuke.Entities.Users;
using R7.University.Models;

namespace R7.University.Security
{
    public class ModuleSecurityContext : ISecurityContext
    {
        public bool IsAdmin { get; protected set; }

        public ModuleSecurityContext (UserInfo user)
        {
            IsAdmin = user.IsSuperUser || user.IsInRole ("Administrators");
        }

        public bool CanAdd<TEntity> () where TEntity : class
        {
            if (typeof (TEntity) == typeof (DivisionInfo)
                || typeof (TEntity) == typeof (EmployeeInfo)
                || typeof (TEntity) == typeof (EduProgramInfo)
                || typeof (TEntity) == typeof (EduProgramProfileInfo)) {
                return IsAdmin;
            }

            return true;
        }

        public bool CanDelete<TEntity> (TEntity entity) where TEntity : class
        {
            if (entity is DivisionInfo
                || entity is EmployeeInfo
                || entity is EduProgramInfo
                || entity is EduProgramProfileInfo) {
                return IsAdmin;
            }

            return true;
        }
    }
}
