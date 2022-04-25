namespace Core.DependencyInjection
{
    public interface IMonoWrapper<out T> where T : class
    {
        [AutoResolve]
        T Resolve();
    }
}