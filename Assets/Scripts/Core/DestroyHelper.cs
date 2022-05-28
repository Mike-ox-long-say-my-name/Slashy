using System;
using Core.Utilities;
using UnityEngine;

namespace Core
{
    public class DestroyHelper : MonoBehaviour
    {
        [SerializeField, Min(0)] private float destroyLaterTime = 0.5f;

        private Timer _destroyTimer;
        private ITimerRunner _timerRunner;

        private void Awake()
        {
            Construct();
        }

        private void Construct()
        {
            _timerRunner = Container.Get<ITimerRunner>();
            _destroyTimer = _timerRunner.CreateTimer(Destroy);
        }

        public void DestroyLater()
        {
            DestroyAfter(destroyLaterTime);
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }   

        private void DestroyAfter(float time)
        {
            if (_destroyTimer.IsRunning && _destroyTimer.TimeRemained < time)
            {
                return;
            }
            _destroyTimer.Start(time);
        }

        private void OnDestroy()
        {
            _timerRunner.RemoveTimer(_destroyTimer);
        }
    }
}