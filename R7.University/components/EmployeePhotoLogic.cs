//
// EmployeePhotoLogic.cs
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
using System.IO;
using System.Globalization;
using System.Web.UI.WebControls;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Services.FileSystem;

namespace R7.University
{
    public static class EmployeePhotoLogic 
    {
        public static void Bind (EmployeeInfo employee, Image imagePhoto, int photoWidth, bool square = false)
        {
            IFileInfo image = null;
            var imageHeight = 0;
            var imageWidth = 0;

            if (!Utils.IsNull (employee.PhotoFileID))
            {
                // REVIEW: Need add ON DELETE rule to FK, linking PhotoFileID & Files.FileID 

                image = FileManager.Instance.GetFile (employee.PhotoFileID.Value);

                if (image != null && square)
                {
                    // trying to get square image
                    // FIXME: Remove hard-coded photo filename options
                    var squareImage = FileManager.Instance.GetFile(
                        FolderManager.Instance.GetFolder (image.FolderId), 
                        Path.GetFileNameWithoutExtension (image.FileName) + "_square" + Path.GetExtension(image.FileName));

                    if (squareImage != null)
                        image = squareImage;
                }
            }

            if (image != null)
            {
                imageHeight = image.Height;
                imageWidth = image.Width;

                // do we need to scale image?
                if (!Null.IsNull (photoWidth) && photoWidth != imageWidth)
                {
                    imagePhoto.ImageUrl = R7.University.Utilities.UrlUtils.FullUrl (string.Format (
                        "/imagehandler.ashx?fileid={0}&width={1}", image.FileId, photoWidth));
                }
                else
                {
                    // use original image
                    imagePhoto.ImageUrl = FileManager.Instance.GetUrl(image);
                }
            }
            else
            {
                // if not photo specified, or not found, use fallback image
                imageWidth = square ? 120 : 192;
                imageHeight = square ? 120 : 256;

                // TODO: Make fallback image resizable through image handler
                imagePhoto.ImageUrl = string.Format ("/DesktopModules/R7.University/R7.University/images/nophoto_{0}{1}.png", 
                    CultureInfo.CurrentCulture.TwoLetterISOLanguageName, square ? "_square" : "");
            }

            // do we need to scale image dimensions?
            if (!Null.IsNull (photoWidth) && photoWidth != imageWidth)
            {
                imagePhoto.Width = photoWidth;
                imagePhoto.Height = (int)(imageHeight * (float)photoWidth / imageWidth);
            }
            else
            {
                imagePhoto.Width = imageWidth;
                imagePhoto.Height = imageHeight;
            }

            // set alt & title for photo
            var fullName = employee.FullName;
            imagePhoto.AlternateText = fullName;
            imagePhoto.ToolTip = fullName;
        }

    }
}
