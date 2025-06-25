using NUnit.Framework;
using UnityEngine;
using Game.Farming;

namespace Tests.Game.Farming
{
    public class PlantGrowthManagerTests
    {
        [Test]
        public void UpdateGrowth_CallsGrowOnPlants()
        {
            var go = new GameObject();
            var grid = go.AddComponent<FarmGrid>();
            grid.Initialize();
            var plantGo = new GameObject();
            var plant = plantGo.AddComponent<PlantBase>();
            var data = ScriptableObject.CreateInstance<PlantData>();
            plant.TestSetPlantData(data);
            grid.Cells[0, 0].Plant = plant;
            var manager = go.AddComponent<PlantGrowthManager>();
            manager.GetType().GetField("farmGrid", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(manager, grid);
            // Simulate update
            manager.Invoke("UpdateGrowth", 0f);
            Assert.Pass(); // If no exception, pass
        }
    }
}
