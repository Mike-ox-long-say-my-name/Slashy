using Core.Player;
using System.Collections;
using UnityEngine;

namespace Core.Levels
{
    public class Bonfire : MonoBehaviour, IInteractable
    {
        [SerializeField] private int id;
        [SerializeField] private BonfireSaveData data;
        [SerializeField] private ParticleSystem fireEffect;
        [SerializeField] private Vector3 respawnOffset;
        [SerializeField] private Vector3 playerPositionOffset = new Vector3(1, 0, 0);

        [SerializeField] private float litDelay;

        private void Awake()
        {
            if (data == null)
            {
                Debug.LogWarning("Bonfire data is null");
                return;
            }

            var isLit = data.IsLit(id);
            if (!isLit)
            {
                return;
            }

            if (fireEffect == null)
            {
                Debug.LogWarning("Fire particlesis null");
                return;
            }

            var main = fireEffect.main;
            main.prewarm = true;
            fireEffect.Play();
        }

        private void OnTouchedBonfire()
        {
            StartCoroutine(PlayParticlesAfter(litDelay));
            PlayerManager.Instance.PlayerTouchedBonfire.RemoveListener(OnTouchedBonfire);
        }

        public Vector3 GetRespawnPosition()
        {
            return transform.position + respawnOffset;
        }

        public Vector3 GetPlayerAnimationPosition()
        {
            return transform.position + playerPositionOffset;
        }

        private void TouchBonfire()
        {
            data.SetStatus(id, true);

            RespawnManager.Instance.UpdateRespawnData(this);
            PlayerManager.Instance.PlayerTouchedBonfire.AddListener(OnTouchedBonfire);
        }

        private IEnumerator PlayParticlesAfter(float time)
        {
            yield return new WaitForSeconds(time);
            if (fireEffect != null)
            {
                fireEffect.Play();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(GetRespawnPosition(), 0.3f);
            Gizmos.DrawSphere(GetPlayerAnimationPosition(), 0.2f);
        }

        public bool IsInteractable { get; set; } = false;
        public InteractionMask Mask => InteractionMask.Bonfire;

        public InteractionResult Interact()
        {
            TouchBonfire();
            return new InteractionResult(InteractionType.TouchedBonfire, this);
        }
    }
}