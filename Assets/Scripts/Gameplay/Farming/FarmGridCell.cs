using UnityEngine;

namespace Game.Farming
{
    /// <summary>
    /// Represents a single cell in the farm grid.
    /// </summary>
    public class FarmGridCell
    {
        public Vector2Int Position { get; private set; }
        public PlantBase Plant { get; set; }
        public ResourceState Resources { get; set; }

        public FarmGridCell(Vector2Int position)
        {
            Position = position;
            Resources = new ResourceState();
        }
    }
}
