using Core;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    [DefaultExecutionOrder(5)]
    public class VolumeSliderController : MonoBehaviour
    {
        [SerializeField] private Slider soundSlider;
        [SerializeField] private Slider musicSlider;

        private void Start()
        {
            var manager = AudioManager.Instance;
            soundSlider.SetValueWithoutNotify(manager.SoundVolume * soundSlider.maxValue);
            musicSlider.SetValueWithoutNotify(manager.MusicVolume * musicSlider.maxValue);
        }

        public void SetSoundVolume(float amount)
        {
            AudioManager.Instance.SetSoundVolume(amount / soundSlider.maxValue);
        }

        public void SetMusicVolume(float amount)
        {
            AudioManager.Instance.SetMusicVolume(amount / musicSlider.maxValue);
        }
    }
}