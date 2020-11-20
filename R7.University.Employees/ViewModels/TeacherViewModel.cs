using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Common.Utilities;
using R7.Dnn.Extensions.Text;
using R7.Dnn.Extensions.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Employees.ViewModels
{
    internal class TeacherViewModel: EmployeeViewModelBase
    {
        public EmployeeDirectoryTeachersViewModel RootViewModel { get; protected set; }

        public IEduProfile EduProfile { get; protected set; }

        public ViewModelIndexer Indexer { get; protected set; }

        public TeacherViewModel (IEmployee model, IEduProfile eduProgramProfile, EmployeeDirectoryTeachersViewModel rootViewModel, ViewModelIndexer indexer)
            : base (model)
        {
            RootViewModel = rootViewModel;
            EduProfile = eduProgramProfile;
            Indexer = indexer;
        }

        public ViewModelContext Context => RootViewModel.Context;

        #region Bindable properties

        public int Order => Indexer.GetNextIndex ();

        string _fullName;
        public string FullName =>
            _fullName ?? (_fullName = Span ("fio", UniversityFormatHelper.FullName (Employee.FirstName, Employee.LastName, Employee.OtherName)));

        string _positionsString;
        public string Positions_String =>
            _positionsString ?? (_positionsString = Span ("post", GetPositionsString ()));

        string _disciplinesString;
        public string Disciplines_String =>
            _disciplinesString ?? (_disciplinesString = Span ("teachingDiscipline", GetDisciplinesString ())
                                   // duplicate edu. program profile data here
                                   + ((EduProfile != null && EduProfile.EduLevel != null)
                                        ? HiddenSpan ("teachingLevel", EduProfile.EduLevel.Title) + HiddenSpan ("teachingQual", EduProfile.FormatTitle ())
                                        : string.Empty)
        );

        string _academicDegreesString;
        public string AcademicDegrees_String =>
            _academicDegreesString ?? (_academicDegreesString = Span ("degree", GetAcademicDegreesString ()));

        string _academicTitlesString;
        public string AcademicTitles_String =>
            _academicTitlesString ?? (_academicTitlesString = Span ("academStat", GetAcademicTitlesString ()));

        string _educationString;
        public string Education_String =>
            _educationString ?? (_educationString = Span ("employeeQualification", GetEducationString ()));

        string _trainingString;
        public string Training_String =>
            _trainingString ?? (_trainingString = Span ("profDevelopment", GetTrainingString ()));

        public string ExperienceYears_String => Span ("genExperience", Employee.ExperienceYears.ToString ());

        public string ExperienceYearsBySpec_String => Span ("specExperience", Employee.ExperienceYearsBySpec.ToString ());

        #endregion

        IEnumerable<EmployeeAchievementViewModel> achievementViewModels;
        protected IEnumerable<EmployeeAchievementViewModel> AchievementViewModels
        {
            get {
                return achievementViewModels
                    ?? (achievementViewModels = Employee.Achievements.Select (a => new EmployeeAchievementViewModel (a, Context)));
            }
        }

        string Span (string microdata, string content)
        {
            return $"<span itemprop=\"{microdata}\">{content}</span>";
        }

        string HiddenSpan (string microdata, string content)
        {
            return $"<span class=\"d-none\" itemprop=\"{microdata}\">{content}</span>";
        }

        string GetPositionsString ()
        {
            var positions = Employee.Positions
                        .OrderByDescending (op => op.IsPrime)
                        .ThenByDescending (op => op.Position.Weight);

            return FormatHelper.JoinNotNullOrEmpty (
                "; ",
                positions.Select (op => FormatHelper.JoinNotNullOrEmpty (": ", op.Position.Title, op.Division.Title))
            );
        }

        string GetDisciplinesString ()
        {
            if (!Null.IsNull (EduProfile.EduProgramProfileID)) {
                var disciplines = Employee.Disciplines
                    .FirstOrDefault (d => d.EduProgramProfileID == EduProfile.EduProgramProfileID);

                if (disciplines != null) {
                    return disciplines.Disciplines;
                }
            }

            return string.Empty;
        }

        string GetAcademicDegreesString ()
        {
            return FormatHelper.JoinNotNullOrEmpty ("; ",
                                         AchievementViewModels
                                         .Where (ach => ach.AchievementType.GetSystemType () == SystemAchievementType.AcademicDegree)
                                         .Select (ach => UniversityFormatHelper.FormatShortTitle (ach.ShortTitle, ach.Title, ach.TitleSuffix))
            );
        }

        string GetAcademicTitlesString ()
        {
            return FormatHelper.JoinNotNullOrEmpty ("; ",
                                         AchievementViewModels
                                         .Where (ach => ach.AchievementType.GetSystemType () == SystemAchievementType.AcademicTitle)
                                         .Select (ach => UniversityFormatHelper.FormatShortTitle (ach.ShortTitle, ach.Title, ach.TitleSuffix))
            );
        }

        string GetEducationString ()
        {
            return FormatHelper.JoinNotNullOrEmpty ("; ", AchievementViewModels
                .Where (ach => ach.AchievementType.IsEducation ())
                .Select (ach => FormatHelper.JoinNotNullOrEmpty ("&nbsp;- ",
                    FormatHelper.JoinNotNullOrEmpty (" ",
                        UniversityFormatHelper.FormatShortTitle (ach.ShortTitle, ach.Title, ach.TitleSuffix),
                        UniversityFormatHelper.WrapNotNullOrEmpty ("(", ach.EduLevel?.Title, ")")),
                    ach.YearBegin)));
        }

        string GetTrainingString ()
        {
            return FormatHelper.JoinNotNullOrEmpty ("; ",
                                         AchievementViewModels
                                         .Where (ach => ach.AchievementType.IsTraining ())
                                         .Select (ach => FormatHelper.JoinNotNullOrEmpty ("&nbsp;- ",
                        UniversityFormatHelper.FormatShortTitle (ach.ShortTitle, ach.Title, ach.TitleSuffix), ach.YearBegin))
            );
        }
    }
}
