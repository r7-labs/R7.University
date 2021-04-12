using System;
using DotNetNuke.Common;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.FileSystem;
using R7.Dnn.Extensions.Text;

namespace R7.University.Utilities
{
    public class UniversityFileHelper
    {
        private static readonly Lazy<UniversityFileHelper> _instance = new Lazy<UniversityFileHelper> ();

        public static UniversityFileHelper Instance => _instance.Value;

        public IFileInfo GetFileByUrl (string url)
        {
            if (Globals.GetURLType (url) != TabType.File) {
                return null;
            }

            var fileId = ParseHelper.ParseToNullable<int> (url.ToLowerInvariant ().Replace ("fileid=", ""));
            if (fileId == null) {
                return null;
            }

            var file = FileManager.Instance.GetFile (fileId.Value);
            return file;
        }

        public IFileInfo GetSignatureFile (IFileInfo file)
        {
            var folder = FolderManager.Instance.GetFolder (file.FolderId);
            var sigFile = FileManager.Instance.GetFile (folder, file.FileName + ".sig");
            return sigFile;
        }
    }
}
