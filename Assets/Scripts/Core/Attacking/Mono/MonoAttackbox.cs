using System.Collections.Generic;
using Core.Attacking.Interfaces;
using System.Linq;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Core.Attacking.Mono
{
    [RequireComponent(typeof(MixinTriggerEventDispatcher))]
    [RequireComponent(typeof(MixinColliderStorage))]
    [RequireComponent(typeof(MixinHitInfoContainer))]
    public class MonoAttackbox : MonoBehaviour
    {
        [SerializeField] private bool disableOnInit = true;
        [SerializeField] private List<MonoHurtbox> ignored = new List<MonoHurtbox>();


        private Team GetTeam()
        {
            var mixinTeam = GetComponentInParent<MixinTeam>();
            return mixinTeam != null ? mixinTeam.Team : Team.None;
        }

        private IAttackbox CreateAttackbox()
        {
            var storage = GetComponent<MixinColliderStorage>();

            var attackbox = new Attackbox(transform, storage.GetColliders().ToArray())
            {
                Ignored = ignored.Select(mono => mono.Hurtbox).ToList(),
                Team = GetTeam()
            };

            if (disableOnInit)
            {
                attackbox.Disable();
            }

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

        private IAttackbox _attackbox;

        public IAttackbox Attackbox
        {
            get
            {
                if (_attackbox != null)
                {
                    return _attackbox;
                }

                _attackbox = CreateAttackbox();
                _attackbox.Hit += hit => hit.ProcessHit(_attackbox, GetHitInfo());
                return _attackbox;
            }
        }
    }
}