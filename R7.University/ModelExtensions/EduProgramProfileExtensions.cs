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
using R7.DotNetNuke.Extensions.Data;
using R7.University.Data;
using R7.University.Models;

namespace R7.University.ModelExtensions
{
    public static class EduProgramProfileExtensions
    {
        [Obsolete]
        public static EduProgramProfileInfo WithEduProgram (
            this EduProgramProfileInfo eduProfile, Dal2DataProvider controller)
        {
            eduProfile.EduProgram = controller.Get<EduProgramInfo> (eduProfile.EduProgramID);

            return eduProfile;
        }

        public static EduProgramProfileInfo WithEduProgram (
            this EduProgramProfileInfo eduProfile)
        {
            eduProfile.EduProgram = UniversityRepository.Instance.DataProvider.Get<EduProgramInfo> (eduProfile.EduProgramID);

            return eduProfile;
        }

        [Obsolete]
        public static IEnumerable<EduProgramProfileInfo> WithEduProgram (
            this IEnumerable<EduProgramProfileInfo> eduProgramProfiles, Dal2DataProvider controller)
        {
            var eduPrograms = controller.GetObjects<EduProgramInfo> ();

            return eduProgramProfiles.Join (eduPrograms, epp => epp.EduProgramID, ep => ep.EduProgramID, 
                delegate (EduProgramProfileInfo epp, EduProgramInfo ep) {
                    epp.EduProgram = ep;
                    return epp;
                }
            );
        }

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

        public static IEnumerable<IEduProgramProfile> WithEduLevel (
            this IEnumerable<IEduProgramProfile> eduProgramProfiles, IEnumerable<IEduLevel> eduLevels)
        {
            foreach (var epp in eduProgramProfiles) {
                epp.EduLevel = eduLevels.First (el => el.EduLevelID == epp.EduLevelId);
            }
        
            return eduProgramProfiles;
        }

        public static IEduProgramProfile WithEduProgramProfileForms (
            this IEduProgramProfile eduProfile, Dal2DataProvider controller)
        {
            eduProfile.EduProgramProfileForms = controller.GetObjects<EduProgramProfileFormInfo> (
                "WHERE [EduProgramProfileID] = @0", eduProfile.EduProgramProfileID)
                .WithEduForms (controller)
                .Cast<IEduProgramProfileForm> ()
                .ToList ();

            return eduProfile;
        }

        public static IEnumerable<IEduProgramProfile> WithEduProgramProfileForms (
            this IEnumerable<IEduProgramProfile> eduProgramProfiles, Dal2DataProvider controller)
        {
            foreach (var eduProgramProfile in eduProgramProfiles) {
                yield return eduProgramProfile.WithEduProgramProfileForms (controller);
            }
        }

        public static IEnumerable<IEduProgramProfile> WithDocuments (
            this IEnumerable<IEduProgramProfile> eduProgramProfiles, IEnumerable<IDocument> documents)
        {
            return eduProgramProfiles.GroupJoin (documents.DefaultIfEmpty (), epp => "EduProgramProfileID=" + epp.EduProgramProfileID, d => d.ItemID,
                (epp, docs) => {
                    epp.Documents = docs.ToList ();
                    return epp;
                }
            );
        }

        public static IEnumerable<IEduProgramProfile> WithDocumentType (
            this IEnumerable<IEduProgramProfile> eduProgramProfiles, IEnumerable<IDocumentType> documentTypes)
        {
            foreach (var eduProgramProfile in eduProgramProfiles) {
                eduProgramProfile.Documents = eduProgramProfile.Documents.WithDocumentType (documentTypes).ToList ();
            }

            return eduProgramProfiles;
        }

        public static IEnumerable<IDocument> GetDocumentsOfType (this IEduProgramProfile eduProgramProfile, SystemDocumentType documentType)
        {
            return eduProgramProfile.Documents.Where (d => d.GetSystemDocumentType () == documentType);
        }
    }
}
