using Core;

namespace Settings
{
    public class PlayerBindings
    {
        public PlayerInputActions Actions { get; }

        public PlayerBindings()
        {
            Actions = new PlayerInputActions();
        }
    }
}