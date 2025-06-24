using NUnit.Framework;
using UnityEngine;
using Game.Farming;

namespace Tests.Game.Farming
{
    public class FarmGridTests
    {
        [Test]
        public void FarmGrid_InitializesWithCorrectSize()
        {
            var go = new GameObject();
            var grid = go.AddComponent<FarmGrid>();
            grid.Initialize();
            Assert.AreEqual(grid.Width, grid.Cells.GetLength(0));
            Assert.AreEqual(grid.Height, grid.Cells.GetLength(1));
        }
    }
}
