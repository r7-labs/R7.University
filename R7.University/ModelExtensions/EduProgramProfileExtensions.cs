//
//  EduProgramProfileExtensions.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
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
using R7.University.ViewModels;

namespace R7.University.ModelExtensions
{
    public static class EduProgramProfileExtensions
    {
        // TODO: Extend IDocument instead, rename to WhereDocumentType
        public static IEnumerable<IDocument> GetDocumentsOfType (this IEduProgramProfile eduProgramProfile, SystemDocumentType documentType)
        {
            return eduProgramProfile.Documents.Where (d => d.GetSystemDocumentType () == documentType);
        }

        public static bool IsPublished (this IEduProgramProfile eduProgramProfile, DateTime now)
        {
            return ModelHelper.IsPublished (now, eduProgramProfile.StartDate, eduProgramProfile.EndDate);
        }

        public static string FormatTitle (this IEduProgramProfile epp)
        {
            return FormatHelper.FormatEduProgramProfileTitle (
                epp.EduProgram.Code,
                epp.EduProgram.Title,
                epp.ProfileCode,
                epp.ProfileTitle
            );
        }
    }
}
