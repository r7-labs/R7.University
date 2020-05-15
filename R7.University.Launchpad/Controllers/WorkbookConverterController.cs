using System.Web.Mvc;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using R7.University.Launchpad.ViewModels;

namespace R7.University.Divisions.Controllers
{
    [DnnHandleError]
    public class WorkbookConverterController : DnnController
    {
        public ActionResult Index ()
        {
            return View (new WorkbookConverterViewModel () {
                LocalResourceFile = LocalResourceFile
            });
        }
    }
}
