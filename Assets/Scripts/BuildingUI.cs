using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingUI : MonoBehaviour
{
    public GameObject farmPrefab;
    public GameObject turretPrefab;
    public float farmWoodCost = 50f;
    public float turretWoodCost = 80f;
    public Button farmButton;
    public Button turretButton;
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI foodText;

    void Update()
    {
        if (ResourceManager.Instance != null)
        {
            woodText.text = $"Wood: {ResourceManager.Instance.wood:F0}";
            foodText.text = $"Food: {ResourceManager.Instance.food:F0}";
        }
        //Grey out buttons if player cant afford 
        farmButton.interactable = ResourceManager.Instance != null && ResourceManager.Instance.HasWood(farmWoodCost);
        turretButton.interactable = ResourceManager.Instance != null && ResourceManager.Instance.HasWood(turretWoodCost);
    }

    public void OnClickBuildFarm()
    {
        BuildingPlacer.Instance.StartPlacing(farmPrefab, farmWoodCost, "farm");
    }

    public void OnClickBuildTurret()
    {
        BuildingPlacer.Instance.StartPlacing(turretPrefab, turretWoodCost, "turret");
    }
}