//
//  EduFormInfo.cs
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
    [TableName ("University_EduForms")]
    [PrimaryKey ("EduFormID", AutoIncrement = true)]
    [Cacheable ("//r7_University/Entities/EduForms")]
    public class EduFormInfo: IEduForm
    {
        #region IEduForm implementation

        /// <summary>
        /// Gets or sets the edu form I.
        /// </summary>
        /// <value>The edu form I.</value>
        public int EduFormID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is system.
        /// </summary>
        /// <value><c>true</c> if this instance is system; otherwise, <c>false</c>.</value>
        public bool IsSystem { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>The title.</value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the short title.
        /// </summary>
        /// <value>The short title.</value>
        public string ShortTitle { get; set; }

        #endregion

        [IgnoreColumn]
        /// <summary>
        /// Gets the system edu form.
        /// </summary>
        /// <value>The system edu form.</value>
        public SystemEduForm SystemEduForm
        {
            get {
                SystemEduForm result;
                return Enum.TryParse<SystemEduForm> (Title, out result) ? result : SystemEduForm.Custom;
            }
        }
    }
}

