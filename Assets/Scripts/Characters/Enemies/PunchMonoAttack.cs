using Core.Attacking;
using System.Collections;
using Core;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using UnityEngine;

namespace Characters.Enemies
{
    public class PunchMonoAttack : MonoAttackHandler
    {
        [SerializeField] private GameObject cube;

        private class PunchAttack : AttackExecutor
        {
            private readonly GameObject _cube;

            public PunchAttack(GameObject cube, ICoroutineHost host, IAttackbox attackbox) : base(host, attackbox)
            {
                _cube = cube;
            }

            protected override IEnumerator Execute()
            {
                yield return new WaitForSeconds(0.3f);
                _cube.SetActive(true);

                Attackbox.Enable();
                yield return new WaitForFixedUpdate();

                Attackbox.Disable();
                yield return new WaitForSeconds(0.3f);
                _cube.SetActive(false);

                yield return new WaitForSeconds(1);
            }

            protected override void OnAttackEnded(bool interrupted)
            {
                _cube.SetActive(false);
                base.OnAttackEnded(interrupted);
            }
        }

        protected override IAttackExecutor CreateExecutor(ICoroutineHost host, IAttackbox attackbox)
        {
            return new PunchAttack(cube, host, attackbox);
        }
    }
}