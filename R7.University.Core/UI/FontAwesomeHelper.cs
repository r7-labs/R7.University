using System;

namespace R7.University.UI
{
    public class FontAwesomeHelper
    {
        private static readonly Lazy<FontAwesomeHelper> _instance = new Lazy<FontAwesomeHelper> ();

        public static FontAwesomeHelper Instance => _instance.Value;

        public string GetBaseIconNameByExtension (string ext)
        {
            switch (ext.ToUpperInvariant ()) {
                case "PDF":
                    return "pdf";

                case "DOC":
                case "DOCX":
                case "ODT":
                    return "word";

                case "XLS":
                case "XLSX":
                case "ODS":
                    return "excel";

                case "PPT":
                case "PPTX":
                case "ODP":
                    return "powerpoint";

                case "ZIP":
                case "7Z":
                case "RAR":
                    return "archive";
            }

            return "alt";
        }

        public string GetBrandColorByExtension (string ext)
        {
            switch (ext.ToUpperInvariant ()) {
                case "PDF":
                    return "red";

                case "DOC":
                case "DOCX":
                case "ODT":
                    return "blue";

                case "XLS":
                case "XLSX":
                case "ODS":
                    return "green";

                case "PPT":
                case "PPTX":
                case "ODP":
                    return "orange";

                case "ZIP":
                case "7Z":
                case "RAR":
                    return "lighbrown";
            }

            return "#000";
        }
    }
}
