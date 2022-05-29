using Core.Levels;
using UnityEngine;

namespace UI
{
    public class BonfireKeyHint : MonoBehaviour
    {
        [SerializeField] private Bonfire bonfire;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Color litColor = Color.gray;
        [SerializeField] private Color unlitColor = Color.red;

        private Color GetHintColor()
        {
            return bonfire.IsLit ? litColor : unlitColor;
        }
        
        public void Show()
        {
            spriteRenderer.enabled = true;
            spriteRenderer.color = GetHintColor();
        }

        public void Hide()
        {
            spriteRenderer.enabled = false;
        }
    }
}