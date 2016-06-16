//
//  TeacherViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Linq;
using DotNetNuke.Common.Utilities;
using R7.DotNetNuke.Extensions.Utilities;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.EmployeeDirectory.ViewModels
{
    public class TeacherViewModel: EmployeeViewModelBase
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
        public ViewModelContext Context
        {
            get { return RootViewModel.Context; }
        }

        #region Bindable properties

        public int Order
        {
            get { return Indexer.GetNextIndex (); }
        }

        public string FullName
        {
            get { return FormatHelper.FullName (Model.FirstName, Model.LastName, Model.OtherName); }
        }

        public string Positions_String
        {
            get {
                var positions = Model.OccupiedPositions
                    .OrderByDescending (op => op.IsPrime)
                    .ThenByDescending (op => op.PositionWeight);

                return TextUtils.FormatList ("; ", 
                    positions.Select (op => TextUtils.FormatList (": ", op.PositionTitle, op.DivisionTitle))
                );
            }
        }

        public string Disciplines_String
        {
            get {
                if (!Null.IsNull (EduProgramProfile.EduProgramProfileID)) {
                    var disciplines = Model.Disciplines
                        .FirstOrDefault (d => d.EduProgramProfileID == EduProgramProfile.EduProgramProfileID);

                    if (disciplines != null) {
                        return disciplines.Disciplines;
                    }
                }
                return string.Empty;
            }
        }

        public string AcademicDegrees_String
        {
            get {
                return TextUtils.FormatList ("; ", Model.Achievements
                    .Where (ach => ach.AchievementType == AchievementType.AcademicDegree)
                    .Select (ach => FormatHelper.FormatShortTitle (ach.ShortTitle, ach.Title, ach.TitleSuffix))
                );
            }
        }

        public string AcademicTitles_String
        {
            get {
                return TextUtils.FormatList ("; ", Model.Achievements
                    .Where (ach => ach.AchievementType == AchievementType.AcademicTitle)
                    .Select (ach => FormatHelper.FormatShortTitle (ach.ShortTitle, ach.Title, ach.TitleSuffix))
                );
            }
        }

        public string Education_String
        {
            get {
                return TextUtils.FormatList ("; ", Model.Achievements
                    .Where (ach => ach.AchievementType == AchievementType.Education)
                    .Select (ach => TextUtils.FormatList ("&nbsp;- ", 
                        FormatHelper.FormatShortTitle (ach.ShortTitle, ach.Title, ach.TitleSuffix), ach.YearBegin))
                );
            }
        }

        public string Training_String
        {
            get {
                return TextUtils.FormatList ("; ", Model.Achievements
                    .Where (ach => ach.AchievementType == AchievementType.Training)
                    .Select (ach => TextUtils.FormatList ("&nbsp;- ", 
                        FormatHelper.FormatShortTitle (ach.ShortTitle, ach.Title, ach.TitleSuffix), ach.YearBegin))
                );
            }
        }

        #endregion
    }
}
