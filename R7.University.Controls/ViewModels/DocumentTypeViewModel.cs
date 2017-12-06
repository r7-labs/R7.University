//
//  DocumentTypeViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2017 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
//
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using R7.Dnn.Extensions.Utilities;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Models;
using R7.University.Utilities;

namespace R7.University.Controls
{
    [Serializable]
    public class DocumentTypeViewModel: IDocumentTypeWritable
    {
        [JsonIgnore]
        protected ViewModelContext Context { get; set; }

        #region IDocumentTypeWritable implementation

        public int DocumentTypeID { get; set; }

        public string Type { get; set; }

        public string Description { get; set; }

        public bool IsSystem { get; set; }

        public string FilenameFormat { get; set; }

        #endregion

        [JsonIgnore]
        public string LocalizedType
        { 
            get {
                return LocalizationHelper.GetStringWithFallback (
                    "SystemDocumentType_" + Type + ".Text", Context.LocalResourceFile, Type
                );
            }
        }

        public DocumentTypeViewModel ()
        {
        }

        public DocumentTypeViewModel (IDocumentTypeWritable documentType, ViewModelContext context)
        {
            CopyCstor.Copy<IDocumentTypeWritable> (documentType, this);
            Context = context;
        }

        public static List<DocumentTypeViewModel> GetBindableList (IEnumerable<DocumentTypeInfo> documentTypes, 
            ViewModelContext context)
        {
            return documentTypes.Select (dt => new DocumentTypeViewModel (dt, context)).ToList ();
        }

        public DocumentTypeInfo ToModel ()
        {
            var document = new DocumentTypeInfo ();
            CopyCstor.Copy<IDocumentTypeWritable> (this, document);

            return document;
        }
    }
}
