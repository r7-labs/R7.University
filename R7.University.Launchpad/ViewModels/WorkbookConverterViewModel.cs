using System.Globalization;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Services.Localization;
using Newtonsoft.Json;

namespace R7.University.Launchpad.ViewModels
{
    public class WorkbookConverterViewModel
    {
        public string LocalResourceFile { get; set; }

        public string ClientResources {
            get {
                var clientResourcesObject = LocalizationProvider.Instance.GetCompiledResourceFile (
                    PortalSettings.Current, LocalResourceFile, CultureInfo.CurrentUICulture.Name
                );
                return JsonConvert.SerializeObject (clientResourcesObject);
            }
        }
    }
}
