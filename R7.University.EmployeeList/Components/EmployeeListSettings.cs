//
// EmployeeListSettings.cs
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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Modules;
using DotNetNuke.UI.Modules;
using R7.DotNetNuke.Extensions.Modules;
using R7.University.Components;

namespace R7.University.EmployeeList.Components
{
    /// <summary>
    /// Provides strong typed access to settings used by module
    /// </summary>
    public class EmployeeListSettings: SettingsWrapper
    {
        public EmployeeListSettings ()
        {
        }

        public EmployeeListSettings (IModuleControl module) : base (module)
        {
        }

        public EmployeeListSettings (ModuleInfo module) : base (module)
        {
        }

        #region Properties for settings

        // cached DivisionID value for frequent use
        private int? divisionId = null;

        /// <summary>
        /// Division's ID to show employees from
        /// </summary>
        public int DivisionID
        {
            get { 
                if (divisionId == null)
                    divisionId = ReadSetting<int> ("EmployeeList_DivisionID", Null.NullInteger);

                return divisionId.Value;
            }
            set { 
                WriteModuleSetting<int> ("EmployeeList_DivisionID", value);
                divisionId = value;
            }
        }

        /// <summary>
        /// Indicates employee info extraction method
        /// </summary>
        /// <value><c>true</c> if resursively include employees from subordinate divisions; otherwise, <c>false</c>.</value>
        public bool IncludeSubdivisions
        {
            get { return ReadSetting<bool> ("EmployeeList_IncludeSubdivisions", false); }
            set { WriteModuleSetting<bool> ("EmployeeList_IncludeSubdivisions", value); }
        }

        /// <summary>
        /// Gets or sets the type of the sort.
        /// </summary>
        /// <value>The type of the sort.</value>
        public int SortType
        {
            get { return ReadSetting<int> ("EmployeeList_SortType", 0); }
            set { WriteTabModuleSetting<int> ("EmployeeList_SortType", value); }
        }

        public int PhotoWidth
        {
			get { return ReadSetting<int> ("EmployeeList_PhotoWidth", UniversityConfig.Instance.EmployeePhoto.SquareDefaultWidth); }
            set { WriteTabModuleSetting<int> ("EmployeeList_PhotoWidth", value); }
        }

        private int? dataCacheTime;

        public int DataCacheTime
        {
            get { 
                if (dataCacheTime == null)
                    dataCacheTime = ReadSetting<int> ("EmployeeList_DataCacheTime", 1200);
				
                return dataCacheTime.Value;
            }
            set { 
                WriteTabModuleSetting<int> ("EmployeeList_DataCacheTime", value); 
                dataCacheTime = value;
            }
        }

        #endregion
    }
}

