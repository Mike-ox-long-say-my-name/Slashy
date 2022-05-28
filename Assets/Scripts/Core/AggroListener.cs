using System;

namespace Core
{
    public class AggroListener : IAggroListener
    {
        public event Action FightStarted;
        public event Action FightEnded;

        private int _aggroCounter = 0;

        public bool IsFighting => _aggroCounter > 0;

        public AggroListener(ISceneLoader sceneLoader)
        {
            sceneLoader.SceneUnloading += _ => ResetAggroCounter();
        }

        public void IncreaseAggroCounter()
        {
            if (_aggroCounter++ == 0)
            {
                FightStarted?.Invoke();
            }
        }

        public void DecreaseAggroCounter()
        {
            if (--_aggroCounter == 0)
            {
                FightEnded?.Invoke();
            }
        }

        public void ResetAggroCounter()
        {
            if (_aggroCounter == 0)
            {
                return;
            }

            _aggroCounter = 0;
            FightEnded?.Invoke();
        }
    }
}