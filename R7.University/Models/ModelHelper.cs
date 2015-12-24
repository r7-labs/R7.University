//
// ModelHelper.cs
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

namespace R7.University
{
    public static class ModelHelper
    {
        public static bool IsPublished (DateTime? startDate, DateTime? endDate)
        {
            var now = DateTime.Now;
            return (startDate == null || now >= startDate) && (endDate == null || now < endDate);
        }

        #region Extension methods

        public static bool IsPublished (this IDocument document)
        {
            return IsPublished (document.StartDate, document.EndDate);
        }

        public static bool IsPublished (this IEduProgram eduProgram)
        {
            return IsPublished (eduProgram.StartDate, eduProgram.EndDate);
        }

        public static bool IsPublished (this IEduProgramProfile eduProgramProfile)
        {
            var isPublished = IsPublished (eduProgramProfile.StartDate, eduProgramProfile.EndDate);
            if (eduProgramProfile.EduProgram != null) {
                return isPublished && eduProgramProfile.EduProgram.IsPublished ();
            }

            return isPublished;
        }

        public static SystemDocumentType GetSystemDocumentType (this IDocumentType documentType)
        {
            SystemDocumentType result;
            return Enum.TryParse<SystemDocumentType> (documentType.Type, out result)? result : SystemDocumentType.Custom;
        }

        public static SystemEduForm GetSystemEduForm (this IEduForm eduForm)
        {
            SystemEduForm result;
            return Enum.TryParse<SystemEduForm> (eduForm.Title, out result)? result : SystemEduForm.Custom;
        }

        #endregion
    }
}

