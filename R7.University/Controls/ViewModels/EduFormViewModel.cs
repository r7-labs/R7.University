//
// DocumentTypeViewModel.cs
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
using System.Linq;
using System.Collections.Generic;
using System.Xml.Serialization;
using DotNetNuke.Services.Localization;
using DotNetNuke.Common.Utilities;

namespace R7.University.Controls
{
    [Serializable]
    public class EduFormViewModel: IEduForm
    {
        [XmlIgnore]
        public ViewModelContext Context { get; set; }

        #region IEduForm implementation

        public int EduFormID { get; set; }

        public bool IsSystem { get; set; }

        public string Title { get; set; }

        public string ShortTitle { get; set; }

        #endregion

        public string TitleLocalized
        { 
            get
            {   
                var titleLocalized = Localization.GetString ("SystemEduForm_" + Title + ".Text", 
                    Context.LocalResourceFile);
                
                return (!string.IsNullOrEmpty (titleLocalized)) ? titleLocalized : Title;
            }
        }

        public EduFormViewModel ()
        {}

        public EduFormViewModel (IEduForm eduForm, ViewModelContext context)
        {
            CopyCstor.Copy<IEduForm> (eduForm, this);
            Context = context;
        }

        public static List<EduFormViewModel> GetBindableList (IEnumerable<EduFormInfo> eduForms, 
            ViewModelContext context, bool withDefaultItem)
        {
            var eduFormVms = eduForms.Select (ef => new EduFormViewModel (ef, context)).ToList ();

            if (withDefaultItem) 
            {
                eduFormVms.Insert (0, new EduFormViewModel {
                    EduFormID = Null.NullInteger,
                    Title = "Default",
                    Context = context
                });
            }

            return eduFormVms;
        }
    }
}

