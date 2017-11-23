//
//  EmployeeDisciplineViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017 Roman M. Yagodin
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
using System.Web;
using Newtonsoft.Json;
using R7.Dnn.Extensions.Utilities;
using R7.Dnn.Extensions.ViewModels;
using R7.University.EditModels;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Controls.ViewModels
{
    [Serializable]
    public class EmployeeDisciplineEditModel : EditModelBase<EmployeeDisciplineInfo>, IEmployeeDisciplineWritable
    {
        #region EditModelBase implementation

        [JsonIgnore]
        public override bool IsPublished => 
            ModelHelper.IsPublished (HttpContext.Current.Timestamp, ProfileStartDate, ProfileEndDate);
      
        public override IEditModel<EmployeeDisciplineInfo> Create (EmployeeDisciplineInfo model, ViewModelContext context)
        {
            var viewModel = new EmployeeDisciplineEditModel ();
            CopyCstor.Copy<IEmployeeDisciplineWritable> (model, viewModel);

            viewModel.Code = model.EduProgramProfile.EduProgram.Code;
            viewModel.Title = model.EduProgramProfile.EduProgram.Title;
            viewModel.ProfileCode = model.EduProgramProfile.ProfileCode;
            viewModel.ProfileTitle = model.EduProgramProfile.ProfileTitle;
            viewModel.ProfileStartDate = model.EduProgramProfile.StartDate;
            viewModel.ProfileEndDate = model.EduProgramProfile.EndDate;
            viewModel.EduLevelString = FormatHelper.FormatShortTitle (model.EduProgramProfile.EduLevel.ShortTitle, model.EduProgramProfile.EduLevel.Title);

            return viewModel;
        }

        public override EmployeeDisciplineInfo CreateModel ()
        {
            var model = new EmployeeDisciplineInfo ();
            CopyCstor.Copy<IEmployeeDisciplineWritable> (this, model);
            return model;
        }

        public override void SetTargetItemId (int targetItemId, string targetItemKey)
        {
            EmployeeID = targetItemId;
        }

        #endregion

        #region IEmployeeDisciplineWritable implementation

        public long EmployeeDisciplineID { get; set; }

        public int EmployeeID { get; set; }

        public int EduProgramProfileID { get; set; }

        public string Disciplines { get; set; }

        [JsonIgnore]
        [Obsolete]
        public EmployeeInfo Employee { get; set; }

        [JsonIgnore]
        [Obsolete]
        public EduProgramProfileInfo EduProgramProfile { get; set; }

        #endregion

        #region External properties

        public string Code { get; set; }

        public string Title { get; set; }

        public string ProfileCode { get; set; }

        public string ProfileTitle { get; set; }

        public DateTime? ProfileStartDate { get; set; }

        public DateTime? ProfileEndDate { get; set; }

        public string EduLevelString { get; set; }

        #endregion

        #region Bindable properties

        [JsonIgnore]
        public string EduProgramProfileString
        {
            get { return FormatHelper.FormatEduProgramProfileTitle (Code, Title, ProfileCode, ProfileTitle); }
        }

        #endregion
    }
}
