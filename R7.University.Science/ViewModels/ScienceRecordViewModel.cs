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

using R7.Dnn.Extensions.ViewModels;
using R7.University.Models;
using R7.University.Science.Models;

namespace R7.University.Science.ViewModels
{
    public class ScienceRecordViewModel: IScienceRecord
    {
        protected IScienceRecord Science;

        protected ViewModelContext<ScienceDirectorySettings> Context;

        #region IScienceRecord implementation

        public long ScienceRecordId => Science.ScienceRecordId;

        public int EduProgramId => Science.EduProgramId;

        public EduProgramInfo EduProgram => Science.EduProgram;

        public int ScienceRecordTypeId => Science.ScienceRecordTypeId;

        public ScienceRecordTypeInfo ScienceRecordType => Science.ScienceRecordType;

        public string Description => Science.Description;

        public decimal? Value1 => Science.Value1;

        public decimal? Value2 => Science.Value2;

        #endregion
    }
}
