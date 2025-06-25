using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using FarmingGame.Data.Inventory;
using FarmingGame.Gameplay.Inventory.Logic;

namespace FarmingGame.Gameplay.Inventory
{
    public class PlayerInventory : MonoBehaviour
    {
        [Header("Inventory Settings")]
        [SerializeField] private int inventoryWidth = 8;
        [SerializeField] private int inventoryHeight = 5;

        public InventoryGrid InventoryGrid { get; private set; }
        private List<InventoryItem> _items;
        public IReadOnlyList<InventoryItem> Items => _items.AsReadOnly();

        // TODO: public UnityEvent OnInventoryChanged; // For UI updates

        private void Awake()
        {
            InventoryGrid = new InventoryGrid(inventoryWidth, inventoryHeight);
            _items = new List<InventoryItem>();
            // TODO: Load inventory if save/load system is in place
        }

        /// <summary>
        /// Tries to add an item (or items) to the inventory.
        /// Handles stacking if the item is stackable and space is available in existing stacks.
        /// Otherwise, tries to find a new spot for the item(s).
        /// </summary>
        /// <param name="itemData">The type of item to add.</param>
        /// <param name="quantity">The amount to add.</param>
        /// <returns>True if all items were successfully added, false otherwise.</returns>
        public bool AddItem(ItemData itemData, int quantity = 1)
        {
            if (itemData == null || quantity <= 0) return false;

            int remainingQuantity = quantity;

            // 1. Try stacking if item is stackable
            if (itemData.isStackable)
            {
                foreach (var existingItem in _items)
                {
                    if (existingItem.itemData == itemData && existingItem.quantity < itemData.maxStackSize)
                    {
                        int canAdd = itemData.maxStackSize - existingItem.quantity;
                        int toAdd = Mathf.Min(remainingQuantity, canAdd);

                        existingItem.AddQuantity(toAdd);
                        remainingQuantity -= toAdd;

                        if (remainingQuantity <= 0)
                        {
                            // OnInventoryChanged?.Invoke();
                            return true;
                        }
                    }
                }
            }

            // 2. If items remain (either non-stackable or no existing stacks with space), try finding new spots
            // For each unstacked item or remaining quantity of stackable items.
            int itemsToPlaceIndividually = itemData.isStackable ? Mathf.CeilToInt((float)remainingQuantity / itemData.maxStackSize) : remainingQuantity;
            int lastPlacedCount = 0;

            for (int i = 0; i < itemsToPlaceIndividually; i++)
            {
                int currentItemStackQuantity = itemData.isStackable ? Mathf.Min(remainingQuantity, itemData.maxStackSize) : 1;
                if (currentItemStackQuantity <= 0) break;

                FindPlacementResult placement = FindFirstAvailableSlot(itemData);
                if (placement.found)
                {
                    InventoryItem newItem = new InventoryItem(itemData, currentItemStackQuantity, placement.position, placement.rotation);
                    _items.Add(newItem);
                    InventoryGrid.PlaceItem(newItem, placement.position, placement.rotation);
                    remainingQuantity -= currentItemStackQuantity;
                    lastPlacedCount++;
                }
                else
                {
                    Debug.LogWarning($"Inventory full or no space for {itemData.itemName}. Could not place all items.");
                    // OnInventoryChanged?.Invoke(); // Even if partially added
                    return lastPlacedCount > 0; // Return true if at least one item/stack was placed
                }
            }

            // OnInventoryChanged?.Invoke();
            return remainingQuantity <= 0;
        }

        private struct FindPlacementResult
        {
            public bool found;
            public Vector2Int position;
            public int rotation;
        }

        private FindPlacementResult FindFirstAvailableSlot(ItemData itemData)
        {
            for (int r = 0; r < 4; r++) // Try all 4 rotations
            {
                // Potentially optimize search pattern (e.g., smarter preferred rotation)
                // For now, 0, 90, 180, 270 degrees
                int rotation = r;
                Vector2Int itemDimensions = InventoryShapeUtility.GetDimensions(itemData.itemShape, rotation);

                if (itemDimensions.x == 0 || itemDimensions.y == 0) continue; // Invalid shape/rotation

                for (int y = 0; y <= InventoryGrid.GridSize.y - itemDimensions.y; y++)
                {
                    for (int x = 0; x <= InventoryGrid.GridSize.x - itemDimensions.x; x++)
                    {
                        Vector2Int position = new Vector2Int(x, y);
                        if (InventoryGrid.CanPlaceItem(itemData, position, rotation))
                        {
                            return new FindPlacementResult { found = true, position = position, rotation = rotation };
                        }
                    }
                }
            }
            return new FindPlacementResult { found = false };
        }

        /// <summary>
        /// Removes a specific quantity of an item from a given InventoryItem instance.
        /// If quantity to remove is equal or greater than the item's quantity (or if not stackable), the item is removed entirely.
        /// </summary>
        public void RemoveItem(InventoryItem itemInstance, int quantity = 1)
        {
            if (itemInstance == null || !_items.Contains(itemInstance) || quantity <= 0) return;

            if (itemInstance.itemData.isStackable)
            {
                if (quantity >= itemInstance.quantity)
                {
                    InventoryGrid.RemoveItem(itemInstance);
                    _items.Remove(itemInstance);
                }
                else
                {
                    itemInstance.RemoveQuantity(quantity);
                }
            }
            else // Non-stackable items are always removed entirely
            {
                InventoryGrid.RemoveItem(itemInstance);
                _items.Remove(itemInstance);
            }
            // OnInventoryChanged?.Invoke();
        }

        /// <summary>
        /// Moves an item already in the inventory to a new position and rotation.
        /// </summary>
        /// <returns>True if the move was successful, false otherwise.</returns>
        public bool MoveItem(InventoryItem itemInstance, Vector2Int newPosition, int newRotation)
        {
            if (itemInstance == null || !_items.Contains(itemInstance)) return false;

            // Temporarily remove the item from the grid to check if the new position is valid
            // without self-collision.
            InventoryGrid.RemoveItem(itemInstance);

            if (InventoryGrid.CanPlaceItem(itemInstance.itemData, newPosition, newRotation, itemToIgnore: null)) // was itemInstance before, but it's already removed
            {
                InventoryGrid.PlaceItem(itemInstance, newPosition, newRotation); // PlaceItem updates itemInstance's pos/rot
                // OnInventoryChanged?.Invoke();
                return true;
            }
            else
            {
                // Move failed, put it back to its original spot.
                InventoryGrid.PlaceItem(itemInstance, itemInstance.gridPosition, itemInstance.rotation); // Place it back using its old (current) pos/rot
                Debug.LogWarning($"Failed to move item {itemInstance.itemData.itemName} to {newPosition} with rotation {newRotation}. Space might be occupied or out of bounds.");
                return false;
            }
        }

        /// <summary>
        /// Gets the InventoryItem whose shape occupies the given grid cell.
        /// This might be the item's anchor or any part of its shape.
        /// </summary>
        public InventoryItem GetItemAtGridPosition(Vector2Int gridPosition)
        {
            return InventoryGrid.GetItemAtCell(gridPosition);
        }

        /// <summary>
        /// Checks if the inventory contains at least a certain quantity of a specific item type.
        /// </summary>
        public bool HasItem(ItemData itemData, int quantity = 1)
        {
            if (itemData == null || quantity <= 0) return false;
            int count = 0;
            foreach (var item in _items)
            {
                if (item.itemData == itemData)
                {
                    count += item.quantity;
                }
                if (count >= quantity) return true;
            }
            return false;
        }

        // Example of how to get all items of a certain type
        public List<InventoryItem> GetAllItemsOfType(ItemData itemData)
        {
            return _items.Where(item => item.itemData == itemData).ToList();
        }
    }
}
