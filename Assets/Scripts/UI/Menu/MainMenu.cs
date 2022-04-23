using UnityEngine;
using UnityEngine.UI;

namespace UI.Menu
{
    public class MainMenu : Menu<MainMenu>
    {
        [SerializeField] private Button continueButton;

        private IGameLoader _gameLoader;

        protected override bool DisableOnStart => false;

        protected override void Awake()
        {
            base.Awake();
            _gameLoader = GameLoader.Instance;

            if (continueButton == null)
            {
                Debug.LogWarning("Continue button is not assigned", this);
            }
            else
            {
                continueButton.interactable = _gameLoader.HasAnyGameProgress;
            }
        }
    }
}