using System;
using System.Linq;
using System.Collections.Generic;
using DotNetNuke.Entities.Modules;
using DotNetNuke.UI.Modules;
using DotNetNuke.Common.Utilities;
using DotNetNuke.R7;
using R7.University;

namespace R7.University.EduProgramProfileDirectory
{
	/// <summary>
	/// Provides strong typed access to settings used by module
	/// </summary>
	public class EduProgramProfileDirectorySettings : SettingsWrapper
	{
        public EduProgramProfileDirectorySettings ()
        {
        }

        public EduProgramProfileDirectorySettings (IModuleControl module) : base (module)
		{
		}

        public EduProgramProfileDirectorySettings (ModuleInfo module) : base (module)
		{
		}

        #region Properties for settings

        public IList<string> EduLevels
        {
            get
            {
                return ReadSetting<string> ("EduProgramProfileDirectory_EduLevels", string.Empty)
                    .Split (new [] {';'}, StringSplitOptions.RemoveEmptyEntries).ToList ();
            }
            set
            {
                WriteModuleSetting<string> ("EduProgramProfileDirectory_EduLevels", string.Join (";", value));
            }
        }

        #endregion
	}
}

