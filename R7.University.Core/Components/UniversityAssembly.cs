using System.Reflection;

namespace R7.University.Components
{
    public static class UniversityAssembly
    {
        public static Assembly GetCoreAssembly ()
        {
            return Assembly.GetExecutingAssembly ();
        }

        public static string SafeGetInformationalVersion (int fieldCount)
        {
            var coreAssembly = GetCoreAssembly ();
            var attr = coreAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute> ();
            if (attr != null) {
                return attr.InformationalVersion;
            }
            return coreAssembly.GetName ().Version.ToString (fieldCount);
        }
    }
}
