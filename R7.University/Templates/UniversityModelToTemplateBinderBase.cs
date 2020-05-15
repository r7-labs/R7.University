//
//  UniversityModelToTemplateBinderBase.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2020 Roman M. Yagodin
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
using R7.University.Components;
using R7.University.Core.Templates;

namespace R7.University.Templates
{
    public abstract class UniversityModelToTemplateBinderBase: ModelToTemplateBinderBase
    {
        public override string Eval (string objectName)
        {
            var retValue = base.Eval (objectName);
            if (retValue != null) {
                return retValue;
            }

            if (objectName == "UniversityVersion") {
                return UniversityAssembly.SafeGetInformationalVersion (3);
            }
            if (objectName == "DataExportedAtDateTime") {
                return DateTime.Now.ToString ();
            }
            if (objectName == "DataExportedByUserName") {
                var user = UserController.Instance.GetCurrentUserInfo ();
                if (user != null) {
                    return user.Username;
                }
            }

            return null;
        }
    }
}
