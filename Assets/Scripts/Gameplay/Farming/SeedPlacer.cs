using UnityEngine;

namespace Game.Farming
{
    /// <summary>
    /// Handles seed placement and validation.
    /// </summary>
    public class SeedPlacer : MonoBehaviour
    {
        [SerializeField] private FarmGrid farmGrid;

        public bool PlaceSeed(int x, int y, PlantBase plantPrefab)
        {
            if (farmGrid.Cells[x, y].Plant != null)
                return false;
            var plant = Instantiate(plantPrefab, transform);
            farmGrid.Cells[x, y].Plant = plant;
            return true;
        }
    }
}
