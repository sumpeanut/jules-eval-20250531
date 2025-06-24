using UnityEngine;
using System.Collections.Generic;

namespace Game.Farming
{
    /// <summary>
    /// Manages the farm grid and cell logic.
    /// </summary>
    public class FarmGrid : MonoBehaviour
    {
        [SerializeField] private int width = 5;
        [SerializeField] private int height = 5;
        private FarmGridCell[,] cells;

        public int Width => width;
        public int Height => height;
        public FarmGridCell[,] Cells => cells;

        public void Initialize()
        {
            cells = new FarmGridCell[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    cells[x, y] = new FarmGridCell(new Vector2Int(x, y));
                }
            }
        }
    }
}
