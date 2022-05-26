using System;

namespace Core
{
    public interface IAggroListener
    {
        event Action FightStarted;
        event Action FightEnded;
        bool IsFighting { get; }
        void IncreaseAggroCounter();
        void DecreaseAggroCounter();
        void ResetAggroCounter();
    }
}