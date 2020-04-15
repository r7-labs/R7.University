using System.Web.Mvc;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Web.Mvc.Framework.Controllers;

namespace R7.University.Divisions.Controllers
{
    [DnnHandleError]
    public class WorkbookConverterController : DnnController
    {
        public ActionResult Index ()
        {
            return View (new object ());
        }
    }
}
