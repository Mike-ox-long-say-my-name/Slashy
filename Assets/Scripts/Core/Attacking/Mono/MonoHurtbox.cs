using Core.Attacking.Interfaces;
using Core.Characters;
using Core.Characters.Interfaces;
using UnityEngine;

namespace Core.Attacking.Mono
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public class MonoHurtbox : MonoBaseHitbox
    {
        private IHitReceiver _attachedTo;

        private void Awake()
        {
            var rigidBody = GetComponent<Rigidbody>();
            ConfigureRigidbody(rigidBody);
        }

        private static void ConfigureRigidbody(Rigidbody rigidBody)
        {
            rigidBody.useGravity = false;
            rigidBody.isKinematic = true;
            rigidBody.constraints = RigidbodyConstraints.FreezeAll;
            rigidBody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }

        public override IHitbox Hitbox => Hurtbox;

        private IHurtbox _hurtbox;

        public IHurtbox Hurtbox
        {
            get
            {
                if (_hurtbox != null)
                {
                    return _hurtbox;
                }

                _hurtbox = CreateHurtbox();
                return _hurtbox;
            }
        }

        private Team GetTeam()
        {
            var mixinTeam = GetComponentInParent<MixinTeam>();
            return mixinTeam != null ? mixinTeam.Team : Team.None;
        }

        private IHurtbox CreateHurtbox()
        {
            _attachedTo = GetComponentInParent<MixinHittable>().HitReceiver;

            var hurtbox = new Hurtbox(transform, colliders)
            {
                Team = GetTeam()
            };
            hurtbox.Hit += _attachedTo.ReceiveHit;

            return hurtbox;
        }
    }
}
