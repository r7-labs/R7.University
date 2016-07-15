//
//  DocumentTypeViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using DotNetNuke.Services.Localization;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.Components;
using R7.University.Models;

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
            ViewModelContext context)
        {
            return documentTypes.Select (dt => new DocumentTypeViewModel (dt, context)).ToList ();
        }

        public DocumentTypeInfo ToModel ()
        {
            var document = new DocumentTypeInfo ();
            CopyCstor.Copy<IDocumentType> (this, document);

            return document;
        }
    }
}
