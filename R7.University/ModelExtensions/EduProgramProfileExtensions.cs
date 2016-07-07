//
//  EduProgramProfileExtensions.cs
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
using R7.University.Data;
using R7.University.Models;

namespace R7.University.ModelExtensions
{
    public static class EduProgramProfileExtensions
    {
        public static IEnumerable<EduProgramProfileInfo> WithEduProgram (
            this IEnumerable<EduProgramProfileInfo> eduProgramProfiles)
        {
            var eduPrograms = UniversityRepository.Instance.DataProvider.GetObjects<EduProgramInfo> ();

            return eduProgramProfiles.Join (eduPrograms, epp => epp.EduProgramID, ep => ep.EduProgramID, 
                delegate (EduProgramProfileInfo epp, EduProgramInfo ep) {
                    epp.EduProgram = ep;
                    return epp;
                }
            );
        }

        public static IEnumerable<IDocument> GetDocumentsOfType (this IEduProgramProfile eduProgramProfile, SystemDocumentType documentType)
        {
            return eduProgramProfile.Documents.Where (d => d.GetSystemDocumentType () == documentType);
        }

        public static bool IsPublished (this IEduProgramProfile eduProgramProfile)
        {
            return ModelHelper.IsPublished (eduProgramProfile.StartDate, eduProgramProfile.EndDate);
        }
    }
}
