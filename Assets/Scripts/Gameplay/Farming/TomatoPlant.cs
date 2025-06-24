using UnityEngine;

namespace Game.Farming
{
    /// <summary>
    /// Example implementation of a specific plant.
    /// </summary>
    public class TomatoPlant : PlantBase
    {
        public override string PlantType => "Tomato";
        public override void Grow(ResourceState resources, float deltaTime)
        {
            // Implement growth logic based on resources
        }
        public override bool CanHybridizeWith(PlantBase neighbor)
        {
            // Example: Tomato can hybridize with Carrot
            return neighbor.PlantType == "Carrot";
        }
        public override PlantBase CreateHybrid(PlantBase neighbor)
        {
            // Return a new hybrid plant instance
            return null;
        }
    }
}
