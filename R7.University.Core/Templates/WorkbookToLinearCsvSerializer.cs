using System;
using System.Collections.Generic;
using System.Text;
using NPOI.SS.UserModel;

namespace R7.University.Templates
{
    /// <summary>
    /// Convert workbook (spreadsheet) into CSV-like representation,
    /// transforming data in horizontal tables into linear name/value form.
    /// </summary>
    public class WorkbookToLinearCsvSerializer: WorkbookToCsvSerializer
    {
        public int NumOfHeaderRows { get; set; } = 2;

        public override StringBuilder Serialize (IWorkbook book, StringBuilder builder)
        {
            if (NumOfHeaderRows < 1) {
                throw new ArgumentOutOfRangeException ("Argument must be greater than 0", nameof (NumOfHeaderRows));
            }
            if (string.IsNullOrWhiteSpace (CommentToken)) {
                throw new ArgumentException ("Argument must not be null, empty nor whitespace", nameof (CommentToken));
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
                    var recordBuilder = new StringBuilder ();
                    var recordIsEmpty = true;

                    foreach (var cell in row.Cells) {
                        if (CheckAndUpdateTableWasProcessed (ref tableWasProcessed, cell)) {
                            SerializeRow (row, builder);
                            break;
                        }
                        if (cell.ColumnIndex == 0) {
                            continue;
                        }

                        var cellValue = FormatCellValue (cell);
                        if (!string.IsNullOrEmpty (cellValue)) {
                            recordIsEmpty = false;
                        }

                        recordBuilder.Append (SafeGetColumnHeader (colHeaders, cell.ColumnIndex - 1));
                        recordBuilder.Append (CellSeparator);
                        recordBuilder.Append (cellValue);
                        recordBuilder.Append (CellSeparator);
                        recordBuilder.AppendLine ();
                    }

                    if (!recordIsEmpty) {
                        builder.Append (recordBuilder.ToString ());
                        builder.AppendLine ();
                    }
                }
            }
            builder.AppendLine ();
        }

        IList<string> ExtractColumnHeaders (ISheet sheet)
        {
            var colHeaders = new List<string> ();
            var headerRow = sheet.GetRow (sheet.FirstRowNum + NumOfHeaderRows - 1);
            foreach (var cell in headerRow.Cells) {
                if (cell.ColumnIndex != 0) {
                    colHeaders.Add (Formatter.FormatCellValue (cell));
                }
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
                if (cellValue.Trim ().StartsWith (CommentToken, StringComparison.CurrentCultureIgnoreCase)) {
                    tableWasProcessed = true;
                }
            }
            return tableWasProcessed;
        }
    }
}
