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

using System.Web;
using R7.Dnn.Extensions.ViewModels;
using R7.University.EduProgramProfiles.Models;
using R7.University.ModelExtensions;
using R7.University.Models;

namespace R7.University.EduProgramProfiles.ViewModels
{
    public class EduVolumeViewModel : IEduVolume
    {
        protected readonly IEduVolume EduVolume;

        protected readonly ViewModelContext<EduVolumeDirectorySettings> Context;

        public EduVolumeViewModel (IEduVolume eduVolume, ViewModelContext<EduVolumeDirectorySettings> context)
        {
            EduVolume = eduVolume;
            Context = context;
        }

        #region IEduVolume implementation

        public int EduVolumeId => EduVolume.EduVolumeId;

        public int EduProgramProfileFormYearId => EduVolume.EduProgramProfileFormYearId;

        public int TimeToLearnHours => EduVolume.TimeToLearnHours;

        public int TimeToLearnMonths => EduVolume.TimeToLearnMonths;

        public int? PracticeType1Cu => EduVolume.PracticeType1Cu;

        public int? PracticeType2Cu => EduVolume.PracticeType2Cu;

        public int? PracticeType3Cu => EduVolume.PracticeType3Cu;

        public int? Year1Cu => EduVolume.Year1Cu;

        public int? Year2Cu => EduVolume.Year2Cu;

        public int? Year3Cu => EduVolume.Year3Cu;

        public int? Year4Cu => EduVolume.Year4Cu;

        public int? Year5Cu => EduVolume.Year5Cu;

        public int? Year6Cu => EduVolume.Year6Cu;

        public IEduProgramProfileFormYear EduProgramProfileFormYear => EduVolume.EduProgramProfileFormYear;

        #endregion

        #region Bindable properties

        public string EditUrl =>
            Context.Module.EditUrl ("eduvolume_id", EduVolume.EduVolumeId.ToString (), "EditEduVolume");

        public string CssClass =>
            EduVolume.EduProgramProfileFormYear.IsPublished (HttpContext.Current.Timestamp) ? string.Empty : "u8y-not-published";

        #endregion
    }
}
