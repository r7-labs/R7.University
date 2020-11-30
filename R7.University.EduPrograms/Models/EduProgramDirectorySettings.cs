using System;
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Entities.Modules.Settings;

namespace R7.University.EduPrograms.Models
{
    /// <summary>
    /// Provides strong typed access to settings used by module
    /// </summary>
    [Serializable]
    public class EduProgramDirectorySettings
    {
        [ModuleSetting (Prefix = "EduProgramDirectory_")]
        public int? DivisionId { get; set; }

        [ModuleSetting (Prefix = "EduProgramDirectory_", ParameterName = "EduLevels")]
        public string EduLevelsInternal { get; set; } = string.Empty;

        public IList<int> EduLevels
        {
            get { return EduLevelsInternal.Split (new [] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                    .Select (el => int.Parse (el))
                    .ToList ();
            }
            set {
                EduLevelsInternal = string.Join (";", value);
            }
        }
    }
}
