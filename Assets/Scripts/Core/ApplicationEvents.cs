using System;
using UnityEngine;

namespace Core
{
    public class ApplicationEvents : MonoBehaviour
    {
        public event Action Exiting;

        private void OnApplicationQuit()
        {
            Exiting?.Invoke();
        }
    }
}