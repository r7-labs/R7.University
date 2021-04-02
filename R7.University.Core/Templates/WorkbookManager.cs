using System;
using System.IO;
using System.Text;
using NPOI.SS.UserModel;

namespace R7.University.Templates
{
    public class WorkbookManager
    {
        public string SerializeWorkbook (string filePath, IWorkbookSerializer serializer)
        {
            var workbookProvider = new HSSFWorkbookProvider ();
            using (var fileStream = new FileStream (filePath, FileMode.Open, FileAccess.Read)) {
                return serializer.Serialize (workbookProvider.CreateWorkbook (fileStream), new StringBuilder ())
                    .ToString ();
            }
        }

        public IWorkbookSerializer GetWorkbookSerializer (WorkbookSerializationFormat format)
        {
            if (format == WorkbookSerializationFormat.LinearCSV) {
                return new WorkbookToLinearCsvSerializer ();
            }

            if (format == WorkbookSerializationFormat.LinearCSV_270) {
                return new WorkbookToLinearCsvSerializer_270 ();
            }

            if (format == WorkbookSerializationFormat.CSV) {
                return new WorkbookToCsvSerializer ();
            }

            return null;
        }

        public IWorkbookSerializer GetWorkbookSerializer (string format)
        {
            var enumFormat = (WorkbookSerializationFormat) Enum.Parse (typeof (WorkbookSerializationFormat), format);
            return GetWorkbookSerializer (enumFormat);
        }

        public WorkbookInfo ReadWorkbookInfo (string filePath)
        {
            var book = LoadWorkbook (filePath);
            if (book != null) {
                return ReadWorkbookInfo (book);
            }
            return null;
        }

        IWorkbook LoadWorkbook (string filePath)
        {
            var workbookProvider = new HSSFWorkbookProvider ();
            using (var fileStream = new FileStream (filePath, FileMode.Open, FileAccess.Read)) {
                return workbookProvider.CreateWorkbook (fileStream);
            }
        }

        WorkbookInfo ReadWorkbookInfo (IWorkbook book)
        {
            var formatter = new DataFormatter ();
            var bookInfo = new WorkbookInfo ();
            var cellEnumerator = new WorkbookCellEnumerator ();

            foreach (var cell in cellEnumerator.GetCells (book)) {
                var cellValue = formatter.FormatCellValue (cell);
                if (cellValue == "Entity Type:") {
                    var nextCell = GetNextCell (cell);
                    if (nextCell != null) {
                        bookInfo.EntityType = formatter.FormatCellValue (nextCell);
                    }
                }
                else if (cellValue == "Entity ID:") {
                    var nextCell = GetNextCell (cell);
                    if (nextCell != null) {
                        if (int.TryParse (formatter.FormatCellValue (nextCell), out int entityId)) {
                            bookInfo.EntityId = entityId;
                        }
                    }
                }
            }

            return bookInfo;
        }

        ICell GetNextCell (ICell cell)
        {
            return cell.Row.GetCell (cell.ColumnIndex + 1);
        }
    }
}
