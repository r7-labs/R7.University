using System;
using System.Reflection;
using R7.University.Components;

namespace R7.University.Controls.ViewModels
{
    public class AgplSignatureViewModel
    {
        public bool ShowRule { get; set; } = true;

        public Assembly BaseAssembly => UniversityAssembly.GetCoreAssembly ();

        public string Name => BaseAssembly.GetName ().Name;

        public Version Version => BaseAssembly.GetName ().Version;

        public string InformationalVersion =>
            BaseAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute> ()?.InformationalVersion;

        public string Product =>
            BaseAssembly.GetCustomAttribute<AssemblyProductAttribute> ()?.Product ?? Name;
    }
}
