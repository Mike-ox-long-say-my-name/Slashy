using UnityEngine;

namespace Core
{
    public class Ambush : MonoBehaviour
    {
        [SerializeField] private EnemyMarker enemyMarker;
        [SerializeField] private GameObject preAmbushObject;
        [SerializeField] private GameObject postAmbushObject;

        [SerializeField] private AudioSource ambushSource;

        private bool _isActivated;

        private void Awake()
        {
            MapDisablingEnemy();
        }

        private void MapDisablingEnemy()
        {
            if (enemyMarker.CreatedEnemy != null)
            {
                enemyMarker.CreatedEnemy.gameObject.SetActive(false);
            }
            else
            {
                enemyMarker.Created += enemy => enemy.gameObject.SetActive(false);
            }
        }

        public void ActivateAmbush()
        {
            if (_isActivated)
            {
                return;
            }

            var createdEnemy = enemyMarker.CreatedEnemy;
            if (createdEnemy == null)
            {
                // Не должно произойти
                Debug.LogWarning("Created enemy was null", this);
                return;
            }

            preAmbushObject.SetActive(false);

            PlayAmbushSound();

            createdEnemy.gameObject.SetActive(true);

            postAmbushObject.SetActive(true);
            _isActivated = true;
        }

        private void PlayAmbushSound()
        {
            if (ambushSource != null)
            {
                ambushSource.Play();
            }
            else
            {
                Debug.LogWarning("Ambush audio source is null", this);
            }
        }

        public void ResetAmbush()
        {
            preAmbushObject.SetActive(true);
            postAmbushObject.SetActive(false);
            _isActivated = false;
            MapDisablingEnemy();
        }
    }
}