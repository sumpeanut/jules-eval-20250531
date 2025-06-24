using UnityEngine;
using UnityEngine.UI;
using Game.Farming;
using TMPro;

namespace Game.UI.Farming
{
    /// <summary>
    /// UI controller for managing resources of a selected farm cell.
    /// </summary>
    public class ResourceManagementPanel : MonoBehaviour
    {
        [SerializeField] private ResourceManager resourceManager;
        [SerializeField] private TMP_Text cellPositionText;
        [SerializeField] private Slider waterSlider;
        [SerializeField] private Slider soilSlider;
        [SerializeField] private Slider sunlightSlider;
        private int selectedX;
        private int selectedY;
        private bool hasSelection;

        public void ShowForCell(int x, int y, ResourceState state)
        {
            selectedX = x;
            selectedY = y;
            hasSelection = true;
            cellPositionText.text = $"Cell: ({x},{y})";
            waterSlider.value = state.Water;
            soilSlider.value = state.SoilQuality;
            sunlightSlider.value = state.Sunlight;
            gameObject.SetActive(true);
        }

        public void OnWaterChanged(float value)
        {
            if (hasSelection) resourceManager.WaterCell(selectedX, selectedY, value - resourceManager.farmGrid.Cells[selectedX, selectedY].Resources.Water);
        }
        public void OnSoilChanged(float value)
        {
            if (hasSelection) resourceManager.FertilizeCell(selectedX, selectedY, value - resourceManager.farmGrid.Cells[selectedX, selectedY].Resources.SoilQuality);
        }
        public void OnSunlightChanged(float value)
        {
            if (hasSelection) resourceManager.SunlightCell(selectedX, selectedY, value - resourceManager.farmGrid.Cells[selectedX, selectedY].Resources.Sunlight);
        }
        public void Hide()
        {
            hasSelection = false;
            gameObject.SetActive(false);
        }
    }
}
