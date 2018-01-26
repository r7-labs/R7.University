//
//  EduProgramProfileViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2018 Roman M. Yagodin
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
using R7.University.Utilities;
using R7.University.ViewModels;

namespace R7.University.EduPrograms.ViewModels
{
    public class EduProgramProfileViewModel: EduProgramProfileViewModelBase
    {
        public EduProgramModuleViewModel RootViewModel { get; protected set; }

        protected ViewModelContext Context => RootViewModel.Context;

        public EduProgramProfileViewModel (IEduProgramProfile model, EduProgramModuleViewModel rootViewModel) : base (model)
        {
            RootViewModel = rootViewModel;
        }

        #region Bindable properties

        public string Title_String => TextUtils.FormatList (
            ": ",
            Localization.GetString ("EduProgramProfile.Text", Context.LocalResourceFile),
            FormatHelper.FormatEduProgramTitle (EduProgramProfile.ProfileCode, EduProgramProfile.ProfileTitle)
        );
        
        public bool AccreditedToDate_Visible => EduProgramProfile.AccreditedToDate != null;

        public string AccreditedToDate_String =>
            EduProgramProfile.AccreditedToDate != null ? EduProgramProfile.AccreditedToDate.Value.ToShortDateString () : string.Empty;

        public bool CommunityAccreditedToDate_Visible => EduProgramProfile.CommunityAccreditedToDate != null;

        public string CommunityAccreditedToDate_String =>
            EduProgramProfile.CommunityAccreditedToDate != null ? EduProgramProfile.CommunityAccreditedToDate.Value.ToShortDateString () : string.Empty;

        public string EduLevel_Title => EduProgramProfile.EduLevel.Title;

        public bool ImplementedEduForms_Visible => !ImplementedEduForms.IsNullOrEmpty ();

        public bool EduFormsForAdmission_Visible => !EduFormsForAdmission.IsNullOrEmpty ();

        public int? YearOfAdmission => EduFormsForAdmission.FirstOrDefault ()?.Year.Year;

        public string ImplementedEduForms_String => FormatEduFormYears (ImplementedEduForms);

        public string EduFormsForAdmission_String => FormatEduFormYears (EduFormsForAdmission);

        public string Edit_Url => Context.Module.EditUrl (
            "eduprogramprofile_id",
            EduProgramProfile.EduProgramProfileID.ToString (),
            "EditEduProgramProfile"
        );

        public string CssClass => EduProgramProfile.IsPublished (HttpContext.Current.Timestamp) ? string.Empty : "u8y-not-published";

        public bool DivisionsVisible =>
            EduProgramProfile.Divisions.Any (epd => epd.Division.IsPublished (HttpContext.Current.Timestamp) || Context.Module.IsEditable);

        public IEnumerable<EduProgramDivisionViewModel> DivisionViewModels =>
            EduProgramProfile.Divisions
                             .Where (epd => epd.Division.IsPublished (HttpContext.Current.Timestamp) || Context.Module.IsEditable)
                             .Select (epd => new EduProgramDivisionViewModel (epd));

        #endregion

        IEnumerable<IEduProgramProfileFormYear> _implementedEduForms;
        protected IEnumerable<IEduProgramProfileFormYear> ImplementedEduForms =>
            _implementedEduForms ?? (_implementedEduForms = GetEduFormYears (forAdmission: false));

        IEnumerable<IEduProgramProfileFormYear> _eduFormsForAdmission;
        protected IEnumerable<IEduProgramProfileFormYear> EduFormsForAdmission =>
            _eduFormsForAdmission ?? (_eduFormsForAdmission = GetEduFormYears (forAdmission: true));

        IEnumerable<IEduProgramProfileFormYear> GetEduFormYears (bool forAdmission)
        {
            return EduProgramProfile.EduProgramProfileFormYears
                .Where (eppfy => eppfy.Year.AdmissionIsOpen == forAdmission && (eppfy.IsPublished (HttpContext.Current.Timestamp) || Context.Module.IsEditable))
                .DistinctByEduForms ();
        }

        string FormatEduFormYears (IEnumerable<IEduProgramProfileFormYear> eppfys)
        {
            if (eppfys != null) {
                var sb = new StringBuilder ();
                foreach (var eppfy in eppfys) {
                    var eduFormTitle = LocalizationHelper.GetStringWithFallback (
                        "TimeToLearn" + eppfy.EduForm.Title + ".Text",
                        Context.LocalResourceFile,
                        eppfy.EduForm.Title
                    ).ToLower ();
                    sb.AppendFormat (
                       "<li>{0}</li>",
                       TextUtils.FormatList (
                          " &ndash; ",
                          eduFormTitle,
                          (eppfy.EduVolume != null)
                             ? FormatHelper.FormatTimeToLearn (eppfy.EduVolume.TimeToLearnMonths, eppfy.EduVolume.TimeToLearnHours, TimeToLearnDisplayMode.Both, "TimeToLearn", Context.LocalResourceFile)
                             : null
                       )
                    );
                }

                return string.Format ("<ul>{0}</ul>", sb);
            }

            return string.Empty;
        }
    }
}
