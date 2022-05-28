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

        private bool _isInitialized;
        private IVolumeControlService _volumeControl;

        private void Awake()
        {
            Construct();
            SetSavedValues();
            _isInitialized = true;
        }

        private void Construct()
        {
            _volumeControl = Container.Get<IVolumeControlService>();
        }

        private void SetSavedValues()
        {
            soundSlider.maxValue = _volumeControl.MaxVolumeValue;
            musicSlider.maxValue = _volumeControl.MaxVolumeValue;
            soundSlider.value = _volumeControl.SoundVolume;
            musicSlider.value = _volumeControl.MusicVolume;
        }

        public void SetSoundVolume(float amount)
        {
            if (_isInitialized)
            {
                _volumeControl.SetSoundVolume(Mathf.RoundToInt(amount));
            }
        }

        public void SetMusicVolume(float amount)
        {
            if (_isInitialized)
            {
                _volumeControl.SetMusicVolume(Mathf.RoundToInt(amount));
            }
        }
    }
}