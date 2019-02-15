//
//  OccupiedPositionEditModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017-2018 Roman M. Yagodin
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
using R7.Dnn.Extensions.Models;
using R7.Dnn.Extensions.Utilities;
using R7.Dnn.Extensions.ViewModels;
using R7.University.EditModels;
using R7.University.Models;

namespace R7.University.Controls.EditModels
{
    [Serializable]
    public class OccupiedPositionEditModel : EditModelBase<OccupiedPositionInfo>, IOccupiedPositionWritable
    {
        #region EditModelBase implementation

        public override IEditModel<OccupiedPositionInfo> Create (OccupiedPositionInfo model, ViewModelContext context)
        {
            var editModel = CopyCstor.New<OccupiedPositionEditModel, IOccupiedPositionWritable> (model);
            editModel.Context = context;
            editModel.PositionTitle = model.Position.Title;
            editModel.DivisionTitle = model.Division.Title;
            editModel.DivisionStartDate = model.Division.StartDate;
            editModel.DivisionEndDate = model.Division.EndDate;

            return editModel;
        }

        public override OccupiedPositionInfo CreateModel ()
        {
            return CopyCstor.New<OccupiedPositionInfo, IOccupiedPositionWritable> (this);
        }

        public override void SetTargetItemId (int targetItemId, string targetItemKey)
        {
            EmployeeID = targetItemId;
        }

        public override bool IsPublished => ModelHelper.IsPublished (HttpContext.Current.Timestamp, DivisionStartDate, DivisionEndDate);

        #endregion

        #region IOccupiedPositionWritable implementation

        public int OccupiedPositionID { get; set; }

        [JsonIgnore]
        [Obsolete]
        public IDivision Division { get; set; }

        public int DivisionID { get; set; }

        [JsonIgnore]
        [Obsolete]
        public IEmployee Employee { get; set; }

        public int EmployeeID { get; set; }

        public bool IsPrime { get; set; }

        [JsonIgnore]
        [Obsolete]
        public IPosition Position { get; set; }
       
        public int PositionID { get; set; }

        public string TitleSuffix { get; set; }

        #endregion

        #region External properties

        public string PositionTitle { get; set; }

        public string DivisionTitle { get; set; }

        public DateTime? DivisionStartDate { get; set; }

        public DateTime? DivisionEndDate { get; set; }

        #endregion

        #region Bindable properties

        public string PositionTitleWithSuffix => PositionTitle + " " + TitleSuffix;

        #endregion
    }
}
