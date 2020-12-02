using System;
using System.Collections.Generic;
using System.Linq;
using R7.Dnn.Extensions.Collections;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.ModelExtensions
{
    public static class EduProfileExtensions
    {
        // TODO: Extend IDocument instead, rename to WhereDocumentType
        public static IEnumerable<IDocument> GetDocumentsOfType (this IEduProfile eduProgramProfile, SystemDocumentType documentType)
        {
            return eduProgramProfile.Documents.Where (d => d.GetSystemDocumentType () == documentType);
        }

        public static bool IsOpenForAdmission (this IEduProfile eduProfile, DateTime now, bool inEditMode)
        {
            return !eduProfile.EduProgramProfileFormYears
                .Where (eppfy => eppfy.Year != null && eppfy.Year.AdmissionIsOpen && (eppfy.IsPublished (now) || inEditMode))
                .IsNullOrEmpty ();
        }

        public static string FormatTitle (this IEduProfile epp, bool withEduProgramCode = true)
        {
            if (withEduProgramCode) {
                return UniversityFormatHelper.FormatEduProfileTitle (
                    epp.EduProgram.Code,
                    epp.EduProgram.Title,
                    epp.ProfileCode,
                    epp.ProfileTitle
                );
            }

            return UniversityFormatHelper.FormatEduProfileTitle (
                epp.EduProgram.Title,
                epp.ProfileCode,
                epp.ProfileTitle
            );
        }
    }
}
