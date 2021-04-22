using System;
using System.Web;
using Newtonsoft.Json;
using R7.Dnn.Extensions.Models;
using R7.Dnn.Extensions.ViewModels;
using R7.University.EditModels;
using R7.University.Models;

namespace R7.University.Controls.EditModels
{
    [Serializable]
    public class OccupiedPositionEditModel : EditModelBase<OccupiedPositionInfo>, IOccupiedPositionWritable
    {
        #region EditModelBase implementation

        public override IEditModel<OccupiedPositionInfo> Create (OccupiedPositionInfo model, ViewModelContext dnn)
        {
            var editModel = CopyCstor.New<OccupiedPositionEditModel, IOccupiedPositionWritable> (model);
            editModel.Dnn = dnn;
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
        [Obsolete ("Don't use this property directly", true)]
        public IDivision Division { get; set; }

        public int DivisionID { get; set; }

        [JsonIgnore]
        [Obsolete ("Don't use this property directly", true)]
        public IEmployee Employee { get; set; }

        public int EmployeeID { get; set; }

        public bool IsPrime { get; set; }

        [JsonIgnore]
        [Obsolete ("Don't use this property directly", true)]
        public IPosition Position { get; set; }

        public int PositionID { get; set; }

        public string TitleSuffix { get; set; }

        #endregion

        #region Flattened external properties

        public string PositionTitle { get; set; }

        public string DivisionTitle { get; set; }

        public DateTime? DivisionStartDate { get; set; }

        public DateTime? DivisionEndDate { get; set; }

        #endregion

        #region Derieved properties

        public string PositionTitleWithSuffix => PositionTitle + " " + TitleSuffix;

        #endregion
    }
}
