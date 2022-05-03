using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Characters.Interfaces;
using Core.Characters.Mono;
using UnityEngine;

namespace Effects
{
    public class DamageTakenParticles : BaseCharacterHitListener
    {
        [SerializeField] private float baseXOffset = 0.1f;
        [SerializeField] private Vector2 maxPositionOffset = new Vector2(0.2f, 0.4f);
        [SerializeField, Min(0)] private float maxYDirectionOffset = 1f;
        [SerializeField] private ParticleSystem bloodParticleSystem;

        private void OnEnable()
        {
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
        }

        protected override void OnHitReceived(IHitReceiver entity, HitInfo hitInfo)
        {
            if (bloodParticleSystem == null)
            {
                Debug.LogWarning("Blood Particle System not found", this);
                return;
            }

            if (hitInfo.Source.Character == null)
            {
                return;
            }

            if (!(entity is ICharacter character))
            {
                return;
            }

            PlayParticles(character.VelocityMovement.BaseMovement.Transform,
                hitInfo.Source.Character.VelocityMovement.BaseMovement.Transform);
        }

        private void PlayParticles(Transform receiver, Transform source)
        {
            var offsetX = Random.Range(0, maxPositionOffset.x) + baseXOffset;
            var offsetY = Random.Range(0, maxPositionOffset.y);

            // Направление по Y влияет на угол полета крови
            var direction = (transform.position - source.position);
            direction.y = Random.Range(-maxYDirectionOffset, maxYDirectionOffset);
            direction.Normalize();

            var shape = bloodParticleSystem.shape;
            // Смещаем по X в сторону от получения удара
            offsetX *= -Mathf.Sign(direction.x);
            // Смещаем по Y в сторону выбранного направления
            offsetY *= -Mathf.Sign(direction.y);

            shape.position = new Vector3(offsetX, offsetY, 0);
            shape.rotation = (Quaternion.LookRotation(direction) * receiver.rotation).eulerAngles;

            bloodParticleSystem.Play();
        }
    }
}
