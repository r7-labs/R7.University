//
//  IDocument.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2017 Roman M. Yagodin
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
    public interface IDocument
    {
        int DocumentID { get; }

        int DocumentTypeID { get; }

        int? EduProgramId { get; }

        int? EduProgramProfileId { get; }

        string Title { get; }

        string Group { get; }

        string Url { get; }

        int SortIndex { get; }

        DateTime? StartDate { get; }

        DateTime? EndDate { get; }

        DocumentTypeInfo DocumentType { get; }
    }

    public interface IDocumentWritable: IDocument
    {
        new int DocumentID { get; set; }

        new int DocumentTypeID { get; set; }

        new int? EduProgramId { get; set; }

        new int? EduProgramProfileId { get; set; }

        new string Title { get; set; }

        new string Group { get; set; }

        new string Url { get; set; }

        new int SortIndex { get; set; }

        new DateTime? StartDate { get; set; }

        new DateTime? EndDate { get; set; }

        new DocumentTypeInfo DocumentType { get; set; }
    }

    public delegate string GetDocumentTitle (IDocument document);
}

