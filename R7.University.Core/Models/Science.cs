namespace R7.University.Models
{
    public interface IScience
    {
        int ScienceId { get; }

        string Directions { get; }

        string Results { get; }

        string Base { get; }

        IEduProgram EduProgram { get; }
    }

    public interface IScienceWritable : IScience
    {
        new int ScienceId { get; set; }

        new string Directions { get; set; }

        new string Results { get; set; }

        new string Base { get; set; }

        new IEduProgram EduProgram { get; set; }
    }

    public class ScienceInfo : IScienceWritable
    {
        public int ScienceId { get; set; }

        public string Directions { get; set; }

        public string Results { get; set; }

        public string Base { get; set; }

        public virtual EduProgramInfo EduProgram { get; set; }

        IEduProgram IScience.EduProgram => EduProgram;

        IEduProgram IScienceWritable.EduProgram {
            get { return EduProgram; }
            set { EduProgram = (EduProgramInfo) value; }
        }
    }
}
