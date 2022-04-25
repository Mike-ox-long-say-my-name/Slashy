using System.Collections;
using Core;
using Core.Attacking;
using Core.Attacking.Interfaces;
using Core.Attacking.Mono;
using UnityEngine;

namespace Characters.Enemies.Examples
{
    public class TestMonoPunchAttack : MonoAttackHandler
    {
        [SerializeField] private GameObject weaponThing;

        private class PunchAttack : AttackExecutor
        {
            private readonly GameObject _cube;

            public PunchAttack(GameObject cube, ICoroutineHost host, IAttackbox attackbox) : base(host, attackbox)
            {
                _cube = cube;
            }
            

            protected override IEnumerator Execute()
            {
                Attackbox.Enable();
                _cube.SetActive(true);

                yield return new WaitForSeconds(0.2f);

                Attackbox.Disable();
                _cube.SetActive(false);
            }
        }

        protected override IAttackExecutor CreateExecutor(ICoroutineHost host, IAttackbox attackbox)
        {
            return new PunchAttack(weaponThing, host, attackbox);
        }
    }
}