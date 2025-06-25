using UnityEngine;

namespace Game.Inventory
{
    /// <summary>
    /// Manages placement of items within a grid-based inventory.
    /// </summary>
    public class InventoryGrid : MonoBehaviour
    {
        [SerializeField] private int width = 5;
        [SerializeField] private int height = 5;

        private InventoryItem[,] cells;

        public int Width => width;
        public int Height => height;
        public InventoryItem[,] Cells => cells;

        public void Initialize()
        {
            cells = new InventoryItem[width, height];
        }

        public bool CanPlaceItem(InventoryItemData data, Vector2Int position)
        {
            foreach (var cell in data.shape)
            {
                int x = position.x + cell.x;
                int y = position.y + cell.y;
                if (x < 0 || x >= width || y < 0 || y >= height)
                    return false;
                if (cells[x, y] != null)
                    return false;
            }
            return true;
        }

        public bool PlaceItem(InventoryItem item, Vector2Int position)
        {
            if (!CanPlaceItem(item.Data, position))
                return false;
            foreach (var cell in item.Data.shape)
                cells[position.x + cell.x, position.y + cell.y] = item;
            return true;
        }

        public void RemoveItem(InventoryItem item)
        {
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (cells[x, y] == item)
                        cells[x, y] = null;
                }
            }
        }

        public InventoryItem GetCell(int x, int y) => cells[x, y];
    }
}
