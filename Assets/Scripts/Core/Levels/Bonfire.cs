using System.Collections;
using UnityEngine;

namespace Core.Levels
{
    public class Bonfire : AbstractInteractable
    {
        [SerializeField] private int id;
        [SerializeField] private BonfireSaveData data;
        [SerializeField] private ParticleSystem fireEffect;
        [SerializeField] private Vector3 respawnOffset;
        [SerializeField] private Vector3 playerPositionOffset = new Vector3(1, 0, 0);

        [SerializeField] private float litDelay;
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip litSound;
        [SerializeField] private AudioClip ambientSound;
        
        private LazyPlayer _player;
        private IRespawnController _respawnController;

        private void Awake()
        {
            Construct();
            
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
                Debug.LogWarning("Fire particles is null");
                return;
            }

            var main = fireEffect.main;
            main.prewarm = true;
            audioSource.loop = true;
            audioSource.clip = ambientSound;
            audioSource.Play();
            fireEffect.Play();
        }

        private void Construct()
        {
            var playerFactory = Container.Get<IPlayerFactory>();
            _player = playerFactory.GetLazyPlayer();

            _respawnController = Container.Get<IRespawnController>();
        }

        private void OnTouchedBonfire()
        {
            StartCoroutine(PlayParticlesAfter(litDelay));
            
            _player.Value.TouchedBonfire -= OnTouchedBonfire;
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

            _respawnController.UpdateRespawnData(this);
            _player.Value.TouchedBonfire += OnTouchedBonfire;
        }

        private IEnumerator PlayParticlesAfter(float time)
        {
            yield return new WaitForSeconds(time);
            PlayLitSound();
            PlayLitParticles();

            yield return new WaitForSeconds(time);

            PlayAmbientSound();

            void PlayLitSound()
            {
                if (audioSource == null || litSound == null)
                {
                    return;
                }
                audioSource.loop = false;
                audioSource.PlayOneShot(litSound);
            }

            void PlayLitParticles()
            {
                if (fireEffect != null)
                {
                    fireEffect.Play();
                }
            }

            void PlayAmbientSound()
            {
                if (audioSource == null || ambientSound == null)
                {
                    return;
                }
                audioSource.loop = true;
                audioSource.clip = ambientSound;
                audioSource.Play();
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(GetRespawnPosition(), 0.3f);
            Gizmos.DrawSphere(GetPlayerAnimationPosition(), 0.2f);
        }

        protected override object InteractInternal()
        {
            TouchBonfire();
            return this;
        }
    }
}