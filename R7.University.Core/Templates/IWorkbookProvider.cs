using System.IO;
using NPOI.SS.UserModel;

namespace R7.University.Core.Templates
{
    public interface IWorkbookProvider
    {
        void CopyRow (ISheet sheet, int srcRowIndex, int dstRowIndex);

        // void CopyWorkbook (IWorkbook srcBook, IWorkbook dstBook, bool copyComments);

        IWorkbook CreateWorkbook ();

        IWorkbook CreateWorkbook (Stream stream);

        void DuplicateRow (ISheet sheet, int rowIndex);

        int GetNumberOfSheets (IWorkbook book);

        ISheet GetSheetAt (IWorkbook book, int index);

        void InsertRows (ISheet sheet, int fromRowIndex, int rowCount);

        void WriteWorkbook (IWorkbook book, Stream stream);
    }
}