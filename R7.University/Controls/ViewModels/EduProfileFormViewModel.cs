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
using R7.University.Utilities;

namespace R7.University.Controls
{
    [Serializable]
    public class EduProgramProfileFormViewModel: EduProgramProfileFormInfo
    {
        public int ViewItemID { get; set; }

        [XmlIgnore]
        protected ViewModelContext Context { get; set; }

        [XmlIgnore]
        public string TitleLocalized
        { 
            get
            {
                var titleLocalized = Localization.GetString ("SystemEduForm_" + EduForm.Title + ".Text", 
                    Context.LocalResourceFile);
                    
                return (!string.IsNullOrEmpty (titleLocalized)) ? titleLocalized : EduForm.Title;
            }
        }

        public EduProgramProfileFormViewModel ()
        {
            ViewItemID = ViewNumerator.GetNextItemID ();
        }

        public EduProgramProfileFormViewModel (
            EduProgramProfileFormInfo eduProfileForm, ViewModelContext viewContext): this ()
        {
            CopyCstor.Copy<EduProgramProfileFormInfo> (eduProfileForm, this);
            Context = viewContext;
        }

        public static void BindToView (
            IEnumerable<EduProgramProfileFormViewModel> eduProfileForms, ViewModelContext context)
        {
            foreach (var eduProfileForm in eduProfileForms)
            {
                eduProfileForm.Context = context;
            }
        }

        public EduProgramProfileFormInfo NewEduProgramProfileFormInfo ()
        {
            var eduProfileForm = new EduProgramProfileFormInfo ();
            CopyCstor.Copy<EduProgramProfileFormInfo> (this, eduProfileForm);

            return eduProfileForm;
        }
    }
}

