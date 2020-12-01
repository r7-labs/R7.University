using R7.Dnn.Extensions.ViewModels;
using R7.University.EduPrograms.Models;

namespace R7.University.EduPrograms.ViewModels
{
    public class EduProgramModuleViewModel
    {
        public ViewModelContext<EduProgramSettings> Context { get; protected set; }

        public EduProgramViewModel EduProgram { get; set; }

        public bool IsEmpty ()
        {
            return EduProgram == null;
        }

        public EduProgramModuleViewModel SetContext (ViewModelContext<EduProgramSettings> context)
        {
            Context = context;
            return this;
        }
    }
}

