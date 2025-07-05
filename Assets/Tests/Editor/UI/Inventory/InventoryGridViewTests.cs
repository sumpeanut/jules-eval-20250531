using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Game.Inventory;
using Game.UI.Inventory;

namespace Tests.UI.Inventory
{
    public class InventoryGridViewTests
    {
        [Test]
        public void UpdateGrid_CreatesCellsForEachGridSlot()
        {
            var gridObj = new GameObject();
            var grid = gridObj.AddComponent<InventoryGrid>();
            grid.Initialize();

            var viewObj = new GameObject();
            var doc = viewObj.AddComponent<UIDocument>();
            doc.visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/Scripts/UI/Inventory/InventoryGridView.uxml");
            viewObj.AddComponent<InventoryGridView>();
            var view = viewObj.GetComponent<InventoryGridView>();
            typeof(InventoryGridView).GetField("inventoryGrid", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).SetValue(view, grid);
            viewObj.GetComponent<UIDocument>().rootVisualElement.styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Scripts/UI/Inventory/InventoryGridView.uss"));
            view.UpdateGrid();

            var gridRoot = doc.rootVisualElement.Q("grid-root");
            Assert.AreEqual(grid.Width * grid.Height, gridRoot.childCount);
        }
    }
}
