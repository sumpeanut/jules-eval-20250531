using UnityEngine;

namespace Game.Farming
{
    /// <summary>
    /// Handles visual feedback for plant state and events.
    /// </summary>
    public class PlantView : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite seedSprite;
        [SerializeField] private Sprite sproutSprite;
        [SerializeField] private Sprite matureSprite;
        [SerializeField] private Sprite hybridSprite;
        [SerializeField] private Color lowResourceColor = Color.yellow;
        [SerializeField] private Color normalColor = Color.white;

        public void UpdateVisual(PlantGrowthStage stage, bool lowResources, bool isHybrid)
        {
            switch (stage)
            {
                case PlantGrowthStage.Seed:
                    spriteRenderer.sprite = seedSprite;
                    break;
                case PlantGrowthStage.Sprout:
                    spriteRenderer.sprite = sproutSprite;
                    break;
                case PlantGrowthStage.Mature:
                    spriteRenderer.sprite = matureSprite;
                    break;
                case PlantGrowthStage.Hybrid:
                    spriteRenderer.sprite = hybridSprite;
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
