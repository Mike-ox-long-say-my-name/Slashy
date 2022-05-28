using System.Collections.Generic;
using Core;
using UnityEngine;

namespace UI.PopupHints
{
    [CreateAssetMenu(menuName = "Other/Hints", fileName = "ShownHints", order = 0)]
    public class ShownHintsSO : ScriptableObject
    {
        [SerializeField] private bool resetOnQuit = true;
        [SerializeField] private List<PopupHintType> shownHints;

        public bool IsShown(PopupHintType hintType) => shownHints.Contains(hintType);
        
        public void MarkAsShown(PopupHintType hintType) => shownHints.Add(hintType);
        public void MarkAsNotShown(PopupHintType hintType) => shownHints.Remove(hintType);

        private void OnDisable()
        {
            if (resetOnQuit)
            {
                ResetShownHints();
            }
        }

        public void ResetShownHints()
        {
            shownHints.Clear();
        }
    }
}