using System;

namespace Core
{
    public interface IPlayerDeathSequencePlayer
    {
        event Action DeathSequenceStarted;
        event Action DeathSequenceEnded;
        void Play();
    }
}