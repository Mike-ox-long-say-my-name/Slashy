using UnityEngine;

namespace Core.Levels
{
    [CreateAssetMenu(menuName = "Other/Bonfires", fileName = "BonfireSaveData", order = 0)]
    public class BonfireSaveData : ScriptableObject
    {
        [SerializeField] private int litStatusBitmask;

        public bool IsLit(int id)
        {
            return (litStatusBitmask & (1 << id)) != 0;
        }

        public void SetStatus(int id, bool isLit)
        {
            var bit = 1 << id;
            if (isLit)
            {
                litStatusBitmask |= bit;
            }
            else
            {
                litStatusBitmask &= ~bit;
            }
        }

        public void ResetBitmask()
        {
            litStatusBitmask = 0;
        }
    }
}