//
//  Year.cs
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

namespace R7.University.Models
{
    public interface IYear
    {
        int YearId { get; }

        int Year { get; }

        bool AdmissionIsOpen { get; }
    }

    public interface IYearWritable: IYear
    {
        new int YearId { get; set; }

        new int Year { get; set; }

        new bool AdmissionIsOpen { get; set; }
    }

    public class YearInfo : IYearWritable
    {
        public int YearId { get; set; }

        public int Year { get; set; }

        public bool AdmissionIsOpen { get; set; }
    }
}
