using UnityEngine;
using System.Collections.Generic; // Required for List<Vector2Int> if we go that route later

namespace FarmingGame.Data.Inventory
{
    public enum ItemShapeType
    {
        _1x1,
        _1x2, _2x1,
        _2x2,
        _1x3, _3x1,
        // More complex shapes - these will require careful handling for rotation
        LShapeLeftUp, // L shape pointing Left and Up (occupies (0,0), (-1,0), (0,1))
        LShapeRightUp, // L shape pointing Right and Up (occupies (0,0), (1,0), (0,1))
        LShapeLeftDown,
        LShapeRightDown,
        TShapeUp,
        TShapeDown,
        TShapeLeft,
        TShapeRight
    }

    [CreateAssetMenu(fileName = "NewItemData", menuName = "Farming Game/Inventory/Item Data")]
    public class ItemData : ScriptableObject
    {
        [Header("Basic Info")]
        public string itemName = "New Item";
        public string description = "Item Description";
        public Sprite icon;

        [Header("Stacking")]
        public bool isStackable = false;
        public int maxStackSize = 1;

        [Header("Inventory Grid Shape")]
        public ItemShapeType itemShape = ItemShapeType._1x1;

        // Width and Height are determined by InventoryShapeUtility based on itemShape (assuming 0 rotation for base dimensions)
        public int width => FarmingGame.Gameplay.Inventory.Logic.InventoryShapeUtility.GetDimensions(itemShape, 0).x;
        public int height => FarmingGame.Gameplay.Inventory.Logic.InventoryShapeUtility.GetDimensions(itemShape, 0).y;

        private void OnValidate()
        {
            if (!isStackable)
            {
                maxStackSize = 1;
            }
            else
            {
                if (maxStackSize < 1) maxStackSize = 1;
            }
        }
    }
}
