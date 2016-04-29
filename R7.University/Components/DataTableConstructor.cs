//
// DataTableConstructor.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015 
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

// based on: 
// http://www.c-sharpcorner.com/UploadFile/1a81c5/list-to-datatable-converter-using-C-Sharp/
// http://stackoverflow.com/questions/701223/net-convert-generic-collection-to-datatable

using System;
using System.Data;
using System.Reflection;
using System.Collections.Generic;

namespace R7.University
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

