using UnityEngine;

namespace Core
{
    public class Marker : MonoBehaviour
    {
        [SerializeField] private Color debugColor = Color.yellow;
        
        public Vector3 Position => transform.position;
        public float Rotation => transform.eulerAngles.y > 0 ? 1 : -1;

        private void OnDrawGizmos()
        {
            GizmosHelper.PushColor(debugColor);
            Gizmos.DrawSphere(Position, 0.5f);
            GizmosHelper.PopColor();
        }
    }
}