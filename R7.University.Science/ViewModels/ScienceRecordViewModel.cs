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

        public IHtmlString Html
        {
            get {
                return new HtmlString ($"{ScienceRecord.Description} {ScienceRecord.Value1} {ScienceRecord.Value2}");
            }
        }
    }
}
