//
//  EduLevel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2019 Roman M. Yagodin
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
    public interface IEduLevel
    {
        int EduLevelID { get; }

        int SortIndex { get; }

        string Title { get; }

        string ShortTitle { get; }

        int? ParentEduLevelId { get; }

        IEduLevel ParentEduLevel { get; }
    }

    public interface IEduLevelWritable: IEduLevel
    {
        new int EduLevelID { get; set; }

        new int SortIndex { get; set; }

        new string Title { get; set; }

        new string ShortTitle { get; set; }

        new int? ParentEduLevelId { get; set; }

        new IEduLevel ParentEduLevel { get; set; }
    }

    public class EduLevelInfo: IEduLevelWritable
    {
        public int EduLevelID { get; set; }

        public string Title { get; set; }

        public string ShortTitle { get; set; }

        public int SortIndex { get; set; }

        public int? ParentEduLevelId { get; set; }

        public virtual EduLevelInfo ParentEduLevel { get; set; }

        IEduLevel IEduLevel.ParentEduLevel => ParentEduLevel;

        IEduLevel IEduLevelWritable.ParentEduLevel {
            get { return ParentEduLevel; }
            set { ParentEduLevel = (EduLevelInfo) value; }
        }
    }
}

