using Core.Modules;
using UnityEngine;

namespace Core
{
    public class Ambush : MonoBehaviour
    {
        [SerializeField] private EnemyMarker enemyMarker;
        [SerializeField] private GameObject preAmbushObject;
        [SerializeField] private GameObject postAmbushObject;

        private bool _isActivated;
        private GameObject _createdEnemy;
        
        private void Awake()
        {
            enemyMarker.Created += OnEnemyCreated;
        }

        private void OnEnemyCreated(MixinMovementBase obj)
        {
            _createdEnemy = obj.gameObject;
            _createdEnemy.SetActive(false);
        }

        public void ActivateAmbush()
        {
            if (_isActivated)
            {
                return;
            }
            preAmbushObject.SetActive(false);
            _createdEnemy.SetActive(true);
            postAmbushObject.SetActive(true);
            _isActivated = true;
        }

        public void ResetAmbush()
        {
            preAmbushObject.SetActive(true);
            postAmbushObject.SetActive(false);
            _isActivated = false;
            _createdEnemy = null;
        }
    }
}