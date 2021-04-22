using DotNetNuke.Entities.Icons;

namespace R7.University.Dnn
{
    public static class UniversityIcons
    {
        public static readonly string Edit = IconController.IconURL ("Edit", IconController.DefaultIconSize, "Gray");

        public static readonly string Add = IconController.IconURL ("Add");

        public static readonly string AddAlternate = IconController.IconURL ("Add", IconController.DefaultIconSize, "Gray");

        public static readonly string Delete = IconController.IconURL ("ActionDelete");

        public static readonly string Details = IconController.IconURL ("View");

        public static readonly string Rollback = "~/DesktopModules/MVC/R7.University/R7.University/images/Rollback_16x16_Gray.png";

        public static readonly string EditDocuments = IconController.IconURL ("EditDisabled");
    }
}
