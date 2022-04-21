using Core.Characters;
using UnityEngine;
using UnityEngine.UIElements;

namespace Characters.Player
{
    public class SaveStructure : ScriptableObject
    {
        public Vector3 Position { get; set; }
        public float Stamina { get; set; }
    }
}