using System.Text;
using DotNetNuke.Common;
using R7.Dnn.Extensions.Collections;
using R7.Dnn.Extensions.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.EduPrograms.ViewModels
{
    internal class EduProgramStandardsViewModel: EduProgramWithDocumentsViewModelBase
    {
        public IIndexer Indexer { get; protected set; }

        protected override ViewModelContext Context { get; set; }

        public EduProgramStandardsViewModel (IEduProgram model, ViewModelContext context, IIndexer indexer)
            : base (model)
        {
            Context = context;
            Indexer = indexer;
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

        public string Standard_Links {
            get {
                var sb = new StringBuilder ();

                var stateEduStandardDocs = GetDocuments (EduProgram.GetDocumentsOfType (SystemDocumentType.StateEduStandard));
                if (!stateEduStandardDocs.IsNullOrEmpty ()) {
                    sb.AppendFormat ("<em>{0}</em>", Context.LocalizeString ("StateEduStandards.Text"));
                    sb.Append (RenderDocumentsList (stateEduStandardDocs, "itemprop=\"eduFedDoc\""));
                }

                var eduStandardDocs = GetDocuments (EduProgram.GetDocumentsOfType (SystemDocumentType.EduStandard));
                if (!eduStandardDocs.IsNullOrEmpty ()) {
                    sb.AppendFormat ("<em>{0}</em>", Context.LocalizeString ("EduStandards.Text"));
                    sb.Append (RenderDocumentsList (eduStandardDocs, "itemprop=\"eduStandartDoc\""));
                }

                var profStandardDocs = GetDocuments (EduProgram.GetDocumentsOfType (SystemDocumentType.ProfStandard));
                if (!profStandardDocs.IsNullOrEmpty ()) {
                    sb.AppendFormat ("<em>{0}</em>", Context.LocalizeString ("ProfStandards.Text"));
                    sb.Append (RenderDocumentsList (profStandardDocs, string.Empty));
                }

                return sb.ToString ();
            }
        }
    }
}

