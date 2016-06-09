//
//  EduProgramProfileViewModel.cs
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

namespace R7.University.EmployeeDirectory.ViewModels
{
    public class EduProgramProfileViewModel: IEduProgramProfile
    {
        public IEduProgramProfile Model { get; protected set; }

        public EmployeeDirectoryTeachersViewModel RootViewModel { get; protected set; }

        public IndexedEnumerable<TeacherViewModel> Teachers { get; set; }

        public EduProgramProfileViewModel (IEduProgramProfile model, EmployeeDirectoryTeachersViewModel rootViewModel)
        {
            Model = model;
            RootViewModel = rootViewModel;
        }

        public ViewModelContext Context
        {
            get { return RootViewModel.Context; }
        }

        #region IEduProgramProfile implementation

        public int EduProgramProfileID
        { 
            get { return Model.EduProgramProfileID; }
            set { throw new NotImplementedException (); }
        }

        public int EduProgramID
        { 
            get { return Model.EduProgramID; }
            set { throw new NotImplementedException (); }
        }

        public string ProfileCode
        { 
            get { return Model.ProfileCode; }
            set { throw new NotImplementedException (); }
        }

        public string ProfileTitle
        { 
            get { return Model.ProfileTitle; }
            set { throw new NotImplementedException (); }
        }

        public string Languages
        { 
            get { return Model.Languages; }
            set { throw new NotImplementedException (); }
        }

        public DateTime? AccreditedToDate
        { 
            get { return Model.AccreditedToDate; }
            set { throw new NotImplementedException (); }
        }

        public DateTime? CommunityAccreditedToDate
        { 
            get { return Model.CommunityAccreditedToDate; }
            set { throw new NotImplementedException (); }
        }

        public DateTime? StartDate
        { 
            get { return Model.StartDate; }
            set { throw new NotImplementedException (); }
        }

        public DateTime? EndDate
        {
            get { return Model.EndDate; }
            set { throw new NotImplementedException (); }
        }

        public int LastModifiedByUserID
        {
            get { return Model.LastModifiedByUserID; }
            set { throw new NotImplementedException (); }
        }

        public DateTime LastModifiedOnDate
        {
            get { return Model.LastModifiedOnDate; }
            set { throw new NotImplementedException (); }
        }

        public int CreatedByUserID
        {
            get { return Model.CreatedByUserID; }
            set { throw new NotImplementedException (); }
        }

        public DateTime CreatedOnDate
        {
            get { return Model.CreatedOnDate; }
            set { throw new NotImplementedException (); }
        }

        public IEduProgram EduProgram
        {
            get { return Model.EduProgram; }
            set { throw new NotImplementedException (); }
        }

        public IList<IEduProgramProfileForm> EduProgramProfileForms
        {
            get { return Model.EduProgramProfileForms; }
            set { throw new NotImplementedException (); }
        }

        public IList<IDocument> Documents
        {
            get { return Model.Documents; }
            set { throw new NotImplementedException (); }
        }

        #endregion

        #region Bindable properties

        public string Title_String
        {
            get {
                return FormatHelper.FormatEduProgramProfileTitle (
                    EduProgram.Code, EduProgram.Title, ProfileCode, ProfileTitle
                );
            }
        }

        #endregion
    }
}

