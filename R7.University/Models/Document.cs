//
//  Document.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2019 Roman M. Yagodin
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

namespace R7.University.Models
{
    public interface IDocument: IPublishableEntity, ITrackableEntity
    {
        int DocumentID { get; }

        int DocumentTypeID { get; }

        int? EduProgramId { get; }

        int? EduProgramProfileId { get; }

        string Title { get; }

        string Group { get; }

        string Url { get; }

        int SortIndex { get; }

        IDocumentType DocumentType { get; }
    }

    public interface IDocumentWritable: IDocument, IPublishableEntityWritable, ITrackableEntityWritable
    {
        new int DocumentID { get; set; }

        new int DocumentTypeID { get; set; }

        new int? EduProgramId { get; set; }

        new int? EduProgramProfileId { get; set; }

        new string Title { get; set; }

        new string Group { get; set; }

        new string Url { get; set; }

        new int SortIndex { get; set; }

        new IDocumentType DocumentType { get; set; }
    }

    public class DocumentInfo: IDocumentWritable
    {
        public int DocumentID { get; set; }

        public int DocumentTypeID { get; set; }

        public int? EduProgramId { get; set; }

        public int? EduProgramProfileId { get; set; }

        public string Title { get; set; }

        public string Group { get; set; }

        public string Url { get; set; }

        public int SortIndex { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int LastModifiedByUserId { get; set; }

        public DateTime LastModifiedOnDate { get; set; }

        public int CreatedByUserId { get; set; }

        public DateTime CreatedOnDate { get; set; }

        public virtual DocumentTypeInfo DocumentType { get; set; }

        IDocumentType IDocument.DocumentType => DocumentType;

        IDocumentType IDocumentWritable.DocumentType {
            get { return DocumentType; }
            set { DocumentType = (DocumentTypeInfo) value; }
        }
    }
}

