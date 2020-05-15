using DotNetNuke.Web.Api;

namespace R7.University.Launchpad.Services
{
    public class WorkbookConverterRouteMapper: IServiceRouteMapper
    {
        public void RegisterRoutes (IMapRoute mapRouteManager)
        {
            mapRouteManager.MapHttpRoute ("R7.University.Launchpad", "UniversityServicesMap2", "{controller}/{action}",
                new [] { "R7.University.Launchpad.Services" });
        }
    }
}
