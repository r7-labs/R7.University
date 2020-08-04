using System;
using R7.University.Core.Templates;

namespace R7.University.Core.Tests
{
    class Program
    {
        static void Main (string [] args)
        {
            Console.WriteLine ("Please enter test number and press [Enter]:");
            Console.WriteLine ("1. Workbook Serialization");
            Console.WriteLine ("0. Exit");

            if (int.TryParse (Console.ReadLine (), out int testNum)) {
                switch (testNum) {
                    case 1: WorkbookSerialization (); break;
                }
            }
        }

        static void WorkbookSerialization ()
        {
            var workbookManager = new WorkbookManager ();
            Console.Write (workbookManager.SerializeWorkbook ("./assets/templates/workbook-1.xls", WorkbookSerializationFormat.CSV));
        }
    }
}
