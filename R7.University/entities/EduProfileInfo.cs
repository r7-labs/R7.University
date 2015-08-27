//
// EduProfileInfo.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015 
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
using DotNetNuke.ComponentModel.DataAnnotations;

namespace R7.University
{
    // TODO: Inherit from EnityBase

    [TableName ("University_EduProfiles")]
    [PrimaryKey ("EduProfileID", AutoIncrement = true)]
    [Scope ("EduProgramID")]
    public class EduProfileInfo
    {
        #region Properties

        public int EduProfileID { get; set; }

        public int EduProgramID { get; set; }

        public string ProfileCode { get; set; }

        public string ProfileTitle { get; set; }

        #endregion

        [IgnoreColumn]
        public string EduProfileString
        {
            get { return FormatEduProfile ("dummmy code", "dummy title", ProfileCode, ProfileTitle); }
        }

        public static string FormatEduProfile (string code, string title, string profileCode, string profileTitle)
        {
            var profileString = Utils.FormatList (" ", profileCode, profileTitle);
            return Utils.FormatList (" ", code, title) +
                (!string.IsNullOrWhiteSpace (profileString)? " (" + profileString + ")" : string.Empty);
        }
    }
}

