using UnityEngine;
using UnityEngine.UI;

namespace Core
{
    public class BlackScreenServiceContainer : MonoBehaviour
    {
        [SerializeField] private Image screenImage;
        [SerializeField] private BlackScreenSettings settings;
        public Image ScreenImage => screenImage;
        public float Threshold => settings.threshold;
        public float DefaultTime => settings.defaultTime;
    }
}