using Settings;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Menu
{
    public class InGameMenuController : MonoBehaviour
    {
        private IMenu _mainMenu;
        private IMenu _settingsMenu;
        private IGameLoader _gameLoader;

        private void Awake()
        {
            _mainMenu = GetComponentInChildren<InGameMainMenu>();
            _settingsMenu = GetComponentInChildren<SettingsMenu>();
            _gameLoader = GameLoader.Instance;
        }

        private void OnEnable()
        {
            var actions = PlayerActionsProxy.Instance.Actions;
            actions.UI.Enable();
            actions.UI.Menu.performed += OnMenuPressed;
        }

        private void OnDisable()
        {
            var actions = PlayerActionsProxy.Instance.Actions;
            actions.UI.Menu.performed -= OnMenuPressed;
        }

        private void OnMenuPressed(InputAction.CallbackContext context)
        {
            if (_settingsMenu.IsShown)
            {
                CloseSettingsMenu();
            }
            else if (_mainMenu.IsShown)
            {
                CloseMainMenu();
            }
            else
            {
                ShowMainMenu();
            }
        }

        public void ShowMainMenu()
        {
            _mainMenu.Show();
            RestrictPlayerInput();
        }

        public void ShowSettingsMenu()
        {
            _mainMenu.ShowSubMenu(_settingsMenu);
        }

        public void CloseMainMenu()
        {
            _mainMenu.Close();
            AllowPlayerInput();
        }

        public void CloseSettingsMenu()
        {
            _settingsMenu.Close();
        }

        public void ContinuePlaying()
        {
            CloseMainMenu();
        }

        public void ExitToMainMenu()
        {
            _gameLoader.LoadMenu();
        }

        private static void RestrictPlayerInput()
        {
            var actions = PlayerActionsProxy.Instance.Actions;
            actions.Player.Fire.Disable();
        }

        private static void AllowPlayerInput()
        {
            var actions = PlayerActionsProxy.Instance.Actions;
            actions.Player.Fire.Enable();
        }
    }
}