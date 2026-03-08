using UnityEngine;
using System.Collections.Generic;

public class EarthquakeAction : GodAction
{
    public override float CalculateUtility(GodContext context)
    {
        if (context.personality.styleSatisfaction > 0.25f) //Only happens when style personality is dissatisfied
        {
            return 0;
        }
        return (1f - context.personality.styleSatisfaction) * 0.85f;
    }

    public override void Execute(GodContext context)
    {
        GameObject[] buildings = GameObject.FindGameObjectsWithTag("Building");
        if (buildings.Length == 0)
        {
            return;
        }
        //Calculate average building position for spread detection
        Vector3 centroid = Vector3.zero;
        int validCount = 0;
        foreach (var b in buildings)
        {
            if (IsGhostOrBeingPlaced(b))
            {
                continue;
            }
            centroid += b.transform.position;
            validCount++;
        }
        if (validCount == 0)
        {
            return;
        }
        centroid /= buildings.Length;
        List<GameObject> targets = new List<GameObject>();

        if (context.personality.style == StyleType.Wild) //Wild is dissatisfied with closely grouped buildings destroy close ones
        {
            foreach (var b in buildings)
            {
                if (IsGhostOrBeingPlaced(b))
                {
                    continue;
                }
                float dist = Vector3.Distance(b.transform.position, centroid);
                if (dist < 5f)
                {
                    targets.Add(b);
                }
            }
        }
        else //Modern is dissatisfied with spread out buildings destroy far ones
        {
            foreach (var b in buildings)
            {
                if (IsGhostOrBeingPlaced(b))
                {
                    continue;
                }
                float dist = Vector3.Distance(b.transform.position, centroid);
                if (dist > 10f)
                {
                    targets.Add(b);
                }
            }
        }
        int destroyCount = Mathf.Min(2, targets.Count); //Destroy up to 2 matching buildings
        for (int i = 0; i < destroyCount; i++)
        {
            int idx = Random.Range(0, targets.Count);
            BuildingHealth bh = targets[idx].GetComponent<BuildingHealth>();
            if (bh != null)
            {
                bh.TakeDamage(999f); //Destory building 
            }
            else
            {
                Object.Destroy(targets[idx]);
            }
            targets.RemoveAt(idx);
        }
        Debug.Log($"Earthquake destroyed {destroyCount} buildings");
    }

    bool IsGhostOrBeingPlaced(GameObject obj)
    {
        if (obj.name == "GhostPreview")
        {
            return true;
        }
        if (BuildingPlacer.Instance != null && BuildingPlacer.Instance.IsPlacing())
        {
            Collider col = obj.GetComponent<Collider>();
            if (col != null && !col.enabled)
            {
                return true;
            }
        }
        return false;
    }
}