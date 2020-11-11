using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Employees.ViewModels
{
    internal class EmployeeDisciplineViewModel: IEmployeeDiscipline
    {
        public IEmployeeDiscipline EmployeeDiscipline { get; protected set; }

        public EmployeeDisciplineViewModel (IEmployeeDiscipline model)
        {
            EmployeeDiscipline = model;
        }

        #region IEmployeeDiscipline implementation

        public long EmployeeDisciplineID => EmployeeDiscipline.EmployeeDisciplineID;

        public int EmployeeID => EmployeeDiscipline.EmployeeID;

        public int EduProgramProfileID => EmployeeDiscipline.EduProgramProfileID;

        public string Disciplines => EmployeeDiscipline.Disciplines;

        public IEmployee Employee => EmployeeDiscipline.Employee;

        public IEduProfile EduProfile => EmployeeDiscipline.EduProfile;

        #endregion

        public string EduProgramProfile_String
        {
            get {
                return UniversityFormatHelper.FormatEduProfileTitle (
                    EmployeeDiscipline.EduProfile.EduProgram.Code,
                    EmployeeDiscipline.EduProfile.EduProgram.Title,
                    EmployeeDiscipline.EduProfile.ProfileCode,
                    EmployeeDiscipline.EduProfile.ProfileTitle
                );
            }
        }

        public string EduLevel_String {
            get {
                return UniversityFormatHelper.FormatShortTitle (
                    EmployeeDiscipline.EduProfile.EduLevel.ShortTitle,
                    EmployeeDiscipline.EduProfile.EduLevel.Title
                );
            }
        }
    }
}
