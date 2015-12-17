//
// FormatHelper.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2015 Roman M. Yagodin
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

namespace R7.University
{
    public static class FormatHelper
    {
        public static string FormatTimeToLearn (int timeToLearn, 
            string fullFormat, string yearsFormat, string monthsFormat)
        {
            var years = timeToLearn / 12;
            var months = timeToLearn % 12;

            if (months == 0) {
                return string.Format (yearsFormat, years);
            }

            if (years == 0) {
                return string.Format (monthsFormat, months);
            }

            return string.Format (fullFormat, years, months);
        }

        public static string FormatEduProgramProfileTitle (string title, 
            string profileCode, string profileTitle)
        {
            var profileString = Utils.FormatList (" ", profileCode, profileTitle);

            var profileStringInBrackets = 
                !string.IsNullOrWhiteSpace (profileString)? "(" + profileString + ")" : string.Empty;

            return Utils.FormatList (" ", title, profileStringInBrackets);
        }

        public static string FormatEduProgramProfileTitle (string code, string title, 
            string profileCode, string profileTitle)
        {
            var profileString = Utils.FormatList (" ", profileCode, profileTitle);

            var profileStringInBrackets = 
                !string.IsNullOrWhiteSpace (profileString)? "(" + profileString + ")" : string.Empty;

            return Utils.FormatList (" ", code, title, profileStringInBrackets);
        }
    }
}

