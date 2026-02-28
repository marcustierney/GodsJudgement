using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance;

    public float food = 100;
    public float wood = 100;
    public int troops = 0;
    public int maxTroops = 50;

    void Awake()
    {
        Instance = this;
    }

    public bool SpendFood(float amount)
    {
        if (food < amount)
            return false;

        food -= amount;
        return true;
    }

    public bool SpendWood(float amount)
    {
        if (wood < amount)
            return false;

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
}