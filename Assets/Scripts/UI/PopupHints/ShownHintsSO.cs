using UnityEngine;

namespace UI.PopupHints
{
    [CreateAssetMenu(menuName = "Other/Hints", fileName = "ShownHints", order = 0)]
    public class ShownHintsSO : ScriptableObject
    {
        [SerializeField] private bool resetOnQuit = true;

        [field: Header("Hints")]
        [field: SerializeField] public bool Move { get; set; }
        [field: SerializeField] public bool Dash { get; set; }
        [field: SerializeField] public bool Jump { get; set; }
        [field: SerializeField] public bool Heal { get; set; }
        [field: SerializeField] public bool LightAttack { get; set; }
        [field: SerializeField] public bool StrongAttack { get; set; }
        [field: SerializeField] public bool Save { get; set; }

        private void OnDisable()
        {
            if (!resetOnQuit)
            {
                return;
            }

            Move = false;
            Dash = false;
            Jump = false;
            Heal = false;
            LightAttack = false;
            StrongAttack = false;
        }
    }
}