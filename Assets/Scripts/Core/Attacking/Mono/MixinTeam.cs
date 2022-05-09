using Core.Characters.Interfaces;
using UnityEngine;

namespace Core.Attacking
{
    public class MixinTeam : MonoBehaviour
    {
        [SerializeField] private Team team;

        public Team Team => team;
    }
}