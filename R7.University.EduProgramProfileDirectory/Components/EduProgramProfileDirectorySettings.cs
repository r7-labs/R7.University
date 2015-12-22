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

        public IList<int> EduLevels
        {
            get
            {
                return ReadSetting<string> ("EduProgramProfileDirectory_EduLevels", string.Empty)
                    .Split (new [] {';'}, StringSplitOptions.RemoveEmptyEntries)
                    .ToList ()
                    .ConvertAll (s => int.Parse (s));
            }
            set
            {
                WriteModuleSetting<string> ("EduProgramProfileDirectory_EduLevels", string.Join (";", value));
            }
        }

        public EduProgramProfileDirectoryMode? Mode
        {
            get { return ReadSetting<EduProgramProfileDirectoryMode?> ("EduProgramProfileDirectory_Mode", null); }
            set { WriteModuleSetting<EduProgramProfileDirectoryMode?> ("EduProgramProfileDirectory_Mode", value); }
        }

        #endregion
	}
}

