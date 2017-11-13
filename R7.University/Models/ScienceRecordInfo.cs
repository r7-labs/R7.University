//
//  ScienceRecordInfo.cs
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
    public class ScienceRecordInfo : IScienceRecordWritable
    {
        public long ScienceRecordId { get; set; }

        public int EduProgramId { get; set; }

        public virtual EduProgramInfo EduProgram { get; set; }

        public int ScienceRecordTypeId { get; set; }

        public virtual ScienceRecordTypeInfo ScienceRecordType { get; set; }
        
        public string Description { get; set; }

        public decimal? Value1 { get; set; }

        public decimal? Value2 { get; set; }
    }
}