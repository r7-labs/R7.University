using System.Reflection;

namespace R7.University.Components
{
    public static class UniversityAssembly
    {
        public static Assembly GetBaseAssembly ()
        {
            return Assembly.GetExecutingAssembly ();
        }

        public static string SafeGetInformationalVersion (int fieldCount)
        {
            var baseAssembly = GetBaseAssembly ();
            var attr = baseAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute> ();
            if (attr != null) {
                return attr.InformationalVersion;
            }
            return baseAssembly.GetName ().Version.ToString (fieldCount);
        }
    }
}
