using System;
using System.IO;
using System.Text;

namespace R7.University.Core.Templates
{
    public class WorkbookManager
    {
        public string SerializeWorkbook (string filePath, WorkbookSerializationFormat format)
        {
            if (format == WorkbookSerializationFormat.CSV) {
                var workbookProvider = new HSSFWorkbookProvider ();
                var workbookSerializer = new WorkbookToCsvSerializer ();

                using (var fileStream = new FileStream (filePath, FileMode.Open, FileAccess.Read)) {
                    return workbookSerializer.Serialize (workbookProvider.CreateWorkbook (fileStream), new StringBuilder ()).ToString ();
                }
            }

            throw new ArgumentException ("Unsupported serialization format!", nameof (format));
        }
    }
}
