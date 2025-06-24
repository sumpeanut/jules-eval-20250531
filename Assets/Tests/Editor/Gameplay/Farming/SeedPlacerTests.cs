using NUnit.Framework;
using UnityEngine;
using Game.Farming;

namespace Tests.Game.Farming
{
    public class SeedPlacerTests
    {
        [Test]
        public void PlaceSeed_EmptyCell_Succeeds()
        {
            var go = new GameObject();
            var grid = go.AddComponent<FarmGrid>();
            grid.Initialize();
            var placer = go.AddComponent<SeedPlacer>();
            placer.GetType().GetField("farmGrid", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(placer, grid);
            var plantPrefab = new GameObject().AddComponent<CarrotPlant>();
            bool result = placer.PlaceSeed(0, 0, plantPrefab);
            Assert.IsTrue(result);
            Assert.IsNotNull(grid.Cells[0, 0].Plant);
        }
    }
}
