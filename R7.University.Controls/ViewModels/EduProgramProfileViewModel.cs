//
//  EduProgramProfileViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2020 Roman M. Yagodin
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

using R7.Dnn.Extensions.ViewModels;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Controls.ViewModels
{
    public class EduProgramProfileViewModel : EduProgramProfileViewModelBase
    {
        protected IViewModelContext Context;

        public EduProgramProfileViewModel (IEduProfile model, IViewModelContext viewModelContext) : base (model)
        {
            Context = viewModelContext;
        }

        public string Title_String
        {
            get {
                return UniversityFormatHelper.FormatEduProgramProfilePartialTitle (
                    EduProgramProfile.ProfileCode,
                    !string.IsNullOrEmpty (EduProgramProfile.ProfileTitle) ? EduProgramProfile.ProfileTitle : Context.LocalizeString ("EmptyProfileTitle.Text"),
                    UniversityFormatHelper.FormatShortTitle (EduProgramProfile.EduLevel.ShortTitle, EduProgramProfile.EduLevel.Title)
                );
            }
        }

    }
}
