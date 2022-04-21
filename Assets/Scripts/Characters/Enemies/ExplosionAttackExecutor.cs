using System.Collections;
using Attacking;
using Core;
using UnityEngine;

namespace Characters.Enemies
{
    public class ExplosionAttackExecutor : AttackExecutor
    {
        [SerializeField] private DamageInfo damageInfo;
        [SerializeField] private ParticleSystem explosionParticles;
        
        private bool _hasParticles = true;

        private void Awake()
        {
            if (explosionParticles == null)
            {
                Debug.LogWarning("Explosion Particles is not assigned", this);
                _hasParticles = true;
            }
        }

        protected override IEnumerator Execute(IHitSource source)
        {
            yield return new WaitForSeconds(1.5f);

            if (_hasParticles)
            {
                explosionParticles.Play();
            }

            Hitbox.EnableWith(hit => hit.ReceiveHit(new HitInfo()
            {
                DamageInfo = damageInfo,
                HitSource = source
            }));
            yield return new WaitForSeconds(0.1f);
            Hitbox.DisableAndClear();
        }
    }
}