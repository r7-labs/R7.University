using System;
using System.Web;
using Newtonsoft.Json;
using R7.Dnn.Extensions.Models;
using R7.Dnn.Extensions.ViewModels;
using R7.University.EditModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Utilities;

namespace R7.University.Controls.EditModels
{
    [Serializable]
    public class DocumentEditModel: EditModelBase<DocumentInfo>, IDocumentWritable
    {
        #region EditModelBase implementation

        public override IEditModel<DocumentInfo> Create (DocumentInfo model, ViewModelContext dnn)
        {
            var viewModel = new DocumentEditModel ();
            CopyCstor.Copy<IDocumentWritable> (model, viewModel);
            CopyCstor.Copy<IPublishableEntityWritable> (model, viewModel);
            CopyCstor.Copy<ITrackableEntityWritable> (model, viewModel);

            // FIXME: Context not updated for referenced viewmodels
            viewModel.DocumentTypeViewModel = new DocumentTypeViewModel (model.DocumentType, dnn);
            viewModel.Dnn = dnn;

            return viewModel;
        }

        public override DocumentInfo CreateModel ()
        {
            var model = new DocumentInfo ();
            CopyCstor.Copy<IDocumentWritable> (this, model);
            CopyCstor.Copy<IPublishableEntityWritable> (this, model);
            CopyCstor.Copy<ITrackableEntityWritable> (this, model);

            return model;
        }

        public override void SetTargetItemId (int targetItemId, string targetItemKey)
        {
            this.SetModelId ((ModelType) Enum.Parse (typeof (ModelType), targetItemKey), targetItemId);
        }

        [JsonIgnore]
        public override bool IsPublished => this.IsPublished (HttpContext.Current.Timestamp);

        #endregion

        #region IDocumentWritable implementation

        public int DocumentID { get; set; }

        public int DocumentTypeID { get; set; }

        [JsonIgnore]
        [Obsolete ("Don't use this property directly", true)]
        public IDocumentType DocumentType { get; set; }

        public DocumentTypeViewModel DocumentTypeViewModel { get; set; }

        public int? EduProgramId { get; set; }

        public int? EduProgramProfileId { get; set; }

        public string Title { get; set; }

        public string Group { get; set; }

        public string Url { get; set; }

        public int SortIndex { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public int LastModifiedByUserId { get; set; }

        public DateTime LastModifiedOnDate { get; set; }

        public int CreatedByUserId { get; set; }

        public DateTime CreatedOnDate { get; set; }

        #endregion

        #region Derieved properties

        [JsonIgnore]
        public string LocalizedType
        {
            get {
                return LocalizationHelper.GetStringWithFallback (
                    "SystemDocumentType_" + DocumentTypeViewModel.Type + ".Text", Dnn.LocalResourceFile, DocumentTypeViewModel.Type
                );
            }
        }

        [JsonIgnore]
        public string FormattedUrl =>
            UniversityUrlHelper.FormatNiceDocumentUrl (Url, Dnn.Module.ModuleId, Dnn.Module.TabId,
                Dnn.Module.PortalId, Dnn.LocalResourceFile);

        [JsonIgnore]
        public string StartEndDates {
            get {
                if (StartDate == null && EndDate == null) {
                    return string.Empty;
                }

                return $"{StartDate?.ToShortDateString () ?? "-"} / {EndDate?.ToShortDateString () ?? "-"}";
            }
        }

        [JsonIgnore]
        public string SignatureLink {
            get {
                var documentFile = UniversityFileHelper.Instance.GetFileByUrl (Url);
                if (documentFile == null) {
                    return string.Empty;
                }

                var sigFile = UniversityFileHelper.Instance.GetSignatureFile (documentFile);
                if (sigFile == null) {
                    return string.Empty;
                }

                return
                    $"<a href=\"{UniversityUrlHelper.LinkClickFile (sigFile.FileId, Dnn.Module.TabId, Dnn.Module.ModuleId)}\""
                    + $" title=\"{Dnn.LocalizeString("SignatureLink_Tooltip.Text")}\"><i class=\"fas fa-signature\"></i></a>";
            }
        }

        #endregion
    }
}
