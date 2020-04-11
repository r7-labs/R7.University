using System.Text;
using System.Text.RegularExpressions;
using NPOI.SS.UserModel;

namespace R7.University.Core.Templates
{
    /// <summary>
    /// Convert workbook (spreadsheet) into CSV-like representation.
    /// </summary>
    public class WorkbookToCsvSerializer: IWorkbookSerializer
    {
        public string CellSeparator { get; set; } = "\t";

        public string SheetHeaderFormat { get; set; } = "== {0}\n\n";

        public string EmptyCellValue { get; set; } = "~";

        public StringBuilder Serialize (IWorkbook book, StringBuilder builder)
        {
            for (var s = 0; s < book.NumberOfSheets; s++) {
                var sheet = book.GetSheetAt (s);
                builder.AppendFormat (SheetHeaderFormat, sheet.SheetName);
                SerializeSheet (sheet, builder);
            }

            return builder;
        }

        protected void SerializeSheet (ISheet sheet, StringBuilder builder)
        {
            for (var r = sheet.FirstRowNum; r <= sheet.LastRowNum; r++) {
                var row = sheet.GetRow (r);
                if (row == null) {
                    continue;
                }
                foreach (var cell in row.Cells) {
                    builder.Append (FormatCellValue (cell));
                    builder.Append (CellSeparator);
                }
                builder.AppendLine ();
            }
        }

        protected virtual string FormatCellValue (ICell cell)
        {
            var text = cell.StringCellValue;

            if (!string.IsNullOrWhiteSpace (text)) {
                text = text.Replace ("\r\n", " ");
                text = text.Replace ("\n\r", " ");
                text = text.Replace ("\n", " ");
                text = text.Replace ("\t", " ");
                text = Regex.Replace (text, " +", " ");
            }
            else {
                text = EmptyCellValue;
            }

            return text;
        }
    }
}
