//
//  DataTableConstructor.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Based on: 
//      http://www.c-sharpcorner.com/UploadFile/1a81c5/list-to-datatable-converter-using-C-Sharp/
//      http://stackoverflow.com/questions/701223/net-convert-generic-collection-to-datatable
//  
//  Copyright (c) 2015 Roman M. Yagodin
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
using System.Data;
using System.Reflection;
using System.Collections.Generic;

namespace R7.University.Components
{
    public static class DataTableConstructor
    {
        public static DataTable FromIEnumerable<T> (IEnumerable<T> items)
        {
            var dataTable = new DataTable (typeof (T).Name);

            // get all the properties
            var props = typeof (T).GetProperties (BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in props) {
                dataTable.Columns.Add (prop.Name, Nullable.GetUnderlyingType (
                        prop.PropertyType) ?? prop.PropertyType);
            }

            foreach (T item in items) {
                var values = new object [props.Length];

                for (var i = 0; i < props.Length; i++) {
                    // inserting property values to datatable rows
                    values [i] = props [i].GetValue (item, null) ?? DBNull.Value;
                }

                dataTable.Rows.Add (values);
            }

            return dataTable;
        }
    }
}

