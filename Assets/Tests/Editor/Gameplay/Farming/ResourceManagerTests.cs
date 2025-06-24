using NUnit.Framework;
using UnityEngine;
using Game.Farming;

namespace Tests.Game.Farming
{
    public class ResourceManagerTests
    {
        [Test]
        public void WaterCell_IncreasesWaterLevel()
        {
            var go = new GameObject();
            var grid = go.AddComponent<FarmGrid>();
            grid.Initialize();
            var manager = go.AddComponent<ResourceManager>();
            manager.GetType().GetField("farmGrid", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(manager, grid);
            float before = grid.Cells[0, 0].Resources.Water;
            manager.WaterCell(0, 0, 0.5f);
            Assert.Greater(grid.Cells[0, 0].Resources.Water, before);
        }
    }
}
