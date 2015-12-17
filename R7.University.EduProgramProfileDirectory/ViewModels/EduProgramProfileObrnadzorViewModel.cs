//
// EduProgramProfileObrnadzorViewModel.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015 Roman M. Yagodin
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

namespace R7.University.EduProgramProfileDirectory
{
    public class EduProgramProfileObrnadzorViewModel: IEduProgramProfile
    {
        #region IEduProgramProfile implementation

        public int EduProgramProfileID 
        { 
            get { return Model.EduProgramProfileID; }
            set { Model.EduProgramProfileID = value; }
        }

        public int EduProgramID 
        { 
            get { return Model.EduProgramID; }
            set { Model.EduProgramID = value; }
        }
      
        public string ProfileCode 
        { 
            get { return Model.ProfileCode; }
            set { Model.ProfileCode = value; }
        }

        public string ProfileTitle 
        { 
            get { return Model.ProfileTitle; }
            set { Model.ProfileTitle = value; }
        }

        public DateTime? AccreditedToDate 
        { 
            get { return Model.AccreditedToDate; }
            set { Model.AccreditedToDate = value; }
        }

        public DateTime? CommunityAccreditedToDate 
        { 
            get { return Model.CommunityAccreditedToDate; }
            set { Model.CommunityAccreditedToDate = value; }
        }

        //...

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public EduProgramInfo EduProgram { get; set; }
       
        public IList<IEduProgramProfileForm> EduProgramProfileForms { get; set; }

        #endregion

        public IEduProgramProfile Model { get; protected set; }

        public ViewModelContext Context { get; protected set; }

        public EduProgramProfileObrnadzorViewModel (IEduProgramProfile model, ViewModelContext context)
        {
            Model = model;
            Context = context;
        }
    }
}
