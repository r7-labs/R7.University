using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

namespace R7.University.Templates
{
    public class XSSFWorkbookProvider: WorkbookProviderBase
    {
        public override IWorkbook CreateWorkbook ()
        {
            return new XSSFWorkbook ();
        }

        public override IWorkbook CreateWorkbook (Stream stream)
        {
            return new XSSFWorkbook (stream);
        }

        protected override void CopySheetTo (ISheet sheet, IWorkbook book)
        {
            (sheet as XSSFSheet).CopyTo (book as XSSFWorkbook, sheet.SheetName, true, true);
        }

        protected override IDrawing GetDrawingPatriarch (ISheet sheet)
        {
            return (sheet as XSSFSheet).GetDrawingPatriarch ();
        }
    }
}
