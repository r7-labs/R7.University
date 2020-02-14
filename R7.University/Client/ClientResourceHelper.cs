//
//  ClientResourceHelper.cs
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

using System.Web.UI;
using DotNetNuke.Framework.JavaScriptLibraries;
using R7.Dnn.Extensions.Client;

namespace R7.University.Client
{
    public static class ClientResourceHelper
    {
        public static void RegisterSelect2 (Page page)
        {
            JavaScript.RequestRegistration ("Select2");

            var select2Library = JavaScriptLibraryHelper.GetHighestVersionLibrary ("Select2");
            if (select2Library != null) {
                JavaScriptLibraryHelper.RegisterStyleSheet (select2Library, page, "css/select2.min.css");
            }
        }
    }
}
