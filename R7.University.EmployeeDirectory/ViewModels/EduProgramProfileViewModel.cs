//
// EduProgramProfileViewModel.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2016 Roman M. Yagodin
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

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

        public ViewModelContext Context { get; protected set; }

        public IndexedEnumerable<TeacherViewModel> Teachers { get; set; }

        public EduProgramProfileViewModel (IEduProgramProfile model, ViewModelContext context)
        {
            Model = model;
            Context = context;
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

