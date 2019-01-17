//
//  UniversityPortalConfig.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2016-2019 Roman M. Yagodin
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

        public VocabulariesConfig Vocabularies { get; set; } = new VocabulariesConfig ();

        public int DataCacheTime { get; set; } = 20;

        public int CuHours { get; set; } = 36;
    }

    public class EmployeePhotoConfig
    {
        public string DefaultPath { get; set; } = "Images/faces/";

        public int DefaultWidth { get; set; } = 192;

        public int ListDefaultWidth { get; set; } = 120;
    }

    public class BarcodeConfig
    {
        public int DefaultWidth { get; set; } = 192;
    }

    public class VocabulariesConfig
    {
        public string OrgStructure { get; set; } = "University_Structure";

        public string WorkingHours { get; set; } = "University_WorkingHours";
    }
}

