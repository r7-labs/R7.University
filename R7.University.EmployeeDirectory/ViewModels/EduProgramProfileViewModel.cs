//
//  EduProgramProfileViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
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

using R7.DotNetNuke.Extensions.Utilities;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.EmployeeDirectory.ViewModels
{
    internal class EduProgramProfileViewModel: EduProgramProfileViewModelBase
    {
        public EmployeeDirectoryTeachersViewModel RootViewModel { get; protected set; }

        public IndexedEnumerable<TeacherViewModel> Teachers { get; set; }

        public EduProgramProfileViewModel (IEduProgramProfile model, EmployeeDirectoryTeachersViewModel rootViewModel)
            : base (model)
        {
            RootViewModel = rootViewModel;
        }

        public ViewModelContext Context
        {
            get { return RootViewModel.Context; }
        }

        #region Bindable properties

        public string Title_String
        {
            get {
                return TextUtils.FormatList (" - ",
                    FormatHelper.FormatEduProgramProfileTitle (EduProgram.Code, EduProgram.Title, ProfileCode, ProfileTitle),
                    (EduLevel != null) ? EduLevel.Title : null
                );
            }
        }

        #endregion
    }
}

