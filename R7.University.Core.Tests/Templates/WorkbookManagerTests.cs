using System;
using R7.University.Templates;
using Xunit;

namespace R7.University.Tests.Templates
{
    public class WorkbookManagerTests
    {
        [Fact]
        public void ReadWorkbookInfoTest ()
        {
            var workbookManager = new WorkbookManager ();
            var bookInfo = workbookManager.ReadWorkbookInfo ("./assets/templates/workbook-1.xls");

            Assert.Equal ("Sample Entity", bookInfo.EntityType);
            Assert.Equal (123, bookInfo.EntityId);
        }

        [Fact]
        public void GetWorkbookSerializerTest ()
        {
            var wbm = new WorkbookManager ();

            Assert.Equal (nameof (WorkbookToCsvSerializer), wbm.GetWorkbookSerializer (WorkbookSerializationFormat.CSV).GetType ().Name);
            Assert.Equal (nameof (WorkbookToLinearCsvSerializer), wbm.GetWorkbookSerializer (WorkbookSerializationFormat.LinearCSV).GetType ().Name);
            Assert.Throws<ArgumentException> (() => wbm.GetWorkbookSerializer ("NoExistentSerializer"));
        }
    }
}
