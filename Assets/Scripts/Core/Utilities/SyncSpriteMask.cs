using UnityEngine;

namespace Core.Utilities
{
    [RequireComponent(typeof(SpriteRenderer))]
    [RequireComponent(typeof(SpriteMask))]
    public class SyncSpriteMask : MonoBehaviour
    {
        private SpriteRenderer _spriteRenderer;
        private SpriteMask _mask;

        private void Awake()
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
            _mask = GetComponent<SpriteMask>();
        }

        private void LateUpdate()
        {
            _mask.sprite = _spriteRenderer.sprite;
        }
    }
}
