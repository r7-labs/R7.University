//
//  EduFormViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2017 Roman M. Yagodin
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
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;
using Newtonsoft.Json;
using R7.Dnn.Extensions.Utilities;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Models;

namespace R7.University.Controls
{
    [Serializable]
    public class EduFormViewModel: IEduFormWritable
    {
        [JsonIgnore]
        public ViewModelContext Context { get; set; }

        #region IEduFormWritable implementation

        public int EduFormID { get; set; }

        public bool IsSystem { get; set; }

        public string Title { get; set; }

        public string ShortTitle { get; set; }

        #endregion

        [JsonIgnore]
        public string TitleLocalized
        { 
            get {   
                var titleLocalized = Localization.GetString ("SystemEduForm_" + Title + ".Text", 
                                         Context.LocalResourceFile);
                
                return (!string.IsNullOrEmpty (titleLocalized)) ? titleLocalized : Title;
            }
        }

        public EduFormViewModel ()
        {
        }

        public EduFormViewModel (IEduFormWritable eduForm, ViewModelContext context)
        {
            CopyCstor.Copy<IEduFormWritable> (eduForm, this);
            Context = context;
        }

        public static List<EduFormViewModel> GetBindableList (IEnumerable<EduFormInfo> eduForms, 
            ViewModelContext context, bool withDefaultItem)
        {
            var eduFormVms = eduForms.Select (ef => new EduFormViewModel (ef, context)).ToList ();

            if (withDefaultItem) {
                eduFormVms.Insert (0, new EduFormViewModel
                    {
                        EduFormID = Null.NullInteger,
                        Title = "Default",
                        Context = context
                    });
            }

            return eduFormVms;
        }
    }
}

