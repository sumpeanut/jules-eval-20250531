using UnityEngine;

namespace Game.Farming
{
    /// <summary>
    /// Example implementation of a specific plant.
    /// </summary>
    public class CarrotPlant : PlantBase
    {
        public override string PlantType => "Carrot";
        public override void Grow(ResourceState resources, float deltaTime)
        {
            // Implement growth logic based on resources
        }
        public override bool CanHybridizeWith(PlantBase neighbor)
        {
            // Example: Carrot can hybridize with Tomato
            return neighbor.PlantType == "Tomato";
        }
        public override PlantBase CreateHybrid(PlantBase neighbor)
        {
            // Return a new hybrid plant instance
            return null;
        }
    }
}
