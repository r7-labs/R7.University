//
//  TeacherViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2018 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
//
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Common.Utilities;
using R7.Dnn.Extensions.Text;
using R7.Dnn.Extensions.Utilities;
using R7.Dnn.Extensions.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Employees.ViewModels
{
    internal class TeacherViewModel: EmployeeViewModelBase
    {
        public EmployeeDirectoryTeachersViewModel RootViewModel { get; protected set; }

        public IEduProgramProfile EduProgramProfile { get; protected set; }

        public ViewModelIndexer Indexer { get; protected set; }

        public TeacherViewModel (IEmployee model, IEduProgramProfile eduProgramProfile, EmployeeDirectoryTeachersViewModel rootViewModel, ViewModelIndexer indexer)
            : base (model)
        {
            RootViewModel = rootViewModel;
            EduProgramProfile = eduProgramProfile;
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
                                   + ((EduProgramProfile != null && EduProgramProfile.EduLevel != null)
                                        ? HiddenSpan ("teachingLevel", EduProgramProfile.EduLevel.Title) + HiddenSpan ("teachingQual", EduProgramProfile.FormatTitle ())
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
            if (!Null.IsNull (EduProgramProfile.EduProgramProfileID)) {
                var disciplines = Employee.Disciplines
                    .FirstOrDefault (d => d.EduProgramProfileID == EduProgramProfile.EduProgramProfileID);

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
            return FormatHelper.JoinNotNullOrEmpty ("; ", 
                                         AchievementViewModels
                                         .Where (ach => ach.AchievementType.IsOneOf (SystemAchievementType.Education, SystemAchievementType.ProfTraining))
                                            .Select (ach => FormatHelper.JoinNotNullOrEmpty ("&nbsp;- ",
                        UniversityFormatHelper.FormatShortTitle (ach.ShortTitle, ach.Title, ach.TitleSuffix), ach.YearBegin))
            );
        }

        string GetTrainingString ()
        {
            return FormatHelper.JoinNotNullOrEmpty ("; ", 
                                         AchievementViewModels
                                         .Where (ach => ach.AchievementType.IsOneOf (SystemAchievementType.Training, SystemAchievementType.ProfRetraining))
                                         .Select (ach => FormatHelper.JoinNotNullOrEmpty ("&nbsp;- ",
                        UniversityFormatHelper.FormatShortTitle (ach.ShortTitle, ach.Title, ach.TitleSuffix), ach.YearBegin))
            );
        }
    }
}
