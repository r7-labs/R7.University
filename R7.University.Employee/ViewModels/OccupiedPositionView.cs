//
// OccupiedPositionView.cs
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
using R7.University.ViewModels;
using R7.University.Data;

namespace R7.University.Employee
{
    [Serializable]
    public class OccupiedPositionView
    {
        public int ItemID { get; set; }

        public int PositionID { get; set; }

        public int DivisionID { get; set; }

        public string PositionShortTitle { get; set; }

        public string DivisionShortTitle { get; set; }

        public bool IsPrime { get; set; }

        public string TitleSuffix { get; set; }

        public string PositionShortTitleWithSuffix
        {
            get { return PositionShortTitle + " " + TitleSuffix; }
        }

        public OccupiedPositionView ()
        {
            ItemID = ViewNumerator.GetNextItemID ();
        }

        public OccupiedPositionInfo NewOccupiedPositionInfo ()
        {
            var opinfo = new OccupiedPositionInfo ();

            opinfo.PositionID = PositionID;
            opinfo.DivisionID = DivisionID;
            opinfo.IsPrime = IsPrime;
            opinfo.TitleSuffix = TitleSuffix;

            return opinfo;
        }

        public OccupiedPositionView (OccupiedPositionInfoEx opex) : this ()
        {
            PositionID = opex.PositionID;
            DivisionID = opex.DivisionID;
            PositionShortTitle = FormatHelper.FormatShortTitle (opex.PositionShortTitle, opex.PositionTitle);
            DivisionShortTitle = FormatHelper.FormatShortTitle (opex.DivisionShortTitle, opex.DivisionTitle);
            IsPrime = opex.IsPrime;
            TitleSuffix = opex.TitleSuffix;
        }
    }
}
