//using Core.Characters;
//using Core.Characters.Interfaces;
//using Core.Player.Interfaces;
//using Core.Utilities;
//using UnityEngine;

//namespace Characters.Player
//{
//    public class PlayerCharacter : Character, IPlayerCharacter
//    {
//        public PlayerStats PlayerStats { get; set; }

//        private readonly StaminaResource _staminaResource;

//        IPlayerMovement IPlayerCharacter.PlayerMovement => (IPlayerMovement)VelocityMovement;
//        public IResource Stamina => _staminaResource;

//        private readonly TimedTrigger _staminaRecoveryDelayed = new TimedTrigger();

//        public PlayerCharacter(CharacterStats characterStats, PlayerStats playerStats)
//            : base(movement, damageStats, characterStats)
//        {
//            PlayerStats = playerStats;
//            _staminaResource = new StaminaResource(this, playerStats.MaxStamina);
//        }

//        public override void Tick(float deltaTime)
//        {
//            base.Tick(deltaTime);

//            RecoverStamina();
//            _staminaRecoveryDelayed.Step(deltaTime);
//        }

//        private void RecoverStamina()
//        {
//            if (_staminaRecoveryDelayed.IsSet || PlayerStats.FreezeStamina)
//            {
//                return;
//            }

//            _staminaResource.Recover(PlayerStats.StaminaRegeneration * Time.deltaTime);
//        }

//        public void SpendStamina(float amount)
//        {
//            if (PlayerStats.FreezeStamina)
//            {
//                return;
//            }

//            _staminaResource.Spend(amount);
//            if (_staminaResource.IsDepleted)
//            {
//                var delay = PlayerStats.StaminaRegenerationDelay + PlayerStats.EmptyStaminaAdditionalRegenerationDelay;
//                _staminaRecoveryDelayed.SetFor(delay);
//            }
//            else
//            {
//                var delay = PlayerStats.StaminaRegenerationDelay;
//                _staminaRecoveryDelayed.SetFor(delay);
//            }
//        }
//    }
//}
