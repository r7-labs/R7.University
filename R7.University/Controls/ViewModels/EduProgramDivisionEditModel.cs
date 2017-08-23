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
using System.Xml.Serialization;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Components;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Controls.ViewModels
{
    [Serializable]
    public class EduProgramDivisionEditModel: IEduProgramDivisionWritable, IEditControlViewModel<EduProgramDivisionInfo>
    {
        public IEditControlViewModel<EduProgramDivisionInfo> Create (EduProgramDivisionInfo model, ViewModelContext context)
        {
            var viewModel = new EduProgramDivisionEditModel ();

            CopyCstor.Copy<IEduProgramDivisionWritable> (model, viewModel);
            viewModel.DivisionTitle = model.Division.Title;
            viewModel.StartDate = model.Division.StartDate;
            viewModel.EndDate = model.Division.EndDate;
            viewModel.Context = context;

            return viewModel;
        }

        public EduProgramDivisionInfo CreateModel ()
        {   
            var epd = new EduProgramDivisionInfo ();
            CopyCstor.Copy<IEduProgramDivisionWritable> (this, epd);

            return epd;
        }

        public void SetTargetItemId (int targetItemId, string targetItemKey)
        {
            this.SetModelId ((ModelType) Enum.Parse (typeof (ModelType), targetItemKey), targetItemId);
        }

        [XmlIgnore]
        public ViewModelContext Context { get; set; }

        [XmlIgnore]
        public string CssClass {
            get {
                var cssClass = string.Empty;
                if (!ModelHelper.IsPublished (HttpContext.Current.Timestamp, StartDate, EndDate)) {
                    cssClass += " u8y-not-published";
                }

                if (EditState == ModelEditState.Deleted) {
                    cssClass += " u8y-deleted";
                } else if (EditState == ModelEditState.Added) {
                    cssClass += " u8y-added";
                } else if (EditState == ModelEditState.Modified) {
                    cssClass += " u8y-updated";
                }

                return cssClass.TrimStart ();
            }
        }

        ModelEditState _editState;
        ModelEditState _prevEditState;
        public ModelEditState EditState {
            get { return _editState; }
            set { _prevEditState = _editState; _editState = value; }
        }

        public void RestoreEditState ()
        {
            _editState = _prevEditState;
        }

        public int ViewItemID { get; set; } = ViewNumerator.GetNextItemID ();

        #region IEduProgramDivisionWritable implementation

        public long EduProgramDivisionId { get; set; }

        public int? EduProgramId { get; set; }

        public int? EduProgramProfileId { get; set; }

        public int DivisionId { get; set; }

        [XmlIgnore]
        public DivisionInfo Division { get; set; }

        public string DivisionRole { get; set; }

        #endregion

        public string DivisionTitle { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}
