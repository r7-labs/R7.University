using R7.University.ViewModels;
using R7.University.Models;
using R7.Dnn.Extensions.ViewModels;

namespace R7.University.EduPrograms.ViewModels
{
    public class EduProfileEditModel: EduProfileViewModelBase
    {
        public ViewModelContext Context { get; protected set; }

        public EduProfileEditModel (IEduProfile model, ViewModelContext context): base (model)
        {
            Context = context;
        }

        #region Bindable properties

        public string EduProgramProfile_String
        {
            get { return UniversityFormatHelper.FormatEduProgramTitle (EduProfile.ProfileCode, EduProfile.ProfileTitle); }
        }

        public string EduLevel_String
        {
            get { return EduProfile.EduLevel.Title; }
        }

        public string Edit_Url => Context.Module.EditUrl (
            "eduprogramprofile_id", EduProfile.EduProgramProfileID.ToString (), "EditEduProgramProfile");

        public string EditDocuments_Url => Context.Module.EditUrl (
	        "eduprogramprofile_id", EduProfile.EduProgramProfileID.ToString (), "EditEduProgramProfileDocuments");

        #endregion
    }
}
