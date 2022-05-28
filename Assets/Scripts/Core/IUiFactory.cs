using UnityEngine;

namespace Core
{
    public interface IUiFactory
    {
        GameObject CreateUi(UiType uiType);
    }
}