//
// EmployeeModuleBase.cs
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
using DotNetNuke.Entities.Modules;
using DotNetNuke.R7;

namespace R7.University.Employee
{
	/// <summary>
	/// Employee module base.
	/// </summary>
	public class EmployeePortalModuleBase : PortalModuleBase
	{
		private EmployeeController ctrl = null;

		protected EmployeeController EmployeeController
		{
			get { return ctrl ?? (ctrl = new EmployeeController ()); }
		}

		private EmployeeSettings settings = null;

		protected EmployeeSettings EmployeeSettings
		{
			get { return settings ?? (settings = new EmployeeSettings (this)); }
		}

        protected void UpdateModuleTitle (string title)
        {
            // replace module title
            var mctrl = new ModuleController ();
            var module = mctrl.GetModule (ModuleId);

            if (module.ModuleTitle != title)
            {
                module.ModuleTitle = title;
                mctrl.UpdateModule (module);
            }
        }

		#region Methods to get associated employee

        protected EmployeeInfo GetEmployee ()
        {
            if (EmployeeSettings.ShowCurrentUser)
            {
                var userId = TypeUtils.ParseToNullable<int> (Request.QueryString ["userid"]);
                if (userId != null)
                    return EmployeeController.GetEmployeeByUserId (userId.Value);
            }

            return EmployeeController.Get<EmployeeInfo> (EmployeeSettings.EmployeeID);
        }

		#endregion
	}

	/// <summary>
	/// Employee module settings base.
	/// </summary>
	public class EmployeeModuleSettingsBase : ModuleSettingsBase
	{
		private EmployeeController ctrl = null;

		protected EmployeeController EmployeeController
		{
			get { return ctrl ?? (ctrl = new EmployeeController ()); }
		}

		private EmployeeSettings settings = null;

		protected EmployeeSettings EmployeeSettings
		{
			get { return settings ?? (settings = new EmployeeSettings (this)); }
		}
	}
}
