using UnityEngine;

public class CharacterShadow : MonoBehaviour
{
    [SerializeField] private SpriteRenderer shadowSprite;
    [SerializeField] private LayerMask shadowCastMask;
    [SerializeField, Range(0, 1)] private float maxTransparency = 0.7f;
    [SerializeField, Min(0)] private float shadowAppearDistance = 2f;
    [SerializeField] private Vector3 hitPointOffset;

    private void Update()
    {
        if (!shadowSprite)
        {
            return;
        }

        var ray = new Ray(transform.position, Vector3.down);
        var isHit = Physics.Raycast(ray, out var hitInfo, shadowAppearDistance, shadowCastMask);

        var transparency = isHit ? (1 - hitInfo.distance / shadowAppearDistance) : 0;
        var color = shadowSprite.color;
        shadowSprite.color = new Color(color.r, color.g, color.b, transparency * maxTransparency);
        shadowSprite.transform.position = hitInfo.point + hitPointOffset;
    }
}
