namespace R7.University.Models
{
    public interface IEmployeeDiscipline
    {
        long EmployeeDisciplineID { get; }

        int EmployeeID { get; }

        int EduProgramProfileID { get; }

        string Disciplines { get; }

        IEmployee Employee { get; }

        IEduProfile EduProfile { get; }
    }

    public interface IEmployeeDisciplineWritable: IEmployeeDiscipline
    {
        new long EmployeeDisciplineID { get; set; }

        new int EmployeeID { get; set; }

        new int EduProgramProfileID { get; set; }

        new string Disciplines { get; set; }

        new IEmployee Employee { get; set; }

        new IEduProfile EduProfile { get; set; }
    }

    public class EmployeeDisciplineInfo: IEmployeeDisciplineWritable
    {
        public long EmployeeDisciplineID { get; set; }

        public int EmployeeID { get; set; }

        public int EduProgramProfileID { get; set; }

        public string Disciplines { get; set; }

        public virtual EmployeeInfo Employee { get; set; }

        IEmployee IEmployeeDiscipline.Employee => Employee;

        IEmployee IEmployeeDisciplineWritable.Employee {
            get { return Employee;  }
            set { Employee = (EmployeeInfo) value; }
        }

        public virtual EduProfileInfo EduProfile { get; set; }

        IEduProfile IEmployeeDiscipline.EduProfile => EduProfile;

        IEduProfile IEmployeeDisciplineWritable.EduProfile {
            get { return EduProfile; }
            set { EduProfile = (EduProfileInfo) value; }
        }
    }
}
