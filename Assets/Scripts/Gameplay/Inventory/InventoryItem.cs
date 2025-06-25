using UnityEngine;
using FarmingGame.Data.Inventory; // Required for ItemData

namespace FarmingGame.Gameplay.Inventory
{
    [System.Serializable] // Useful if we want to serialize a list of these directly, e.g. for saving game state
    public class InventoryItem
    {
        public ItemData itemData;
        public int quantity;
        public Vector2Int gridPosition; // Top-left anchor of the item in the inventory grid
        public int rotation; // Current rotation (0, 1, 2, 3 representing 0, 90, 180, 270 degrees clockwise)

        public InventoryItem(ItemData data, int qty = 1, Vector2Int pos = default, int rot = 0)
        {
            itemData = data;
            quantity = data.isStackable ? qty : 1; // Ensure non-stackable items always have quantity 1
            gridPosition = pos;
            rotation = (rot % 4 + 4) % 4; // Normalize rotation
        }

        // Convenience properties to get the item's current dimensions based on its rotation
        public int currentWidth => Logic.InventoryShapeUtility.GetDimensions(itemData.itemShape, rotation).x;
        public int currentHeight => Logic.InventoryShapeUtility.GetDimensions(itemData.itemShape, rotation).y;

        /// <summary>
        /// Gets the list of relative cell coordinates for this item's current shape and rotation.
        /// </summary>
        public List<System.Collections.Generic.List<Vector2Int>> GetRelativeShapeCells()
        {
            // This seems to be returning a list of lists of Vector2Int, which is not what GetShapeCells returns.
            // It should return List<Vector2Int>
            // return Logic.InventoryShapeUtility.GetShapeCells(itemData.itemShape, rotation);
            // Correcting the above based on InventoryShapeUtility's signature
            return new List<System.Collections.Generic.List<Vector2Int>> { Logic.InventoryShapeUtility.GetShapeCells(itemData.itemShape, rotation) };
        }


        /// <summary>
        /// Gets the list of absolute grid cell coordinates occupied by this item.
        /// </summary>
        public List<Vector2Int> GetAbsoluteOccupiedCells()
        {
            List<Vector2Int> relativeCells = Logic.InventoryShapeUtility.GetShapeCells(itemData.itemShape, rotation);
            List<Vector2Int> absoluteCells = new List<Vector2Int>();

            foreach (var cell in relativeCells)
            {
                absoluteCells.Add(gridPosition + cell);
            }
            return absoluteCells;
        }

        public void AddQuantity(int amount)
        {
            if (!itemData.isStackable) return;
            quantity += amount;
            if (quantity > itemData.maxStackSize)
            {
                quantity = itemData.maxStackSize;
            }
        }

        public void RemoveQuantity(int amount)
        {
            if (!itemData.isStackable) // Non-stackable items are removed entirely, not by quantity
            {
                quantity = 0; // Or handle removal logic elsewhere
                return;
            }
            quantity -= amount;
            if (quantity < 0)
            {
                quantity = 0;
            }
        }
    }
}
