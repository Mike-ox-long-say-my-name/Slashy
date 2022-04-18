using UnityEngine;

namespace Attacking
{
    public interface IHitSource
    {
        Transform Transform { get; }
        HittableEntity Source { get; }
    }
}