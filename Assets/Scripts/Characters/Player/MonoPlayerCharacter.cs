using Core.Characters;
using Core.Characters.Interfaces;
using Core.Characters.Mono;
using Core.Player;
using Core.Player.Interfaces;
using UnityEngine;

namespace Characters.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class MonoPlayerCharacter : MonoCharacter
    {
        [SerializeField] private MonoPlayerStats monoPlayerStats;
        [SerializeField] private MonoPlayerMovementConfig monoPlayerConfig;

        protected override ICharacter CreateCharacter(MovementConfig movementConfig,
            DamageStats damageState, CharacterStats characterStats)
        {
            var controller = GetComponent<CharacterController>();
            var movement = new Movement(controller);
            var playerMovement = new PlayerMovement(movement, movementConfig, monoPlayerConfig.Config);
            return new PlayerCharacter(playerMovement, damageState, characterStats, monoPlayerStats.PlayerStats);
        }

        public IPlayerCharacter Player => Character as IPlayerCharacter;
    }
}
