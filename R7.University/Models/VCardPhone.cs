//
//  VCardPhone.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014 Roman M. Yagodin
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

namespace R7.University.Models
{
    [Flags]
    public enum VCardPhoneType
    {
        None = 0,
        Home = 1,
        Msg = 2,
        Work = 4,
        Pref = 8,
        Voice = 16,
        Fax = 32,
        Cell = 64,
        Video = 128,
        Pager = 256,
        Bbs = 512,
        Modem = 1024,
        Car = 2048,
        Isdn = 4096,
        Pcs = 8192
    }

    public class VCardPhone
    {
        public string Number { get; set; }

        public VCardPhoneType Type { get; set; }
    }
}
