//
//  DocumentExtensions.cs
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
using R7.University.Models;

namespace R7.University.ModelExtensions
{
    public static class DocumentExtensions
    {
        public static SystemDocumentType GetSystemDocumentType (this IDocument document)
        {
            SystemDocumentType result;
            return Enum.TryParse<SystemDocumentType> (document.DocumentType.Type, out result) ? result : SystemDocumentType.Custom;
        }

        public static void SetModelId (this IDocumentWritable document, ModelType modelType, int modelId)
        {
            if (modelType == ModelType.EduProgram) {
                document.EduProgramId = modelId;
            } 
            else if (modelType == ModelType.EduProgramProfile) {
                document.EduProgramProfileId = modelId;
            }
            else {
                throw new ArgumentException ($"Wrong modelType={modelType} argument.");
            }
        }

        public static IEnumerable<IDocument> WherePublished (this IEnumerable<IDocument> documents, DateTime now, bool isEditable)
        {
            return documents.Where (d => isEditable || d.IsPublished (now));
        }

        public static IEnumerable<IDocument> OrderByGroupDescThenSortIndex (this IEnumerable<IDocument> documents)
        {
            return documents.OrderByDescending (d => d.Group, DocumentGroupComparer.Instance).ThenBy (d => d.SortIndex).ThenBy (d => d.Title);
        }
    }
}

