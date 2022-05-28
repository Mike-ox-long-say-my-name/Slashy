using System;

namespace Characters.Enemies.Boss
{
    public interface IGameEndedSequencePlayer
    {
        event Action GameEndSequenceEnded;
        void Play();
    }
}