//
//  IScience.cs
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

namespace R7.University.Models
{
    public interface IScience
    {
        int ScienceId { get; }

        string Directions { get; }

        string Base { get; }

        int? Scientists { get; }

        int? Students { get; }

        int? Monographs { get; }

        int? Articles { get; }

        int? ArticlesForeign { get; }

        int? Patents { get; }

        int? PatentsForeign { get; }

        int? Certificates { get; }

        int? CertificatesForeign { get; }

        decimal? FinancingByScientist { get; }

        IEduProgram EduProgram { get; }
    }

    public interface IScienceWritable : IScience
    {
        new int ScienceId { get; set; }

        new string Directions { get; set; }

        new string Base { get; set; }

        new int? Scientists { get; set; }

        new int? Students { get; set; }

        new int? Monographs { get; set; }

        new int? Articles { get; set; }

        new int? ArticlesForeign { get; set; }

        new int? Patents { get; set; }

        new int? PatentsForeign { get; set; }

        new int? Certificates { get; set; }

        new int? CertificatesForeign { get; set; }

        new decimal? FinancingByScientist { get; set; }

        new IEduProgram EduProgram { get; set; }
    }

    public class ScienceInfo : IScienceWritable
    {
        public int ScienceId { get; set; }

        public string Directions { get; set; }

        public string Base { get; set; }

        public int? Scientists { get; set; }

        public int? Students { get; set; }

        public int? Monographs { get; set; }

        public int? Articles { get; set; }

        public int? ArticlesForeign { get; set; }

        public int? Patents { get; set; }

        public int? PatentsForeign { get; set; }

        public int? Certificates { get; set; }

        public int? CertificatesForeign { get; set; }

        public decimal? FinancingByScientist { get; set; }

        public virtual EduProgramInfo EduProgram { get; set; }

        IEduProgram IScience.EduProgram => EduProgram;

        IEduProgram IScienceWritable.EduProgram {
            get { return EduProgram; }
            set { EduProgram = (EduProgramInfo) value; }
        }
    }
}
