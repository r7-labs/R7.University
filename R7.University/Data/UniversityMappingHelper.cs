//
//  UniversityMappingHelper.cs
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

using System.Data.Entity.Infrastructure.Pluralization;
using DotNetNuke.Common.Utilities;

namespace R7.University.Data
{
    public static class UniversityMappingHelper
    {
        static readonly IPluralizationService pluralizationService = new EnglishPluralizationService ();

        public static string GetTableName<T> (bool pluralize = true) where T: class
        {
            return Config.GetObjectQualifer () + "University_" + Pluralize (typeof (T).Name.Replace ("Info", string.Empty), pluralize);
        }

        static string Pluralize (string word, bool pluralize)
        {
            return pluralize? pluralizationService.Pluralize (word) : word;
        }
    }
}
