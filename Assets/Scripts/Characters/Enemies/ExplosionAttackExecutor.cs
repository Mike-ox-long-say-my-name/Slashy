using System.Collections;
using Attacking;
using Core;
using Environment.Damaging;
using UnityEngine;

namespace Characters.Enemies
{
    public class ExplosionAttackExecutor : AttackExecutor
    {
        [SerializeField] private DamageInfo damageInfo;
        [SerializeField] private ParticleSystem explosionParticles;
        [SerializeField] private GameObject bloodFirePrefab;

        [SerializeField, Min(2)] private int fireRows = 6;
        [SerializeField, Min(1)] private int firesPerRow = 3;
        [SerializeField, Min(0)] private float fireRowLength = 3;
        [SerializeField, Min(0)] private float baseOffsetDistance = 0.3f;
        [SerializeField, Min(0)] private float minFireLifeTime = 1;
        [SerializeField, Min(0)] private float maxFireLifeTime = 5;
        
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
            yield return new WaitForSeconds(0.4f);

            if (_hasParticles)
            {
                explosionParticles.Play();
            }

            Hitbox.EnableWith(hit => hit.ReceiveHit(new HitInfo()
            {
                DamageInfo = damageInfo,
                HitSource = source
            }));
            
            CreateBloodFires();

            yield return new WaitForSeconds(0.5f);

            Hitbox.DisableAndClear();
        }

        private void CreateBloodFires()
        {
            var groundOffset = Vector3.down * 0.6f;   

            var deltaAngle = Mathf.PI * 2 / fireRows;
            var deltaDistance = firesPerRow / fireRowLength;

            var angle = 0f;
            for (int i = 0; i < fireRows; i++)
            {
                var distance = baseOffsetDistance;
                var direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

                for (int j = 0; j < firesPerRow; j++)
                {
                    var fireObject = Instantiate(bloodFirePrefab, 
                        transform.position + groundOffset + direction * distance,
                        Quaternion.identity);

                    var lifeTime = Random.Range(minFireLifeTime, maxFireLifeTime);
                    fireObject.GetComponent<GroundBloodFire>().Fire(lifeTime);

                    distance += deltaDistance;
                }

                angle += deltaAngle;
            }
        }
    }
}