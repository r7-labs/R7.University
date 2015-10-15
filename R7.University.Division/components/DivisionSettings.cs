//
// DivisionSettings.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2014 
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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.UI.Modules;
using DotNetNuke.R7;
using R7.University;

namespace R7.University.Division
{
	/// <summary>
	/// Provides strong typed access to settings used by module
	/// </summary>
	public class DivisionSettings : SettingsWrapper
	{
		public DivisionSettings (IModuleControl module) : base (module)
		{
		}

		public DivisionSettings (ModuleInfo module) : base (module)
		{
		}

		#region Properties for settings

		private int? divisionId;

		/// <summary>
		/// Division ID
		/// </summary>
		public int DivisionID
		{
			get
			{
				if (divisionId == null)
					divisionId = ReadSetting<int> ("Division_DivisionID", Null.NullInteger); 
			
				return divisionId.Value;
			}
			set
			{ 
				WriteModuleSetting<int> ("Division_DivisionID", value); 
				divisionId = value;
			}
		}

		/// <summary>
		/// Gets or sets the width of the bar code.
		/// </summary>
		/// <value>The width of the bar code.</value>
		public int BarcodeWidth
		{
			get { return ReadSetting<int> ("Division_BarcodeWidth", 120); }
			set { WriteTabModuleSetting<int> ("Division_BarcodeWidth", value); }
		}

		#endregion
	}
}

