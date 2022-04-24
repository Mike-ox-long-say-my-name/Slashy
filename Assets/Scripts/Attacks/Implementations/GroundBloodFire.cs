using UnityEngine;

namespace Attacks.Implementations
{
    [RequireComponent(typeof(DotAttackExecutor))]
    public class GroundBloodFire : MonoBehaviour
    {
        private MonoDotAttackHandler _attackExecutor;
        [SerializeField] private ParticleSystem fireEffect;
        [SerializeField] private bool fireOnStart;

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
            _attackExecutor.Executor.EnableDot();
        }

        private void Start()
        {
            _attackExecutor = GetComponent<MonoDotAttackHandler>();
            if (fireEffect != null)
            {
                fireEffect.gameObject.SetActive(false);
            }
            if (fireOnStart)
            {
                //Fire();
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