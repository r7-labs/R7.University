namespace R7.University.Models
{
    public interface ISystemEntity
    {
        bool IsSystem { get; }
    }

    public interface ISystemEntityWritable: ISystemEntity
    {
        new bool IsSystem { get; set; }
    }
}
