using Core.Attacking.Interfaces;
using Core.Utilities;
using UnityEngine;

namespace Core.Attacking.Mono
{
    public class MonoAnimationAttackExecutor : MonoAbstractAttackExecutor
    {
        [SerializeField] private AudioClip attackSound;

        private AudioSource _audioSource;
        private AnimationAttackExecutor _executor;

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
        }

        public override IAttackExecutor GetExecutor()
        {
            return _executor ??= CreateExecutor();
        }

        protected virtual void ConfigureExecutor(AnimationAttackExecutor executor)
        {
            executor.UseDefaultHandler();
        }

        private AnimationAttackExecutor CreateExecutor()
        {
            var attackbox = GetComponentInChildren<MonoAttackbox>().Attackbox;

            var executor = new AnimationAttackExecutor(this.ToCoroutineHost(), attackbox);
            attackbox.Hit += (hit) =>
            {
                executor.RegisterHit(hit);
            };

            ConfigureExecutor(executor);

            var dispatcher = GetComponentInParent<MixinAttackAnimationEventDispatcher>();
            SubscribeToDispatcher(dispatcher, executor);
            return executor;
        }

        private void SubscribeToDispatcher(MixinAttackAnimationEventDispatcher dispatcher, AnimationAttackExecutor executor)
        {
            dispatcher.EnableHitbox.AddListener(() =>
            {
                if (executor.IsAttacking)
                {
                    executor.OnEnableHitbox();
                    TryPlaySound(attackSound);
                }
            });
            dispatcher.DisableHitbox.AddListener(() =>
            {
                if (executor.IsAttacking)
                {
                    executor.OnDisableHitbox();
                }
            });
            dispatcher.EndAttack.AddListener(() =>
            {
                if (executor.IsAttacking)
                {
                    executor.OnAnimationEnded();
                }
            });
        }

        private void TryPlaySound(AudioClip sound)
        {
            if (_audioSource && sound)
            {
                _audioSource.PlayOneShot(sound);
            }
        }
    }
}