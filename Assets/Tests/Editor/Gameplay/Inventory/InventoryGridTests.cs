using NUnit.Framework;
using UnityEngine;
using Game.Inventory;

namespace Tests.Game.Inventory
{
    public class InventoryGridTests
    {
        [Test]
        public void InventoryGrid_InitializesWithCorrectSize()
        {
            var go = new GameObject();
            var grid = go.AddComponent<InventoryGrid>();
            grid.Initialize();
            Assert.AreEqual(grid.Width, grid.Cells.GetLength(0));
            Assert.AreEqual(grid.Height, grid.Cells.GetLength(1));
        }

        [Test]
        public void PlaceItem_FailsWhenOverlap()
        {
            var go = new GameObject();
            var grid = go.AddComponent<InventoryGrid>();
            grid.Initialize();
            var data = ScriptableObject.CreateInstance<InventoryItemData>();
            data.shape = new Vector2Int[] { Vector2Int.zero };
            var item1 = new GameObject().AddComponent<InventoryItem>();
            item1.GetType().GetField("itemData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(item1, data);
            var item2 = new GameObject().AddComponent<InventoryItem>();
            item2.GetType().GetField("itemData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(item2, data);
            Assert.IsTrue(grid.PlaceItem(item1, Vector2Int.zero));
            Assert.IsFalse(grid.PlaceItem(item2, Vector2Int.zero));
        }

        [Test]
        public void PlaceItem_SucceedsWhenSpaceAvailable()
        {
            var go = new GameObject();
            var grid = go.AddComponent<InventoryGrid>();
            grid.Initialize();
            var data = ScriptableObject.CreateInstance<InventoryItemData>();
            data.shape = new Vector2Int[] { Vector2Int.zero };
            var item = new GameObject().AddComponent<InventoryItem>();
            item.GetType().GetField("itemData", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(item, data);
            Assert.IsTrue(grid.PlaceItem(item, new Vector2Int(1, 1)));
            Assert.AreEqual(item, grid.GetCell(1, 1));
        }
    }
}
