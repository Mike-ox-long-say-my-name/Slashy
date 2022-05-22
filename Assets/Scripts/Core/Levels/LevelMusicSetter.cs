using UnityEngine;

namespace Core.Levels
{
    [DefaultExecutionOrder(-5)]
    public class LevelMusicSetter : MonoBehaviour
    {
        [SerializeField] private AudioClip chillMusic;
        [SerializeField] private AudioClip fightMusic;

        private void Start()
        {
            BackgroundMusicManager.Instance.SetMusic(chillMusic, fightMusic);
        }
    }
}