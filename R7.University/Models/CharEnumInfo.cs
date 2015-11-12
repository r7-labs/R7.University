//
// CharEnumInfo.cs
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
    // TODO: Replace with EnumValueInfo class
    public class CharEnumInfo<T> where T: struct
	{
        public CharEnumInfo (T type)
        {
            // means: where T: enum
            if (!typeof (T).IsEnum) throw new NotSupportedException ();

            Type = type;
        }

		public T Type { get; set; }

        /*public string TypeString 
        {
            get { return Type.ToString () [0].ToString (); }
        }
        */

		public string LocalizedType
		{
			get { return OnLocalize != null ? OnLocalize (ResourceKey) : ResourceKey; }
		}

		#region Private members

		private event LocalizeHandler OnLocalize;

		public string ResourceKey
		{
            get { return GetResourceKey (Type); } 
		}

		#endregion

		#region Static members

        public static List<CharEnumInfo<T>> GetLocalizedTypes (LocalizeHandler localizeHandler)
		{
            var types = new List<CharEnumInfo<T>> ();
			foreach (T type1 in Enum.GetValues(typeof(T)))
			{   
                var type = new CharEnumInfo<T>(type1);
				type.OnLocalize = localizeHandler;
				types.Add (type);
			}

			return types;
		}

		public static string GetResourceKey (T? type)
		{
			if (type != null)
                return type.GetType ().Name + type.Value + ".Text";
		
            return type.GetType ().Name + ".Text";
		}

		#endregion
	}
}

