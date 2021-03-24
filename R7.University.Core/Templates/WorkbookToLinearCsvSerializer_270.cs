using System;
using System.Collections.Generic;
using System.Text;
using NPOI.SS.UserModel;

namespace R7.University.Templates
{
    [Obsolete]
    public class WorkbookToLinearCsvSerializer_270: WorkbookToCsvSerializer
    {
        public int NumOfHeaderRows { get; set; } = 2;

        public string TableWasProcessedToken { get; set; } = "##";

        public override StringBuilder Serialize (IWorkbook book, StringBuilder builder)
        {
            if (NumOfHeaderRows < 1) {
                throw new ArgumentOutOfRangeException ("Argument must be greater than 0", nameof (NumOfHeaderRows));
            }
            if (string.IsNullOrWhiteSpace (TableWasProcessedToken)) {
                throw new ArgumentException ("Argument must not be null, empty nor whitespace", nameof (TableWasProcessedToken));
            }

            for (var s = 0; s < book.NumberOfSheets; s++) {
                var sheet = book.GetSheetAt (s);
                builder.AppendFormat (SheetHeaderFormat, sheet.SheetName);
                if (IsHSplittedSheet (sheet)) {
                    SerializeHSplittedSheet (sheet, builder);
                }
                else {
                    SerializeSheet (sheet, builder);
                }
            }
            return builder;
        }

        protected new void SerializeSheet (ISheet sheet, StringBuilder builder)
        {
            for (var r = sheet.FirstRowNum; r <= sheet.LastRowNum; r++) {
                var row = sheet.GetRow (r);
                if (row == null) {
                    builder.AppendLine ();
                    continue;
                }
                SerializeRow (row, builder);
            }
            builder.AppendLine ();
        }

        protected new void SerializeRow (IRow row, StringBuilder builder)
        {
            foreach (var cell in row.Cells) {
                builder.Append (FormatCellValue (cell));
                builder.Append (CellSeparator);
            }
            builder.AppendLine ();
        }

        bool IsHSplittedSheet (ISheet sheet)
        {
            var pane = sheet.PaneInformation;
            if (pane == null) {
                return false;
            }
            if (pane.IsFreezePane () && pane.HorizontalSplitPosition > 0) {
                return true;
            }
            return false;
        }

        protected void SerializeHSplittedSheet (ISheet sheet, StringBuilder builder)
        {
            var colHeaders = ExtractColumnHeaders (sheet);
            var tableWasProcessed = false;

            for (var r = sheet.FirstRowNum; r <= sheet.LastRowNum; r++) {
                var row = sheet.GetRow (r);
                if (row == null) {
                    builder.AppendLine ();
                    continue;
                }
                if (r == sheet.FirstRowNum + NumOfHeaderRows - 1) {
                    builder.AppendLine ();
                    continue;
                }
                if (r < sheet.FirstRowNum + NumOfHeaderRows - 1 || tableWasProcessed) {
                    SerializeRow (row, builder);
                }
                else {
                    foreach (var cell in row.Cells) {
                        if (CheckAndUpdateTableWasProcessed (ref tableWasProcessed, cell)) {
                            SerializeRow (row, builder);
                            break;
                        }
                        builder.Append (SafeGetColumnHeader (colHeaders, cell.ColumnIndex));
                        builder.Append (CellSeparator);
                        builder.Append (FormatCellValue (cell));
                        builder.Append (CellSeparator);
                        builder.AppendLine ();
                    }
                    builder.AppendLine ();
                }
            }
            builder.AppendLine ();
        }

        IList<string> ExtractColumnHeaders (ISheet sheet)
        {
            var colHeaders = new List<string> ();
            var headerRow = sheet.GetRow (sheet.FirstRowNum + NumOfHeaderRows - 1);
            foreach (var cell in headerRow.Cells) {
                colHeaders.Add (Formatter.FormatCellValue (cell));
            }
            return colHeaders;
        }

        string SafeGetColumnHeader (IList<string> colHeaders, int index)
        {
            if (index < colHeaders.Count) {
                return colHeaders [index];
            }
            return EmptyCellValue;
        }

        bool CheckAndUpdateTableWasProcessed (ref bool tableWasProcessed, ICell cell)
        {
            if (tableWasProcessed) {
                return true;
            }
            if (cell.ColumnIndex == 0) {
                var cellValue = Formatter.FormatCellValue (cell);
                if (cellValue.Trim ().StartsWith (TableWasProcessedToken, StringComparison.CurrentCultureIgnoreCase)) {
                    tableWasProcessed = true;
                }
            }
            return tableWasProcessed;
        }
    }
}
