using UnityEngine;
using Game.Farming;

namespace Game.Farming
{
    /// <summary>
    /// Example implementation of a specific plant.
    /// </summary>
    public class CarrotPlant : PlantBase
    {
        public override string PlantType => "Carrot";

        private PlantGrowthStage growthStage = PlantGrowthStage.Seed;
        private float growthProgress = 0f;
        private PlantView plantView;
        private const float sproutThreshold = 2f;
        private const float matureThreshold = 5f;
        private const float hybridThreshold = 7f;

        private void Awake()
        {
            plantView = GetComponent<PlantView>();
            if (plantView != null)
                plantView.UpdateVisual(growthStage, false, false);
        }

        public override void Grow(ResourceState resources, float deltaTime)
        {
            bool lowResource = resources.Water < 0.5f || resources.SoilQuality < 0.5f || resources.Sunlight < 0.5f;
            float growthRate = (resources.Water + resources.SoilQuality + resources.Sunlight) / 3f;
            if (lowResource && plantView != null)
                plantView.ShowLowResourceWarning("Resource");
            growthProgress += growthRate * deltaTime * (lowResource ? 0.5f : 1f);
            PlantGrowthStage prevStage = growthStage;
            if (growthProgress >= hybridThreshold)
                growthStage = PlantGrowthStage.Hybrid;
            else if (growthProgress >= matureThreshold)
                growthStage = PlantGrowthStage.Mature;
            else if (growthProgress >= sproutThreshold)
                growthStage = PlantGrowthStage.Sprout;
            else
                growthStage = PlantGrowthStage.Seed;
            if (plantView != null)
                plantView.UpdateVisual(growthStage, lowResource, growthStage == PlantGrowthStage.Hybrid);
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
