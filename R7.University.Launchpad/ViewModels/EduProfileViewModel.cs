using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Launchpad.ViewModels
{
    internal class EduProfileViewModel: EduProfileViewModelBase
    {
        public string Code
        {
            get { return EduProfile.EduProgram.Code; }
        }

        public string Title
        {
            get { return EduProfile.EduProgram.Title; }
        }

        public EduProfileViewModel (IEduProfile model): base (model)
        {
        }
    }
}

