using System;
using System.Linq;
using System.Collections.Generic;
using DotNetNuke.Entities.Modules;
using DotNetNuke.UI.Modules;
using DotNetNuke.Common.Utilities;
using DotNetNuke.R7;
using R7.University;

namespace R7.University.EduProgramDirectory
{
	/// <summary>
	/// Provides strong typed access to settings used by module
	/// </summary>
	public partial class EduProgramDirectorySettings : SettingsWrapper
	{
        public EduProgramDirectorySettings ()
        {
        }

		public EduProgramDirectorySettings (IModuleControl module) : base (module)
		{
		}

		public EduProgramDirectorySettings (ModuleInfo module) : base (module)
		{
		}

		#region Properties for settings

        public IList<string> EduLevels
        {
            get
            {
                return ReadSetting<string> ("EduProgramDirectory_EduLevels", string.Empty)
                    .Split (';').ToList ();
            }
            set
            {
                WriteModuleSetting<string> ("EduProgramDirectory_EduLevels", string.Join (";", value));
            }
        }

		#endregion
	}
}

