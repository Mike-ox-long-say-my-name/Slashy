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

        private bool _isInitializing = true;

        private void Start()
        {
            _isInitializing = true;
            var manager = VolumeControlService.Instance;
            soundSlider.value = manager.SoundVolume * soundSlider.maxValue;
            musicSlider.value = manager.MusicVolume * musicSlider.maxValue;
            _isInitializing = false;
        }

        public void SetSoundVolume(float amount)
        {
            if (_isInitializing)
            {
                return;
            }
            VolumeControlService.Instance.SetSoundVolume(amount / soundSlider.maxValue);
        }

        public void SetMusicVolume(float amount)
        {
            if (_isInitializing)
            {
                return;
            }
            VolumeControlService.Instance.SetMusicVolume(amount / musicSlider.maxValue);
        }
    }
}