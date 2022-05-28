using Core;
using Settings;
using UnityEngine;
using UnityEngine.InputSystem;

namespace UI.Menu
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private MainMenu mainMenu;
        [SerializeField] private SettingsMenu settingsMenu;
        
        private PlayerBindings _bindings;
        private IGameLoader _gameLoader;

        private void Reset()
        {
            mainMenu = GetComponentInChildren<MainMenu>();
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
        }

        public void ShowSettingsMenu()
        {
            mainMenu.ShowSubMenu(settingsMenu);
        }

        public void CloseSettingsMenu()
        {
            settingsMenu.Close();
        }

        public void ContinuePlaying()
        {
            _gameLoader.LoadGame();
        }

        public void StartNewGame()
        {
            _gameLoader.LoadNewGame();
        }

        public void ExitGame()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}