//
// DivisionViewModel.cs
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
using System.Web.UI.WebControls;
using DotNetNuke.Common;
using DotNetNuke.R7;

namespace R7.University.Division
{
    public class SubDivisionViewModel: DivisionInfo
    {
        protected ViewModelContext Context { get; set; }

        public string HomePageLink 
        { 
            get
            {
                int homeTabId;
                if (int.TryParse (HomePage, out homeTabId) && Context.Module.TabId != homeTabId)
                {
                    return string.Format ("<a href=\"{0}\">{1}</a>", Globals.NavigateURL (homeTabId), Title);
                }

                return Title;
            }
        }

        public string CssClass
        {
            get { return !IsPublished ? "not-published-division" : string.Empty; }
        }
            
        public SubDivisionViewModel (DivisionInfo division, ViewModelContext context)
        {
            Context = context;

            Title = division.Title;
            HomePage = division.HomePage;
            StartDate = division.StartDate;
            EndDate = division.EndDate;
        }
    }
}

