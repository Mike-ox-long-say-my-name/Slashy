using System.Collections.Generic;
using Core.Attacking.Interfaces;
using System.Linq;
using UnityEngine;

namespace Core.Attacking.Mono
{
    [RequireComponent(typeof(MixinTriggerEventDispatcher))]
    [RequireComponent(typeof(MixinColliderStorage))]
    [RequireComponent(typeof(MixinHitInfoContainer))]
    public class MonoDotAttackbox : MonoBehaviour
    {
        [SerializeField, Range(0.01f, 5)] private float hitInterval = 1f;
        [SerializeField, Min(0)] private int damageGroup;
        [SerializeField] private List<MonoHurtbox> ignored = new List<MonoHurtbox>();

        private IDotAttackbox CreateAttackbox()
        {
            var storage = GetComponent<MixinColliderStorage>();

            var attackbox = new DotAttackbox(transform, damageGroup, storage.GetColliders().ToArray())
            {
                HitInterval = hitInterval,
                Ignored = ignored.Select(mono => mono.Hurtbox).ToList()
            };
            attackbox.Hit += hit => hit.ProcessHit(attackbox, GetHitInfo());
            return attackbox;
        }

        private void Awake()
        {
            var dispatcher = GetComponent<MixinTriggerEventDispatcher>();
            SubscribeToDispatcher(dispatcher);
        }

        private void SubscribeToDispatcher(MixinTriggerEventDispatcher dispatcher)
        {
            dispatcher.Enter.AddListener(ProcessHit);
            dispatcher.Stay.AddListener(ProcessHit);
        }

        private void ProcessHit(Collider trigger)
        {
            var hurtbox = trigger.TryGetComponent<MonoHurtbox>(out var monoHurtbox) ? monoHurtbox.Hurtbox : null;
            if (hurtbox == null)
            {
                return;
            }

            Attackbox.ProcessHit(hurtbox);
        }

        private MixinHitInfoContainer _container;

        private HitInfo GetHitInfo()
        {
            if (_container == null)
            {
                _container = GetComponent<MixinHitInfoContainer>();
            }

            return _container.GetHitInfo();
        }

        private IDotAttackbox _attackbox;

        public IDotAttackbox Attackbox
        {
            get
            {
                if (_attackbox != null)
                {
                    return _attackbox;
                }

                _attackbox = CreateAttackbox();
                return _attackbox;
            }
        }

        private void Update()
        {
            Attackbox.Tick(Time.deltaTime);
        }
    }
}