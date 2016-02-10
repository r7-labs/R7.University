//
// EduProgramProfileForm.cs
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
using DotNetNuke.ComponentModel.DataAnnotations;

namespace R7.University
{
    public interface IEduProgramProfileForm
    {
        long EduProgramProfileFormID { get; set; }

        int EduProgramProfileID { get; set; }

        int EduFormID { get; set; }

        int TimeToLearn { get; set; }

        bool IsAdmissive { get; set; }

        IEduForm EduForm { get; set; }
    }

    [TableName ("University_EduProgramProfileForms")]
    [PrimaryKey ("EduProgramProfileFormID", AutoIncrement = true)]
    public class EduProgramProfileFormInfo: IEduProgramProfileForm
    {
        #region IEduProgramProfileForm implementation

        public long EduProgramProfileFormID { get; set; }

        public int EduProgramProfileID { get; set; }

        public int EduFormID { get; set; }

        public int TimeToLearn { get; set; }

        // REVIEW: Rename?
        public bool IsAdmissive { get; set; }

        [IgnoreColumn]
        public IEduForm EduForm { get; set; }

        #endregion

        [IgnoreColumn]
        public EduProgramProfileInfo EduProgramProfile { get; set; }

        public void SetTimeToLearn (int years, int months)
        {
            TimeToLearn = years * 12 + months;
        }
    }
}

