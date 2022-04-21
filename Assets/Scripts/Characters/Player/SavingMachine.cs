using System;
using Characters.Player;
using Core;
using Core.Characters;
using Core.Utilities;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.Player
{
    public class SavingMachine : PersistentSingleton<SavingMachine>
    {
        [SerializeField] private int autoSaveTimeout;
        private TimedTrigger _autoSaveTrigger;
        
        private SaveStructure _bonefire;
        private SaveStructure _lastPosition;

        protected override void Awake()
        {
            base.Awake();
            _autoSaveTrigger = new TimedTrigger();
            _bonefire = ScriptableObject.CreateInstance<SaveStructure>();
            _lastPosition = ScriptableObject.CreateInstance<SaveStructure>();
            _autoSaveTrigger.SetIn(autoSaveTimeout);
        }

        private void Update()
        {
            _autoSaveTrigger.Step();
            if (_autoSaveTrigger.CheckAndReset())
            {
                SetLastPosition(PlayerManager.Instance.PlayerData.PlayerCharacter);
                _autoSaveTrigger.SetIn(autoSaveTimeout);
            }
        }

        public SaveStructure GetSave(bool bonefire) => bonefire ? _bonefire : _lastPosition;

        public void SetBonefire(PlayerCharacter character)
        {
            Debug.Log("set bonefire position");
            _bonefire.Position = character.transform.position;
            _bonefire.Stamina = character.Stamina.MaxValue;
        }

        public void SetLastPosition(PlayerCharacter character)
        {
            Debug.Log("set current position");
            _lastPosition.Position = character.transform.position;
            _lastPosition.Stamina = character.Stamina.Value;
        }
    }
}