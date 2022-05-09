using Core.Utilities;
using UnityEngine;

namespace Core
{
    public class MixinDestroyable : MonoBehaviour
    {
        [SerializeField, Min(0)] private float destroyLaterTime = 0.5f;

        private readonly Timer _destroyTimer = new Timer();

        private void Awake()
        {
            _destroyTimer.Timeout += Destroy;
        }

        public void Destroy()
        {
            Destroy(gameObject);
        }

        public void DestroyAfter(float time)
        {
            if (_destroyTimer.IsRunning && _destroyTimer.TimeRemained < time)
            {
                return;
            }
            _destroyTimer.Start(time);
        }

        private void Update()
        {
            _destroyTimer.Tick(Time.deltaTime);
        }

        public void DestroyLater()
        {
            DestroyAfter(destroyLaterTime);
        }
    }
}