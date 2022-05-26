namespace Core
{
    public interface IObjectLocator
    {
        T[] FindAll<T>();
    }
}