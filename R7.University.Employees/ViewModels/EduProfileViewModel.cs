using R7.Dnn.Extensions.Text;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Employees.ViewModels
{
    internal class EduProfileViewModel: EduProfileViewModelBase
    {
        public EmployeeDirectoryTeachersViewModel RootViewModel { get; protected set; }

        public IndexedEnumerable<TeacherViewModel> Teachers { get; set; }

        public EduProfileViewModel (IEduProfile model, EmployeeDirectoryTeachersViewModel rootViewModel)
            : base (model)
        {
            RootViewModel = rootViewModel;
        }

        public ViewModelContext Context
        {
            get { return RootViewModel.Context; }
        }

        #region Bindable properties

        public string Title_String
        {
            get {
                return FormatHelper.JoinNotNullOrEmpty (": ",
                    UniversityFormatHelper.FormatEduProfileTitle (EduProgram.Code, EduProgram.Title, ProfileCode, ProfileTitle),
                    (EduLevel != null) ? EduLevel.Title : null
                );
            }
        }

        #endregion
    }
}

