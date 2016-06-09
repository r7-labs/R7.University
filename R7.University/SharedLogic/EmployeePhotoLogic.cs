//
//  EmployeePhotoLogic.cs
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
using System.Globalization;
using System.IO;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.FileSystem;
using R7.DotNetNuke.Extensions.Utilities;
using R7.University.Components;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.SharedLogic
{
    public static class EmployeePhotoLogic
    {
        public static void Bind (IEmployee employee, Image imagePhoto, int photoWidth, bool square = false)
        {
            IFileInfo image = null;
            var imageHeight = 0;
            var imageWidth = 0;

            if (!TypeUtils.IsNull (employee.PhotoFileID)) {
                image = FileManager.Instance.GetFile (employee.PhotoFileID.Value);
                if (image != null && square) {
                    // trying to get square image
                    var squareImage = FileManager.Instance.GetFile (
                                          FolderManager.Instance.GetFolder (image.FolderId), 
                                          Path.GetFileNameWithoutExtension (image.FileName)
                                          + UniversityConfig.Instance.EmployeePhoto.SquareSuffix
                                          + Path.GetExtension (image.FileName));

                    if (squareImage != null)
                        image = squareImage;
                }
            }

            if (image != null) {
                imageHeight = image.Height;
                imageWidth = image.Width;

                // do we need to scale image?
                if (!Null.IsNull (photoWidth) && photoWidth != imageWidth) {
                    imagePhoto.ImageUrl = R7.University.Utilities.UrlUtils.FullUrl (string.Format (
                            "/imagehandler.ashx?fileid={0}&width={1}", image.FileId, photoWidth));
                }
                else {
                    // use original image
                    imagePhoto.ImageUrl = FileManager.Instance.GetUrl (image);
                }
            }
            else {
                // if not photo specified, or not found, use fallback image
                imageWidth = square ? UniversityConfig.Instance.EmployeePhoto.SquareDefaultWidth : UniversityConfig.Instance.EmployeePhoto.DefaultWidth;
                imageHeight = square ? UniversityConfig.Instance.EmployeePhoto.SquareDefaultWidth : UniversityConfig.Instance.EmployeePhoto.DefaultWidth * 4 / 3;

                // TODO: Make fallback image resizable through image handler
                imagePhoto.ImageUrl = string.Format ("/DesktopModules/R7.University/R7.University/images/nophoto_{0}{1}.png", 
                    CultureInfo.CurrentCulture.TwoLetterISOLanguageName, square ? 
                        UniversityConfig.Instance.EmployeePhoto.SquareSuffix : "");
            }

            // do we need to scale image dimensions?
            if (!Null.IsNull (photoWidth) && photoWidth != imageWidth) {
                imagePhoto.Width = photoWidth;
                imagePhoto.Height = (int) (imageHeight * (float) photoWidth / imageWidth);
            }
            else {
                imagePhoto.Width = imageWidth;
                imagePhoto.Height = imageHeight;
            }

            // set alt & title for photo
            var fullName = FormatHelper.FullName (employee.FirstName, employee.LastName, employee.OtherName);
            imagePhoto.AlternateText = fullName;
            imagePhoto.ToolTip = fullName;
        }

    }
}
