//
// EduProgramStandardObrnadzorViewModel.cs
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
using System.Linq;
using System.Threading;
using DotNetNuke.Common;
using DotNetNuke.Services.Localization;
using DotNetNuke.R7;
using DotNetNuke.Entities.Urls;
using DotNetNuke.Entities.Tabs;

namespace R7.University.EduProgramDirectory
{
    public class EduProgramStandardObrnadzorViewModel: EduProgramInfo
    {
        public ViewModelContext Context { get; protected set; }

        public EduProgramStandardObrnadzorViewModel (EduProgramInfo ep, ViewModelContext context)
        {
            CopyCstor.Copy<EduProgramInfo> (ep, this);
            Context = context;
        }
             
        public string EduStandardLink
        {
            get
            {
                var eduStandardDocuments = EduStandardDocuments;
                if (eduStandardDocuments != null 
                    && eduStandardDocuments.Count > 0
                    && !string.IsNullOrWhiteSpace (eduStandardDocuments [0].Url))
                {
                    return string.Format ("<a href=\"{0}\" {1} itemprop=\"EduStandartDoc\">{2}</a>",
                        Globals.LinkClick (eduStandardDocuments [0].Url, Context.ModuleContext.TabId, Context.ModuleContext.ModuleId), 
                        Globals.GetURLType (eduStandardDocuments [0].Url) == TabType.Url? "target=\"_blank\"" : string.Empty,
                        Localization.GetString ("EduProgramStandardLink.Text", Context.Control.LocalResourceFile)
                    );
                }

                return string.Empty;
            }
        }
    }
}

