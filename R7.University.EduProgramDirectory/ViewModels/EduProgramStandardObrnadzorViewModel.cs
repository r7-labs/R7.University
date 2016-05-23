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
using DotNetNuke.Common;
using DotNetNuke.Entities.Tabs;
using DotNetNuke.Services.Localization;
using R7.DotNetNuke.Extensions.ViewModels;
using R7.University.Components;
using R7.University.Utilities;
using R7.University.ViewModels;
using R7.University.Data;
using R7.University.Models;
using R7.University.ModelExtensions;

namespace R7.University.EduProgramDirectory
{
    public class EduProgramStandardObrnadzorViewModel: EduProgramInfo
    {
        public ViewModelContext Context { get; protected set; }

        public EduProgramStandardObrnadzorViewModel (
            EduProgramInfo ep,
            ViewModelContext context,
            ViewModelIndexer indexer)
        {
            CopyCstor.Copy<EduProgramInfo> (ep, this);
            Context = context;
            Order = indexer.GetNextIndex ();
        }

        public int Order { get; protected set; }

        public string EduLevelString
        {
            get { return FormatHelper.FormatShortTitle (EduLevel.ShortTitle, EduLevel.Title); }
        }

        public string EduStandardLink
        {
            get {
                var eduStandardDocuments = EduStandardDocuments
                    .Where (d => d.IsPublished () || Context.Module.IsEditable).ToList ();
                
                if (eduStandardDocuments != null && eduStandardDocuments.Count > 0) {
                    var eduStandardDocument = eduStandardDocuments [0];

                    if (!string.IsNullOrWhiteSpace (eduStandardDocument.Url)) {
                        return string.Format ("<a href=\"{0}\"{1}{2} itemprop=\"EduStandartDoc\">{3}</a>",
                            UrlUtils.LinkClickIdnHack (
                                eduStandardDocument.Url,
                                Context.Module.TabId,
                                Context.Module.ModuleId), 
                            Globals.GetURLType (eduStandardDocument.Url) == TabType.Url ? " target=\"_blank\"" : string.Empty,
                            !eduStandardDocument.IsPublished () ? " class=\"not-published-document\"" : string.Empty,
                            !string.IsNullOrWhiteSpace (eduStandardDocument.Title) ? eduStandardDocument.Title 
                            : Localization.GetString ("EduProgramStandardLink.Text", Context.LocalResourceFile)
                        );
                    }
                }

                return string.Empty;
            }
        }
    }
}

