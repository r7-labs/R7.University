using System.Collections.Generic;
using System.Web;
using R7.Dnn.Extensions.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.EduPrograms.ViewModels
{
    public abstract class EduProgramWithDocumentsViewModelBase: EduProgramViewModelBase
    {
        public EduProgramWithDocumentsViewModelBase (IEduProgram model) : base (model)
        {
        }

        protected abstract ViewModelContext Context { get; set; }

        protected IEnumerable<IDocument> GetDocuments (IEnumerable<IDocument> documents)
        {
            return documents.WherePublished (HttpContext.Current.Timestamp, Context.Module.IsEditable).OrderByGroupDescThenSortIndex ();
        }

        protected string FormatDocumentLinks (IEnumerable<IDocument> documents, string microdata)
        {
            return UniversityFormatHelper.FormatDocumentLinks (
                documents,
                Context,
                "<li class=\"list-inline-item\">{0}</li>",
                "<ul class=\"list-inline\">{0}</ul>",
                "<ul class=\"list-inline\">{0}</ul>",
                microdata,
                DocumentGroupPlacement.InTitle
            );
        }
    }
}
