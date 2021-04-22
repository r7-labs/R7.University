using R7.Dnn.Extensions.Configuration;

namespace R7.University.Configuration
{
    public static class UniversityConfig
    {
        static readonly ExtensionYamlConfig<UniversityPortalConfig> _config;

        static UniversityConfig ()
        {
            _config = new ExtensionYamlConfig<UniversityPortalConfig> ("R7.University.yml", null);
        }

        public static UniversityPortalConfig GetInstance (int portalId)
        {
            return _config.GetInstance (portalId);
        }

        public static UniversityPortalConfig Instance {
            get {
                return _config.Instance;
            }
        }
    }
}
