using System.Collections;
using Core;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using Core.Utilities;
using UnityEngine;

namespace Characters.Enemies.Boss
{
    public class BossSpikeAttackExecutor : MonoAbstractAttackExecutor
    {
        [SerializeField] private float strikeDelay = 0.3f;
        [SerializeField] private float lingerTime = 0.15f;
        [SerializeField] private GameObject spriteObject;

        private class CustomExecutor : AttackExecutor
        {
            public float StrikeDelay { get; set; }
            public float LingerTime { get; set; }
            public GameObject SpriteObject { get; set; }

            public CustomExecutor(ICoroutineHost host, IAttackbox attackbox) : base(host, attackbox)
            {
            }

            protected override IEnumerator Execute()
            {
                yield return new WaitForSeconds(StrikeDelay);
                Attackbox.Enable();
                SpriteObject.SetActive(true);

                yield return new WaitForFixedUpdate();
                yield return new WaitForSeconds(LingerTime);
                Attackbox.Disable();
            }
        }


        private IAttackExecutor _executor;

        public override IAttackExecutor GetExecutor()
        {
            return _executor ??= CreateExecutor();
        }

        private IAttackExecutor CreateExecutor()
        {
            var attackbox = GetComponentInChildren<MonoAttackbox>().Attackbox;
            return new CustomExecutor(this.ToCoroutineHost(), attackbox)
            {
                StrikeDelay = strikeDelay,
                LingerTime = lingerTime,
                SpriteObject = spriteObject
            };
        }
    }
}