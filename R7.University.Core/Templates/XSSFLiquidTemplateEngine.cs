//
//  XSSFLiquidTemplateEngine.cs
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

using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using NPOI.SS.UserModel;

namespace R7.University.Core.Templates
{
    public class XSSFLiquidTemplateEngine
    {
        public IModelToTemplateBinder Binder;

        public XSSFWorkbookProvider WorkbookProvider = new XSSFWorkbookProvider ();

        public XSSFLiquidTemplateEngine (IModelToTemplateBinder binder)
        {
            Binder = binder;
        }

        public void Apply (string templateFilePath)
        {
            // TODO: Support {% endfor %} and multi-row loops
            // https://github.com/tonyqus/npoi/blob/master/examples/xssf/CopySheet/Program.cs

            using (var file = new FileStream (templateFilePath, FileMode.Open, FileAccess.Read)) {

                var templateBook = WorkbookProvider.CreateWorkbook (file);
                var book = WorkbookProvider.CreateWorkbook ();
                WorkbookProvider.CopyWorkbook (templateBook, book, true);

                for (var s = 0; s < WorkbookProvider.GetNumberOfSheets (book); s++) {
                    var sheet = WorkbookProvider.GetSheetAt (book, s);
                    EvaluateObjects (sheet);
                    EvaluateLoops (sheet);
                    Cleanup (sheet);
                }

                WorkbookProvider.WriteWorkbook (book, new FileStream (templateFilePath.Replace ("_template.xlsx", ".xlsx"), FileMode.Create, FileAccess.ReadWrite));
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
                    if (IsLiquidObject (cell.StringCellValue)) {
                        var value = Binder.Eval (UnwrapLiquidObject (cell.StringCellValue));
                        if (value != null) {
                            cell.SetCellValue (value);
                        }
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
                    var cellValue = cell.StringCellValue;
                    if (IsLiquidTag (cellValue) && Regex.IsMatch (cellValue, @"{%\s*for")) {
                        var loop = LiquidLoop.Parse (UnwrapLiquidTag (cellValue));
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
                var cellValue = cell.StringCellValue;
                if (IsLiquidObject (cellValue)) {
                    var objectName = UnwrapLiquidObject (cellValue);
                    // strip loop variable name
                    objectName = Regex.Replace (objectName, @"^" + loop.VariableName + @"\.", "");
                    var value = Binder.Eval (objectName, loop.CollectionName, loop.Index);
                    if (value != null) {
                        cell.SetCellValue (value);
                    }
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
                    if (IsLiquidTag (cell.StringCellValue)) {
                        // TODO: This leaves empty rows
                        sheet.RemoveRow (row);
                        // check only first cell
                        break;
                    }

                    if (IsLiquidObject (cell.StringCellValue)) {
                        cell.SetCellValue (string.Empty);
                    }
                }
            }
        }

        public string UnwrapLiquidObject (string obj)
        {
            return obj.TrimStart ('{').TrimEnd ('}').Trim ();
        }

        public string UnwrapLiquidTag (string obj)
        {
            return obj.TrimStart ('{').TrimEnd ('}').Trim ('%').Trim ();
        }

        public bool IsLiquidObject (string value)
        {
            return value.StartsWith ("{{", StringComparison.Ordinal) && value.EndsWith ("}}", StringComparison.Ordinal);
        }

        public bool IsLiquidTag (string value)
        {
            return value.StartsWith ("{%", StringComparison.Ordinal) && value.EndsWith ("%}", StringComparison.Ordinal);
        }
    }
}