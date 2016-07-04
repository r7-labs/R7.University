//
//  DocumentInfo.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2015-2016 Roman M. Yagodin
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
using DotNetNuke.ComponentModel.DataAnnotations;

namespace R7.University.Models
{
    [TableName ("University_Documents")]
    [PrimaryKey ("DocumentID", AutoIncrement = true)]
    public class DocumentInfo: IDocument
    {
        #region IDocument implementation

        public int DocumentID { get; set; }

        public int DocumentTypeID { get; set; }

        public int? EduProgramID { get; set; }

        // public int EduProgramProfileId { get; set; }

        public string ItemID { get; set; }

        public string Title { get; set; }

        public string Group { get; set; }

        public string Url { get; set; }

        public int SortIndex { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        [IgnoreColumn]
        public virtual DocumentTypeInfo DocumentType { get; set; }

        #endregion
    }
}

