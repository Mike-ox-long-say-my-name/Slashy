using UnityEngine;
using UnityEngine.Audio;

namespace Core
{
    public class VolumeControlServiceContainer : MonoBehaviour
    {
        [SerializeField] private AudioMixer audioMixer;
        public AudioMixer AudioMixer => audioMixer;
    }
}