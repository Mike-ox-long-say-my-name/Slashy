using UnityEngine;
using UnityEngine.Audio;

namespace Core
{
    public class VolumeControlServiceContainer : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        public AudioMixer AudioMixer => audioMixer;
    }

    public interface IVolumeControlService
    {
        void SetMusicVolume(int value);
        void SetSoundVolume(int value);
    }

    public class VolumeControlService : IVolumeControlService
    {
        private readonly VolumeControlServiceContainer _container;
        private const string SoundVolumeParam = "SoundVolume";
        private const string MusicVolumeParam = "MusicVolume";

        public VolumeControlService(VolumeControlServiceContainer container, int defaultVolume = 5)
        {
            _container = container;

            ReadPlayerPrefs(defaultVolume);
        }

        private void ReadPlayerPrefs(int defaultVolume)
        {
            var sound = PlayerPrefs.GetInt(SoundVolumeParam, defaultVolume);
            var music = PlayerPrefs.GetInt(MusicVolumeParam, defaultVolume);

            SetSoundVolume(sound);
            SetMusicVolume(music);
        }

        public void SetMusicVolume(int value) => SetVolume(MusicVolumeParam, value);

        public void SetSoundVolume(int value) => SetVolume(SoundVolumeParam, value);

        private void SetVolume(string param, int value)
        {
            var db = ConvertRawVolumeToDB(value);
            _container.AudioMixer.SetFloat(param, db);

            UpdatePlayerPrefs(param, value);
        }

        private static void UpdatePlayerPrefs(string param, int value)
        {
            PlayerPrefs.SetInt(param, value);
        }

        private static float ConvertRawVolumeToDB(float amount)
        {
            var linear = amount / 10;
            if (Mathf.Approximately(linear, 0))
            {
                return -144;
            }

            return 20 * Mathf.Log10(linear);
        }
    }
}