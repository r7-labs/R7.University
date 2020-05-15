using DotNetNuke.Common;

namespace R7.University.Components
{
    public static class UniversityGlobals
    {
        public const string INSTALL_PATH = "/DesktopModules/MVC/R7.University";

        public const string LIBRARY_INSTALL_PATH = INSTALL_PATH + "/R7.University";

        public const string ASSETS_PATH = LIBRARY_INSTALL_PATH + "/assets";

        public const string TEMPLATES_PATH = ASSETS_PATH + "/templates";

        public static string GetAbsoluteTemplatesPath () => Globals.ApplicationMapPath + TEMPLATES_PATH;
    }
}
