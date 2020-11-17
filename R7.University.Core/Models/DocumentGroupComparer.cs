//
//  DocumentGroupComparer.cs
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
using System.Collections.Generic;

namespace R7.University.Models
{
    public class DocumentGroupComparer : IComparer<string>
    {
        #region IComparer implementation

        public int Compare (string x, string y)
        {
            var xne = string.IsNullOrEmpty (x);
            var yne = string.IsNullOrEmpty (y);

            if (!xne && !yne) {
                return x.CompareTo (y);
            }

            if (!xne && yne) {
                return -1;
            }

            if (xne && !yne) {
                return 1;
            }

            return 0;
        }

        public int GetHashCode (string obj)
        {
            return obj.GetHashCode ();
        }

        #endregion

        #region Singleton

        static readonly Lazy<DocumentGroupComparer> _instance = new Lazy<DocumentGroupComparer> ();

        public static DocumentGroupComparer Instance => _instance.Value;

        #endregion
    }
}
