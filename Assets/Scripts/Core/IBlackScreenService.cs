namespace Core
{
    public interface IBlackScreenService
    {
        void Blackout(float time);
        void Whiteout(float time);
    }
}