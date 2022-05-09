using Core.Attacking.Interfaces;
using Core.Utilities;

namespace Core.Attacking.Mono
{
    public class MonoAnimationAttackExecutor : MonoAbstractAttackExecutor
    {
        private AnimationAttackExecutor _executor;

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

        private static void SubscribeToDispatcher(MixinAttackAnimationEventDispatcher dispatcher, AnimationAttackExecutor executor)
        {
            dispatcher.EnableHitbox.AddListener(() =>
            {
                if (executor.IsAttacking)
                {
                    executor.OnEnableHitbox();
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
    }
}