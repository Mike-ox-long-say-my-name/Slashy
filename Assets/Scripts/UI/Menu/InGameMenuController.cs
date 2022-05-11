using System;
using Core;
using Settings;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Menu
{
    public class InGameMenuController : MonoBehaviour
    {
        private IMenu _mainMenu;
        private IMenu _settingsMenu;

        private void Awake()
        {
            _mainMenu = GetComponentInChildren<InGameMainMenu>();
            _settingsMenu = GetComponentInChildren<SettingsMenu>();
        }

        private void OnEnable()
        {
            var actions = PlayerActionsProxy.Instance.Actions;
            actions.UI.Enable();
            actions.UI.Menu.performed += OnMenuPressed;
        }

        private void OnDisable()
        {
            var proxy = PlayerActionsProxy.TryGetInstance();
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
            GameLoader.Instance.LoadMenu();
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