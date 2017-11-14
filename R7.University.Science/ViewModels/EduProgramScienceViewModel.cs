﻿//
//  EduProgramScienceViewModel.cs
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

using System.Collections.Generic;
using System.Linq;
using System.Web;
using R7.Dnn.Extensions.ViewModels;
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.Science.Models;
using R7.University.ViewModels;

namespace R7.University.Science.ViewModels
{
    public class EduProgramScienceViewModel: EduProgramViewModelBase
    {
        protected ViewModelContext<ScienceDirectorySettings> Context;

        public EduProgramScienceViewModel (IEduProgram eduProgram, ViewModelContext<ScienceDirectorySettings> context)
            : base (eduProgram)
        {
            Context = context;
        }

        IEnumerable<ScienceRecordViewModel> _scienceRecordViewModels;
        public IEnumerable<ScienceRecordViewModel> ScienceRecordViewModels => 
            _scienceRecordViewModels ?? (_scienceRecordViewModels = EduProgram.ScienceRecords
                                         .Select (sr => new ScienceRecordViewModel (sr, Context)));

        ScienceRecordViewModel GetScienceRecordByType (SystemScienceRecordType systemScienceRecordType)
        {
            return ScienceRecordViewModels
                .FirstOrDefault (sr => sr.ScienceRecordType.GetSystemScienceRecordType () == systemScienceRecordType);
        }

        public IHtmlString GetScienceRecordHtml (SystemScienceRecordType systemScienceRecordType)
        {
            var scienceRecord = GetScienceRecordByType (systemScienceRecordType);
            if (scienceRecord != null) {
                return scienceRecord.GetHtml (systemScienceRecordType);
            }

            // TODO: Return default markup if science record don't exists?
            return new HtmlString (string.Empty);
        }

        public IHtmlString DirectionsHtml => GetScienceRecordHtml (SystemScienceRecordType.Directions);

        public IHtmlString BaseHtml => GetScienceRecordHtml (SystemScienceRecordType.Base);

        public IHtmlString ScientistsHtml => GetScienceRecordHtml (SystemScienceRecordType.Scientists);

        public IHtmlString StudentsHtml => GetScienceRecordHtml (SystemScienceRecordType.Students);

        public IHtmlString MonographsHtml => GetScienceRecordHtml (SystemScienceRecordType.Monographs);

        public IHtmlString ArticlesHtml => GetScienceRecordHtml (SystemScienceRecordType.Articles);

        public IHtmlString PatentsHtml => GetScienceRecordHtml (SystemScienceRecordType.Patents);

        public IHtmlString CertificatesHtml => GetScienceRecordHtml (SystemScienceRecordType.Certificates);

        public IHtmlString FinancesHtml => GetScienceRecordHtml (SystemScienceRecordType.Finances);

        public string EditUrl =>
            Context.Module.EditUrl ("eduprogram_id", EduProgram.EduProgramID.ToString (), "EditScience");

        public string CssClass =>
            EduProgram.IsPublished (HttpContext.Current.Timestamp) ? string.Empty : "u8y-not-published";
    }
}