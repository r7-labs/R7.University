//
// EnumViewModel.cs
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

using System;
using System.Collections.Generic;
using DotNetNuke.Services.Localization;

namespace R7.University
{
    public class EnumViewModel<T> where T: struct
    {
        #region Protected members

        protected ViewModelContext Context { get; set; }

        #endregion

        public EnumViewModel (T? value)
        {
            // where T: enum
            if (!typeof (T).IsEnum)
            {
                throw new NotSupportedException ("Type parameter of EnumViewModel must be enum.");
            }

            Value = value;
        }

        #region Public properties

        public T? Value { get; protected set; }

        public string ValueLocalized
        {
            get { return Localization.GetString (ValueResourceKey, Context.LocalResourceFile); }
        }

        public string ValueResourceKey
        {
            get { return GetValueResourceKey (Value); } 
        }

        #endregion

        #region Static members

        public static List<EnumViewModel<T>> GetValues (ViewModelContext context, bool includeDefault)
        {
            var values = new List<EnumViewModel<T>> ();

            if (includeDefault)
            {
                var v1 = new EnumViewModel<T> (null);
                v1.Context = context;
                values.Add (v1);
            }

            foreach (T value in Enum.GetValues (typeof (T)))
            {   
                var v1 = new EnumViewModel<T> (value);
                v1.Context = context;
                values.Add (v1);
            }

            return values;
        }

        public static string GetValueResourceKey (T? value)
        {
            if (value != null)
            {
                return typeof (T).Name + "_" + value.Value + ".Text";
            }

            return typeof (T).Name + "_Default.Text";
        }

        #endregion
    }
}

