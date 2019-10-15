//
//  DocumentEditModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2019 Roman M. Yagodin
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
using DotNetNuke.Common.Utilities;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.FileSystem;
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

        public override IEditModel<DocumentInfo> Create (DocumentInfo model, ViewModelContext viewContext)
        {
            var viewModel = new DocumentEditModel ();
            CopyCstor.Copy<IDocumentWritable> (model, viewModel);
            CopyCstor.Copy<IPublishableEntityWritable> (model, viewModel);
            CopyCstor.Copy<ITrackableEntityWritable> (model, viewModel);

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
        [Obsolete ("Use DocumentTypeViewModel property instead", true)] 
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
                var label = string.Empty;
                var title = string.Empty;

                if (Globals.GetURLType (Url) == TabType.File) {
                    var file = FileManager.Instance.GetFile (int.Parse (Url.ToUpperInvariant ().Replace ("FILEID=", "")));
                    if (file != null) {
                        label = file.FileName;
                        title = file.RelativePath;
                    }
                    else {
                        label = Context.LocalizeString ("FileNotFound.Text");
                    }
                }
                else if (Globals.GetURLType (Url) == TabType.Tab) {
                    var tab = TabController.Instance.GetTab (int.Parse (Url), Context.Module.PortalId);
                    if (tab != null) {
                        label = Context.LocalizeString ("Page.Text") + " " + tab.LocalizedTabName;
                        title = tab.TabPath.Replace ("//", "/");
                    }
                    else {
                        label = Context.LocalizeString ("PageNotFound.Text");
                    }
                }
                else {
                    label = HttpUtility.HtmlEncode (HtmlUtils.Shorten (Url, 25, "…"));
                    title = HttpUtility.HtmlAttributeEncode (Url);
                }

                var url = UniversityUrlHelper.LinkClickIdnHack (Url, Context.Module.TabId, Context.Module.ModuleId);
                return $"<a href={url} target=\"_blank\" title=\"{title}\">{label}</a>";
            }
        }

        #endregion
    }
}
