using NUnit.Framework;
using UnityEngine;
using Game.Farming;

namespace Tests.Game.Farming
{
    public class PlantDataTests
    {
        [Test]
        public void PlantData_Fields_AreConfigurable()
        {
            var plantData = ScriptableObject.CreateInstance<PlantData>();
            plantData.plantType = "TestPlant";
            plantData.sproutThreshold = 1f;
            plantData.matureThreshold = 2f;
            plantData.hybridThreshold = 3f;
            plantData.minWater = 0.2f;
            plantData.minSoilQuality = 0.3f;
            plantData.minSunlight = 0.4f;
            plantData.hybridizableWith = new string[] { "OtherPlant" };
            Assert.AreEqual("TestPlant", plantData.plantType);
            Assert.AreEqual(1f, plantData.sproutThreshold);
            Assert.AreEqual(2f, plantData.matureThreshold);
            Assert.AreEqual(3f, plantData.hybridThreshold);
            Assert.AreEqual(0.2f, plantData.minWater);
            Assert.AreEqual(0.3f, plantData.minSoilQuality);
            Assert.AreEqual(0.4f, plantData.minSunlight);
            Assert.AreEqual("OtherPlant", plantData.hybridizableWith[0]);
        }
    }

    public class PlantBaseTests
    {
        [Test]
        public void PlantBase_Grow_AdvancesGrowthStage()
        {
            var go = new GameObject();
            var plant = go.AddComponent<PlantBase>();
            var data = ScriptableObject.CreateInstance<PlantData>();
            data.sproutThreshold = 1f;
            data.matureThreshold = 2f;
            data.hybridThreshold = 3f;
            plant.TestSetPlantData(data);
            var resources = new ResourceState { Water = 1f, SoilQuality = 1f, Sunlight = 1f };
            plant.Grow(resources, 2f);
            Assert.GreaterOrEqual(plant.TestGetGrowthStage(), PlantGrowthStage.Sprout);
        }
    }
}
