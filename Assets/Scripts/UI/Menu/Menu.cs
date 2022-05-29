using UnityEngine;
using UnityEngine.Events;

namespace UI.Menu
{
    public abstract class Menu : MonoBehaviour, IMenu
    {
        [SerializeField] private UnityEvent showed;
        [SerializeField] private UnityEvent closed;

        public UnityEvent Closed => closed;

        public UnityEvent Showed => showed;

        private GameObject _menuUI;

        private IMenu _parent;

        protected virtual bool DisableOnStart => true;

        protected virtual void Awake()
        {
            _menuUI = gameObject;
        }

        protected virtual void Start()
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

            Showed?.Invoke();
        }

        public void Close()
        {
            _menuUI.SetActive(false);
            _parent?.OnSubMenuClosed(this);

            Closed?.Invoke();
        }

        public virtual void ShowSubMenu(IMenu subMenu)
        {
            Close();
            Cursor.visible = true;
            subMenu.Show(this);
        }

        public virtual void OnSubMenuClosed(IMenu subMenu)
        {
            Show();
        }
    }
}