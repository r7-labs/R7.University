using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using R7.Dnn.Extensions.Collections;
using R7.Dnn.Extensions.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.EduPrograms.ViewModels
{
    public class EduProgramViewModel: EduProgramWithDocumentsViewModelBase
    {
        public EduProgramModuleViewModel RootViewModel { get; protected set; }

        protected override ViewModelContext Context
        {
            get { return RootViewModel.Context; }
            set { throw new InvalidOperationException (); }
        }

        public EduProgramViewModel (IEduProgram model, EduProgramModuleViewModel rootViewModel): base (model)
        {
            RootViewModel = rootViewModel;
        }

        #region Bindable properties

        public string Title_String => UniversityFormatHelper.FormatEduProgramTitle (EduProgram.Code, EduProgram.Title);

        public string EduLevel_Title => EduProgram.EduLevel.Title;

        public bool StateEduStandards_Visible =>
            !GetDocuments (EduProgram.GetDocumentsOfType (SystemDocumentType.StateEduStandard)).IsNullOrEmpty ();

        public bool EduStandards_Visible =>
            !GetDocuments (EduProgram.GetDocumentsOfType (SystemDocumentType.EduStandard)).IsNullOrEmpty ();

        public bool ProfStandards_Visible =>
            !GetDocuments (EduProgram.GetDocumentsOfType (SystemDocumentType.ProfStandard)).IsNullOrEmpty ();

        public string StateEduStandard_Links {
            get {
                var stateEduStandardDocs = GetDocuments (EduProgram.GetDocumentsOfType (SystemDocumentType.StateEduStandard));
                return FormatDocumentLinks (stateEduStandardDocs, string.Empty);
            }
        }

        public string EduStandard_Links {
            get {
                var eduStandardDocs = GetDocuments (EduProgram.GetDocumentsOfType (SystemDocumentType.EduStandard));
                return FormatDocumentLinks (eduStandardDocs, string.Empty);
            }
        }

        public string ProfStandard_Links {
            get {
                var profStandardDocs = GetDocuments (EduProgram.GetDocumentsOfType (SystemDocumentType.ProfStandard));
                return FormatDocumentLinks (profStandardDocs, string.Empty);
            }
        }

        public string Edit_Url
        {
            get {
                return Context.Module.EditUrl (
                    "eduprogram_id",
                    EduProgram.EduProgramID.ToString (),
                    "EditEduProgram"
                );
            }
        }

        public string CssClass => EduProgram.IsPublished (HttpContext.Current.Timestamp) ? string.Empty : "u8y-not-published";

        public bool EduProfiles_Visible => EduProfileViewModels
            .Any (epp => epp.IsPublished (HttpContext.Current.Timestamp) || Context.Module.IsEditable);

        public IEnumerable<EduProfileViewModel> EduProfileViewModels {
            get {
                var now = HttpContext.Current.Timestamp;
                return EduProgram.EduProfiles
                                 .Where (epp => epp.IsPublished (now) || Context.Module.IsEditable)
                                 .OrderByDescending (epp => epp.IsOpenForAdmission (now, Context.Module.IsEditable))
                                 .ThenBy (epp => epp.ProfileCode)
                                 .ThenBy (epp => epp.ProfileTitle)
                                 .ThenBy (epp => epp.EduLevel.SortIndex)
                                 .Select (epp => new EduProfileViewModel (epp, RootViewModel));
            }
        }

        public bool DivisionsVisible
        {
            get {
                var now = HttpContext.Current.Timestamp;
                return EduProgram.Divisions.Any (epd => epd.Division.IsPublished (now) || Context.Module.IsEditable);
            }
        }

        public IEnumerable<EduProgramDivisionViewModel> DivisionViewModels {
            get {
                var now = HttpContext.Current.Timestamp;
                return EduProgram.Divisions
                                 .Where (epd => epd.Division.IsPublished (now) || Context.Module.IsEditable)
                                 .Select (epd => new EduProgramDivisionViewModel (epd));
            }
        }

        #endregion
    }
}

