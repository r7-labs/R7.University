//
// EmployeeSettings.cs
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

namespace R7.University.Employee.Components
{
    /// <summary>
    /// Provides strong typed access to settings used by module
    /// </summary>
    public class EmployeeSettings : SettingsWrapper
    {
        public EmployeeSettings ()
        {
        }

        public EmployeeSettings (IModuleControl module) : base (module)
        {
        }

        public EmployeeSettings (ModuleInfo module) : base (module)
        {
        }

        #region Properties for settings

        private int? employeeId;

        /// <summary>
        /// Gets or sets the EmployeeID setting value. 
        /// Use <see cref="EmployeePortalModuleBase.GetEmployee()"/> 
        /// and <see cref="EmployeePortalModuleBase.GetEmployeeId()"/>
        /// to get employee info in the view contols.
        /// </summary>
        /// <value>The employee Id.</value>
        public int EmployeeID
        {
            get { 
                if (employeeId == null)
                    employeeId = ReadSetting<int> ("Employee_EmployeeID", Null.NullInteger); 
				
                return employeeId.Value;
            }
            set { 
                WriteModuleSetting<int> ("Employee_EmployeeID", value); 
                employeeId = value;
            }
        }

        public bool ShowCurrentUser
        {
            get { return ReadSetting<bool> ("Employee_ShowCurrentUser", false); }
            set { WriteModuleSetting<bool> ("Employee_ShowCurrentUser", value); }
        }

        public bool AutoTitle
        {
            get { return ReadSetting<bool> ("Employee_AutoTitle", true); }
            set { WriteModuleSetting<bool> ("Employee_AutoTitle", value); }
        }

        public int PhotoWidth
        {
			get { return ReadSetting<int> ("Employee_PhotoWidth", UniversityConfig.Instance.EmployeePhoto.DefaultWidth); }
            set { WriteTabModuleSetting<int> ("Employee_PhotoWidth", value); }
        }

        private int? dataCacheTime;

        public int DataCacheTime
        {
            get { 
                if (dataCacheTime == null)
                    dataCacheTime = ReadSetting<int> ("Employee_DataCacheTime", 1200);
				
                return dataCacheTime.Value;
            }
            set { 
                WriteTabModuleSetting<int> ("Employee_DataCacheTime", value); 
                dataCacheTime = value;
            }
        }

        #endregion
    }
}

