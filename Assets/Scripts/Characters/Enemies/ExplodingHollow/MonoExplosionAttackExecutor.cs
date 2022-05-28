using Core;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Core.Utilities;
using Miscellaneous;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Characters.Enemies.ExplodingHollow
{
    public class MonoExplosionAttackExecutor : MonoAbstractAttackExecutor
    {
        private AudioSource _audioSource;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        [SerializeField, Range(0, 2)] private float explosionDelay = 0.4f;
        [SerializeField, Range(0.1f, 3)] private float explosionHitboxLingerTime = 1.5f;

        [SerializeField] private ParticleSystem explosionParticles;
        [SerializeField] private ExplodingHollowAttackConfig config;

        private class ExplosionAttack : AttackExecutor
        {
            private readonly AttackData _data;

            public ExplosionAttack(AttackData data, ICoroutineRunner coroutineRunner, IAttackbox attackbox)
                : base(coroutineRunner, attackbox)
            {
                _data = data;
            }

            protected override IEnumerator Execute()
            {
                yield return new WaitForSeconds(_data.ExplosionDelay);

                if (_data.Particles)
                {
                    _data.Particles.Play();
                }

                if (_data.Config.ExplosionSound && _data.AudioSource)
                {
                    _data.AudioSource.PlayOneShot(_data.Config.ExplosionSound);
                }

                Attackbox.Enable();

                CreateBloodFires();

                yield return new WaitForFixedUpdate();
                yield return new WaitForSeconds(_data.LingerTime);

                Attackbox.Disable();
            }

            private void CreateBloodFires()
            {
                var config = _data.Config;

                var groundOffset = Vector3.down;

                var deltaAngle = Mathf.PI * 2 / config.FireRows;
                var deltaDistance = config.FireRowLength / config.FiresPerRow;

                var angle = 0f;
                CreateFireRows();

                void CreateFireRows()
                {
                    for (var i = 0; i < config.FireRows; i++)
                    {
                        var distance = config.BaseOffsetDistance;
                        var direction = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle));

                        CreateFiresRow();
                        angle += deltaAngle;

                        void CreateFiresRow()
                        {
                            for (var j = 0; j < config.FiresPerRow; j++)
                            {
                                var fireObject = Instantiate(config.BloodFirePrefab,
                                    _data.Transform.position + groundOffset + direction * distance,
                                    Quaternion.identity);

                                var lifeTime = Random.Range(config.MinFireLifeTime, config.MaxFireLifeTime);
                                fireObject.GetComponent<GroundBloodFire>().Fire(lifeTime);

                                distance += deltaDistance;
                            }
                        }
                    }
                }
            }
        }

        private struct AttackData
        {
            public ParticleSystem Particles { get; set; }
            public AudioSource AudioSource { get; set; }
            public ExplodingHollowAttackConfig Config { get; set; }
            public Transform Transform { get; set; }
            public float ExplosionDelay { get; set; }
            public float LingerTime { get; set; }
        }

        private IAttackExecutor _executor;

        public override IAttackExecutor GetExecutor()
        {
            return _executor ??= CreateExecutor();
        }

        private IAttackExecutor CreateExecutor()
        {
            var attackData = new AttackData
            {
                Particles = explosionParticles,
                Config = config,
                Transform = transform,
                ExplosionDelay = explosionDelay,
                LingerTime = explosionHitboxLingerTime,
                AudioSource = _audioSource
            };

            var attackbox = GetComponentInChildren<MonoAttackbox>().Attackbox;
            var coroutineRunner = Container.Get<ICoroutineRunner>();
            return new ExplosionAttack(attackData, coroutineRunner, attackbox);
        }
    }
}