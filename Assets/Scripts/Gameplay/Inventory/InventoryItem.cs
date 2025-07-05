using UnityEngine;

namespace Game.Inventory
{
    /// <summary>
    /// MonoBehaviour representing a placed inventory item.
    /// </summary>
    public class InventoryItem : MonoBehaviour
    {
        [SerializeField] private InventoryItemData itemData;

        public InventoryItemData Data => itemData;
    }
}
