using Core;
using Settings;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace UI.Menu
{
    public class InGameMenuController : MonoBehaviour
    {
        [SerializeField] private UnityEvent menuOpened;
        [SerializeField] private UnityEvent menuClosed;

        private IMenu _mainMenu;
        private IMenu _settingsMenu;

        public UnityEvent MenuClosed => menuClosed;

        public UnityEvent MenuOpened => menuOpened;

        private void Awake()
        {
            _mainMenu = GetComponentInChildren<InGameMainMenu>();
            _settingsMenu = GetComponentInChildren<SettingsMenu>();
        }

        private void OnEnable()
        {
            var actions = PlayerBindings.Instance.Actions;
            actions.UI.Enable();
            actions.UI.Menu.performed += OnMenuPressed;
        }

        private void OnDisable()
        {
            var proxy = PlayerBindings.TryGetInstance();
            if (proxy == null)
            {
                return;
            }

            var actions = proxy.Actions;
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
            MenuOpened?.Invoke();
        }

        public void ShowSettingsMenu()
        {
            _mainMenu.ShowSubMenu(_settingsMenu);
        }

        public void CloseMainMenu()
        {
            _mainMenu.Close();
            AllowPlayerInput();
            MenuClosed?.Invoke();
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
            GameLoader.Instance.LoadMenu();
        }

        private static void RestrictPlayerInput()
        {
            var actions = PlayerBindings.Instance.Actions;
            actions.Player.Fire.Disable();
        }

        private static void AllowPlayerInput()
        {
            var actions = PlayerBindings.Instance.Actions;
            actions.Player.Fire.Enable();
        }
    }
}