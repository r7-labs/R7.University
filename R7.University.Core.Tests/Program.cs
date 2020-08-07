using System;
using R7.University.Core.Templates;

namespace R7.University.Core.Tests
{
    class Program
    {
        static void Main (string [] args)
        {
            while (true) {
                Console.WriteLine ("> Please enter test number and press [Enter]:");
                Console.WriteLine ("---");
                Console.WriteLine ("1. Workbook to CSV");
                Console.WriteLine ("2. Workbook to linear CSV");
                Console.WriteLine ("0. Exit");

                if (int.TryParse (Console.ReadLine (), out int testNum)) {
                    switch (testNum) {
                        case 1: WorkbookToCsv (); break;
                        case 2: WorkbookToLinearCsv (); break;
                        case 0: return;
                    }
                    Console.WriteLine ("> Press any key to continue...");
                    Console.ReadKey ();
                    Console.WriteLine ();
                }
            }
        }

        static void WorkbookToCsv ()
        {
            Console.WriteLine ("--- Start test output");
            var workbookManager = new WorkbookManager ();
            Console.Write (workbookManager.SerializeWorkbook ("./assets/templates/workbook-1.xls", WorkbookSerializationFormat.CSV));
            Console.WriteLine ("--- End test output");
        }

        static void WorkbookToLinearCsv ()
        {
            Console.WriteLine ("--- Start test output");
            var workbookManager = new WorkbookManager ();
            Console.Write (workbookManager.SerializeWorkbook ("./assets/templates/workbook-1.xls", WorkbookSerializationFormat.LinearCSV));
            Console.WriteLine ("--- End test output");
        }
    }
}
