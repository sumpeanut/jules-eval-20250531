using UnityEngine;
using Game.Farming;

namespace Game.Farming
{
    /// <summary>
    /// Base class for all plants.
    /// </summary>
    public class PlantBase : MonoBehaviour
    {
        [SerializeField] protected PlantData plantData;
        protected PlantGrowthStage growthStage = PlantGrowthStage.Seed;
        protected float growthProgress = 0f;
        protected PlantView plantView;

        protected virtual void Awake()
        {
            plantView = GetComponent<PlantView>();
            if (plantView != null && plantData != null)
            {
                plantView.SetPlantData(plantData);
                plantView.UpdateVisual(growthStage, false, false);
            }
        }

        public virtual void Grow(ResourceState resources, float deltaTime)
        {
            if (plantData == null) return;
            bool lowResource = resources.Water < plantData.minWater || resources.SoilQuality < plantData.minSoilQuality || resources.Sunlight < plantData.minSunlight;
            float growthRate = (resources.Water + resources.SoilQuality + resources.Sunlight) / 3f;
            if (lowResource && plantView != null)
                plantView.ShowLowResourceWarning("Resource");
            growthProgress += growthRate * deltaTime * (lowResource ? 0.5f : 1f);
            PlantGrowthStage prevStage = growthStage;
            if (growthProgress >= plantData.hybridThreshold)
                growthStage = PlantGrowthStage.Hybrid;
            else if (growthProgress >= plantData.matureThreshold)
                growthStage = PlantGrowthStage.Mature;
            else if (growthProgress >= plantData.sproutThreshold)
                growthStage = PlantGrowthStage.Sprout;
            else
                growthStage = PlantGrowthStage.Seed;
            if (plantView != null)
                plantView.UpdateVisual(growthStage, lowResource, growthStage == PlantGrowthStage.Hybrid);
        }

        public virtual string PlantType => plantData != null ? plantData.plantType : "Unknown";

        public virtual bool CanHybridizeWith(PlantBase neighbor)
        {
            if (plantData == null || neighbor == null || neighbor.plantData == null) return false;
            foreach (var type in plantData.hybridizableWith)
            {
                if (neighbor.PlantType == type)
                    return true;
            }
            return false;
        }
        public virtual PlantBase CreateHybrid(PlantBase neighbor)
        {
            if (plantData != null && plantData.hybridResult != null)
            {
                // Instantiate a new plant with the hybridResult PlantData
                var go = new GameObject("HybridPlant");
                var hybrid = go.AddComponent<PlantBase>();
                hybrid.plantData = plantData.hybridResult;
                return hybrid;
            }
            return null;
        }

        // Expose for testing
        public PlantData TestGetPlantData() => plantData;
        public void TestSetPlantData(PlantData data) => plantData = data;
        public PlantGrowthStage TestGetGrowthStage() => growthStage;
    }
}
