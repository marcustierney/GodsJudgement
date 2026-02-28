using System.Collections.Generic;
using UnityEngine;

public class VillageBrain : MonoBehaviour
{
    public float thinkInterval = 1f; 
    private float timer;
    public List<VillageAction> actions = new List<VillageAction>();
    private VillageContext context;
    public GameObject troopPrefab;
    public GameObject farmPrefab;
    public GameObject turretPrefab;

    void Start()
    {
        context = new VillageContext();
        context.townHall = GameManager.Instance.townHall;
        context.food = ResourceManager.Instance.food;
        context.wood = ResourceManager.Instance.wood;
        context.currentTroops = ResourceManager.Instance.troops;
        context.maxTroops = ResourceManager.Instance.maxTroops;

        actions.Add(new TrainTroopAction { troopPrefab = troopPrefab });
        actions.Add(new BuildFarmAction { farmPrefab = farmPrefab });
        actions.Add(new BuildTurretAction { turretPrefab = turretPrefab });
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= thinkInterval)
        {
            timer = 0;
            UpdateContext();
            ChooseBestAction();
        }
    }

    void UpdateContext()
    {
        context.food = ResourceManager.Instance.food;
        context.wood = ResourceManager.Instance.wood;
        context.currentTroops = ResourceManager.Instance.troops;
        context.maxTroops = ResourceManager.Instance.maxTroops;
        context.farmCount = FindObjectsOfType<Farm>().Length;
        context.turretCount = FindObjectsOfType<Turret>().Length;
    }

    void ChooseBestAction()
    {
        float bestScore = 0;
        VillageAction bestAction = null;

        foreach (var action in actions)
        {
            float score = action.CalculateUtility(context);
            if (score > bestScore)
            {
                bestScore = score;
                bestAction = action;
            }
        }
        if (bestAction != null)
        {
            bestAction.Execute(context);
        }
    }
}