using UnityEngine;
using System.Collections.Generic;

namespace Game.Farming
{
    /// <summary>
    /// Handles plant growth, hybridization, and updates.
    /// </summary>
    public class PlantGrowthManager : MonoBehaviour
    {
        [SerializeField] private FarmGrid farmGrid;
        [SerializeField] private float growthInterval = 1f;
        private float timer;

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= growthInterval)
            {
                timer = 0f;
                UpdateGrowth();
            }
        }

        private void UpdateGrowth()
        {
            for (int x = 0; x < farmGrid.Width; x++)
            {
                for (int y = 0; y < farmGrid.Height; y++)
                {
                    var cell = farmGrid.Cells[x, y];
                    if (cell.Plant != null)
                    {
                        cell.Plant.Grow(cell.Resources, growthInterval);
                        TryHybridize(cell, x, y);
                    }
                }
            }
        }

        private void TryHybridize(FarmGridCell cell, int x, int y)
        {
            var neighbors = GetNeighbors(x, y);
            foreach (var neighbor in neighbors)
            {
                if (neighbor.Plant != null && cell.Plant.CanHybridizeWith(neighbor.Plant))
                {
                    var hybrid = cell.Plant.CreateHybrid(neighbor.Plant);
                    if (hybrid != null)
                    {
                        // Replace with hybrid plant (example logic)
                        cell.Plant = hybrid;
                    }
                }
            }
        }

        private List<FarmGridCell> GetNeighbors(int x, int y)
        {
            var neighbors = new List<FarmGridCell>();
            int[,] dirs = { {0,1},{1,0},{0,-1},{-1,0} };
            for (int i = 0; i < 4; i++)
            {
                int nx = x + dirs[i,0];
                int ny = y + dirs[i,1];
                if (nx >= 0 && nx < farmGrid.Width && ny >= 0 && ny < farmGrid.Height)
                {
                    neighbors.Add(farmGrid.Cells[nx, ny]);
                }
            }
            return neighbors;
        }
    }
}
