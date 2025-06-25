using UnityEngine;

namespace Game.Inventory
{
    /// <summary>
    /// Scriptable object describing the shape of an inventory item.
    /// </summary>
    [CreateAssetMenu(menuName = "Inventory/ItemData")]
    public class InventoryItemData : ScriptableObject
    {
        public string itemName;
        public Vector2Int[] shape;
    }
}
