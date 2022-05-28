using UnityEngine;

namespace Core
{
    public class AmbushTrigger : MonoBehaviour
    {
        [SerializeField] private Ambush[] ambushes;
        
        public void Activate()
        {
            foreach (var ambush in ambushes)
            {
                ambush.ActivateAmbush();
            }
        }
    }
}