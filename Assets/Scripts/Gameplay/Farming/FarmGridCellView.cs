using UnityEngine;
using UnityEngine.Events;

namespace Game.Farming
{
    /// <summary>
    /// Handles the visual representation and user interaction for a single farm grid cell.
    /// </summary>
    public class FarmGridCellView : MonoBehaviour
    {
        public UnityEvent<int, int> OnCellClicked;
        private int x, y;

        public void Init(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public void OnMouseUpAsButton()
        {
            OnCellClicked?.Invoke(x, y);
        }

        public void SetStateEmpty() { /* TODO: update visuals for empty cell */ }
        public void SetStatePlanted(PlantBase plant) { /* TODO: update visuals for planted cell */ }
        public void SetStateHarvestable(PlantBase plant) { /* TODO: update visuals for harvestable plant */ }
    }
}
