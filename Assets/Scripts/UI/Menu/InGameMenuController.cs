using System;
using Core;
using JetBrains.Annotations;
using Settings;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Menu
{
    public class InGameMenuController : MonoBehaviour
    {
        [SerializeField] private InGameMainMenu mainMenu;
        [SerializeField] private SettingsMenu settingsMenu;

        private PlayerBindings _bindings;
        private IGameLoader _gameLoader;

        public event Action MenuClosed;
        public event Action MenuOpened;

        private void Reset()
        {
            mainMenu = GetComponentInChildren<InGameMainMenu>();
            settingsMenu = GetComponentInChildren<SettingsMenu>();
        }

        private void Awake()
        {
            Construct();
        }

        private void Construct()
        {
            _gameLoader = Container.Get<IGameLoader>();
            _bindings = Container.Get<PlayerBindings>();
        }

        private void OnEnable()
        {
            var actions = _bindings.Actions;
            actions.UI.Enable();
            actions.UI.Menu.performed += OnMenuPressed;
        }

        private void OnDisable()
        {
            var actions = _bindings.Actions;
            actions.UI.Menu.performed -= OnMenuPressed;
        }

        private void OnMenuPressed(InputAction.CallbackContext context)
        {
            if (settingsMenu.IsShown)
            {
                CloseSettingsMenu();
            }
            else if (mainMenu.IsShown)
            {
                CloseMainMenu();
            }
            else
            {
                ShowMainMenu();
            }
        }

        [UsedImplicitly]
        public void ShowMainMenu()
        {
            Cursor.visible = true;
            mainMenu.Show();
            RestrictPlayerInput();
            MenuOpened?.Invoke();
        }

        public void ShowSettingsMenu()
        {
            mainMenu.ShowSubMenu(settingsMenu);
        }

        [UsedImplicitly]
        public void CloseMainMenu()
        {
            Cursor.visible = false;
            mainMenu.Close();
            AllowPlayerInput();
            MenuClosed?.Invoke();
        }

        public void CloseSettingsMenu()
        {
            settingsMenu.Close();
        }

        public void ContinuePlaying()
        {
            CloseMainMenu();
        }

        public void ExitToMainMenu()
        {
            _gameLoader.LoadMenu();
        }

        private void RestrictPlayerInput()
        {
            var actions = _bindings.Actions;
            actions.Player.Fire.Disable();
            actions.Player.Fire2.Disable();
        }

        private void AllowPlayerInput()
        {
            var actions = _bindings.Actions;
            actions.Player.Fire.Enable();
            actions.Player.Fire2.Enable();
        }
    }
}