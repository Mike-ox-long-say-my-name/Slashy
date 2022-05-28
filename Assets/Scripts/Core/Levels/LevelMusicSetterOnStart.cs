using UnityEngine;

namespace Core.Levels
{
    [DefaultExecutionOrder(-5)]
    public class LevelMusicSetterOnStart : MonoBehaviour
    {
        [SerializeField] private AudioClip chillMusic;
        [SerializeField] private AudioClip fightMusic;

        private void Start()
        {
            var musicPlayer = Container.Get<IBackgroundMusicPlayer>();
            musicPlayer.SetMusic(chillMusic, fightMusic);
        }
    }
}