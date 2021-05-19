using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using R7.Dnn.Extensions.Text;
using R7.Dnn.Extensions.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.UI;
using R7.University.Utilities;
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

        protected string RenderDocumentsList (IEnumerable<IDocument> documents, string microdata)
        {
            return RenderDocumentsList (
                documents,
                "<ul class=\"list-inline\">{0}</ul>",
                "<li class=\"list-inline-item\">{0}</li>",
                microdata
            );
        }

        string RenderDocumentsList (IEnumerable<IDocument> documents, string listTemplate, string itemTemplate, string microdata)
        {
            var markupBuilder = new StringBuilder ();
            foreach (var document in documents) {
                var linkMarkup = RenderDocumentLink (document,
                    document.Title,
                    Context.LocalizeString ("LinkOpen.Text"),
                    microdata,
                    HttpContext.Current.Timestamp
                );

                if (!string.IsNullOrEmpty (linkMarkup)) {
                    markupBuilder.Append (string.Format (itemTemplate, linkMarkup));
                }
            }

            var markup = markupBuilder.ToString ();
            if (!string.IsNullOrEmpty (markup)) {
                return string.Format (listTemplate, markup);
            }

            return string.Empty;
        }

        string RenderDocumentLink (IDocument document, string documentTitle,
            string defaultTitle, string microdata, DateTime now)
        {
            var title = !string.IsNullOrWhiteSpace (documentTitle)
                ? FormatHelper.JoinNotNullOrEmpty (": ", document.Group, documentTitle)
                : defaultTitle;

            if (!string.IsNullOrWhiteSpace (document.Url)) {
                var fa = FontAwesomeHelper.Instance;
                var documentFile = UniversityFileHelper.Instance.GetFileByUrl (document.Url);

                if (documentFile == null) {
                    return $" <a href=\"{UniversityUrlHelper.LinkClick (document.Url, Context.Module.TabId, Context.Module.ModuleId)}\" "
                        + FormatHelper.JoinNotNullOrEmpty (" ", !document.IsPublished (now) ? "class=\"u8y-not-published-element\"" : string.Empty, microdata)
                        + $" target=\"_blank\">{title}</a>";
                }

                var linkMarkup =
                    $"<i class=\"fas fa-file-{fa.GetBaseIconNameByExtension(documentFile.Extension)}\""
                    + $"style=\"color:{fa.GetBrandColorByExtension(documentFile.Extension)}\"></i>"
                    + $" <a href=\"{UniversityUrlHelper.LinkClick (document.Url, Context.Module.TabId, Context.Module.ModuleId)}\" "
                    + FormatHelper.JoinNotNullOrEmpty (" ", !document.IsPublished (now) ? "class=\"u8y-not-published-element\"" : string.Empty, microdata)
                    + $" target=\"_blank\">{title}</a>";

                var sigFile = UniversityFileHelper.Instance.GetSignatureFile (documentFile);
                if (sigFile != null) {
                    linkMarkup += "<span> + </span>"
                                  + $"<a href=\"{UniversityUrlHelper.LinkClickFile (sigFile.FileId, Context.Module.TabId, Context.Module.ModuleId)}\" "
                                  + $"title=\"{Context.LocalizeString ("Signature.Text")}\"><i class=\"fas fa-signature\"></i></a>";
                }

                return linkMarkup;
            }

            return string.Empty;
        }
    }
}
