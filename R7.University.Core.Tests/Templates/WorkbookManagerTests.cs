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
    }
}
