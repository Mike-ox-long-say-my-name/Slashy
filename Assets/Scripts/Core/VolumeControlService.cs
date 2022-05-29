using UnityEngine;

namespace Core
{
    public class VolumeControlService : IVolumeControlService
    {
        public int MaxVolumeValue => 10;
        public int SoundVolume { get; private set; }
        public int MusicVolume { get; private set; }

        private readonly VolumeControlServiceContainer _container;
        private readonly int _defaultVolume;
        private const string SoundVolumeParam = "SoundVolume";
        private const string MusicVolumeParam = "MusicVolume";

        public VolumeControlService(VolumeControlServiceContainer container, int defaultVolume = 5)
        {
            _container = container;
            _defaultVolume = defaultVolume;
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

        public void InitializeFromPlayerPrefs()
        {
            var sound = PlayerPrefs.GetInt(SoundVolumeParam, _defaultVolume);
            var music = PlayerPrefs.GetInt(MusicVolumeParam, _defaultVolume);

            SetSoundVolume(sound);
            SetMusicVolume(music);
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