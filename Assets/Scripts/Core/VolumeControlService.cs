using UnityEngine;

namespace Core
{
    public class VolumeControlService : IVolumeControlService
    {
        public int MaxVolumeValue => 10;
        public int SoundVolume { get; private set; }
        public int MusicVolume { get; private set; }

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

        public void SetMusicVolume(int value)
        {
            SetVolume(MusicVolumeParam, value);
            MusicVolume = value;
        }

        public void SetSoundVolume(int value)
        {
            SetVolume(SoundVolumeParam, value);
            SoundVolume = value;
        }

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

        private float ConvertRawVolumeToDB(float amount)
        {
            var linear = amount / MaxVolumeValue;
            if (Mathf.Approximately(linear, 0))
            {
                return -144;
            }

            return 40 * Mathf.Log10(linear);
        }
    }
}