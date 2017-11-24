//
//  AgplSignature.ascx.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2017 Roman M. Yagodin
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
using System.Reflection;
using System.Web.UI;
using DotNetNuke.Services.Localization;
using R7.University.Components;
using DnnWebUiUtilities = DotNetNuke.Web.UI.Utilities;

namespace R7.University.Controls
{
    public class AgplSignature: UserControl
    {
        bool showRule = true;
        public bool ShowRule
        {
            get { return showRule; }
            set { showRule = value; }
        }

        string localResourceFile;
        protected string LocalResourceFile
        {
            get {
                if (localResourceFile == null) {
                    localResourceFile = DnnWebUiUtilities.GetLocalResourceFile (this);
                }

                return localResourceFile;
            }
        }

        protected string LocalizeString (string key)
        {
            return Localization.GetString (key, LocalResourceFile);
        }

        protected virtual Assembly CoreAssembly => UniversityAssembly.GetCoreAssembly ();

        protected virtual string Name => CoreAssembly.GetName ().Name;

        protected virtual Version Version => CoreAssembly.GetName ().Version;

        protected virtual string InformationalVersion =>
            CoreAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute> ()?.InformationalVersion;
    }
}
