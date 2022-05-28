using UnityEngine;

namespace Core
{
    public class BackgroundMusicAudioSourceContainer : MonoBehaviour
    {
        [SerializeField] private AudioSource source;

        public AudioSource Source => source;
    }
}