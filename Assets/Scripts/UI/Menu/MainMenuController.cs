using Settings;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Menu
{
    public class MainMenuController : MonoBehaviour
    {
        private IGameLoader _gameLoader;
        private IMenu _mainMenu;
        private IMenu _settingsMenu;

        private void Awake()
        {
            _mainMenu = FindObjectOfType<MainMenu>();
            _settingsMenu = FindObjectOfType<SettingsMenu>();
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
        }

        public void ShowSettingsMenu()
        {
            _mainMenu.ShowSubMenu(_settingsMenu);
        }

        public void CloseSettingsMenu()
        {
            _settingsMenu.Close();
        }

        public void ContinuePlaying()
        {
            // TODO
        }

        public void StartNewGame()
        {
            _gameLoader.LoadNewGame();
        }

        public void ExitGame()
        {
            if (Application.isEditor)
            {
                EditorApplication.isPlaying = false;
            }
            else
            {
                Application.Quit();
            }
        }
    }
}