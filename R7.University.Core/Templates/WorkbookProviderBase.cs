using System.IO;
using NPOI.SS.UserModel;

namespace R7.University.Core.Templates
{
    public abstract class WorkbookProviderBase: IWorkbookProvider
    {
        public abstract IWorkbook CreateWorkbook ();

        public abstract IWorkbook CreateWorkbook (Stream stream);

        public void WriteWorkbook (IWorkbook book, Stream stream)
        {
            book.Write (stream);
        }

        public ISheet GetSheetAt (IWorkbook book, int index)
        {
            return book.GetSheetAt (index);
        }

        public int GetNumberOfSheets (IWorkbook book)
        {
            return book.NumberOfSheets;
        }

        protected abstract void CopySheetTo (ISheet sheet, IWorkbook book);

        public void CopyWorkbook (IWorkbook srcBook, IWorkbook dstBook, bool copyComments)
        {
            for (var i = 0; i < srcBook.NumberOfSheets; i++) {
                var sheet = srcBook.GetSheetAt (i) as ISheet;
                CopySheetTo (sheet, dstBook);
            }

            if (copyComments) {
                CopyComments (srcBook, dstBook);
            }
        }

        protected abstract IDrawing GetDrawingPatriarch (ISheet sheet);

        void CopyComments (IWorkbook srcBook, IWorkbook dstBook)
        {
            for (var i = 0; i < srcBook.NumberOfSheets; i++) {
                var srcSheet = srcBook.GetSheetAt (i) as ISheet;
                var dstSheet = dstBook.GetSheetAt (i) as ISheet;

                for (var r = srcSheet.FirstRowNum; r <= srcSheet.LastRowNum; r++) {
                    var row = srcSheet.GetRow (r);
                    if (row == null) {
                        continue;
                    }
                    foreach (var srcCell in row.Cells) {
                        if (srcCell.CellComment != null) {
                            var partiarch = GetDrawingPatriarch (dstSheet) ?? dstSheet.CreateDrawingPatriarch ();
                            var dstComment = partiarch.CreateCellComment (srcCell.CellComment.ClientAnchor);

                            dstComment.Author = srcCell.CellComment.Author;
                            dstComment.Row = srcCell.CellComment.Row;
                            dstComment.Column = srcCell.CellComment.Column;
                            dstComment.String = srcCell.CellComment.String;
                            dstComment.Visible = srcCell.CellComment.Visible;

                            var dstCell = GetMirrorCell (srcCell, dstSheet);
                            dstCell.CellComment = dstComment;
                        }
                    }
                }
            }
        }

        ICell GetMirrorCell (ICell cell, ISheet dstSheet)
        {
            return dstSheet.GetRow (cell.RowIndex).Cells [cell.ColumnIndex];
        }

        // backported from https://stackoverflow.com/questions/6673623/npoi-insert-row-like-excel
        public void InsertRows (ISheet sheet, int fromRowIndex, int rowCount)
        {
            sheet.ShiftRows (fromRowIndex, sheet.LastRowNum, rowCount);

            for (var rowIndex = fromRowIndex; rowIndex < fromRowIndex + rowCount; rowIndex++) {
                var srcRow = sheet.GetRow (rowIndex + rowCount);
                var insRow = sheet.CreateRow (rowIndex);
                insRow.Height = srcRow.Height;
                // copy cells
                for (var colIndex = 0; colIndex < srcRow.LastCellNum; colIndex++) {
                    var srcCell = srcRow.GetCell (colIndex);
                    var cellInsert = insRow.CreateCell (colIndex);
                    if (srcCell != null) {
                        cellInsert.CellStyle = srcCell.CellStyle;
                    }
                }
            }
        }

        public void CopyRow (ISheet sheet, int srcRowIndex, int dstRowIndex)
        {
            var srcRow = sheet.GetRow (srcRowIndex);
            var dstRow = sheet.GetRow (dstRowIndex);
            // copy cells
            for (var colIndex = 0; colIndex < srcRow.LastCellNum; colIndex++) {
                var srcCell = srcRow.GetCell (colIndex);
                var dstCell = dstRow.GetCell (colIndex);
                if (srcCell != null && dstCell != null) {
                    // TODO: Other value types?
                    dstCell.SetCellValue (srcCell.StringCellValue);
                    dstCell.CellStyle = srcCell.CellStyle;
                }
            }
        }

        public void DuplicateRow (ISheet sheet, int rowIndex)
        {
            InsertRows (sheet, rowIndex, 1);
            CopyRow (sheet, rowIndex + 1, rowIndex);
        }
    }
}
