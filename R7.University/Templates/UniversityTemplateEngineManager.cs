using DotNetNuke.Entities.Portals;
using R7.University.Components;
using R7.University.Dnn;
using R7.University.Models;

namespace R7.University.Templates
{
    public class UniversityTemplateEngineManager
    {
        public WorkbookLiquidTemplateEngine GetEmployeeTemplateEngine (IEmployee employee, PortalSettings portalSettings)
        {
            var employeeBinder = new EmployeeToTemplateBinder (employee, portalSettings,
                "~" + UniversityGlobals.INSTALL_PATH + "/R7.University.Employees/App_LocalResources/SharedResources.resx");

            return new WorkbookLiquidTemplateEngine (employeeBinder, new HSSFWorkbookProvider ());
        }
    }
}
