using UnityEngine;

namespace Characters.Enemies.ExplodingHollow
{
    [CreateAssetMenu(menuName = "Attacks/Exploding Attack Config", fileName = "ExplodingHollowAttack", order = 0)]
    public class ExplodingHollowAttackConfig : ScriptableObject
    {
        [SerializeField] private GameObject bloodFirePrefab;

        [SerializeField, Min(2)] private int fireRows = 6;
        [SerializeField, Min(1)] private int firesPerRow = 3;
        [SerializeField, Min(0)] private float fireRowLength = 3;
        [SerializeField, Min(0)] private float baseOffsetDistance = 0.3f;
        [SerializeField, Min(0)] private float minFireLifeTime = 1;
        [SerializeField, Min(0)] private float maxFireLifeTime = 5;

        public GameObject BloodFirePrefab => bloodFirePrefab;

        public int FireRows => fireRows;

        public int FiresPerRow => firesPerRow;

        public float FireRowLength => fireRowLength;

        public float BaseOffsetDistance => baseOffsetDistance;

        public float MinFireLifeTime => minFireLifeTime;

        public float MaxFireLifeTime => maxFireLifeTime;
    }
}