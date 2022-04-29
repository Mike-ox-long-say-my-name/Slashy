using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using UnityEngine;

namespace Miscellaneous
{
    [RequireComponent(typeof(DotAttackExecutor))]
    public class GroundBloodFire : MonoBehaviour
    {
        public const float InfinityLifeTime = -1;

        private IDotAttackExecutor _attackExecutor;
        [SerializeField] private ParticleSystem fireEffect;
        [SerializeField] private bool fireOnAwake;

        private bool _fired;
        private float _timeRemained;

        public void Fire(float lifeTime = InfinityLifeTime)
        {
            if (_fired)
            {
                return;
            }
            _timeRemained = lifeTime;
            _fired = true;

            if (fireEffect != null)
            {
                fireEffect.gameObject.SetActive(true);
                fireEffect.Play();
            }

            _attackExecutor.EnableDot();
        }

        private void Awake()
        {
            _attackExecutor = GetComponent<MonoDotAttackHandler>().Executor;
            if (fireEffect != null)
            {
                fireEffect.gameObject.SetActive(false);
            }
            if (fireOnAwake)
            {
                Fire();
            }
        }

        private void Update()
        {
            if (!_fired)
            {
                return;
            }

            if (_fired && _timeRemained > 0)
            {
                _timeRemained -= Time.deltaTime;
                if (_timeRemained <= 0)
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}