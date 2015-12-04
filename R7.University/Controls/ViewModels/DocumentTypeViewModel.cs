//
// DocumentTypeViewModel.cs
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
using System.Linq;
using DotNetNuke.Services.Localization;
using DotNetNuke.Common.Utilities;
using System.Collections.Generic;

namespace R7.University.Controls
{
    public class DocumentTypeViewModel: DocumentTypeInfo
    {
        public string LocalizedType { get; set; }

        public DocumentTypeViewModel ()
        {}

        public DocumentTypeViewModel (DocumentTypeInfo documentType, string resourceFile)
        {
            CopyCstor.Copy<DocumentTypeInfo> (documentType, this);
            Localize (resourceFile);
        }

        public void Localize (string resourceFile)
        {
            var localizedType = Localization.GetString ("SystemDocumentType_" + Type + ".Text", resourceFile);
            LocalizedType = (!string.IsNullOrEmpty (localizedType)) ? localizedType : Type;
        }

        public static DocumentTypeViewModel GetDefaultItem (string resourceFile)
        {
            var defaultItem = new DocumentTypeViewModel { 
                DocumentTypeID = Null.NullInteger,
                Type = "Default"
            };

            defaultItem.Localize (resourceFile);
            return defaultItem;
        }

        public static List<DocumentTypeViewModel> GetBindableList (IEnumerable<DocumentTypeInfo> documentTypes, string resourceFile, bool withDefaultItem)
        {
            var documentTypeVms = documentTypes.Select (dt => new DocumentTypeViewModel (dt, resourceFile)).ToList ();

            if (withDefaultItem) 
            {
                documentTypeVms.Insert (0, DocumentTypeViewModel.GetDefaultItem (resourceFile));
            }

            return documentTypeVms;
        }
    }
}

