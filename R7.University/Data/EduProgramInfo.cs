//
// EduProgramInfo.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015-2016 Roman M. Yagodin
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
using R7.University.Models;
using R7.University.ModelExtensions;

namespace R7.University.Data
{
    [TableName ("University_EduPrograms")]
    [PrimaryKey ("EduProgramID", AutoIncrement = true)]
    public class EduProgramInfo: IEduProgram
    {
        public EduProgramInfo ()
        {
            Documents = new List<IDocument> ();
        }

        #region IEduProgram implementation

        public int EduProgramID { get; set; }

        public int EduLevelID { get; set; }

        public string Code { get; set; }

        public string Title { get; set; }

        public string Generation { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int LastModifiedByUserID { get; set; }

        public DateTime LastModifiedOnDate { get; set; }

        public int CreatedByUserID { get; set; }

        public DateTime CreatedOnDate { get; set; }

        [IgnoreColumn]
        public IEduLevel EduLevel { get; set; }

        [IgnoreColumn]
        public IList<IDocument> Documents { get; set; }

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
        public IList<IDocument> EduStandardDocuments
        {
            get {
                return Documents.Where (d => d.DocumentType.GetSystemDocumentType () == SystemDocumentType.EduStandard).ToList ();
            }
        }
    }
}
