//
// EmployeeEduProgramView.cs
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
using R7.University;

namespace R7.University.Employee
{
    [Serializable]
    public class EmployeeEduProgramView: EmployeeEduProgramInfoEx
	{
        public int ItemID { get; set; }

        public EmployeeEduProgramView ()
        {
            ItemID = ViewNumerator.GetNextItemID ();
        }

        public EmployeeEduProgramView (EmployeeEduProgramInfoEx program): this ()
        {
            foreach (var pi in typeof (EmployeeEduProgramInfoEx).GetProperties ())
                if (pi.GetSetMethod () != null)
                    pi.SetValue (this, pi.GetValue(program, null), null);
        }

        public EmployeeEduProgramInfo NewEmployeeEduProgramInfo ()
        {
            return new EmployeeEduProgramInfo {
                EmployeeEduProgramID = EmployeeEduProgramID,
                EmployeeID = EmployeeID,
                EduProgramID = EduProgramID,
                Disciplines = Disciplines
            };
        }

        public override string ToString ()
        {
            return string.Format ("EmployeeEduProgramID={0},ItemID={1},Code={2},Title={3},EmployeeID={4},EduProgramID={5},Disciplines={6}",
                EmployeeEduProgramID, ItemID, Code, Title, EmployeeID, EduProgramID, Disciplines
            );
        }
	}
}

