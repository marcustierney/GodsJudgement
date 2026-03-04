using UnityEngine;


public class EarthquakeAction : GodAction
{
    public override float CalculateUtility(GodContext context)
    {
        return 0.3f * context.chaos;
    }

    public override void Execute(GodContext context)
    {
        Farm[] farms = GameObject.FindObjectsOfType<Farm>();

        foreach (var farm in farms)
        {
            Object.Destroy(farm.gameObject);
        }
        Debug.Log("God caused earthquake");
    }
}