//
//  EduVolumeViewModel.cs
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
using DotNetNuke.Common.Utilities;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Components;
using R7.University.EduProgramProfiles.Models;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.EduProgramProfiles.ViewModels
{
    public class EduVolumeViewModel: IEduProgramProfileFormYear
    {
        protected readonly IEduProgramProfileFormYear FormYear;

        protected readonly ViewModelContext<EduVolumeDirectorySettings> Context;

        public EduVolumeViewModel (IEduProgramProfileFormYear formYear, ViewModelContext<EduVolumeDirectorySettings> context)
        {
            FormYear = formYear;
            Context = context;
        }

        #region IEduProgramProfileFormYear implementation

        public int EduProgramProfileFormYearId => FormYear.EduProgramProfileFormYearId;

        public int EduProgramProfileId => FormYear.EduProgramProfileId;

        public int EduFormId => FormYear.EduFormId;

        public int? YearId => FormYear.YearId;

        public IEduForm EduForm => FormYear.EduForm;

        public IYear Year => FormYear.Year;

        public IEduVolume EduVolume => FormYear.EduVolume;

        public IContingent Contingent => FormYear.Contingent;

        public IEduProfile EduProfile => FormYear.EduProfile;

        public DateTime? StartDate => FormYear.EduProfile.StartDate ?? FormYear.StartDate;

        public DateTime? EndDate => FormYear.EduProfile.EndDate ?? FormYear.EndDate;

        #endregion

        #region Bindable properties

        public string EditIconUrl => FormYear.EduVolume != null ? UniversityIcons.Edit : UniversityIcons.AddAlternate;

        public string EditUrl =>
            FormYear.EduVolume != null
                ? Context.Module.EditUrl ("eduvolume_id", FormYear.EduVolume.EduVolumeId.ToString (), "EditEduVolume")
                : Context.Module.EditUrl ("eduprogramprofileformyear_id", FormYear.EduProgramProfileFormYearId.ToString (), "EditEduVolume");

        public string CssClass =>
            this.IsPublished (HttpContext.Current.Timestamp) ? string.Empty : "u8y-not-published";

        public string EduProgramProfileTitle => FormYear.EduProfile.FormatTitle (withEduProgramCode: false)
                                                        .Append (FormYear.EduProfile.IsAdopted ? Context.LocalizeString ("IsAdopted.Text") : null, " - ");

        public string Year1Cu => UniversityFormatHelper.ValueOrDash (FormYear.EduVolume?.Year1Cu);

        public string Year2Cu => UniversityFormatHelper.ValueOrDash (FormYear.EduVolume?.Year2Cu);

        public string Year3Cu => UniversityFormatHelper.ValueOrDash (FormYear.EduVolume?.Year3Cu);

        public string Year4Cu => UniversityFormatHelper.ValueOrDash (FormYear.EduVolume?.Year4Cu);

        public string Year5Cu => UniversityFormatHelper.ValueOrDash (FormYear.EduVolume?.Year5Cu);

        public string Year6Cu => UniversityFormatHelper.ValueOrDash (FormYear.EduVolume?.Year6Cu);

        public string PracticeType1Cu => UniversityFormatHelper.ValueOrDash (FormYear.EduVolume?.PracticeType1Cu);

        public string PracticeType2Cu => UniversityFormatHelper.ValueOrDash (FormYear.EduVolume?.PracticeType2Cu);

        public string PracticeType3Cu => UniversityFormatHelper.ValueOrDash (FormYear.EduVolume?.PracticeType3Cu);

        public string TimeToLearnMonths => FormYear.EduVolume != null
            ? UniversityFormatHelper.FormatTimeToLearnMonths (FormYear.EduVolume.TimeToLearnMonths, "TimeToLearn", Context.LocalResourceFile)
            : string.Empty;

        public string TimeToLearnHours => FormYear.EduVolume != null
            ? ((FormYear.EduVolume.TimeToLearnHours > 0)? FormYear.EduVolume.TimeToLearnHours.ToString () : string.Empty)
            : string.Empty;

        public string EduFormTitle {
            get {
                var sysEduForm = FormYear.EduForm.GetSystemEduForm ();
                if (sysEduForm != SystemEduForm.Custom) {
                    return Context.LocalizeString ($"EduForm_{sysEduForm}.Text").ToLower ();
                }
                return FormYear.EduForm.Title.ToLower ();
            }
        }

        public string ItemProp =>
            Context.Settings.Mode == EduVolumeDirectoryMode.Practices
                   ? (FormYear.EduProfile.IsAdopted ? "adEduPr" : "eduPr")
                   : "eduOp";

        public string HtmlElementId => $"eduvolume_{Context.Module.ModuleId}_{EduProgramProfileFormYearId}";

        #endregion
    }
}
