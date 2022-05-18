using UnityEngine;
using UnityEngine.Events;

namespace Core.Characters
{
    public class MixinBossEventDispatcher : MonoBehaviour
    {
        [SerializeField] private UnityEvent fightStarted;
        [SerializeField] private UnityEvent died;

        public UnityEvent FightStarted => fightStarted;
        public UnityEvent Died => died;
    }
}