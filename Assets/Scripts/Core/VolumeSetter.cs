using System;
using UnityEngine;

namespace Core
{
    [DefaultExecutionOrder(-10)]
    public class VolumeSetter : MonoBehaviour
    {
        private void Awake()
        {
            var volumeControlService = Container.Get<IVolumeControlService>();
            volumeControlService.InitializeFromPlayerPrefs();
        }
    }
}