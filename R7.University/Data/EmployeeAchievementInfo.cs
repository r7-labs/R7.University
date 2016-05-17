//
// EmployeeAchievementInfo.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015-2016 Roman M. Yagodin
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using DotNetNuke.ComponentModel.DataAnnotations;
using R7.University.Models;

namespace R7.University.Data
{
    // More attributes for class:
    // Set caching for table: [Cacheable("R7.University_Divisions", CacheItemPriority.Default, 20)]
    // Explicit mapping declaration: [DeclareColumns]
    // More attributes for class properties:
    // Custom column name: [ColumnName("DivisionID")]
    // Explicit include column: [IncludeColumn]
    // Note: DAL 2 have no AutoJoin analogs from PetaPOCO at this time
    [TableName ("University_EmployeeAchievements")]
    [PrimaryKey ("EmployeeAchievementID", AutoIncrement = true)]
    [Serializable]
    public class EmployeeAchievementInfo: IEmployeeAchievement
    {
        #region IEmployeeAchievement implementation

        public int EmployeeAchievementID { get; set; }

        public int EmployeeID  { get; set; }

        public int? AchievementID { get; set; }

        public string Description { get; set; }

        public string Title { get; set; }

        public string ShortTitle  { get; set; }

        public int? YearBegin { get; set; }

        public int? YearEnd { get; set; }

        public bool IsTitle { get; set; }

        public string DocumentURL { get; set; }

        public string TitleSuffix { get; set; }

        [IgnoreColumn]
        public AchievementType? AchievementType
        {
            get { 
                if (!string.IsNullOrWhiteSpace (AchievementTypeString))
                    return (AchievementType) AchievementTypeString [0];

                return null;
            }
            set { 
                if (value != null)
                    AchievementTypeString = ((char) value).ToString ();
                else
                    AchievementTypeString = null;
            }
        }

        #endregion

        [ColumnName ("AchievementType")]
        public string AchievementTypeString { get; set; }

        [IgnoreColumn]
        public string FormatYears
        {
            get {
                if (YearBegin != null && YearEnd == null)
                    return YearBegin.ToString (); 
				
                if (YearBegin == null && YearEnd != null) {
                    if (YearEnd.Value != 0)
                        return "? - " + YearEnd; 
                }

                if (YearBegin != null && YearEnd != null) {
                    if (YearEnd.Value != 0)
                        return string.Format ("{0} - {1}", YearBegin, YearEnd);

                    return YearBegin + " - {ATM}";
                }

                return string.Empty;
            }
        }
		
    }
}
