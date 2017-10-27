//
//  ScienceRecordViewModel.cs
//
//  Author:
//       Roman M. Yagodin <roman.yagodin@gmail.com>
//
//  Copyright (c) 2017 Roman M. Yagodin
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

using System.Web;
using R7.Dnn.Extensions.ViewModels;
using R7.University.Models;
using R7.University.Science.Models;

namespace R7.University.Science.ViewModels
{
    public class ScienceRecordViewModel : IScienceRecord
    {
        protected IScienceRecord ScienceRecord;

        protected ViewModelContext<ScienceDirectorySettings> Context;

        public ScienceRecordViewModel (IScienceRecord scienceRecord, ViewModelContext<ScienceDirectorySettings> context)
        {
            ScienceRecord = scienceRecord;
            Context = context;
        }

        #region IScienceRecord implementation

        public long ScienceRecordId => ScienceRecord.ScienceRecordId;

        public int EduProgramId => ScienceRecord.EduProgramId;

        public EduProgramInfo EduProgram => ScienceRecord.EduProgram;

        public int ScienceRecordTypeId => ScienceRecord.ScienceRecordTypeId;

        public ScienceRecordTypeInfo ScienceRecordType => ScienceRecord.ScienceRecordType;

        public string Description => ScienceRecord.Description;

        public decimal? Value1 => ScienceRecord.Value1;

        public decimal? Value2 => ScienceRecord.Value2;

        #endregion

        public IHtmlString GetHtml (SystemScienceRecordType scienceRecordType, string valueFormat)
        {
            var valuesHtml = GetValuesHtml (scienceRecordType, valueFormat);
            var descriptionHtml = GetDesciptionHtml (scienceRecordType);

            return new HtmlString ($"{valuesHtml}{descriptionHtml}");
        }

        string GetValuesHtml (SystemScienceRecordType scienceRecordType, string valueFormat)
        {
            if (ScienceRecord.ScienceRecordType.NumOfValues == 1) {
                var microdataAttr = GetMicrodataAttrForValue (scienceRecordType, 1);
                return $"<span{microdataAttr} class=\"values\">{FormatValue (ScienceRecord.Value1, valueFormat)}</span>";
            }

            if (ScienceRecord.ScienceRecordType.NumOfValues == 2) {
                var microdataAttr1 = GetMicrodataAttrForValue (scienceRecordType, 1);
                var value1 = FormatValue (ScienceRecord.Value1, valueFormat);
                var value2 = FormatValue (ScienceRecord.Value2, valueFormat);

                // TODO: Add parameter?
                // special case for articles which have single microdata attribute for two values
                if (scienceRecordType == SystemScienceRecordType.Articles) {
                    return $"<span{microdataAttr1} class=\"values\">{value1}&nbsp;/&nbsp;{value2}</span>";
                }

                var microdataAttr2 = GetMicrodataAttrForValue (scienceRecordType, 2);
                return $"<span class=\"values\"><span{microdataAttr1}>{value1}</span>&nbsp;/&nbsp;<span{microdataAttr2}>{value2}</span></span>";
            }

            return string.Empty;
        }

        string GetDesciptionHtml (SystemScienceRecordType scienceRecordType)
        {
            var microdataAttr = GetMicrodataAttrForDescription (scienceRecordType);
            if (!string.IsNullOrEmpty (ScienceRecord.Description)) {
                return $" <span{microdataAttr} class=\"hidden description\">{HttpUtility.HtmlDecode (ScienceRecord.Description)}</span>"
                    + "<a type=\"button\" href=\"#\" data-toggle=\"modal\""
                    + $" data-target=\"#u8y-sciencerecord-description-dialog-{Context.Module.ModuleId}\">[&#8230;]</a>";
            }

            if (ScienceRecord.ScienceRecordType.DescriptionIsRequired) {
                return $" <span{microdataAttr}>-</span>";
            }

            return string.Empty;
        }

        string GetMicrodataAttrForValue (SystemScienceRecordType scienceRecordType, int n = 0)
        {
            switch (scienceRecordType) {
                case SystemScienceRecordType.Scientists: return ItemProp ("nprNir");
                case SystemScienceRecordType.Students: return ItemProp ("studNir");
                case SystemScienceRecordType.Monographs: return ItemProp ("monografNir");
                case SystemScienceRecordType.Articles: return ItemProp ("articleNir");
                case SystemScienceRecordType.Patents: return (n == 1)? ItemProp ("patentRNir") : ItemProp ("patentZNir");
                case SystemScienceRecordType.Certificates: return (n == 1)? ItemProp ("svidRNir") : ItemProp ("svidZNir");
            }

            return string.Empty;
        }

        string GetMicrodataAttrForDescription (SystemScienceRecordType scienceRecordType)
        {
            switch (scienceRecordType) {
                case SystemScienceRecordType.Directions: return ItemProp ("perechenNir");
                case SystemScienceRecordType.Base: return ItemProp ("baseNir");
            }

            return string.Empty;
        }

        string ItemProp (string microdata) => $" itemprop=\"{microdata}\"";

        string FormatValue (decimal? value, string format) => value != null? value.Value.ToString (format) : "-";
    }
}
