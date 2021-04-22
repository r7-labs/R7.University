using System;
using System.Reflection;
using R7.University.Components;

namespace R7.University.Controls.ViewModels
{
    public class AgplSignatureViewModel
    {
        public bool ShowRule { get; set; } = true;

        public virtual Assembly BaseAssembly => UniversityAssembly.GetCoreAssembly ();

        public virtual string Name => BaseAssembly.GetName ().Name;

        public virtual Version Version => BaseAssembly.GetName ().Version;

        public virtual string InformationalVersion =>
            BaseAssembly.GetCustomAttribute<AssemblyInformationalVersionAttribute> ()?.InformationalVersion;
    }
}
