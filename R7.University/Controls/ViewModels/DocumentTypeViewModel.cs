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
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.Localization;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.Components;

namespace R7.University.Controls
{
    [Serializable]
    public class DocumentTypeViewModel: IDocumentType
    {
        [XmlIgnore]
        protected ViewModelContext Context { get; set; }

        #region IDocumentType implementation

        public int DocumentTypeID { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public bool IsSystem { get; set; }

        #endregion

        [XmlIgnore]
        public string LocalizedType
        { 
            get { 
                var localizedType = Localization.GetString ("SystemDocumentType_" + Type + ".Text", 
                                        Context.LocalResourceFile);
                
                return (!string.IsNullOrEmpty (localizedType)) ? localizedType : Type;
            }
        }

        public DocumentTypeViewModel ()
        {
        }

        public DocumentTypeViewModel (IDocumentType documentType, ViewModelContext context)
        {
            CopyCstor.Copy<IDocumentType> (documentType, this);
            Context = context;
        }

        public static List<DocumentTypeViewModel> GetBindableList (IEnumerable<DocumentTypeInfo> documentTypes, 
            ViewModelContext context, bool withDefaultItem)
        {
            var documentTypeVms = documentTypes.Select (dt => new DocumentTypeViewModel (dt, context)).ToList ();

            if (withDefaultItem) {
                documentTypeVms.Insert (0, new DocumentTypeViewModel
                    {
                        DocumentTypeID = Null.NullInteger,
                        Type = "Default",
                        Context = context
                    });
            }

            return documentTypeVms;
        }

        public DocumentTypeInfo ToModel ()
        {
            var document = new DocumentTypeInfo ();
            CopyCstor.Copy<IDocumentType> (this, document);

            return document;
        }
    }
}
