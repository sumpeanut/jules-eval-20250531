using UnityEngine;

namespace Game.Farming
{
    /// <summary>
    /// Manages resource allocation and updates.
    /// </summary>
    public class ResourceManager : MonoBehaviour
    {
        [SerializeField] private FarmGrid farmGrid;

        public void WaterCell(int x, int y, float amount)
        {
            farmGrid.Cells[x, y].Resources.Water += amount;
        }
        public void FertilizeCell(int x, int y, float amount)
        {
            farmGrid.Cells[x, y].Resources.SoilQuality += amount;
        }
        public void SunlightCell(int x, int y, float amount)
        {
            farmGrid.Cells[x, y].Resources.Sunlight += amount;
        }
    }
}
