using Core.Characters;
using UnityEngine;

namespace Core
{
    public interface IHitSource
    {
        Transform Transform { get; }
        HittableEntity Source { get; }
    }
}