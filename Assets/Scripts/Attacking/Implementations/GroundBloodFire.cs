using Attacking;
using UnityEngine;

namespace Environment.Damaging
{
    [RequireComponent(typeof(DotAttackExecutor))]
    public class GroundBloodFire : MonoBehaviour
    {
        private DotAttackExecutor _attackExecutor;
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
                fireEffect.Play();
            }
            _attackExecutor.Enable();
        }

        private void Awake()
        {
            _attackExecutor = GetComponent<DotAttackExecutor>();
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