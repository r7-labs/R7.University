//
//  RepeaterPositionsLogic.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2016 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Web.UI.WebControls;
using DotNetNuke.Entities.Modules;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.ViewModels;
using R7.University.Models;

namespace R7.University.Employee.SharedLogic
{
    public static class RepeaterPositionsLogic
    {
        public static void ItemDataBound (PortalModuleBase module, object sender, RepeaterItemEventArgs e)
        {
            // exclude header & footer
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem) {
                var opex = (OccupiedPositionInfoEx) e.Item.DataItem;

                var labelPosition = (Label) e.Item.FindControl ("labelPosition");
                var labelDivision = (Label) e.Item.FindControl ("labelDivision");
                var linkDivision = (HyperLink) e.Item.FindControl ("linkDivision");

                labelPosition.Text = FormatHelper.FormatShortTitle (opex.PositionShortTitle, opex.PositionTitle);

                // don't display division title for highest level divisions
                if (TypeUtils.IsNull (opex.ParentDivisionID)) {
                    labelDivision.Visible = false;
                    linkDivision.Visible = false;
                }
                else {
                    var divisionShortTitle = FormatHelper.FormatShortTitle (
                                                 opex.DivisionShortTitle,
                                                 opex.DivisionTitle);

                    if (!string.IsNullOrWhiteSpace (opex.HomePage)) {
                        // link to division's homepage
                        labelDivision.Visible = false;
                        linkDivision.NavigateUrl = R7.University.Utilities.Utils.FormatCrossPortalTabUrl (
                            module,
                            int.Parse (opex.HomePage),
                            false);
                        linkDivision.Text = divisionShortTitle;
                    }
                    else {   
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

