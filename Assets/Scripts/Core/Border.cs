using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(BoxCollider))]
    public class Border : MonoBehaviour
    {
        [SerializeField] private bool isAggroBorder = false;

        [SerializeField] private BoxCollider borderCollider;
        
        public bool IsAggroBorder => isAggroBorder;


        public float PositionX => transform.position.x;
        public bool IsEnabled => borderCollider.enabled;

        private void OnValidate()
        {
            borderCollider = GetComponent<BoxCollider>();
            if (isAggroBorder)
            {
                Disable();
            }
        }

        public void Enable()
        {
            borderCollider.enabled = true;
        }

        public void Disable()
        {
            borderCollider.enabled = false;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = new Color(0.2f, 0.2f, 0.2f, 0.5f);
            Gizmos.DrawCube(transform.position, borderCollider.size);
        }
    }
}