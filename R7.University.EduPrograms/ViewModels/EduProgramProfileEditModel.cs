//
//  EduProgramProfileEditModel.cs
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

using R7.University.ViewModels;
using R7.University.Models;
using R7.Dnn.Extensions.ViewModels;

namespace R7.University.EduPrograms.ViewModels
{
    public class EduProgramProfileEditModel: EduProfileViewModelBase
    {
        public ViewModelContext Context { get; protected set; }

        public EduProgramProfileEditModel (IEduProfile model, ViewModelContext context): base (model)
        {
            Context = context;
        }

        #region Bindable properties

        public string EduProgramProfile_String
        {
            get { return UniversityFormatHelper.FormatEduProgramTitle (EduProfile.ProfileCode, EduProfile.ProfileTitle); }
        }

        public string EduLevel_String
        {
            get { return EduProfile.EduLevel.Title; }
        }

        public string Edit_Url => Context.Module.EditUrl (
            "eduprogramprofile_id", EduProfile.EduProgramProfileID.ToString (), "EditEduProgramProfile");

        public string EditDocuments_Url => Context.Module.EditUrl (
	        "eduprogramprofile_id", EduProfile.EduProgramProfileID.ToString (), "EditEduProgramProfileDocuments");

        #endregion
    }
}
