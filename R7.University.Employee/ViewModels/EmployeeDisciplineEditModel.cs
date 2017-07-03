//
//  EmployeeDisciplineEditModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
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
using System.Xml.Serialization;
using R7.University.Components;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.Employee.ViewModels
{
    [Serializable]
    public class EmployeeDisciplineEditModel: IEmployeeDisciplineWritable
    {
        #region IEmployeeDisciplineWritable implementation

        public long EmployeeDisciplineID { get; set; }

        public int EmployeeID { get; set; }

        public int EduProgramProfileID { get; set; }

        public string Disciplines { get; set; }

        [XmlIgnore]
        public EmployeeInfo Employee { get; set; }

        [XmlIgnore]
        public EduProgramProfileInfo EduProgramProfile { get; set; }

        #endregion

        #region External properties

        public string Code { get; set; }

        public string Title { get; set; }

        public string ProfileCode { get; set; }

        public string ProfileTitle { get; set; }

        public DateTime? ProfileStartDate { get; set; }

        public DateTime? ProfileEndDate { get; set; }

        public string EduLevel_String { get; set; }

        #endregion

        #region Bindable properties

        public string EduProgramProfile_String
        {
            get { return FormatHelper.FormatEduProgramProfileTitle (Code, Title, ProfileCode, ProfileTitle); }
        }

        #endregion

        public int ItemID { get; set; }

        public EmployeeDisciplineEditModel ()
        {
            ItemID = ViewNumerator.GetNextItemID ();
        }

        public EmployeeDisciplineEditModel (IEmployeeDisciplineWritable employeeDiscipline) : this ()
        {
            CopyCstor.Copy<IEmployeeDisciplineWritable> (employeeDiscipline, this);

            Code = employeeDiscipline.EduProgramProfile.EduProgram.Code;
            Title = employeeDiscipline.EduProgramProfile.EduProgram.Title;
            ProfileCode = employeeDiscipline.EduProgramProfile.ProfileCode;
            ProfileTitle = employeeDiscipline.EduProgramProfile.ProfileTitle;
            ProfileStartDate = employeeDiscipline.EduProgramProfile.StartDate;
            ProfileEndDate = employeeDiscipline.EduProgramProfile.EndDate;
            EduLevel_String = FormatHelper.FormatShortTitle (employeeDiscipline.EduProgramProfile.EduLevel.ShortTitle, employeeDiscipline.EduProgramProfile.EduLevel.Title);
        }

        public EmployeeDisciplineInfo NewEmployeeDisciplineInfo ()
        {
            return new EmployeeDisciplineInfo
            {
                EmployeeID = EmployeeID,
                EduProgramProfileID = EduProgramProfileID,
                Disciplines = Disciplines
            };
        }
    }
}

