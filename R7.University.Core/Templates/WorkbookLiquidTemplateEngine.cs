using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using NPOI.SS.UserModel;

namespace R7.University.Core.Templates
{
    public class WorkbookLiquidTemplateEngine
    {
        public IModelToTemplateBinder Binder;

        public IWorkbookProvider WorkbookProvider;

        protected DataFormatter Formatter = new DataFormatter ();

        public WorkbookLiquidTemplateEngine (IModelToTemplateBinder binder, IWorkbookProvider workbookProvider)
        {
            Binder = binder;
            WorkbookProvider = workbookProvider;
        }

        public Stream ApplyAndWrite (string templateFilePath, Stream stream)
        {
            var book = Apply (templateFilePath);
            WorkbookProvider.WriteWorkbook (book, stream);
            return stream;
        }

        public StringBuilder ApplyAndSerialize (string templateFilePath, IWorkbookSerializer serializer)
        {
            var book = Apply (templateFilePath);
            return serializer.Serialize (book, new StringBuilder ());
        }

        public IWorkbook Apply (string templateFilePath)
        {
            // TODO: Support {% endfor %} and multi-row loops
            // https://github.com/tonyqus/npoi/blob/master/examples/xssf/CopySheet/Program.cs

            using (var file = new FileStream (templateFilePath, FileMode.Open, FileAccess.Read)) {

                var book = WorkbookProvider.CreateWorkbook (file);

                for (var s = 0; s < WorkbookProvider.GetNumberOfSheets (book); s++) {
                    var sheet = WorkbookProvider.GetSheetAt (book, s);
                    EvaluateObjects (sheet);
                    EvaluateLoops (sheet);
                    Cleanup (sheet);
                }

                return book;
            }
        }

        public void EvaluateObjects (ISheet sheet)
        {
            for (var r = sheet.FirstRowNum; r <= sheet.LastRowNum; r++) {
                var row = sheet.GetRow (r);
                if (row == null) {
                    continue;
                }
                foreach (var cell in row.Cells) {
                    var cellValue = GetStringCellValue (cell);
                    var cellOriginalValue = cellValue;
                    foreach (var liquidObject in LiquidHelper.GetLiquidObjects (cellValue)) {
                        var value = Binder.Eval (LiquidHelper.UnwrapLiquidObject (liquidObject));
                        if (value != null) {
                            cellValue = cellValue.Replace (liquidObject, value);
                        }
                    }
                    if (cellValue != cellOriginalValue) {
                        cell.SetCellValue (cellValue);
                    }
                }
            }
        }

        public void EvaluateLoops (ISheet sheet)
        {
            for (var r = sheet.FirstRowNum; r <= sheet.LastRowNum; r++) {
                var row = sheet.GetRow (r);
                if (row == null) {
                    continue;
                }
                var rowIndex = 0;
                foreach (var cell in row.Cells) {
                    // skip affected rows
                    if (rowIndex > row.RowNum) {
                        continue;
                    }
                    var cellValue = GetStringCellValue (cell);
                    if (LiquidHelper.ContainsLiquidTag (cellValue) && Regex.IsMatch (cellValue, @"{%\s*for")) {
                        var loop = LiquidLoop.Parse (LiquidHelper.UnwrapLiquidTag (cellValue));
                        if (loop == null) {
                            continue;
                        }
                        loop.NumOfRepeats = Binder.Count (loop.CollectionName);
                        rowIndex = row.RowNum + 1;
                        while (loop.Next ()) {
                            WorkbookProvider.DuplicateRow (sheet, rowIndex);
                            EvaluateRow (sheet.GetRow (rowIndex), loop);
                            rowIndex++;
                        }
                        // check only first cell
                        break;
                    }
                }
            }
        }

        public void EvaluateRow (IRow row, LiquidLoop loop)
        {
            foreach (var cell in row.Cells) {
                var cellValue = GetStringCellValue (cell);
                var cellOriginalValue = cellValue;
                foreach (var liquidObject in LiquidHelper.GetLiquidObjects (cellValue)) {
                    var objectName = LiquidHelper.UnwrapLiquidObject (liquidObject);
                    // strip loop variable name
                    objectName = Regex.Replace (objectName, @"^" + loop.VariableName + @"\.", "");
                    var value = Binder.Eval (objectName, loop.CollectionName, loop.Index);
                    if (value != null) {
                        cellValue = cellValue.Replace (liquidObject, value);
                    }
                }
                if (cellValue != cellOriginalValue) {
                    cell.SetCellValue (cellValue);
                }
            }
        }

        public void Cleanup (ISheet sheet)
        {
            for (var r = sheet.FirstRowNum; r <= sheet.LastRowNum; r++) {
                var row = sheet.GetRow (r);
                if (row == null) {
                    continue;
                }
                foreach (var cell in row.Cells) {
                    var cellValue = GetStringCellValue (cell);
                    var cellOriginalValue = cellValue;
                    if (LiquidHelper.ContainsLiquidTag (cellValue)) {
                        // TODO: This leaves empty rows
                        sheet.RemoveRow (row);
                        // check only first cell
                        break;
                    }

                    foreach (var liquidObject in LiquidHelper.GetLiquidObjects (cellValue)) {
                        cellValue = cellValue.Replace (liquidObject, string.Empty);
                    }
                    if (cellValue != cellOriginalValue) {
                        cell.SetCellValue (cellValue);
                    }
                }
            }
        }

        string GetStringCellValue (ICell cell)
        {
            return Formatter.FormatCellValue (cell);
        }
    }
}
