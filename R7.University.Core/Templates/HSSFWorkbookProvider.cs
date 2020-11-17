using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;

namespace R7.University.Templates
{
    public class HSSFWorkbookProvider: WorkbookProviderBase
    {
        public override IWorkbook CreateWorkbook ()
        {
            return new HSSFWorkbook ();
        }

        public override IWorkbook CreateWorkbook (Stream stream)
        {
            return new HSSFWorkbook (stream);
        }

        protected override void CopySheetTo (ISheet sheet, IWorkbook book)
        {
            (sheet as HSSFSheet).CopyTo (book as HSSFWorkbook, sheet.SheetName, true, true);
        }

        protected override IDrawing GetDrawingPatriarch (ISheet sheet)
        {
            return (sheet as HSSFSheet).DrawingPatriarch;
        }
    }
}
