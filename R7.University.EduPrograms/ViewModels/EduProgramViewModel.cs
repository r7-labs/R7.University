using System.Collections.Generic;
using System.Linq;
using System.Web;
using R7.Dnn.Extensions.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.EduPrograms.ViewModels
{
    public class EduProgramViewModel: EduProgramViewModelBase
    {
        public EduProgramModuleViewModel RootViewModel { get; protected set; }

        protected ViewModelContext Context
        {
            get { return RootViewModel.Context; }
        }

        public EduProgramViewModel (IEduProgram model, EduProgramModuleViewModel rootViewModel): base (model)
        {
            RootViewModel = rootViewModel;
        }

        protected IEnumerable<IDocument> GetDocuments (IEnumerable<IDocument> documents)
        {
            return documents.WherePublished (HttpContext.Current.Timestamp, Context.Module.IsEditable).OrderByGroupDescThenSortIndex ();
        }

        #region Bindable properties

        public string Title_String
        {
            get { return UniversityFormatHelper.FormatEduProgramTitle (EduProgram.Code, EduProgram.Title); }
        }

        public string EduLevel_Title
        {
            get { return EduProgram.EduLevel.Title; }
        }

        public bool EduStandard_Visible
        {
            get { return GetDocuments (EduProgram.GetDocumentsOfType (SystemDocumentType.EduStandard)).Any (); }
        }

        public string EduStandard_Links
        {
            get {
                return UniversityFormatHelper.FormatDocumentLinks (
                    GetDocuments (EduProgram.GetDocumentsOfType (SystemDocumentType.EduStandard)),
                    Context,
                    "<li>{0}</li>",
                    "<ul class=\"list-inline u8y-inline\">{0}</ul>",
                    "<ul class=\"list-inline u8y-inline\">{0}</ul>",
                    string.Empty,
                    DocumentGroupPlacement.InTitle
                );
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

        public string CssClass
        {
            get {
                return EduProgram.IsPublished (HttpContext.Current.Timestamp) ? string.Empty : "u8y-not-published";
            }
        }

        public bool EduProfiles_Visible
        {
            get {
                return EduProgramProfileViewModels.Any (epp => epp.IsPublished (HttpContext.Current.Timestamp)
                    || Context.Module.IsEditable
                );
            }
        }

        public IEnumerable<EduProfileViewModel> EduProgramProfileViewModels {
            get {
                var now = HttpContext.Current.Timestamp;
                return EduProgram.EduProfiles
                                 .Where (epp => epp.IsPublished (now) || Context.Module.IsEditable)
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

