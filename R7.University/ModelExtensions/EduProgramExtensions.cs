//
// EduProgramExtensions.cs
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
using System.Linq;
using System.Collections.Generic;
using R7.DotNetNuke.Extensions.Data;

namespace R7.University.ModelExtensions
{
    public static class EduProgramExtensions
    {
        public static EduProgramInfo WithEduLevel (this EduProgramInfo eduProgram, Dal2DataProvider controller)
        {
            eduProgram.EduLevel = controller.Get<EduLevelInfo> (eduProgram.EduLevelID);
            return eduProgram;
        }

        public static IEnumerable<EduProgramInfo> WithEduLevel (
            this IEnumerable<EduProgramInfo> eduPrograms,
            Dal2DataProvider controller)
        {
            foreach (var eduProgram in eduPrograms) {
                eduProgram.WithEduLevel (controller);
            }

            return eduPrograms;
        }

        public static EduProgramInfo WithDocuments (this EduProgramInfo eduProgram, Dal2DataProvider controller)
        {
            eduProgram.Documents = controller.GetObjects<DocumentInfo> (
                "WHERE [ItemID] = @0", "EduProgramID=" + eduProgram.EduProgramID).ToList ();

            eduProgram.Documents.WithDocumentType (controller);

            return eduProgram;
        }

        public static IEnumerable<EduProgramInfo> WithDocuments (
            this IEnumerable<EduProgramInfo> eduPrograms,
            Dal2DataProvider controller)
        {
            foreach (var eduProgram in eduPrograms) {
                eduProgram.WithDocuments (controller);
            }

            return eduPrograms;
        }
    }
}

