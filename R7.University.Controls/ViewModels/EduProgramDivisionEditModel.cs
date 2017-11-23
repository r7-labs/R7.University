//
//  EduProgramDivisionEditModel.cs
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
using R7.University.ModelExtensions;
using R7.University.Models;

namespace R7.University.Controls.ViewModels
{
    [Serializable]
    public class EduProgramDivisionEditModel: EditModelBase<EduProgramDivisionInfo>, IEduProgramDivisionWritable
    {
        #region EditModelBase implementation

        public override IEditModel<EduProgramDivisionInfo> Create (EduProgramDivisionInfo model, ViewModelContext context)
        {
            var viewModel = new EduProgramDivisionEditModel ();

            CopyCstor.Copy<IEduProgramDivisionWritable> (model, viewModel);
            viewModel.DivisionTitle = model.Division.Title;
            viewModel.StartDate = model.Division.StartDate;
            viewModel.EndDate = model.Division.EndDate;
            viewModel.Context = context;

            return viewModel;
        }

        public override EduProgramDivisionInfo CreateModel ()
        {   
            var epd = new EduProgramDivisionInfo ();
            CopyCstor.Copy<IEduProgramDivisionWritable> (this, epd);

            return epd;
        }

        public override void SetTargetItemId (int targetItemId, string targetItemKey)
        {
            this.SetModelId ((ModelType) Enum.Parse (typeof (ModelType), targetItemKey), targetItemId);
        }

        [JsonIgnore]
        public override bool IsPublished =>
            ModelHelper.IsPublished (HttpContext.Current.Timestamp, StartDate, EndDate);

        #endregion

        #region IEduProgramDivisionWritable implementation

        public long EduProgramDivisionId { get; set; }

        public int? EduProgramId { get; set; }

        public int? EduProgramProfileId { get; set; }

        public int DivisionId { get; set; }

        [JsonIgnore]
        public DivisionInfo Division { get; set; }

        public string DivisionRole { get; set; }

        #endregion

        #region External properties

        public string DivisionTitle { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        #endregion
    }
}
