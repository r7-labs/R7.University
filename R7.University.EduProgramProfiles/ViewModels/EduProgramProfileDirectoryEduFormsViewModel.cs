using R7.Dnn.Extensions.ViewModels;
using R7.University.EduProgramProfiles.Models;
using R7.University.ViewModels;

namespace R7.University.EduProgramProfiles.ViewModels
{
    internal class EduProgramProfileDirectoryEduFormsViewModel
    {
        public IndexedEnumerable<EduProgramProfileEduFormsViewModel> EduProfiles { get; set; }

        public ViewModelContext<EduProgramProfileDirectorySettings> Context { get; protected set; }

        public EduProgramProfileDirectoryEduFormsViewModel SetContext (ViewModelContext<EduProgramProfileDirectorySettings> context)
        {
            Context = context;
            return this;
        }
    }
}

