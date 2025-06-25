using UnityEngine;
using System.Collections.Generic;

namespace Game.Farming
{
    /// <summary>
    /// Manages the visual grid and user interaction for the farm.
    /// </summary>
    public class FarmGridView : MonoBehaviour
    {
        [SerializeField] private FarmGrid farmGrid;
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private Transform gridParent;
        [SerializeField] private Game.UI.Farming.SeedSelectionPanel seedSelectionPanel;
        [SerializeField] private List<PlantData> availableSeeds;
        [SerializeField] private PlantBase plantPrefab;
        private FarmGridCellView[,] cellViews;
        private int? pendingPlantX = null;
        private int? pendingPlantY = null;

        public void Initialize()
        {
            int width = farmGrid.Width;
            int height = farmGrid.Height;
            cellViews = new FarmGridCellView[width, height];
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    var cellObj = Instantiate(cellPrefab, gridParent);
                    var cellView = cellObj.GetComponent<FarmGridCellView>();
                    cellView.Init(x, y);
                    cellView.OnCellClicked.AddListener(OnCellClicked);
                    cellViews[x, y] = cellView;
                }
            }
        }

        private void OnCellClicked(int x, int y)
        {
            var cell = farmGrid.Cells[x, y];
            if (cell.Plant == null)
            {
                // Show seed selection UI and store cell coordinates
                pendingPlantX = x;
                pendingPlantY = y;
                seedSelectionPanel.Show(availableSeeds, OnSeedSelected);
            }
            else if (cell.Plant.TestGetGrowthStage() == PlantGrowthStage.Mature || cell.Plant.TestGetGrowthStage() == PlantGrowthStage.Hybrid)
            {
                // Harvest plant
                Destroy(cell.Plant.gameObject);
                cell.Plant = null;
                UpdateCellVisual(x, y);
                // TODO: Trigger reward/animation
            }
            else
            {
                // Show resource management UI for this cell (if implemented)
                // TODO: Hook up ResourceManagementPanel.ShowForCell(x, y, cell.Resources)
            }
        }

        private void OnSeedSelected(PlantData plantData)
        {
            if (pendingPlantX.HasValue && pendingPlantY.HasValue)
            {
                int x = pendingPlantX.Value;
                int y = pendingPlantY.Value;
                var cell = farmGrid.Cells[x, y];
                var plant = Instantiate(plantPrefab, cellViews[x, y].transform);
                plant.TestSetPlantData(plantData);
                cell.Plant = plant;
                UpdateCellVisual(x, y);
                pendingPlantX = null;
                pendingPlantY = null;
            }
        }

        public void UpdateCellVisual(int x, int y)
        {
            var cell = farmGrid.Cells[x, y];
            var view = cellViews[x, y];
            if (cell.Plant == null)
                view.SetStateEmpty();
            else if (cell.Plant.TestGetGrowthStage() == PlantGrowthStage.Mature || cell.Plant.TestGetGrowthStage() == PlantGrowthStage.Hybrid)
                view.SetStateHarvestable(cell.Plant);
            else
                view.SetStatePlanted(cell.Plant);
        }
    }
}
