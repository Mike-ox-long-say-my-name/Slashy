using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Core
{
    public class UiFactory : IUiFactory
    {
        private const string LevelUiWithBossBar = "LevelUiWithBossBar";
        private const string LevelUiWithHints = "LevelUiWithHints";
        private const string BaseLevelUi = "BaseLevelUi";
        
        private static GameObject _baseUiPrefab;
        private static GameObject _uiWithHintsPrefab;
        private static GameObject _uiWithBossBarPrefab;
        
        public UiFactory()
        {
            LoadResources();
        }

        private static void LoadResources()
        {
            if (!_baseUiPrefab)
            {
                _baseUiPrefab = Resources.Load<GameObject>(BaseLevelUi);
            }
            if (!_uiWithHintsPrefab)
            {
                _uiWithHintsPrefab = Resources.Load<GameObject>(LevelUiWithHints);
            }
            if (!_uiWithBossBarPrefab)
            {
                _uiWithBossBarPrefab = Resources.Load<GameObject>(LevelUiWithBossBar);
            }
        }

        public GameObject CreateUi(UiType uiType)
        {
            return uiType switch
            {
                UiType.Default => Object.Instantiate(_baseUiPrefab),
                UiType.WithBossBar => Object.Instantiate(_uiWithBossBarPrefab),
                UiType.WithHints => Object.Instantiate(_uiWithHintsPrefab),
                _ => throw new ArgumentOutOfRangeException(nameof(uiType), uiType, null)
            };
        }
    }
}