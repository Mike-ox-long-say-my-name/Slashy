using Core.Characters.Mono;
using Core.Modules;
using UnityEngine;

namespace Core.Characters
{
    [RequireComponent(typeof(MixinMovementBase))]
    [RequireComponent(typeof(MixinCharacter))]
    [RequireComponent(typeof(MixinAggro))]
    public class EnemyConductor : MonoBehaviour
    {
        [SerializeField, HideInInspector] private MixinMovementBase movementBase;
        [SerializeField, HideInInspector] private MixinCharacter character;
        [SerializeField, HideInInspector] private MixinAggro aggro;

        public MixinMovementBase MovementBase => movementBase;
        public MixinCharacter Character => character;
        public MixinAggro Aggro => aggro;

        private void OnValidate()
        {
            movementBase = GetComponent<MixinMovementBase>();
            character = GetComponent<MixinCharacter>();
            aggro = GetComponent<MixinAggro>();
        }
    }
}