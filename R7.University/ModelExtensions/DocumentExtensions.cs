//
// DocumentExtensions.cs
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
using R7.University.Models;

namespace R7.University.ModelExtensions
{
    public static class DocumentExtensions
    {
        public static bool IsPublished (this IDocument document)
        {
            return ModelHelper.IsPublished (document.StartDate, document.EndDate);
        }

        public static IEnumerable<IDocument> WithDocumentType (
            this IEnumerable<IDocument> documents, 
            IEnumerable<IDocumentType> documentTypes)
        {
            return documents.Join (documentTypes, d => d.DocumentTypeID, dt => dt.DocumentTypeID,
                (d, dt) => {
                    d.DocumentType = dt;
                    return d;
                }
            );
        }

        public static SystemDocumentType GetSystemDocumentType (this IDocument document)
        {
            SystemDocumentType result;
            return Enum.TryParse<SystemDocumentType> (document.DocumentType.Type, out result) ? result : SystemDocumentType.Custom;
        }
    }
}

