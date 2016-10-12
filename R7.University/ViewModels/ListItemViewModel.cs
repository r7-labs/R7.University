//
//  ListItemViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
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

namespace R7.University.ViewModels
{
    // TODO: Move to the base library
    public class ListItemViewModel
    {
        public ListItemViewModel (string value, string text)
        {
            Value = value;
            Text = text;
        }

        public ListItemViewModel (object value, string text)
        {
            Value = value.ToString ();
            Text = text;
        }

        public string Value { get; protected set; }

        public string Text { get; protected set; }
    }
}
