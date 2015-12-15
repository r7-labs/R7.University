//
// DocumentViewModel.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015 
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
using System.Collections.Generic;
using System.Xml.Serialization;
using DotNetNuke.Services.Localization;

namespace R7.University.Controls
{
    [Serializable]
    public class EduProgramProfileFormViewModel: IEduProgramProfileForm, IEditControlViewModel<EduProgramProfileFormInfo>
    {
        #region IEditControlViewModel implementation

        public int ViewItemID { get; set; }

        public int TargetItemID
        {
            get { return EduProgramProfileID; }
            set { EduProgramProfileID = value; }
        }

        [XmlIgnore]
        public ViewModelContext Context { get; set; }

        public IEditControlViewModel<EduProgramProfileFormInfo> FromModel (
            EduProgramProfileFormInfo model, ViewModelContext context)
        {
            var viewModel = new EduProgramProfileFormViewModel ();
            CopyCstor.Copy<IEduProgramProfileForm> (model, this);
            viewModel.EduFormTitle = model.EduForm.Title;
            viewModel.Context = context;

            return viewModel;
        }

        public EduProgramProfileFormInfo ToModel ()
        {
            var model = new EduProgramProfileFormInfo ();
            CopyCstor.Copy<IEduProgramProfileForm> (this, model);

            return model;
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

        #endregion

        public string EduFormTitle { get; set; }

        [XmlIgnore]
        public string EduFormTitleLocalized
        {
            get
            {
                var title = Localization.GetString ("SystemEduForm_" + EduFormTitle + ".Text", 
                    Context.LocalResourceFile);
                    
                return (!string.IsNullOrEmpty (title)) ? title : EduFormTitle;
            }
        }

        [XmlIgnore]
        public string TimeToLearnString
        {
            get
            { 
                return string.Format (Localization.GetString ("TimeToLearn.Format", Context.LocalResourceFile),
                    TimeToLearn / 12, TimeToLearn % 12, TimeToLearn);
            }
        }
    }
}
