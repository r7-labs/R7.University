//
// EnumValueInfo.cs
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

namespace R7.University
{
    public class EnumValueInfo<T> where T: struct
    {
        #region Protected members

        protected event LocalizeHandler OnLocalize;

        #endregion

        public EnumValueInfo (T? value)
        {
            // where T: enum
            if (!typeof (T).IsEnum)
            {
                throw new NotSupportedException ("Type parameter of EnumValueInfo must be enum.");
            }

            Value = value;
        }

        #region Properties

        public T? Value { get; protected set; }

        public string LocalizedValue
        {
            get { return (OnLocalize != null) ? OnLocalize (ResourceKey) : ResourceKey; }
        }

        public string ResourceKey
        {
            get { return GetResourceKey (Value); } 
        }

        #endregion

        #region Static members

        public static List<EnumValueInfo<T>> GetLocalizedValues (LocalizeHandler localizeHandler, bool includeDefault)
        {
            var values = new List<EnumValueInfo<T>> ();

            if (includeDefault)
            {
                values.Add (new EnumValueInfo<T> (null));
            }

            foreach (T value in Enum.GetValues (typeof(T)))
            {   
                var v1 = new EnumValueInfo<T> (value);
                v1.OnLocalize = localizeHandler;
                values.Add (v1);
            }

            return values;
        }

        public static string GetResourceKey (T? value)
        {
            if (value != null)
                return value.GetType ().Name + "_" + value.Value + ".Text";

            return value.GetType ().Name + "_Default.Text";
        }

        #endregion
    }
}

