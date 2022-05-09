using Core.Characters.Interfaces;
using Core.Utilities;

namespace Core.Player.Interfaces
{
    public class PlayerCharacter : IPlayerCharacter
    {
        public PlayerCharacter(ICharacter character, IResource stamina)
        {
            Guard.NotNull(character);
            Guard.NotNull(stamina);

            Character = character;
            Stamina = stamina;
            
            character.Team = Team.Player;
        }

        public ICharacter Character { get; }
        public IResource Stamina { get; }
    }
}