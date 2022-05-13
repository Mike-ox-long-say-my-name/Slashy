using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(BoxCollider))]
    public class Border : MonoBehaviour
    {
        [SerializeField] private bool isAggroBorder = false;

        public bool IsAggroBorder => isAggroBorder;

        private BoxCollider _borderCollider;

        public float X => transform.position.x;
        public bool IsEnabled => _borderCollider.enabled;

        private void Awake()
        {
            _borderCollider = GetComponent<BoxCollider>();
            if (isAggroBorder)
            {
                Disable();
            }
        }

        public void Enable()
        {
            _borderCollider.enabled = true;
        }

        public void Disable()
        {
            _borderCollider.enabled = false;
        }
    }
}