using Core;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Miscellaneous;
using System.Collections;
using UnityEngine;

namespace Characters.Enemies
{
    public class ExplosionMonoAttackHandler : MonoAttackHandler
    {
        private class ExplosionAttack : AttackExecutor
        {
            private readonly AttackData _data;

            public ExplosionAttack(AttackData data, ICoroutineHost host, IAttackbox attackbox)
                : base(host, attackbox)
            {
                _data = data;
            }

            protected override IEnumerator Execute()
            {
                yield return new WaitForSeconds(0.4f);

                if (_data.Particles != null)
                {
                    _data.Particles.Play();
                }

                Attackbox.Enable();

                CreateBloodFires();

                yield return new WaitForFixedUpdate();
                yield return new WaitForSeconds(0.2f);

                Attackbox.Disable();
            }

            private void CreateBloodFires()
            {
                var config = _data.Config;

                var groundOffset = Vector3.down * 0.6f;

                var deltaAngle = Mathf.PI * 2 / config.FireRows;
                var deltaDistance = config.FiresPerRow / config.FireRowLength;

                var angle = 0f;
                for (int i = 0; i < config.FireRows; i++)
                {
                    var distance = config.BaseOffsetDistance;
                    var direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

                    for (int j = 0; j < config.FiresPerRow; j++)
                    {
                        var fireObject = Instantiate(config.BloodFirePrefab,
                            _data.Transform.position + groundOffset + direction * distance,
                            Quaternion.identity);

                        var lifeTime = Random.Range(config.MinFireLifeTime, config.MaxFireLifeTime);
                        fireObject.GetComponent<GroundBloodFire>().Fire(lifeTime);

                        distance += deltaDistance;
                    }

                    angle += deltaAngle;
                }
            }
        }

        [SerializeField] private ParticleSystem explosionParticles;
        [SerializeField] private ExplodingHollowAttackConfig config;

        private struct AttackData
        {
            public ParticleSystem Particles { get; set; }
            public ExplodingHollowAttackConfig Config { get; set; }
            public Transform Transform { get; set; }
        }

        protected override IAttackExecutor CreateExecutor(ICoroutineHost host, IAttackbox attackbox)
        {
            var attackData = new AttackData
            {
                Particles = explosionParticles,
                Config = config,
                Transform = transform
            };
            return new ExplosionAttack(attackData, host, attackbox);
        }
    }
}