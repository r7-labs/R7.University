//
//  DocumentExtensions.cs
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
using R7.University.Models;

namespace R7.University.ModelExtensions
{
    public static class DocumentExtensions
    {
        public static bool IsPublished (this IDocument document)
        {
            return ModelHelper.IsPublished (document.StartDate, document.EndDate);
        }

        public static SystemDocumentType GetSystemDocumentType (this IDocument document)
        {
            SystemDocumentType result;
            return Enum.TryParse<SystemDocumentType> (document.DocumentType.Type, out result) ? result : SystemDocumentType.Custom;
        }

        public static void SetModelId (this IDocument document, string forModel, int modelId)
        {
            if (forModel == "EduProgram") {
                document.EduProgramId = modelId;
            } else if (forModel == "EduProgramProfile") {
                document.EduProgramProfileId = modelId;
            }
        }
    }
}

