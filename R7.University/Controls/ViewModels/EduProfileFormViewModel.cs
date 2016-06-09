//
//  EduProfileFormViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Xml.Serialization;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.Components;
using R7.University.ViewModels;
using R7.University.Models;
using R7.University.Data;

namespace R7.University.Controls
{
    [Serializable]
    public class EduProgramProfileFormViewModel: IEduProgramProfileForm, IEditControlViewModel<EduProgramProfileFormInfo>
    {
        #region IEditControlViewModel implementation

        public int ViewItemID { get; set; }

        [XmlIgnore]
        public ViewModelContext Context { get; set; }

        public IEditControlViewModel<EduProgramProfileFormInfo> FromModel (
            EduProgramProfileFormInfo model, ViewModelContext context)
        {
            var viewModel = new EduProgramProfileFormViewModel ();
            CopyCstor.Copy<IEduProgramProfileForm> (model, viewModel);
            viewModel.EduForm = new EduFormViewModel (model.EduForm, context);
            viewModel.Context = context;

            return viewModel;
        }

        public EduProgramProfileFormInfo ToModel ()
        {
            var model = new EduProgramProfileFormInfo ();
            CopyCstor.Copy<IEduProgramProfileForm> (this, model);

            return model;
        }

        public void SetTargetItemId (int targetItemId, string targetItemKey)
        {
            EduProgramProfileID = targetItemId;
        }

        #endregion

        public EduProgramProfileFormViewModel ()
        {
            ViewItemID = ViewNumerator.GetNextItemID ();
        }

        #region IEduProgramProfileForm implementation

        public long EduProgramProfileFormID { get; set; }

        public int EduProgramProfileID { get; set; }

        public int EduFormID { get; set; }

        public int TimeToLearn { get; set; }

        public bool IsAdmissive { get; set; }

        [XmlIgnore]
        public IEduForm EduForm { get; set; }

        [XmlIgnore]
        public IEduProgramProfile EduProgramProfile { get; set; }

        public EduFormViewModel EduFormViewModel
        {
            get { return (EduFormViewModel) EduForm; }
            set { EduForm = value; }
        }

        #endregion

        [XmlIgnore]
        public string EduFormTitleLocalized
        {
            get {
                EduFormViewModel.Context = Context;
                return EduFormViewModel.TitleLocalized;
            }
        }

        [XmlIgnore]
        public string TimeToLearnString
        {
            get { 
                return FormatHelper.FormatTimeToLearn (TimeToLearn,
                    "TimeToLearnYears.Format", "TimeToLearnMonths.Format", Context.LocalResourceFile);
            }
        }
    }
}
