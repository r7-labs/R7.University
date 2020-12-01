using System.Collections.Generic;
using System.Text;
using System.Web;
using DotNetNuke.Common;
using R7.Dnn.Extensions.Collections;
using R7.Dnn.Extensions.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.EduPrograms.ViewModels
{
    internal class EduProgramStandardObrnadzorViewModel: EduProgramViewModelBase
    {
        public IIndexer Indexer { get; protected set; }

        public ViewModelContext Context { get; protected set; }

        public EduProgramStandardObrnadzorViewModel (IEduProgram model, ViewModelContext context, IIndexer indexer)
            : base (model)
        {
            Context = context;
            Indexer = indexer;
        }

        protected IEnumerable<IDocument> GetDocuments (IEnumerable<IDocument> documents)
        {
            return documents.WherePublished (HttpContext.Current.Timestamp, Context.Module.IsEditable).OrderByGroupDescThenSortIndex ();
        }

        public int Order
        {
            get { return Indexer.GetNextIndex (); }
        }

        public string Title_Link
        {
            get {
                if (!string.IsNullOrWhiteSpace (EduProgram.HomePage)) {
                    return string.Format ("<a href=\"{0}\" target=\"_blank\">{1}</a>",
                        Globals.NavigateURL (int.Parse (EduProgram.HomePage)),
                        EduProgram.Title);
                }

                return EduProgram.Title;
            }
        }

        public string EduLevel_String
        {
            get { return UniversityFormatHelper.FormatShortTitle (EduLevel.ShortTitle, EduLevel.Title); }
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

        public string Standard_Links {
            get {
                var sb = new StringBuilder ();
                var eduStandardDocs = GetDocuments (EduProgram.GetDocumentsOfType (SystemDocumentType.EduStandard));

                if (!eduStandardDocs.IsNullOrEmpty ()) {
                    sb.AppendFormat ("<em>{0}</em>", Context.LocalizeString ("EduStandards.Text"));
                    sb.Append (FormatDocumentLinks (eduStandardDocs, "itemprop=\"eduFedDoc\""));
                }

                var profStandardDocs = GetDocuments (EduProgram.GetDocumentsOfType (SystemDocumentType.ProfStandard));
                if (!profStandardDocs.IsNullOrEmpty ()) {
                    sb.AppendFormat ("<em>{0}</em>", Context.LocalizeString ("ProfStandards.Text"));
                    sb.Append (FormatDocumentLinks (profStandardDocs, string.Empty));
                }

                return sb.ToString ();
            }
        }
    }
}

