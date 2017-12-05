//
//  AgplSignatureViewModel.cs
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
using System.Reflection;
using R7.University.Components;

namespace R7.University.Controls.ViewModels
{
    public class AgplSignatureViewModel
    {
        public bool ShowRule { get; set; } = true;

        public virtual Assembly CoreAssembly => UniversityAssembly.GetCoreAssembly ();

        public virtual string Name => CoreAssembly.GetName ().Name;

        public virtual Version Version => CoreAssembly.GetName ().Version;

        public virtual string InformationalVersion =>
            CoreAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute> ()?.InformationalVersion;
    }
}
