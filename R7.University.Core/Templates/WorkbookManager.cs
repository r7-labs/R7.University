using System;
using System.IO;
using System.Text;
using NPOI.SS.UserModel;

namespace R7.University.Templates
{
    public class WorkbookInfo
    {
        public string EntityType;

        public int? EntityId;
    }

    public class WorkbookManager
    {
        public string SerializeWorkbook (string filePath, WorkbookSerializationFormat format)
        {
            var workbookProvider = new HSSFWorkbookProvider ();
            var workbookSerializer = GetWorkbookSerializer (format);
            using (var fileStream = new FileStream (filePath, FileMode.Open, FileAccess.Read)) {
                return workbookSerializer.Serialize (workbookProvider.CreateWorkbook (fileStream), new StringBuilder ())
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

            throw new ArgumentException ("Unsupported serialization format!", nameof (format));
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

            // TODO: Create CellEnumerator!
            for (var s = 0; s < book.NumberOfSheets; s++) {
                var sheet = book.GetSheetAt (s);
                for (var r = sheet.FirstRowNum; r <= sheet.LastRowNum; r++) {
                    var row = sheet.GetRow (r);
                    if (row == null) {
                        continue;
                    }
                    foreach (var cell in row.Cells) {
                        var cellValue = formatter.FormatCellValue (cell);
                        if (cellValue == "Entity Type:") {
                            var nextCell = row.GetCell (cell.ColumnIndex + 1);
                            if (nextCell != null) {
                                bookInfo.EntityType = formatter.FormatCellValue (nextCell);
                            }
                        }
                        if (cellValue == "Entity ID:") {
                            var nextCell = row.GetCell (cell.ColumnIndex + 1);
                            if (nextCell != null) {
                                if (int.TryParse (formatter.FormatCellValue (nextCell), out int entityId)) {
                                    bookInfo.EntityId = entityId;
                                }
                            }
                        }
                    }
                }
            }

            return bookInfo;
        }
    }
}
