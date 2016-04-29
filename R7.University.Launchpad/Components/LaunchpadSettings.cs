//
// LaunchpadSettings.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2014-2016 Roman M. Yagodin
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
using DotNetNuke.UI.Modules;
using R7.DotNetNuke.Extensions.Modules;
using R7.DotNetNuke.Extensions.Utilities;

namespace R7.University.Launchpad
{
    /// <summary>
    /// Provides strong typed access to settings used by module
    /// </summary>
    public class LaunchpadSettings : SettingsWrapper
    {
        public LaunchpadSettings ()
        {
        }

        public LaunchpadSettings (IModuleControl module) : base (module)
        {
        }

        #region Properties for settings

        public int PageSize
        {
            get { return ReadSetting<int> ("Launchpad_PageSize", 20); }
            set { WriteTabModuleSetting<int> ("Launchpad_PageSize", value); }
        }

        private List<string> tables;

        public List<string> Tables
        {
            get { 
                if (tables == null) {
                    tables = new List<string> ();
                    tables.AddRange (
                        ReadSetting<string> ("Launchpad_Tables", string.Empty)
						.Split (new [] { ';' }, StringSplitOptions.RemoveEmptyEntries));
                }
				
                return tables;
            }
            set { 
                WriteTabModuleSetting<string> ("Launchpad_Tables", TextUtils.FormatList (";", value.ToArray ())); 
            }
        }

        #endregion
	
    }
}

