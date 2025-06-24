using UnityEngine;

namespace Game.Farming
{
    /// <summary>
    /// Represents the resource state for a grid cell or plant.
    /// </summary>
    [System.Serializable]
    public class ResourceState
    {
        public float Water = 1f;
        public float SoilQuality = 1f;
        public float Sunlight = 1f;
    }
}
