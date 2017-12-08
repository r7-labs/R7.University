//
//  EduProgramProfileFormYearExtensions.cs
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
using R7.University.Models;

namespace R7.University.ModelExtensions
{
    public static class EduProgramProfileFormYearExtensions
    {
        struct IntPair: IEquatable<IntPair>
        {
            public readonly int x;
            public readonly int y;

            public IntPair (int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public bool Equals (IntPair other)
            {
                return x == other.x && y == other.y;
            }

            public override int GetHashCode ()
            {
                unchecked {
                    var hash = 17;
                    hash = hash * 23 + x;
                    hash = hash * 23 + y;
                    return hash;
                }
            }
        }

        public static IEnumerable<EduProgramProfileFormYearInfo> LastYearOnly (this IEnumerable<EduProgramProfileFormYearInfo> eppfys)
        {
            var profileForms = new Dictionary<IntPair,int> ();
            int yearId;
            foreach (var eppfy in eppfys) {
                var key = new IntPair (eppfy.EduProgramProfileId, eppfy.EduFormId);
                if (!profileForms.TryGetValue (key, out yearId)) {
                    profileForms.Add (key, eppfy.YearId);
                    yield return eppfy;
                }
            }
        }
    }
}
