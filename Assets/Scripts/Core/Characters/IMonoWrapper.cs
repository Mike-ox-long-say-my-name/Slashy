namespace Core.Characters
{
    public interface IMonoWrapper<out T> where T : class
    {
        T Native { get; }
    }
}