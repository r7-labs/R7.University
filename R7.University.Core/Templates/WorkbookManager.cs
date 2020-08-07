using System;
using System.IO;
using System.Text;

namespace R7.University.Core.Templates
{
    public class WorkbookManager
    {
        public string SerializeWorkbook (string filePath, WorkbookSerializationFormat format)
        {
            var workbookProvider = new HSSFWorkbookProvider ();
            var workbookSerializer = GetWorkbookSerializer (format);
            using (var fileStream = new FileStream (filePath, FileMode.Open, FileAccess.Read)) {
                return workbookSerializer.Serialize (workbookProvider.CreateWorkbook (fileStream), new StringBuilder ()).ToString ();
            }
        }

        IWorkbookSerializer GetWorkbookSerializer (WorkbookSerializationFormat format)
        {
            if (format == WorkbookSerializationFormat.LinearCSV) {
                return new WorkbookToLinearCsvSerializer ();
            }
            if (format == WorkbookSerializationFormat.CSV) {
                return new WorkbookToCsvSerializer ();
            }
            throw new ArgumentException ("Unsupported serialization format!", nameof (format));
        }
    }
}
