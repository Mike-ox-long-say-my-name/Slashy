using System.Collections.Generic;
using UnityEngine;

namespace UI.PopupHints
{
    public class PopupHintContainer : MonoBehaviour
    {
        [SerializeField] private ShownHintsSO shownHints;
        [SerializeField] private PopupHintDescription[] hintDescriptions;

        public ShownHintsSO ShownHints => shownHints;
        public IEnumerable<PopupHintDescription> HintDescriptions => hintDescriptions;
    }
}