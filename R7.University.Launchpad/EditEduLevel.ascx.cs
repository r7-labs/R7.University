//
// EditEduLevel.ascx.cs
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
using R7.DotNetNuke.Extensions.Modules;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University;
using R7.University.Data;

namespace R7.University.Launchpad
{
    public partial class EditEduLevel: EditPortalModuleBase<EduLevelInfo,int>
    {
        protected EditEduLevel () : base ("edulevel_id")
        {
        }

        protected override void InitControls ()
        {
            InitControls (buttonUpdate, buttonDelete, linkCancel);
        }

        protected override void LoadItem (EduLevelInfo item)
        {
            textTitle.Text = item.Title;
            textShortTitle.Text = item.ShortTitle;
            textSortIndex.Text = item.SortIndex.ToString ();
        }

        protected override void BeforeUpdateItem (EduLevelInfo item)
        {
            item.Title = textTitle.Text.Trim ();
            item.ShortTitle = textShortTitle.Text.Trim ();
            item.SortIndex = TypeUtils.ParseToNullable<int> (textSortIndex.Text) ?? 0;
        }

        #region implemented abstract members of EditPortalModuleBase

        protected override EduLevelInfo GetItem (int itemId)
        {
            return UniversityRepository.Instance.DataProvider.Get<EduLevelInfo> (itemId);
        }

        protected override int AddItem (EduLevelInfo item)
        {
            UniversityRepository.Instance.DataProvider.Add<EduLevelInfo> (item);
            return item.EduLevelID;
        }

        protected override void UpdateItem (EduLevelInfo item)
        {
            UniversityRepository.Instance.DataProvider.Update<EduLevelInfo> (item);
        }

        protected override void DeleteItem (EduLevelInfo item)
        {
            UniversityRepository.Instance.DataProvider.Delete<EduLevelInfo> (item);
        }

        #endregion
    }
}

