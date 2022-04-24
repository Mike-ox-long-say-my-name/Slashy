namespace Characters.Player
{
    public static class PlayerExtensions
    {
        public static bool HasStamina(this IPlayerCharacter player)
        {
            return player.Stamina.Value > 0;
        }
    }
}