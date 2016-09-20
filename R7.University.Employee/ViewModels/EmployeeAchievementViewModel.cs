//
//  EmployeeAchievementViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
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
using DotNetNuke.Services.Localization;
using R7.DotNetNuke.Extensions.Utilities;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Employee.ViewModels
{
    internal class EmployeeAchievementViewModel: EmployeeAchievementViewModelBase
    {
        public ViewModelContext Context { get; protected set; }

        public EmployeeAchievementViewModel (IEmployeeAchievement model, ViewModelContext context): base (model)
        {
            Context = context;
        }

        #region Bindable properties

        public string Title_String
        {
            get { return TextUtils.FormatList (" ", Title, TitleSuffix); }
        }

        public string Title_Link
        {
            get { 
                if (!string.IsNullOrWhiteSpace (Description)) {
                    return string.Format ("<a data-module-id=\"{2}\" "
                        + "data-description=\"{1}\" "
                        + "data-dialog-title=\"{0}\" "
                        + "onclick=\"showEmployeeAchievementDescriptionDialog(this)\">{0}</a>", 
                        Title_String, Description, Context.Module.ModuleId);
                }

                return Title_String;
            }
        }

        public string DocumentUrl_Link
        {
            get {
                if (!string.IsNullOrWhiteSpace (DocumentURL)) {
                    return string.Format ("<a href=\"{0}\" target=\"_blank\">{1}</a>", 
                        R7.University.Utilities.UrlUtils.LinkClickIdnHack (DocumentURL, Context.Module.TabId, Context.Module.ModuleId),
                        Localization.GetString ("DocumentUrl.Text",  Context.LocalResourceFile));
                }

                return string.Empty;
            }
        }

        public string Years_String
        {
            get {  
                return FormatHelper.FormatYears (Model.YearBegin, Model.YearEnd)
                    .Replace ("{ATM}", Localization.GetString ("AtTheMoment.Text", Context.LocalResourceFile));
            }
        }

        public string AchievementType_String
        {
            get {
                return Localization.GetString (
                    AchievementTypeInfo.GetResourceKey (AchievementType),
                    Context.LocalResourceFile);
            }
        }

        #endregion
    }
}

