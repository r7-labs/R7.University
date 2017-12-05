//
//  DocumentEditModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2017 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
//
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Web;
using DotNetNuke.Common;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.FileSystem;
using DotNetNuke.Services.Localization;
using Newtonsoft.Json;
using R7.Dnn.Extensions.Utilities;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Controls.ViewModels;
using R7.University.EditModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Utilities;
using R7.University.ViewModels;

namespace R7.University.Controls.EditModels
{
    [Serializable]
    public class DocumentEditModel: EditModelBase<DocumentInfo>, IDocumentWritable
    {
        #region EditModelBase implementation

        public override IEditModel<DocumentInfo> Create (DocumentInfo model, ViewModelContext viewContext)
        {
            var viewModel = new DocumentEditModel ();
            CopyCstor.Copy<IDocumentWritable> (model, viewModel);
            CopyCstor.Copy<IPublishableEntityWritable> (model, viewModel);

            // FIXME: Context not updated for referenced viewmodels
            viewModel.DocumentTypeViewModel = new DocumentTypeViewModel (model.DocumentType, viewContext);
            viewModel.Context = viewContext;

            return viewModel;
        }

        public override DocumentInfo CreateModel ()
        {
            var model = new DocumentInfo ();
            CopyCstor.Copy<IDocumentWritable> (this, model);
            CopyCstor.Copy<IPublishableEntityWritable> (this, model);

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
        [Obsolete ("Use DocumentTypeViewModel property instead", true)] 
        public DocumentTypeInfo DocumentType { get; set; }

        public DocumentTypeViewModel DocumentTypeViewModel { get; set; }

        public int? EduProgramId { get; set; }

        public int? EduProgramProfileId { get; set; }

        public string Title { get; set; }

        public string Group { get; set; }

        public string Url { get; set; }

        public int SortIndex { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        #endregion

        #region Bindable properties

        [JsonIgnore]
        public string LocalizedType
        { 
            get {
                return LocalizationHelper.GetStringWithFallback (
                    "SystemDocumentType_" + DocumentTypeViewModel.Type + ".Text", Context.LocalResourceFile, DocumentTypeViewModel.Type
                );
            }
        }

        [JsonIgnore]
        public string FormattedUrl
        {
            get { 
                if (!string.IsNullOrWhiteSpace (Url)) {
                    return string.Format ("<a href=\"{0}\" target=\"_blank\">{1}</a>",
                        UniversityUrlHelper.LinkClickIdnHack (Url, Context.Module.TabId, Context.Module.ModuleId),
                        Localization.GetString ("DocumentUrlLabel.Text", Context.LocalResourceFile)
                    );
                }

                return string.Empty;
            }
        }

        [JsonIgnore]
        public string FileNameWithPathRaw
        {
            get {
                if (Globals.GetURLType (Url) == TabType.File) {
                    var file = FileManager.Instance.GetFile (int.Parse (Url.ToUpperInvariant ().Replace ("FILEID=","")));
                    if (file != null) {
                        return $"<span title=\"{file.RelativePath}\">{file.FileName}</span>";
                    }
                }

                return string.Empty;
            }
        }

        #endregion
    }
}
