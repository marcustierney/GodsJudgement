using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingUI : MonoBehaviour
{
    public GameObject farmPrefab;
    public GameObject turretPrefab;
    public GameObject lumermillPrefab;
    public GameObject barracksPrefab;
    public GameObject wallPrefab;
    public float lumbermillWoodCost = 30f;
    public float farmWoodCost = 50f;
    public float turretWoodCost = 80f;
    public float barracksWoodCost = 60f;
    public float wallWoodCost = 20f;
    public Button farmButton;
    public Button turretButton;
    public Button lumbermillButton;
    public Button barracksButton;
    public Button wallButton;
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI foodText;

    void Update()
    {
        if (ResourceManager.Instance != null)
        {
            woodText.text = $"Wood: {ResourceManager.Instance.wood:F0}";
            foodText.text = $"Food: {ResourceManager.Instance.food:F0}";
        }
        bool hasWoodForFarm;
        bool hasWoodForTurret;
        bool hasWoodForLumbermill;
        bool hasWoodForBarracks;
        if (ResourceManager.Instance != null)
        {
            hasWoodForFarm = ResourceManager.Instance.HasWood(farmWoodCost);
            hasWoodForTurret = ResourceManager.Instance.HasWood(turretWoodCost);
            hasWoodForLumbermill = ResourceManager.Instance.HasWood(lumbermillWoodCost);
            hasWoodForBarracks = ResourceManager.Instance.HasWood(barracksWoodCost);
            wallButton.interactable = ResourceManager.Instance.HasWood(wallWoodCost);
        }
        else
        {
            hasWoodForFarm = false;
            hasWoodForTurret = false;
            hasWoodForLumbermill = false;
            hasWoodForBarracks = false;
        }
    }

    public void OnClickBuildFarm()
    {
        BuildingPlacer.Instance.StartPlacing(farmPrefab, farmWoodCost, "farm");
    }

    public void OnClickBuildTurret()
    {
        BuildingPlacer.Instance.StartPlacing(turretPrefab, turretWoodCost, "turret");
    }

    public void OnClickBuildLumbermill()
    {
        BuildingPlacer.Instance.StartPlacing(lumermillPrefab, lumbermillWoodCost, "lumbermill");
    }
    public void OnClickBuildBarracks()
    {
        BuildingPlacer.Instance.StartPlacing(barracksPrefab, barracksWoodCost, "barracks");
    }

    public void OnClickBuildWall()
    {
        BuildingPlacer.Instance.StartPlacing(wallPrefab, wallWoodCost, "wall");
    }
}
