using UnityEngine;

namespace Core.Levels
{
    [CreateAssetMenu(menuName = "Other/Exit Data", fileName = "ExitData", order = 2)]
    public class ExitData : ScriptableObject
    {
        [field: SerializeField] public Vector3 ExitPosition { get; set; }
        [field: SerializeField] public bool Dead { get; set; }
        [field: SerializeField] public float Health { get; set; }
        [field: SerializeField] public float Stamina { get; set; }
        [field: SerializeField] public float Balance { get; set; }
        [field: SerializeField] public string Level { get; set; }
    }
}