using Core;

namespace Settings
{
    public class PlayerBindings
    {
        public PlayerInputActions Actions { get; private set; }

        protected override void Awake()
        {
            Actions = new PlayerInputActions();
        }
    }
}