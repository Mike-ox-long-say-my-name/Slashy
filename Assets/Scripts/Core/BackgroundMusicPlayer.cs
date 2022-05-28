using UnityEngine;

namespace Core
{
    public class BackgroundMusicPlayer : IBackgroundMusicPlayer
    {
        private readonly BackgroundMusicAudioSourceContainer _musicAudioSourceContainer;

        private AudioClip _chillMusic;
        private AudioClip _fightMusic;

        public BackgroundMusicPlayer(BackgroundMusicAudioSourceContainer musicAudioSourceContainer,
            IAggroListener aggroListener)
        {
            _musicAudioSourceContainer = musicAudioSourceContainer;

            SubscribeToAggroListener();

            void SubscribeToAggroListener()
            {
                aggroListener.FightStarted += () => SwitchClip(_fightMusic);
                aggroListener.FightEnded += () => SwitchClip(_chillMusic);
            }
        }

        public void SetMusic(AudioClip chill, AudioClip fight)
        {
            _chillMusic = chill;
            _fightMusic = fight;

            SwitchClip(_chillMusic);
        }

        private void SwitchClip(AudioClip clip)
        {
            var source = _musicAudioSourceContainer.Source;
            source.clip = clip;
            source.Play();
        }
    }
}