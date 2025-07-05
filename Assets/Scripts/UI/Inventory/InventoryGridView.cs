using UnityEngine;
using UnityEngine.UIElements;
using Game.Inventory;

namespace Game.UI.Inventory
{
    /// <summary>
    /// Renders an InventoryGrid using the Unity UI Toolkit.
    /// </summary>
    [RequireComponent(typeof(UIDocument))]
    public class InventoryGridView : MonoBehaviour
    {
        [SerializeField] private InventoryGrid inventoryGrid;
        private VisualElement gridRoot;

        private void Awake()
        {
            var doc = GetComponent<UIDocument>();
            gridRoot = doc.rootVisualElement.Q("grid-root");
        }

        private void OnEnable()
        {
            UpdateGrid();
        }

        /// <summary>
        /// Rebuilds the visual grid based on the current InventoryGrid state.
        /// </summary>
        public void UpdateGrid()
        {
            if (gridRoot == null)
            {
                var doc = GetComponent<UIDocument>();
                gridRoot = doc != null ? doc.rootVisualElement.Q("grid-root") : null;
            }

            if (gridRoot == null || inventoryGrid == null || inventoryGrid.Cells == null)
                return;

            gridRoot.Clear();
            for (int y = 0; y < inventoryGrid.Height; y++)
            {
                for (int x = 0; x < inventoryGrid.Width; x++)
                {
                    var cell = new VisualElement();
                    cell.AddToClassList("inventory-cell");
                    if (inventoryGrid.GetCell(x, y) != null)
                        cell.AddToClassList("item");
                    gridRoot.Add(cell);
                }
            }
        }
    }
}
