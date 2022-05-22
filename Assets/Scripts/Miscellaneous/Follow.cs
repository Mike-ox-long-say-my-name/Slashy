using UnityEngine;

namespace Miscellaneous
{
    public class Follow : MonoBehaviour
    {
        [SerializeField] private Transform anchor;

        private void LateUpdate()
        {
            if (anchor != null)
            {
                transform.position = anchor.position;
            }
        }
    }
}