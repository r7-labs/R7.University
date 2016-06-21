//
//  EduProgramViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.EduProgram.ViewModels
{
    public class EduProgramViewModel: EduProgramViewModelBase
    {
        public EduProgramModuleViewModel RootViewModel { get; protected set; }

        protected ViewModelContext Context
        {
            get { return RootViewModel.Context; }
        }
        
        public EduProgramViewModel (IEduProgram model, EduProgramModuleViewModel rootViewModel): base (model)
        {
            RootViewModel = rootViewModel;
        }

        public IEnumerable<EduProgramProfileViewModel> EduProgramProfileViewModels { get; set; }

        #region Bindable properties

        public string Title_String
        {
            get { return FormatHelper.FormatEduProgramTitle (Model.Code, Model.Title); }
        }

        #endregion
    }
}

