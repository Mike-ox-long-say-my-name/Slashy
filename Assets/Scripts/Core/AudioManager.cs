using UnityEngine;
using UnityEngine.Audio;

namespace Core
{
    public class AudioManager : PublicSingleton<AudioManager>
    {
        [SerializeField] private AudioMixer mixer;

        public const string SoundVolumeParam = "SoundVolume";
        public const string MusicVolumeParam = "MusicVolume";

        public float SoundVolume { get; private set; }

        public float MusicVolume { get; private set; }

        private void Start()
        {
            ReadPlayerPrefs();
        }

        private void ReadPlayerPrefs()
        {
            const float defaultVolume = 5;
            var sound = PlayerPrefs.GetFloat(SoundVolumeParam, defaultVolume);
            var music = PlayerPrefs.GetFloat(MusicVolumeParam, defaultVolume);

            SetSoundVolume(sound);
            SetMusicVolume(music);
        }

        public void SetMusicVolume(float value)
        {
            SetVolume(MusicVolumeParam, value);
            MusicVolume = value;
        }

        public void SetSoundVolume(float value)
        {
            SetVolume(SoundVolumeParam, value);
            SoundVolume = value;
        }

        private void SetVolume(string param, float value)
        {
            var db = ConvertRawVolumeToDB(value);
            mixer.SetFloat(param, db);
            PlayerPrefs.SetFloat(param, value);
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