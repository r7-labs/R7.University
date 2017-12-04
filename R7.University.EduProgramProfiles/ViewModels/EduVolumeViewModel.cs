//
//  EduVolumeViewModel.cs
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
using R7.Dnn.Extensions.ViewModels;
using R7.University.EduProgramProfiles.Models;
using R7.University.ModelExtensions;
using R7.University.Models;

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

        public int YearId => FormYear.YearId;

        public IEduForm EduForm => FormYear.EduForm;

        public IYear Year => FormYear.Year;

        public IEduVolume EduVolume => FormYear.EduVolume;

        public IEduProgramProfile EduProgramProfile => FormYear.EduProgramProfile;

        public DateTime? StartDate => FormYear.StartDate;

        public DateTime? EndDate => FormYear.EndDate;

        #endregion

        #region Bindable properties

        public string EditUrl =>
            (FormYear.EduVolume != null)
                ? Context.Module.EditUrl ("eduvolume_id", FormYear.EduVolume.EduVolumeId.ToString (), "EditEduVolume")
                : Context.Module.EditUrl ("eduprogramprofileformyear_id", FormYear.EduProgramProfileFormYearId.ToString (), "EditEduVolume");

        public string CssClass =>
            FormYear.IsPublished (HttpContext.Current.Timestamp) ? string.Empty : "u8y-not-published";

        public string Year1Cu => FormatYearCu (() => FormYear.EduVolume?.Year1Cu);

        public string Year2Cu => FormatYearCu (() => FormYear.EduVolume?.Year2Cu);

        public string Year3Cu => FormatYearCu (() => FormYear.EduVolume?.Year3Cu);

        public string Year4Cu => FormatYearCu (() => FormYear.EduVolume?.Year4Cu);

        public string Year5Cu => FormatYearCu (() => FormYear.EduVolume?.Year5Cu);

        public string Year6Cu => FormatYearCu (() => FormYear.EduVolume?.Year6Cu);

        #endregion

        string FormatYearCu (Func<int?> getYearCu)
        {
            var yearCu = getYearCu ();
            return yearCu != null ? yearCu.ToString () : "-";
        }
    }
}
