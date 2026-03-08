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
        UpdateButton(farmButton, ResourceManager.Instance.HasWood(farmWoodCost));
        UpdateButton(turretButton, ResourceManager.Instance.HasWood(turretWoodCost));
        UpdateButton(lumbermillButton, ResourceManager.Instance.HasWood(lumbermillWoodCost));
        UpdateButton(barracksButton, ResourceManager.Instance.HasWood(barracksWoodCost));
        UpdateButton(wallButton, ResourceManager.Instance.HasWood(wallWoodCost));
    }

    void UpdateButton(Button button, bool canAfford)
    {
        button.interactable = canAfford;

        Image img = button.GetComponent<Image>();
        if (img != null)
        {
            Color c = img.color;
            if (canAfford)
            {
                c.a = 1f;
            }
            else
            {
                c.a = 0.4f;
            }
            img.color = c;
        }

        TextMeshProUGUI label = button.GetComponentInChildren<TextMeshProUGUI>();
        if (label != null)
        {
            Color tc = label.color;
            if (canAfford)
            {
                tc.a = 1f;
            }
            else
            {
                tc.a = 0.4f;
            }
            label.color = tc;
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
