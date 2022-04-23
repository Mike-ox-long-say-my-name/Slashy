using Core;

namespace UI.Menu
{
    public class NullMenu<T> : NullObject<T>, IMenu
    {
        public bool IsShown => false;

        public void Show(IMenu parent = null) => Report();

        public void Close() => Report();

        public void ShowSubMenu(IMenu subMenu) => Report();

        public void OnSubMenuClosed(IMenu subMenu) => Report();
    }
}