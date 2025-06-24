using UnityEngine;
using Game.Farming;

namespace Game.Farming
{
    /// <summary>
    /// Handles visual feedback for plant state and events.
    /// </summary>
    public class PlantView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        private PlantData plantData;
        [SerializeField] private Color lowResourceColor = Color.yellow;
        [SerializeField] private Color normalColor = Color.white;

        public void SetPlantData(PlantData data)
        {
            plantData = data;
        }

        public void UpdateVisual(PlantGrowthStage stage, bool lowResources, bool isHybrid)
        {
            if (plantData == null) return;
            switch (stage)
            {
                case PlantGrowthStage.Seed:
                    spriteRenderer.sprite = plantData.seedSprite;
                    break;
                case PlantGrowthStage.Sprout:
                    spriteRenderer.sprite = plantData.sproutSprite;
                    break;
                case PlantGrowthStage.Mature:
                    spriteRenderer.sprite = plantData.matureSprite;
                    break;
                case PlantGrowthStage.Hybrid:
                    spriteRenderer.sprite = plantData.hybridSprite;
                    break;
            }
            spriteRenderer.color = lowResources ? lowResourceColor : normalColor;
        }

        public void ShowLowResourceWarning(string resourceName)
        {
            // Optionally, display a UI warning (e.g., floating text, icon, or animation)
            Debug.Log($"Warning: Low {resourceName} for plant at {transform.position}");
            // You can expand this to trigger a UI element or animation
        }

        public void ShowHybridizationEffect()
        {
            // Optionally, play a particle effect or animation for hybridization
            Debug.Log($"Hybridization occurred at {transform.position}");
            // You can expand this to trigger a VFX or UI feedback
        }
    }
}
