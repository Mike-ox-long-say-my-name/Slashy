using Core.Attacking;
using Core.Attacking.Interfaces;
using UnityEngine;

namespace Miscellaneous
{
    [RequireComponent(typeof(DotAttackExecutor))]
    public class GroundBloodFire : MonoBehaviour
    {
        private IDotAttackExecutor _attackExecutor;
        [SerializeField] private ParticleSystem fireEffect;
        [SerializeField] private bool fireOnAwake;

        private bool _fired;
        private float _timeRemained;

        public void Fire()
        {
            Fire(-1);
        }

        public void Fire(float lifeTime)
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
            _attackExecutor = GetComponent<IMonoDotAttackExecutor>()?.Resolve();
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