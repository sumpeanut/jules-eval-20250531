using UnityEngine;

namespace Game.Farming
{
    /// <summary>
    /// ScriptableObject holding plant configuration data for growth, visuals, and hybridization.
    /// </summary>
    [CreateAssetMenu(fileName = "PlantData", menuName = "Farming/PlantData")]
    public class PlantData : ScriptableObject
    {
        public string plantType;
        public float sproutThreshold = 2f;
        public float matureThreshold = 5f;
        public float hybridThreshold = 7f;
        public float minWater = 0.5f;
        public float minSoilQuality = 0.5f;
        public float minSunlight = 0.5f;
        public Sprite seedSprite;
        public Sprite sproutSprite;
        public Sprite matureSprite;
        public Sprite hybridSprite;
        public string[] hybridizableWith; // List of plant types this plant can hybridize with
        public PlantData hybridResult; // The resulting hybrid plant data
    }
}
