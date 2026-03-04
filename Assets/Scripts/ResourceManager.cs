using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    public float food = 100;
    public float wood = 100;
    public int troops = 0;
    public int maxTroops = 50;
    public float resourceInterval = 2f;
    private float resourceTimer = 0f;

    void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        resourceTimer += Time.deltaTime;
        if (resourceTimer >= resourceInterval)
        {
            resourceTimer = 0f; 
            food += 1f;
            wood += 1f;
        }
    }

    public bool HasFood(float amount)
    {
        return food >= amount;
    }

    public bool SpendFood(float amount)
    {
        if (food < amount)
        {
            return false;
        }
        food -= amount;
        return true;
    }

    public bool HasWood(float amount)
    {
        return wood >= amount;
    }

    public bool SpendWood(float amount)
    {
        if (wood < amount)
        {
            return false;
        }
        wood -= amount;
        return true;
    }

    public void AddFood(float amount)
    {
        food += amount;
    }

    public void AddWood(float amount)
    {
        wood += amount;
    }

    public bool CanCreateTroop(int foodCost)
    {
        return food >= foodCost && troops < maxTroops;
    }

    public void RegisterTroop()
    {
        troops++;
    }

    public void RemoveTroop()
    {
        troops--;
    }

    public void DeregisterTroop()
    {
        troops = Mathf.Max(0, troops - 1);
    }
}