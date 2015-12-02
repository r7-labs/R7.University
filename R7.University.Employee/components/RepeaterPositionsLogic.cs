//
// RepeaterPositionsLogic.cs
//
// Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
// Copyright (c) 2014 
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
using DotNetNuke.Entities.Modules;
using R7.University;

namespace R7.University.Employee
{
    public static class RepeaterPositionsLogic
    {
        public static void ItemDataBound (PortalModuleBase module, object sender, RepeaterItemEventArgs e)
        {
            // exclude header & footer
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var opex = (OccupiedPositionInfoEx) e.Item.DataItem;

                var labelPosition = (Label) e.Item.FindControl ("labelPosition");
                var labelDivision = (Label) e.Item.FindControl ("labelDivision");
                var linkDivision = (HyperLink) e.Item.FindControl ("linkDivision");

                labelPosition.Text = PositionInfo.FormatShortTitle (opex.PositionTitle, opex.PositionShortTitle);

                // don't display division title for highest level divisions
                if (Utils.IsNull (opex.ParentDivisionID))
                {
                    labelDivision.Visible = false;
                    linkDivision.Visible = false;
                }
                else
                {
                    var divisionShortTitle = DivisionInfo.FormatShortTitle (opex.DivisionTitle, opex.DivisionShortTitle);

                    if (!string.IsNullOrWhiteSpace (opex.HomePage))
                    {
                        // link to division's homepage
                        labelDivision.Visible = false;
                        linkDivision.NavigateUrl = Utils.FormatCrossPortalTabUrl (module, int.Parse (opex.HomePage), false);
                        linkDivision.Text = divisionShortTitle;
                    }
                    else
                    {   
                        // only division title
                        linkDivision.Visible = false;
                        labelDivision.Text = divisionShortTitle;
                    }

                    labelPosition.Text += ": "; // to prev label!
                }
            }
        }
    }
}

