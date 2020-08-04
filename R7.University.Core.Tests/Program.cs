using System;
using R7.University.Core.Templates;

namespace R7.University.Core.Tests
{
    class Program
    {
        static void Main (string [] args)
        {
            var workbookManager = new WorkbookManager ();
            Console.Write (workbookManager.SerializeWorkbook ("./assets/templates/workbook-1.xls", WorkbookSerializationFormat.CSV));
        }
    }
}
