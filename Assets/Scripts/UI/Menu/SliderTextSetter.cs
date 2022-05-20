using TMPro;
using UnityEngine;

namespace UI.Menu
{
    public class SliderTextSetter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI textMesh;

        public void SetText(float value)
        {
            textMesh.text = Mathf.RoundToInt(value).ToString();
        }
    }
}