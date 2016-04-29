//
// EduProgramInfo.cs
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
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.ComponentModel.DataAnnotations;
using R7.DotNetNuke.Extensions.Utilities;

namespace R7.University
{
    public interface IEduProgram
    {
        int EduProgramID { get; set; }

        int EduLevelID { get; set; }

        string Code { get; set; }

        string Title { get; set; }

        string Generation { get; set; }

        DateTime? AccreditedToDate { get; set; }

        DateTime? StartDate { get; set; }

        DateTime? EndDate { get; set; }

        EduLevelInfo EduLevel { get; set; }

        IList<DocumentInfo> Documents { get; set; }

    }

    [TableName ("University_EduPrograms")]
    [PrimaryKey ("EduProgramID", AutoIncrement = true)]
    public class EduProgramInfo: UniversityEntityBase, IEduProgram
    {
        public EduProgramInfo ()
        {
            Documents = new List<DocumentInfo> ();
        }

        #region IEduProgram implementation

        public int EduProgramID { get; set; }

        public int EduLevelID { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public string Generation { get; set; }

        public DateTime? AccreditedToDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [IgnoreColumn]
        public EduLevelInfo EduLevel { get; set; }

        [IgnoreColumn]
        public IList<DocumentInfo> Documents { get; set; }

        #endregion

        // TODO: Move to viewmodel
        [IgnoreColumn]
        public string EduProgramString
        {
            get { return TextUtils.FormatList (" ", Code, Title); }
        }

        [IgnoreColumn]
        public bool IsPublished
        {
            get {
                var now = DateTime.Now;
                return (StartDate == null || now >= StartDate) && (EndDate == null || now < EndDate);
            }
        }

        [IgnoreColumn]
        public IList<DocumentInfo> EduStandardDocuments
        {
            get { 
                return Documents.Where (d => d.DocumentType != null
                    && d.DocumentType.GetSystemDocumentType () == SystemDocumentType.EduStandard).ToList ();
            }
        }
    }
}
