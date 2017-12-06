//
//  ScienceRecordType.cs
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
    public interface IScienceRecordType: ISystemEntity
    {
        int ScienceRecordTypeId { get; }

        string Type { get; }

        bool DescriptionIsRequired { get; }

        int NumOfValues { get; }

        string TypeOfValues { get; }

        int SortIndex { get; }
    }

    public interface IScienceRecordTypeWritable: IScienceRecordType, ISystemEntityWritable
    {
        new int ScienceRecordTypeId { get; set; }

        new string Type { get; set; }

        new bool DescriptionIsRequired { get; set; }

        new int NumOfValues { get; set; }

        new string TypeOfValues { get; set; }

        new int SortIndex { get; set; }
    }

    public class ScienceRecordTypeInfo: IScienceRecordTypeWritable
    {
        public int ScienceRecordTypeId { get; set; }
        
        public string Type { get; set; }
        
        public bool IsSystem { get; set; }

        public bool DescriptionIsRequired { get; set; }

        public int NumOfValues { get; set; }

        public string TypeOfValues { get; set; }

        public int SortIndex { get; set; }
    }
}