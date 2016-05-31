//
// EduProgramProfileExtensions.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015 Roman M. Yagodin
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

        public static IEnumerable<EduProgramProfileInfo> WithEduLevel (
            this IEnumerable<EduProgramProfileInfo> eduProgramProfiles, IEnumerable<IEduLevel> allEduLevels)
        {
            eduProgramProfiles.Select (epp => epp.EduProgram).WithEduLevel (allEduLevels);
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

        public static IEduProgramProfile WithDocuments (
            this IEduProgramProfile eduProgramProfile, 
            IEnumerable<IDocumentType> documentTypes, 
            Dal2DataProvider controller)
        {
            eduProgramProfile.Documents = DocumentRepository.Instance.GetDocuments (
                "EduProgramProfileID=" + eduProgramProfile.EduProgramProfileID)
                .ToList ();
            
            eduProgramProfile.Documents.WithDocumentType (documentTypes);

            return eduProgramProfile;
        }

        public static IEnumerable<IEduProgramProfile> WithDocuments (
            this IEnumerable<IEduProgramProfile> eduProgramProfiles, IEnumerable<IDocumentType> documentTypes, Dal2DataProvider controller)
        {
            foreach (var eduProgramProfile in eduProgramProfiles) {
                yield return eduProgramProfile.WithDocuments (documentTypes, controller);
            }
        }

        public static IEnumerable<IDocument> GetDocumentsOfType (this IEduProgramProfile eduProgramProfile, SystemDocumentType documentType)
        {
            return eduProgramProfile.Documents.Where (d => d.GetSystemDocumentType () == documentType);
        }
    }
}
