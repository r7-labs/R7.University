using System.Collections.Generic;
using NPOI.SS.UserModel;

namespace R7.University.Templates
{
    public class WorkbookCellEnumerator
    {
        public IEnumerable<ICell> GetCells (IWorkbook book)
        {
            for (var s = 0; s < book.NumberOfSheets; s++) {
                var sheet = book.GetSheetAt (s);
                for (var r = sheet.FirstRowNum; r <= sheet.LastRowNum; r++) {
                    var row = sheet.GetRow (r);
                    if (row == null) {
                        continue;
                    }
                    foreach (var cell in row.Cells) {
                        yield return cell;
                    }
                }
            }
        }
    }
}
