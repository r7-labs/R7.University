//
//  UniversityPortalConfig.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016 Roman M. Yagodin
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

namespace R7.University.Components
{
    public class UniversityPortalConfig
    {
        public EmployeePhotoConfig EmployeePhoto { get; set; } = new EmployeePhotoConfig ();

        public BarcodeConfig Barcode { get; set; } = new BarcodeConfig ();

        public int DataCacheTime { get; set; } = 1200;
    }

    public class EmployeePhotoConfig
    {
        public string DefaultPath { get; set; } = "Images/faces/";

        public string SquareSuffix { get; set; } = "_square";

        public int DefaultWidth { get; set; } = 192;

        public int SquareDefaultWidth { get; set; } = 120;
    }

    public class BarcodeConfig
    {
        public int DefaultWidth { get; set; } = 192;
    }
}

