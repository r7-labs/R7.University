//
//  UniversityModuleSettingsBase.cs
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
using System.Web.UI.WebControls;
using R7.DotNetNuke.Extensions.Modules;
using R7.University.Security;

namespace R7.University.Modules
{
    public class UniversityModuleSettingsBase<TSettings>: ModuleSettingsBase<TSettings>
        where TSettings: class, new ()
    {
        #region Controls

        protected Panel panelGeneralSettings;

        #endregion

        #region Properties

        IModuleSecurityContext securityContext;
        protected IModuleSecurityContext SecurityContext
        {
            get { return securityContext ?? (securityContext = new ModuleSecurityContext (UserInfo, this)); }
        }

        #endregion

        protected override void OnInit (EventArgs e)
        {
            base.OnInit (e);

            if (panelGeneralSettings != null) {
                panelGeneralSettings.Visible = SecurityContext.CanManageModule ();
            }
        }
    }
}
