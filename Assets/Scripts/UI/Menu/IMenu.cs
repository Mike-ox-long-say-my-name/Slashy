namespace UI.Menu
{
    public interface IMenu
    {
        bool IsShown { get; }

        void Show(IMenu parent = null);
        void Close();

        void ShowSubMenu(IMenu subMenu);
        void OnSubMenuClosed(IMenu subMenu);
    }
}