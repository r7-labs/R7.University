//
//  EduProgramDivisionViewModel.cs
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
using R7.University.ModelExtensions;
using R7.University.Models;
using R7.University.ViewModels;

namespace R7.University.EduProgram.ViewModels
{
    public class EduProgramDivisionViewModel: IEduProgramDivision
    {
        readonly IEduProgramDivision Model;

        public EduProgramDivisionViewModel (IEduProgramDivision division)
        {
            Model = division;
        }

        public DivisionInfo Division => Model.Division;

        public int DivisionId => Model.DivisionId;

        public string DivisionRole => Model.DivisionRole;

        public long EduProgramDivisionId => Model.EduProgramDivisionId;

        public int? EduProgramId => Model.EduProgramId;

        public int? EduProgramProfileId => Model.EduProgramProfileId;

        public string DivisionLink => FormatHelper.FormatEduProgramDivisionLink (Model.Division, Model.DivisionRole, Model.Division.IsPublished (HttpContext.Current.Timestamp));
    }
}
