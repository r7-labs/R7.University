//
//  ViewModelBaseTests.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017 Roman M. Yagodin
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

using System;
using System.Reflection;
using Ploeh.AutoFixture;
using R7.University.Models;
using R7.University.ViewModels;
using Xunit;

namespace R7.University.Tests.ViewModels
{
    public class EmployeeViewModel: EmployeeViewModelBase
    {
        public EmployeeViewModel (IEmployee employee): base (employee) {}
    }

    public class EmployeeAchievementViewModel: EmployeeAchievementViewModelBase
    {
        public EmployeeAchievementViewModel (IEmployeeAchievement employeeAchievement): base (employeeAchievement) {}
    }

    public class EduProgramViewModel: EduProgramViewModelBase
    {
        public EduProgramViewModel (IEduProgram eduProgram): base (eduProgram) {}
    }

    public class EduProgramProfileViewModel: EduProgramProfileViewModelBase
    {
        public EduProgramProfileViewModel (IEduProgramProfile eduProgramProfile): base (eduProgramProfile) {}
    }

    public class ViewModelBaseTests
    {
        [Fact]
        public void ViewModelBaseTest ()
        {
            var fixture = new Fixture ();

            fixture.Customize<EmployeeInfo> (c => c.Without (e => e.Achievements).Without (e => e.Positions).Without (e => e.Disciplines));
            var employee = fixture.Create<EmployeeInfo> ();
            var employeeViewModel = new EmployeeViewModel (employee);
            CheckPropertiesEqual (typeof (IEmployee), employee, employeeViewModel);

            // FIXME: Test will fail because merging with base achievement not implemented in EmployeeAchievementInfo
            // var employeeAchievement = fixture.Create<EmployeeAchievementInfo> ();
            // var employeeAchievementViewModel = new EmployeeAchievementViewModel (employeeAchievement);
            // CheckProperties (typeof (IEmployeeAchievement), employeeAchievement, employeeAchievementViewModel);

            fixture.Customize<EduProgramInfo> (c => c.Without (ep => ep.Divisions)
                                               .Without (ep => ep.EduProgramProfiles)
                                               .Without (ep => ep.ScienceRecords)
                                               .Without (ep => ep.EduLevel));
            var eduProgram = fixture.Create<EduProgramInfo> ();
            var eduProgramViewModel = new EduProgramViewModel (eduProgram);
            CheckPropertiesEqual (typeof (IEduProgram), eduProgram, eduProgramViewModel);

            fixture.Customize<EduProgramProfileInfo> (c => c.Without (epp => epp.Divisions).Without (epp => epp.EduLevel));
            var eduProgramProfile = fixture.Create<EduProgramProfileInfo> ();
            var eduProgramProfileViewModel = new EduProgramProfileViewModel (eduProgramProfile);
            CheckPropertiesEqual (typeof (IEduProgramProfile), eduProgramProfile, eduProgramProfileViewModel);
        }

        void CheckPropertiesEqual (Type type, object object1, object object2)
        {
            foreach (var prop in type.GetProperties (BindingFlags.Public | BindingFlags.Instance)) {
                Assert.Equal (prop.GetValue (object1), prop.GetValue (object2));
            }
        }
    }
}
