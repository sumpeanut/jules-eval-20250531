using UnityEngine;

namespace Game.Farming
{
    /// <summary>
    /// Base class for all plants.
    /// </summary>
    public abstract class PlantBase : MonoBehaviour
    {
        public abstract string PlantType { get; }
        public abstract void Grow(ResourceState resources, float deltaTime);
        public abstract bool CanHybridizeWith(PlantBase neighbor);
        public abstract PlantBase CreateHybrid(PlantBase neighbor);
    }
}
