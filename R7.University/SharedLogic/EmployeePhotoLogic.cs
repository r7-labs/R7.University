//
//  EmployeePhotoLogic.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2014-2020 Roman M. Yagodin
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU Affero General Public License for more details.
//
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Globalization;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.FileSystem;
using R7.University.Components;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Utilities;

namespace R7.University.SharedLogic
{
    public static class EmployeePhotoLogic
    {
        public static void Bind (IEmployee employee, Image imagePhoto, int photoWidth, bool listMode = false)
        {
            var image = default (IFileInfo);

            var photoFileId = GetPhotoFileId (employee, listMode);
            if (photoFileId != null && !Null.IsNull (photoFileId.Value)) {
                image = FileManager.Instance.GetFile (photoFileId.Value);
            }

            var defaultWidth = listMode
                ? UniversityConfig.Instance.EmployeePhoto.ListDefaultWidth
                : UniversityConfig.Instance.EmployeePhoto.DefaultWidth;

            // TODO: Make PhotoWidth settings nullable
            photoWidth = (photoWidth > 0) ? photoWidth : defaultWidth;

            // if no photo specified (or not found), use fallback image
            var noPhotoUrl = default (string);
            if (image == null) {
                image = new FileInfo ();
                image.Width = defaultWidth;
                image.Height = defaultWidth;
                noPhotoUrl = $"/DesktopModules/MVC/R7.University/R7.University/images/nophoto.png";
            }

            if (noPhotoUrl == null) {
                imagePhoto.ImageUrl = UniversityUrlHelper.FullUrl ($"/imagehandler.ashx?fileid={image.FileId}&width={photoWidth}");
            }
            else {
                // files inside DesktopModules folder have no fileid
                imagePhoto.ImageUrl = UniversityUrlHelper.FullUrl ($"/imagehandler.ashx?file={noPhotoUrl}&width={photoWidth}");
            }

            // apply CSS classes
            var imageWidth = image.Width;
            var imageHeight = image.Height;

            if (Math.Abs (imageWidth - imageHeight) <= 1) {
                // for square and "almost" square images
                imagePhoto.CssClass += " img-fluid rounded-circle";
            }
            else {
                imagePhoto.CssClass += " img-fluid rounded";
            }

            if (noPhotoUrl != null) {
                imagePhoto.CssClass += " u8y-img-employee-nophoto";
            }

            // set alt & title
                var fullName = employee.FullName ();
            imagePhoto.AlternateText = fullName;
            imagePhoto.ToolTip = fullName;
        }

        private static int? GetPhotoFileId (IEmployee employee, bool listMode)
        {
            return listMode ? employee.PhotoFileID : (employee.AltPhotoFileId != null ? employee.AltPhotoFileId : employee.PhotoFileID);
        }
    }
}
