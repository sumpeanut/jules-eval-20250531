using UnityEngine;
using Game.Farming;

namespace Game.Farming
{
    /// <summary>
    /// Example implementation of a specific plant.
    /// </summary>
    [System.Obsolete("This class is now obsolete due to the PlantBase/PlantData refactor. Use PlantBase with a PlantData asset instead.")]
    public class CarrotPlant : PlantBase
    {
        public override string PlantType => "Carrot";

        // This class is now obsolete due to the PlantBase/PlantData refactor. Use PlantBase with a PlantData asset instead.
        // Intentionally left blank
    }
}
