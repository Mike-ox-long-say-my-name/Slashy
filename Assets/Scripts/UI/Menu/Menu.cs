using Core;
using UnityEngine;

namespace UI.Menu
{
    public abstract class Menu<T> : Singleton<T>, IMenu where T : Component
    {
        private GameObject _menuUI;

        private IMenu _parent;

        protected virtual bool DisableOnStart => true;

        protected override void Awake()
        {
            base.Awake();
            _menuUI = gameObject;
        }

        private void Start()
        {
            if (DisableOnStart)
            {
                _menuUI.SetActive(false);
            }
        }

        public bool IsShown => _menuUI.activeSelf;

        public void Show(IMenu parent = null)
        {
            _parent = parent;
            _menuUI.SetActive(true);
        }

        public void Close()
        {
            _menuUI.SetActive(false);
            _parent?.OnSubMenuClosed(this);
        }

        public virtual void ShowSubMenu(IMenu subMenu)
        {
            Close();
            subMenu.Show(this);
        }

        public virtual void OnSubMenuClosed(IMenu subMenu)
        {
            Show();
        }
    }
}