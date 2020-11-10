using R7.Dnn.Extensions.ViewModels;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Controls.ViewModels
{
    public class EduProfileViewModel : EduProfileViewModelBase
    {
        protected IViewModelContext Context;

        public EduProfileViewModel (IEduProfile model, IViewModelContext viewModelContext) : base (model)
        {
            Context = viewModelContext;
        }

        public string Title_String
        {
            get {
                return UniversityFormatHelper.FormatEduProgramProfilePartialTitle (
                    EduProgramProfile.ProfileCode,
                    !string.IsNullOrEmpty (EduProgramProfile.ProfileTitle) ? EduProgramProfile.ProfileTitle : Context.LocalizeString ("EmptyProfileTitle.Text"),
                    UniversityFormatHelper.FormatShortTitle (EduProgramProfile.EduLevel.ShortTitle, EduProgramProfile.EduLevel.Title)
                );
            }
        }

    }
}
