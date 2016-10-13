//
//  XmlSerializationHelper.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
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

using System.IO;
using System.Xml.Serialization;

namespace R7.University.Utilities
{
    public static class XmlSerializationHelper
    {
        public static string Serialize<T> (T value) where T: class, new()
        {
            var xmlSerializer = new XmlSerializer (typeof (T));
            var stringWritter = new StringWriter ();
            xmlSerializer.Serialize (stringWritter, value);
            return stringWritter.ToString (); 
        }

        public static T Deserialize<T> (object value) where T: class, new()
        {
            if (value != null && !string.IsNullOrEmpty (value.ToString ())) {
                var xmlSerializer = new XmlSerializer (typeof (T));
                var stringReader = new StringReader (value.ToString ());
                return xmlSerializer.Deserialize (stringReader) as T;
            }

            return null;
        }
    }
}

