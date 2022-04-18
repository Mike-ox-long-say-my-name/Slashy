using UnityEngine;

namespace Effects
{
    public class CharacterShadow : MonoBehaviour
    {
        [SerializeField] private Transform castParent;
        [SerializeField] private SpriteRenderer shadowSpriteRenderer;

        [SerializeField] private LayerMask shadowCastMask;
        [SerializeField, Range(0, 1)] private float maxTransparency = 0.7f;
        [SerializeField, Min(0)] private float shadowAppearDistance = 2f;
        [SerializeField] private Vector3 hitPointOffset;

        private void Awake()
        {
            if (castParent == null)
            {
                Debug.LogWarning("Cast Parent is not assigned", this);
                enabled = false;
            }
            if (shadowSpriteRenderer == null)
            {
                Debug.LogWarning("Shadow Sprite Renderer is not assigned", this);
                enabled = false;
            }
        }

        private void Update()
        {
            var ray = new Ray(castParent.position, Vector3.down);
            var isHit = Physics.Raycast(ray, out var hitInfo, shadowAppearDistance, shadowCastMask);

            var transparency = isHit ? (1 - hitInfo.distance / shadowAppearDistance) : 0;
            var color = shadowSpriteRenderer.color;
            shadowSpriteRenderer.color = new Color(color.r, color.g, color.b, transparency * maxTransparency);
            shadowSpriteRenderer.transform.position = hitInfo.point + hitPointOffset;
        }
    }
}
