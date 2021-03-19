using System;

namespace R7.University.Models
{
    public interface IScience
    {
        int ScienceId { get; }

        string Directions { get; }

        string Results { get; }

        string Base { get; }

        [Obsolete]
        int? Scientists { get; }

        [Obsolete]
        int? Students { get; }

        [Obsolete]
        int? Monographs { get; }

        [Obsolete]
        int? Articles { get; }

        [Obsolete]
        int? ArticlesForeign { get; }

        [Obsolete]
        int? Patents { get; }

        [Obsolete]
        int? PatentsForeign { get; }

        [Obsolete]
        int? Certificates { get; }

        [Obsolete]
        int? CertificatesForeign { get; }

        [Obsolete]
        decimal? FinancingByScientist { get; }

        IEduProgram EduProgram { get; }
    }

    public interface IScienceWritable : IScience
    {
        new int ScienceId { get; set; }

        [Obsolete]
        new string Directions { get; set; }

        [Obsolete]
        new string Results { get; set; }

        [Obsolete]
        new string Base { get; set; }

        [Obsolete]
        new int? Scientists { get; set; }

        [Obsolete]
        new int? Students { get; set; }

        [Obsolete]
        new int? Monographs { get; set; }

        [Obsolete]
        new int? Articles { get; set; }

        [Obsolete]
        new int? ArticlesForeign { get; set; }

        [Obsolete]
        new int? Patents { get; set; }

        [Obsolete]
        new int? PatentsForeign { get; set; }

        [Obsolete]
        new int? Certificates { get; set; }

        [Obsolete]
        new int? CertificatesForeign { get; set; }

        [Obsolete]
        new decimal? FinancingByScientist { get; set; }

        new IEduProgram EduProgram { get; set; }
    }

    public class ScienceInfo : IScienceWritable
    {
        public int ScienceId { get; set; }

        public string Directions { get; set; }

        public string Results { get; set; }

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
