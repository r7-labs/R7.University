//
//  LiquidLoop.cs
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

using System.Text.RegularExpressions;

namespace R7.University.Templates
{
    public class LiquidLoop
    {
        public string VariableName { get; set; }

        public string CollectionName { get; set; }

        public int NumOfRepeats { get; set; }

        public int Index { get; protected set; } = -1;

        public bool Next ()
        {
            if (Index < NumOfRepeats) {
                Index++;
            }

            if (Index < NumOfRepeats) {
                return true;
            }

            return false;
        }

        public static LiquidLoop Parse (string tag)
        {
            var match = Regex.Match (tag, @"for\s+([A-Za-z0-9_\.]+)\s+in\s+([A-Za-z0-9_\.]+)");
            if (match.Success) {
                if (match.Groups.Count >= 3) {
                    return new LiquidLoop {
                        VariableName = match.Groups [1].Value,
                        CollectionName = match.Groups [2].Value
                    };
                }
            }
            return null;
        }
    }
}
