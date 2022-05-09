using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Characters.Interfaces;
using Core.Characters.Mono;
using UnityEngine;

namespace Effects
{
    public class DamageTakenParticles : AbstractHitListener
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

            var source = hitInfo.Source.Transform;
            if (source != null)
            {
                PlayParticles(source);
            }
        }

        private void PlayParticles(Transform source)
        {
            var offsetX = Random.Range(0, maxPositionOffset.x) + baseXOffset;
            var offsetY = Random.Range(0, maxPositionOffset.y);

            // ����������� �� Y ������ �� ���� ������ �����
            var direction = (transform.position - source.position);
            direction.y = Random.Range(-maxYDirectionOffset, maxYDirectionOffset);
            direction.Normalize();

            var shape = bloodParticleSystem.shape;
            // ������� �� X � ������� �� ��������� �����
            offsetX *= -Mathf.Sign(direction.x);
            // ������� �� Y � ������� ���������� �����������
            offsetY *= -Mathf.Sign(direction.y);

            shape.position = new Vector3(offsetX, offsetY, 0);
            shape.rotation = (Quaternion.LookRotation(direction) * transform.rotation).eulerAngles;

            bloodParticleSystem.Play();
        }
    }
}
