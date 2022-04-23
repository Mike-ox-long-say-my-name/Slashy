using Core;

namespace Settings
{
    public class PlayerActionsProxy : PublicSingleton<PlayerActionsProxy>
    {
        public PlayerInputActions Actions { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Actions = new PlayerInputActions();
        }
    }
}