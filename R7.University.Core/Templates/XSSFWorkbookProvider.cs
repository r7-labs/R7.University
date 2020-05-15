//
//  XSSFWorkbookProvider.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2020 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
//
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace R7.University.Core.Templates
{
    public class XSSFWorkbookProvider: IWorkbookProvider
    {
        public IWorkbook CreateWorkbook ()
        {
            return new XSSFWorkbook ();
        }

        public IWorkbook CreateWorkbook (Stream stream)
        {
            return new XSSFWorkbook (stream);
        }

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

        public void CopyWorkbook (IWorkbook srcBook, IWorkbook dstBook, bool copyComments)
        {
            for (var i = 0; i < srcBook.NumberOfSheets; i++) {
                var sheet = srcBook.GetSheetAt (i) as XSSFSheet;
                sheet.CopyTo ((XSSFWorkbook) dstBook, sheet.SheetName, true, true);
            }

            if (copyComments) {
                CopyComments (srcBook, dstBook);
            }
        }

        void CopyComments (IWorkbook srcBook, IWorkbook dstBook)
        {
            for (var i = 0; i < srcBook.NumberOfSheets; i++) {
                var srcSheet = srcBook.GetSheetAt (i) as XSSFSheet;
                var dstSheet = dstBook.GetSheetAt (i) as XSSFSheet;

                for (var r = srcSheet.FirstRowNum; r <= srcSheet.LastRowNum; r++) {
                    var row = srcSheet.GetRow (r);
                    if (row == null) {
                        continue;
                    }
                    foreach (var srcCell in row.Cells) {
                        if (srcCell.CellComment != null) {
                            var partiarch = dstSheet.GetDrawingPatriarch () ?? dstSheet.CreateDrawingPatriarch ();
                            var dstComment = partiarch.CreateCellComment (srcCell.CellComment.ClientAnchor);

                            dstComment.Author = srcCell.CellComment.Author;
                            dstComment.Row = srcCell.CellComment.Row;
                            dstComment.Column = srcCell.CellComment.Column;
                            dstComment.String = srcCell.CellComment.String;
                            dstComment.Visible = srcCell.CellComment.Visible;

                            var dstCell = GetMirrorCell (srcCell, dstSheet);
                            (dstCell as XSSFCell).CellComment = dstComment;
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