//
//  EduProgramProfileViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2017 Roman M. Yagodin
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

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using DotNetNuke.Services.Localization;
using R7.Dnn.Extensions.Utilities;
using R7.Dnn.Extensions.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.EduProgram.ViewModels
{
    public class EduProgramProfileViewModel: EduProgramProfileViewModelBase
    {
        public EduProgramModuleViewModel RootViewModel { get; protected set; }

        protected ViewModelContext Context
        {
            get { return RootViewModel.Context; }
        }
        
        public EduProgramProfileViewModel (IEduProgramProfile model, EduProgramModuleViewModel rootViewModel): base (model)
        {
            RootViewModel = rootViewModel;
        }

        #region Bindable properties

        public string Title_String
        {
            get {
                return TextUtils.FormatList (": ", 
                    Localization.GetString ("EduProgramProfile.Text", Context.LocalResourceFile),
                    FormatHelper.FormatEduProgramTitle (EduProgramProfile.ProfileCode, EduProgramProfile.ProfileTitle)
                );
            }
        }

        public bool AccreditedToDate_Visible
        {
            get { return EduProgramProfile.AccreditedToDate != null; }
        }

        public string AccreditedToDate_String
        {
            get { 
                return EduProgramProfile.AccreditedToDate != null ?
                    EduProgramProfile.AccreditedToDate.Value.ToShortDateString () :
                    string.Empty;
            }
        }

        public bool CommunityAccreditedToDate_Visible
        {
            get { return EduProgramProfile.CommunityAccreditedToDate != null; }
        }

        public string CommunityAccreditedToDate_String
        {
            get { 
                return EduProgramProfile.CommunityAccreditedToDate != null ?
                    EduProgramProfile.CommunityAccreditedToDate.Value.ToShortDateString () :
                    string.Empty;
            }
        }

        public string EduLevel_Title
        {
            get { return EduProgramProfile.EduLevel.Title; }
        }

        public bool EduForms_Visible
        {
            get { return !EduProgramProfile.EduProgramProfileForms.IsNullOrEmpty (); }
        }

        public string EduForms_String
        {
            get {
                if (EduProgramProfile.EduProgramProfileForms != null) {
                    var sb = new StringBuilder ();
                    foreach (var eppf in EduProgramProfile.EduProgramProfileForms) {
                        sb.AppendFormat ("<li>{0} &ndash; {1}</li>", 
                            Localization.GetString ("TimeToLearn" + eppf.EduForm.Title + ".Text", Context.LocalResourceFile),
                            FormatHelper.FormatTimeToLearn (eppf.TimeToLearn, eppf.TimeToLearnHours, TimeToLearnDisplayMode.Both, "TimeToLearn", Context.LocalResourceFile)
                        );
                    }

                    return string.Format ("<ul>{0}</ul>", sb);
                }

                return null;
            }
        }

        public string Edit_Url
        {
            get {
                return Context.Module.EditUrl (
                    "eduprogramprofile_id",
                    EduProgramProfile.EduProgramProfileID.ToString (),
                    "EditEduProgramProfile"
                );
            }
        }

        public string CssClass
        {
            get { return EduProgramProfile.IsPublished (HttpContext.Current.Timestamp) ? string.Empty : "u8y-not-published"; }
        }

        public bool DivisionsVisible
        {
            get {
                var now = HttpContext.Current.Timestamp;
                return EduProgramProfile.Divisions.Any (epd => epd.Division.IsPublished (now) || Context.Module.IsEditable);
            }
        }

        public IEnumerable<EduProgramDivisionViewModel> DivisionViewModels {
            get {
                var now = HttpContext.Current.Timestamp;
                return EduProgramProfile.Divisions
                                        .Where (epd => epd.Division.IsPublished (now) || Context.Module.IsEditable)
                                        .Select (epd => new EduProgramDivisionViewModel (epd));
            }
        }

        #endregion
    }
}

