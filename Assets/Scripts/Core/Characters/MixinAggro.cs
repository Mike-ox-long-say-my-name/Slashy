using System;
using UnityEngine;

namespace Core.Characters
{
    
    public class MixinAggro : MonoBehaviour
    {
        [SerializeField] private float aggroDistance;
        private IAggroListener _aggroListener;

        public float AggroDistance => aggroDistance;

        private bool _aggroed;
        private bool _deaggroed;

        public event Action Aggroed;
        private void Awake()
        {
            _aggroListener = Container.Get<IAggroListener>();
        }

        public void Aggro()
        {
            if (_aggroed)
            {
                return;
            }
            _aggroListener.IncreaseAggroCounter();
            _aggroed = true;
            Aggroed?.Invoke();
        }

        public void Deaggro()
        {
            if (_deaggroed || !_aggroed)
            {
                return;
            }
            
            _aggroListener.DecreaseAggroCounter();
            _deaggroed = true;
        }
    }
}