using UnityEngine;
using System.Collections.Generic;
using FarmingGame.Data.Inventory; // For ItemData, ItemShapeType

namespace FarmingGame.Gameplay.Inventory.Logic
{
    public class InventoryGrid
    {
        public Vector2Int GridSize { get; private set; }
        private readonly InventoryItem[,] _occupiedCells; // Stores a reference to the InventoryItem occupying the cell

        public InventoryGrid(int width, int height)
        {
            if (width <= 0 || height <= 0)
            {
                Debug.LogError("InventoryGrid dimensions must be positive.");
                width = Mathf.Max(1, width);
                height = Mathf.Max(1, height);
            }
            GridSize = new Vector2Int(width, height);
            _occupiedCells = new InventoryItem[width, height];
            // Initialize all cells to null (empty)
        }

        /// <summary>
        /// Calculates the absolute grid cell coordinates an item would occupy if placed at origin with given rotation.
        /// </summary>
        private List<Vector2Int> GetAbsoluteItemCells(Vector2Int origin, ItemShapeType shapeType, int rotation)
        {
            List<Vector2Int> relativeCells = InventoryShapeUtility.GetShapeCells(shapeType, rotation);
            List<Vector2Int> absoluteCells = new List<Vector2Int>();
            foreach (var cell in relativeCells)
            {
                absoluteCells.Add(origin + cell);
            }
            return absoluteCells;
        }

        /// <summary>
        /// Checks if a specific cell is within the grid boundaries.
        /// </summary>
        public bool IsCellWithinBounds(Vector2Int cellPosition)
        {
            return cellPosition.x >= 0 && cellPosition.x < GridSize.x &&
                   cellPosition.y >= 0 && cellPosition.y < GridSize.y;
        }

        /// <summary>
        /// Checks if a cell is occupied by any part of an item.
        /// </summary>
        public bool IsCellOccupied(Vector2Int cellPosition)
        {
            if (!IsCellWithinBounds(cellPosition))
            {
                return true; // Out of bounds is considered "occupied" for placement purposes
            }
            return _occupiedCells[cellPosition.x, cellPosition.y] != null;
        }

        /// <summary>
        /// Gets the InventoryItem that occupies the given cell. Returns null if empty or out of bounds.
        /// </summary>
        public InventoryItem GetItemAtCell(Vector2Int cellPosition)
        {
            if (!IsCellWithinBounds(cellPosition))
            {
                return null;
            }
            return _occupiedCells[cellPosition.x, cellPosition.y];
        }

        /// <summary>
        /// Checks if an item (defined by its ItemData, origin, and rotation) can be placed on the grid.
        /// The 'itemToIgnore' parameter is used when checking placement for an item that is already on the grid (e.g. when moving it).
        /// </summary>
        public bool CanPlaceItem(ItemData itemData, Vector2Int origin, int rotation, InventoryItem itemToIgnore = null)
        {
            if (itemData == null) return false;

            List<Vector2Int> cellsToOccupy = GetAbsoluteItemCells(origin, itemData.itemShape, rotation);

            if (cellsToOccupy.Count == 0) return false; // Should not happen with valid shapes

            foreach (var cell in cellsToOccupy)
            {
                if (!IsCellWithinBounds(cell))
                {
                    return false; // Part of the item is out of bounds
                }
                InventoryItem occupyingItem = _occupiedCells[cell.x, cell.y];
                if (occupyingItem != null && occupyingItem != itemToIgnore)
                {
                    return false; // Cell is occupied by another item
                }
            }
            return true;
        }

        /// <summary>
        /// Places an item onto the grid. Assumes CanPlaceItem has already been checked and returned true.
        /// Updates the item's gridPosition and rotation.
        /// </summary>
        public void PlaceItem(InventoryItem item, Vector2Int origin, int rotation)
        {
            if (item == null || item.itemData == null) return;

            // Clear item from old position first, if it was already on grid (e.g. during a move)
            // This check ensures RemoveItem is only called if the item instance is known to the grid.
            // A more robust way might be to ensure RemoveItem is idempotent or only called when necessary.
            if (GetItemAtCell(item.gridPosition) == item) // A basic check
            {
                 // It implies the item might already be on the grid. If so, it should be removed from its old spot
                 // before being placed in a new one. This is especially important if 'item' is being moved.
                 // However, a simple PlaceItem might not know the item's old state.
                 // Let's assume if an item is being "placed", it's either new or its old position is handled by the caller.
                 // For a "move" operation, the caller should Remove then Place.
                 // For now, we'll remove based on its *current* properties before updating them.
                 // This is a bit tricky. If PlaceItem is called for a brand new item, item.gridPosition might be default.
                 // A better approach: PlayerInventory calls RemoveItem(itemToMove) then PlaceItem(itemToMove, newPos, newRot)
            }


            List<Vector2Int> cellsToOccupy = GetAbsoluteItemCells(origin, item.itemData.itemShape, rotation);

            // Update item's properties
            item.gridPosition = origin;
            item.rotation = (rotation % 4 + 4) % 4;

            foreach (var cell in cellsToOccupy)
            {
                if (IsCellWithinBounds(cell))
                {
                    _occupiedCells[cell.x, cell.y] = item;
                }
                else
                {
                    // This should ideally not happen if CanPlaceItem was checked.
                    Debug.LogError($"Attempting to place item partly out of bounds at {cell}, which should have been caught by CanPlaceItem.");
                }
            }
        }

        /// <summary>
        /// Removes an item from the grid. It clears all cells occupied by this specific item instance.
        /// </summary>
        public InventoryItem RemoveItem(InventoryItem itemToRemove)
        {
            if (itemToRemove == null) return null;

            // Use the item's stored gridPosition and rotation to find its cells
            List<Vector2Int> cellsOccupiedByItem = GetAbsoluteItemCells(itemToRemove.gridPosition, itemToRemove.itemData.itemShape, itemToRemove.rotation);

            bool itemFoundAndRemoved = false;
            foreach (var cell in cellsOccupiedByItem)
            {
                if (IsCellWithinBounds(cell) && _occupiedCells[cell.x, cell.y] == itemToRemove)
                {
                    _occupiedCells[cell.x, cell.y] = null;
                    itemFoundAndRemoved = true;
                }
                // else: Cell might be out of bounds (error in logic) or occupied by a different item (also error)
                // or was already null.
            }

            // if (!itemFoundAndRemoved) {
            //     Debug.LogWarning($"RemoveItem: Item '{itemToRemove.itemData.itemName}' was not found at its registered position/shape on the grid, or was already removed.");
            // }
            return itemToRemove;
        }

        /// <summary>
        /// Clears all items from the grid.
        /// </summary>
        public void ClearGrid()
        {
            for (int x = 0; x < GridSize.x; x++)
            {
                for (int y = 0; y < GridSize.y; y++)
                {
                    _occupiedCells[x, y] = null;
                }
            }
        }
    }
}
