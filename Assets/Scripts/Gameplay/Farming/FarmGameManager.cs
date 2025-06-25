using UnityEngine;
using Game.Farming;

public class FarmGameManager : MonoBehaviour
{
    [SerializeField] private FarmGrid farmGrid;
    [SerializeField] private FarmGridView farmGridView;

    private void Start()
    {
        farmGrid.Initialize();
        farmGridView.Initialize();
    }
}
