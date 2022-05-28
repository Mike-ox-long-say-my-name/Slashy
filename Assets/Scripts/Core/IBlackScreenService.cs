using System;

namespace Core
{
    public interface IBlackScreenService
    {
        void Blackout();
        void Whiteout();
        
        void Blackout(float time);
        void Whiteout(float time);
    }
}