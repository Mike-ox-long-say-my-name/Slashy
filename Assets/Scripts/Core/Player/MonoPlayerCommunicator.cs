using System;
using UnityEngine;
using UnityEngine.Events;

namespace Core.Player
{
    public class MonoPlayerCommunicator : MonoBehaviour
    {
        [Serializable]
        public struct Out
        {
            [SerializeField] private UnityEvent died;
            [SerializeField] private UnityEvent interacted;
            [SerializeField] private UnityEvent loaded;

            public UnityEvent Died => died;
            public UnityEvent Interacted => interacted;
            public UnityEvent Loaded => loaded;
        }
        
        [Serializable]
        public struct In
        {
            [SerializeField] private UnityEvent touchedBonfire;
            
            public UnityEvent TouchedBonfire => touchedBonfire;
        }

        [SerializeField] private Out outEvents;
        [SerializeField] private In inEvents;

        public Out OutEvents => outEvents;
        public In InEvents => inEvents;
    }
}