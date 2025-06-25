using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using Game.Farming;

namespace Game.UI.Farming
{
    /// <summary>
    /// UI for selecting which seed/plant to plant.
    /// </summary>
    public class SeedSelectionPanel : MonoBehaviour
    {
        [SerializeField] private Button seedButtonPrefab;
        [SerializeField] private Transform buttonParent;
        private System.Action<PlantData> onSeedSelected;

        public void Show(List<PlantData> availableSeeds, System.Action<PlantData> onSelected)
        {
            onSeedSelected = onSelected;
            foreach (Transform child in buttonParent)
                Destroy(child.gameObject);
            foreach (var seed in availableSeeds)
            {
                var btn = Instantiate(seedButtonPrefab, buttonParent);
                var label = btn.GetComponentInChildren<TMPro.TMP_Text>();
                if (label != null)
                    label.text = seed.plantType;
                btn.onClick.AddListener(() => SelectSeed(seed));
            }
            gameObject.SetActive(true);
        }

        private void SelectSeed(PlantData data)
        {
            onSeedSelected?.Invoke(data);
            gameObject.SetActive(false);
        }
    }
}
