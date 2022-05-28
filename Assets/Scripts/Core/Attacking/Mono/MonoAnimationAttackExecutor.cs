using System;
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
        private ICoroutineRunner _coroutineRunner;

        [SerializeField] private MonoAttackbox attackbox;
        [SerializeField] private AttackAnimationEvents animationEvents;

        private void Reset()
        {
            attackbox = GetComponentInChildren<MonoAttackbox>();
            animationEvents = GetComponentInParent<AttackAnimationEvents>();
        }

        private void Awake()
        {
            Construct();
        }

        public override IAttackExecutor GetExecutor()
        {
            return _executor ??= CreateExecutor();
        }

        private void Construct()
        {
            _audioSource = GetComponent<AudioSource>();
            _coroutineRunner = Container.Get<ICoroutineRunner>();
        }

        protected virtual void ConfigureExecutor(AnimationAttackExecutor executor)
        {
            executor.UseDefaultHandler();
        }

        private AnimationAttackExecutor CreateExecutor()
        {
            var nativeAttackbox = attackbox.Attackbox;

            var executor = new AnimationAttackExecutor(_coroutineRunner, nativeAttackbox);

            ConfigureExecutor(executor);
            SubscribeToDispatcher(executor);

            return executor;
        }

        private void SubscribeToDispatcher(AnimationAttackExecutor executor)
        {
            animationEvents.EnableHitbox += () =>
            {
                if (executor.IsAttacking)
                {
                    executor.OnEnableHitbox();
                    TryPlaySound(attackSound);
                }
            };
            animationEvents.DisableHitbox += () =>
            {
                if (executor.IsAttacking)
                {
                    executor.OnDisableHitbox();
                }
            };
            animationEvents.AnimationEnded += () =>
            {
                if (executor.IsAttacking)
                {
                    executor.OnAnimationEnded();
                }
            };
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