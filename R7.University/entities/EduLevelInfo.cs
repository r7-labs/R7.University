//
// EduLevelInfo.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015 
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

namespace R7.University
{
    public enum EduType
    {
        School = 'S',
        Intermediate = 'I',
        High = 'H',
        Additional = 'A'
    }

    [TableName ("University_EduLevels")]
    [PrimaryKey ("EduLevelID", AutoIncrement = true)]
    public class EduLevelInfo: IReferenceEntity
    {
        #region Properties

        public int EduLevelID { get; set; }

        [ColumnName ("Type")]
        public string EduTypeString { get; set; }

        [IgnoreColumn]
        public EduType EduType
        {
            get { return (EduType)EduTypeString [0]; }
            set { EduTypeString = ((char)value).ToString (); }
        }

        #endregion

        #region IReferenceEntity implementation

        public string Title { get; set; }

        public string ShortTitle { get; set; }

        [IgnoreColumn]
        public string DisplayShortTitle
        {
            get { return FormatShortTitle (Title, ShortTitle); }
        }

        public static string FormatShortTitle (string title, string shortTitle)
        {
            return !string.IsNullOrWhiteSpace (shortTitle)? shortTitle : title;
        }

        #endregion
    }
}

