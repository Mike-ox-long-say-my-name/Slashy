using UnityEngine;

namespace Core
{
    [DefaultExecutionOrder(-10)]
    public class BackgroundMusicManager : PublicSingleton<BackgroundMusicManager>
    {
        private AudioSource _source;
        private AudioClip _chillMusic;
        private AudioClip _fightMusic;

        public void SetMusic(AudioClip chill, AudioClip fight)
        {
            _chillMusic = chill;
            _fightMusic = fight;

            SwitchClip(_chillMusic);
        }

        protected override void Awake()
        {
            base.Awake();

            _source = GetComponent<AudioSource>();
        }

        private void Start()
        {
            var fightManager = FightManager.Instance;
            fightManager.FightStarted += () => SwitchClip(_fightMusic);
            fightManager.FightEnded += () => SwitchClip(_chillMusic);
        }

        private void SwitchClip(AudioClip clip)
        {
            _source.clip = clip;
            _source.Play();
        }
    }
}