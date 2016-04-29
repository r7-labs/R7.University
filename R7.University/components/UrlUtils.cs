//
// UrlUtils.cs
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
using DotNetNuke.Common;
using DotNetNuke.Entities.Portals;
using DotNetNuke.Entities.Tabs;

namespace R7.University.Utilities
{
    public static class UrlUtils
    {
        public static string FullUrl (string url)
        {
            return Globals.AddHTTP (PortalSettings.Current.PortalAlias.HTTPAlias + url);
        }

        /// <summary>
        /// Temp workaround for LinkClick and internationalized domain names (IDN) issue:
        /// https://dnntracker.atlassian.net/browse/DNN-7919
        /// </summary>
        /// <returns>Return raw (untrackable) URL for external URLs.</returns>
        public static string LinkClickIdnHack (string url, int tabId, int moduleId)
        {
            switch (Globals.GetURLType (url)) {
                case TabType.Url:
                    return url;
                    
                default:
                    return Globals.LinkClick (url, tabId, moduleId);
            }
        }

    }
}

