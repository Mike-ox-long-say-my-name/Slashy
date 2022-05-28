namespace Characters.Player.States
{
    public interface IPlayerActionResourceSpender
    {
        bool HasEnoughResourcesFor(PlayerResourceAction action);
        void SpendFor(PlayerResourceAction action);
    }
}